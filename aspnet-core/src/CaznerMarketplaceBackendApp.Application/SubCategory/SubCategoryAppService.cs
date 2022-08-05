using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.Connections;
using CaznerMarketplaceBackendApp.MultiTenancy;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.SubCategory.Dto;
using System;
using System.Data;
using System.Web;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Linq;
using static CaznerMarketplaceBackendApp.AppConsts;
using Abp.Authorization;

using FimApp.EntityFrameworkCore;
using System.Collections.Generic;
using Dapper;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using CaznerMarketplaceBackendApp.Product.Dto;
using Abp.UI;

namespace CaznerMarketplaceBackendApp.SubCategory
{
    public class SubCategoryAppService :
         AsyncCrudAppService<SubCategoryMaster, SubCategoryDto, long, SubCategoryDto>, ISubCategoryAppService
    {
        private readonly IRepository<SubCategoryMaster, long> _repository;
        private readonly IRepository<CategoryGroupMaster, long> _categoryGroupRepository;
        private readonly IRepository<ProductMaster, long> _ProductMasterRepository;
        private readonly IRepository<CategoryGroups, long> _categoryGroupsRepository;
        private readonly IRepository<CategoryMaster, long> _repositoryMaster;
        string FolderName = string.Empty;
        string LogoFolderName = string.Empty;
        string CurrentWebsiteUrl = string.Empty;
        private IConfiguration _configuration;
        string AzureProductFolder = string.Empty;
        string AzureStorageUrl = string.Empty;
        private readonly IRepository<Tenant, int> _tenantManager;
        private readonly IRepository<CategoryMaster, long> _repositoryCategoryMaster;
        private readonly DbConnectionUtility _connectionUtility;
        private IDbConnection _db;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ProductAssignedSubSubCategories, long> _repositoryProductAssignedSubCategories;
        private readonly IRepository<ProductImages, long> _productImagesRepository;
        private readonly IRepository<CategorySubCategories, long> _repositoryCategorySubCategory;
        public SubCategoryAppService(IRepository<SubCategoryMaster, long> repository, IUnitOfWorkManager unitOfWorkManager,
            IConfiguration configuration, IRepository<CategoryMaster, long> repositoryCategoryMaster,
            IRepository<CategorySubCategories, long> repositoryCategorySubCategory,
            IRepository<CategoryGroupMaster, long> categoryGroupRepository,
            IRepository<CategoryGroups, long> categoryGroupsRepository,
            DbConnectionUtility connectionUtility, IRepository<Tenant, int> tenantManager,
            IRepository<ProductMaster, long> ProductMasterRepository,
            IRepository<ProductImages, long> productImagesRepository,
            IRepository<ProductAssignedSubSubCategories, long> repositoryProductAssignedSubCategories) : base(repository)
        {
            _repository = repository;
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = configuration;
            FolderName = _configuration["FileUpload:FolderName"];
            LogoFolderName = _configuration["FileUpload:FrontEndLogoFolderName"];
            CurrentWebsiteUrl = _configuration["FileUpload:WebsireDomainPath"];
            _repositoryCategoryMaster = repositoryCategoryMaster;
            _repositoryCategorySubCategory = repositoryCategorySubCategory;
            _tenantManager = tenantManager;
            AzureProductFolder = _configuration["FileUpload:AzureProductFolder"];
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];
            _connectionUtility = connectionUtility;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);
            _repositoryProductAssignedSubCategories = repositoryProductAssignedSubCategories;
            _categoryGroupRepository = categoryGroupRepository;
            _ProductMasterRepository = ProductMasterRepository;
            _categoryGroupsRepository = categoryGroupsRepository;
            _productImagesRepository = productImagesRepository;
        }


        public async Task<SubCategoryCustomlistDto> GetAllSubCategoryList(SubCategoryResultRequestDto Input)
        {
            Utility utility = new Utility();
            int TotalCount = 0;
            string TenantId = string.Empty;
            List<AllCategoryResponseDto> CategoryList = new List<AllCategoryResponseDto>();
            List<AllCategoryResponseDto> CategoryListDistinct = new List<AllCategoryResponseDto>();
            IQueryable<AllCategoryResponseDto> CategoryFinalList = Enumerable.Empty<AllCategoryResponseDto>().AsQueryable();
            IQueryable<AllCategoryResponseDto> CategoryListData = Enumerable.Empty<AllCategoryResponseDto>().AsQueryable();
            bool IsLoadMore = false;
            SubCategoryCustomlistDto response = new SubCategoryCustomlistDto();
            try
            {
                if (string.IsNullOrEmpty(Input.EncryptedTenantId))
                {
                    int? SessionTenantId = AbpSession.TenantId;
                    if (SessionTenantId.HasValue)
                    {
                        TenantId = SessionTenantId.ToString();
                    }
                }
                else
                {
                    TenantId = utility.DecryptString(Input.EncryptedTenantId);
                }
                if (string.IsNullOrEmpty(TenantId))
                {
                    throw new AbpAuthorizationException("Tenant user is unauthorized");
                }

                using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
                {
                    dynamic category;

                    await _connectionUtility.EnsureConnectionOpenAsync();

                    IEnumerable<AllCategoryResponseDto> Categories = await _db.QueryAsync<AllCategoryResponseDto>("usp_GetSubCategoryList", new
                    {
                        TenantId = Convert.ToInt32(TenantId),
                        SearchText = Input.SearchText,
                        categoryId = Input.SubCategoryId
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    // CategoryListDistinct = Categories.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).Distinct().ToList();
                    CategoryFinalList = (from Categorymaster in Categories
                                         select new AllCategoryResponseDto
                                         {
                                             Id = Categorymaster.Id,
                                             SubSubCategoryId = Categorymaster.SubSubCategoryId,
                                             SubSubCategoryTitle = Categorymaster.SubSubCategoryTitle,
                                             SubCategoryId = Categorymaster.SubCategoryId,
                                             SubCategoryTitle = Categorymaster.SubCategoryTitle,
                                             CategoryId = Categorymaster.CategoryId,
                                             CategoryTitle = Categorymaster.CategoryTitle,
                                             ImagePath = Categorymaster.ImagePath,
                                             CreationTime = Categorymaster.CreationTime,
                                             ImageObj = new ProductImageType { Ext = Categorymaster.Ext, Name = Categorymaster.Name, Type = Categorymaster.Type, Url = Categorymaster.Url, Size = Categorymaster.Size, FileName = Categorymaster.Name },
                                             product = (from data in _ProductMasterRepository.GetAll()
                                                        join grp in _repositoryProductAssignedSubCategories.GetAll() on data.Id equals grp.ProductId
                                                        where grp.SubSubCategoryId == Categorymaster.SubSubCategoryId
                                                        select new ProductListDto
                                                        {
                                                            Id = data.Id,
                                                            ProductSKU = data.ProductSKU,
                                                            ProductTitle = data.ProductTitle,
                                                            ImageObj = (from dataImg in _ProductMasterRepository.GetAll()
                                                                        join grpImg in _productImagesRepository.GetAll() on dataImg.Id equals grpImg.ProductId
                                                                        where (dataImg.Id == grp.ProductId) && (grpImg.IsDefaultImage)
                                                                        select new ProductImageType
                                                                        {
                                                                            Ext = grpImg.Ext,
                                                                            Name = grpImg.Name,
                                                                            Size = grpImg.Size,
                                                                            Url = grpImg.Url,
                                                                            Type = grpImg.Type,
                                                                        }).ToList(),
                                                        }).ToList(),

                                         }).AsQueryable();

                    TotalCount = CategoryFinalList.ToList().Count();

                    CategoryListData = await ApplyDtoSorting(CategoryFinalList, Input);
                    CategoryListData = CategoryListData.Skip(Input.SkipCount).Take(Input.MaxResultCount);

                    if (Input.SkipCount + Input.MaxResultCount < TotalCount)
                    {
                        IsLoadMore = true;
                    }

                    response.SkipCount = Input.SkipCount;
                    response.items = ObjectMapper.Map<List<AllCategoryResponseDto>>(CategoryListData);
                    response.TotalCount = TotalCount;
                    response.IsLoadMore = IsLoadMore;
                }
            }
            catch (System.Exception ex)
            {
                //throw;
            }
            return response;
        }
        #region sorting 

        private async Task<IQueryable<AllCategoryResponseDto>> ApplyDtoSorting(IQueryable<AllCategoryResponseDto> query, SubCategoryResultRequestDto input)
        {
            if (input.Sorting.HasValue)
            {
                switch (input.Sorting)
                {
                    case 1:
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.Id);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Id);
                        }

                        break;
                    case 2://
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.SubCategoryTitle);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.SubCategoryTitle);
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.SubCategoryTitle);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.SubCategoryTitle);
                        }
                        break;

                    default:
                        {
                            query = query.OrderBy(x => x.SubCategoryTitle);
                        }
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(x => x.SubSubCategoryId);
            }
            return query;
        }

        #endregion

        public async Task<SubCategoryDto> CreateSubCategory(CreateSubCategoryDto Model)
        {
            bool isExists = false;
            SubCategoryDto response = new SubCategoryDto();
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                var IsExists = _repository.GetAllList(i => i.Title.ToLower().Trim() == Model.SubCategoryTitle.ToLower().Trim()).FirstOrDefault();
                long Id = 0;
                if (IsExists == null)
                {
                    SubCategoryMaster master = new SubCategoryMaster();
                    if (Model.ImageObj != null)
                    {

                        string ImageLocation = AzureStorageUrl + folderPath + Model.ImageObj.FileName;


                        if (!string.IsNullOrEmpty(Model.ImageObj.FileName))
                        {
                            master.ImageUrl = ImageLocation;
                            master.Name = Model.ImageObj.FileName;
                            master.Ext = Model.ImageObj.Ext;
                            master.Url = ImageLocation;
                            master.Size = Model.ImageObj.Size;
                            master.Type = Model.ImageObj.Type;
                        }
                    }
                    master.Title = Model.SubCategoryTitle;
                    Id = await _repository.InsertAndGetIdAsync(master);

                    if (master != null && Model.CategoryIds != null)
                    {
                        foreach (var item in Model.CategoryIds)
                        {
                            CategorySubCategories category = new CategorySubCategories();
                            category.CategoryId = item;
                            category.SubCategoryId = Id;
                            category.IsActive = true;
                            await _repositoryCategorySubCategory.InsertAsync(category);
                        }
                    }


                }
                else
                {
                    isExists = true;
                }
            }
            catch (Exception ex)
            {

            }
            if (isExists)
            {
                throw new UserFriendlyException(L("SubSubCategoryAlreadyExists"), L("SubSubCategoryAlreadyExists"));
            }
            else
            {
                return response;
            }
        }

        public async Task<AllCategoryResponseDto> GetSubCategoryById(long Id)
        {
            AllCategoryResponseDto Response = new AllCategoryResponseDto();
            var Category = _repository.GetAllList(i => i.Id == Id).FirstOrDefault();

            if (Category != null)
            {
                Response.SubCategoryId = _repositoryCategorySubCategory.GetAllList(i => i.SubCategoryId == Id).Select(i => i.CategoryId).LastOrDefault();
                Response.SubSubCategoryId = Category.Id;


                Response.CategoryTitle = (from coll in _categoryGroupRepository.GetAllList()
                                          join grp in _categoryGroupsRepository.GetAllList() on coll.Id equals grp.CategoryGroupId
                                          join cat in _repositoryCategorySubCategory.GetAllList() on grp.CategoryMasterId equals cat.CategoryId
                                          where cat.SubCategoryId == Id
                                          select coll.GroupTitle).FirstOrDefault();
                Response.SubCategoryTitle = (from cat in _repositoryCategoryMaster.GetAllList()
                                             join sub in _repositoryCategorySubCategory.GetAllList() on cat.Id equals sub.CategoryId
                                             where sub.SubCategoryId == Id
                                             select cat.CategoryTitle).LastOrDefault();
                Response.product = (from data in _ProductMasterRepository.GetAll()
                                    join grp in _repositoryProductAssignedSubCategories.GetAll() on data.Id equals grp.ProductId
                                    where grp.SubSubCategoryId == Category.Id
                                    select new ProductListDto
                                    {
                                        Id = data.Id,
                                        ProductSKU = data.ProductSKU,
                                        ProductTitle = data.ProductTitle,
                                        ImageObj = (from dataImg in _ProductMasterRepository.GetAll()
                                                    join grpImg in _productImagesRepository.GetAll() on dataImg.Id equals grpImg.ProductId
                                                    where (dataImg.Id == grp.ProductId) && (grpImg.IsDefaultImage)
                                                    select new ProductImageType
                                                    {
                                                        Ext = grpImg.Ext,
                                                        Name = grpImg.Name,
                                                        Size = grpImg.Size,
                                                        Url = grpImg.Url,
                                                        Type = grpImg.Type,
                                                    }).ToList(),
                                    }).ToList();
                Response.ImagePath = Category.ImageUrl;
                Response.SubSubCategoryTitle = Category.Title;
                if (Response.ImagePath != null)
                {
                    Response.ImageObj = new ProductImageType { Ext = Category.Ext, Name = Category.Name, Type = Category.Type, Url = Category.Url, Size = Category.Size, FileName = Category.ImageName };
                }
                else
                {
                    Response.ImageObj = null;
                }


            }

            return Response;
        }
        public async Task<ProductSubSubCategoryDto> GetProductBySubSubCategoryId(ProductDataRequestBySubSubCategoryDto Input)
        {
            int TotalCount = 0;
            bool IsLoadMore = false;
            ProductSubSubCategoryDto response = new ProductSubSubCategoryDto();
            List<ProductListViewDto> ProductData = new List<ProductListViewDto>();
            Utility utility = new Utility();
            try
            {
                string TenantId = string.Empty;
                if (string.IsNullOrEmpty(Input.EncryptedTenantId))
                {
                    int? SessionTenantId = AbpSession.TenantId;
                    if (SessionTenantId.HasValue)
                    {
                        TenantId = SessionTenantId.ToString();
                    }
                }
                else
                {
                    TenantId = utility.DecryptString(Input.EncryptedTenantId);
                }
                if (string.IsNullOrEmpty(TenantId))
                {
                    throw new AbpAuthorizationException("Tenant user is unauthorized");
                }

                using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
                {
                    dynamic category;

                    await _connectionUtility.EnsureConnectionOpenAsync();


                    IEnumerable<ProductListViewDto> ProductDataa = await _db.QueryAsync<ProductListViewDto>("usp_GetProductBySubSubCategoryId", new
                    {
                        SearchText = Input.SearchText,
                        TenantId = AbpSession.TenantId.Value,
                        subSubCategoryID = Input.SubSubCategoryId
                    }, commandType: System.Data.CommandType.StoredProcedure);

                    ProductData = ProductDataa.ToList();
                    var subCategory = _repository.GetAllList(i => i.Id == Input.SubSubCategoryId).FirstOrDefault();
                    var catSubCat = _repositoryCategorySubCategory.GetAllList(i => i.SubCategoryId == Input.SubSubCategoryId).FirstOrDefault();
                    var SubCategoryId = _repositoryCategoryMaster.GetAllList(i => i.Id == catSubCat.CategoryId).FirstOrDefault();
                    var cat = _categoryGroupsRepository.GetAllList(i => i.CategoryMasterId == SubCategoryId.Id).FirstOrDefault();
                    var catMaster = _categoryGroupRepository.GetAllList(i => i.Id == cat.CategoryGroupId).FirstOrDefault();
                    response.CatergoryId = catMaster.Id;
                    response.CatergoryName = catMaster.GroupTitle;
                    response.SubCategoryId = SubCategoryId.Id;
                    response.SubCategoryName = SubCategoryId.CategoryTitle;
                    response.SubSubcategoryId = subCategory.Id;
                    response.SubSubCategoryTitle = subCategory.Title;
                    response.product = ProductData.ToList();
                    TotalCount = ProductData.Count();

                    ProductData = ProductData.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).ToList();
                    ProductData = await ApplyDtoSortingListForProduct(ProductData, Input);
                    ProductData = ProductData.Skip(Input.SkipCount).Take(Input.MaxResultCount).ToList();

                    if (Input.SkipCount + Input.MaxResultCount < TotalCount)
                    {
                        IsLoadMore = true;
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return response;
        }

        public async Task<CreateSubCategoryDto> UpdateSubCategory(CreateSubCategoryDto Model)
        {
            bool IsNameExists = false;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                var IsExists = _repository.GetAllList(i => i.Title.ToLower().Trim() == Model.SubCategoryTitle.ToLower().Trim() && i.Id != Model.SubCategoryId).FirstOrDefault();

                if (IsExists == null)
                {
                    var Category = _repository.GetAllList(i => i.Id == Model.SubCategoryId).FirstOrDefault();
                    if (Model.ImageObj != null)
                    {
                        string ImageLocation = AzureStorageUrl + folderPath + Model.ImageObj.FileName;

                        if (!string.IsNullOrEmpty(Model.ImageObj.FileName))
                        {
                            Category.ImageUrl = ImageLocation;
                            Category.Name = Model.ImageObj.FileName;
                            Category.Ext = Model.ImageObj.Ext;
                            Category.Url = ImageLocation;
                            Category.Size = Model.ImageObj.Size;
                            Category.Type = Model.ImageObj.Type;
                        }
                    }

                    Category.Title = Model.SubCategoryTitle;

                    await _repository.UpdateAsync(Category);

                    if (Category != null && Model.CategoryIds != null)
                    {
                        var ExistingData = _repositoryCategorySubCategory.GetAllList(i => i.SubCategoryId == Model.SubCategoryId);
                        var OldData = Model.CategoryIds.Where(i => i > 0);
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2 == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteCategorySubCategory(DeletedData, AbpSession.TenantId.Value);
                        }

                        foreach (var item in Model.CategoryIds)
                        {

                            if (item > 0)
                            {
                                var ChkCategory = _repositoryCategorySubCategory.GetAllList(i => i.SubCategoryId == Model.SubCategoryId && i.CategoryId == item).FirstOrDefault();
                                if (ChkCategory == null)
                                {
                                    CategorySubCategories category = new CategorySubCategories();
                                    category.CategoryId = item;
                                    category.SubCategoryId = Model.SubCategoryId;
                                    category.IsActive = true;
                                    await _repositoryCategorySubCategory.InsertAsync(category);
                                }
                            }

                        }
                    }
                    else
                    {
                        var IsDataExists = _repositoryCategorySubCategory.GetAllList(i => i.SubCategoryId == Model.SubCategoryId).ToList();
                        if (IsDataExists.Count > 0)
                        {
                            await DeleteCategorySubCategory(IsDataExists, AbpSession.TenantId.Value);
                        }
                    }
                }
                else
                {
                    IsNameExists = true;
                }
            }
            catch (Exception ex)
            {

            }
            if (IsNameExists)
            {
                throw new UserFriendlyException(L("SubSubCategoryAlreadyExists"), L("SubSubCategoryAlreadyExists"));
            }
            else
            {

                return Model;
            }
        }
        public async Task<bool> DeleteCategorySubCategory(List<CategorySubCategories> Category, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryCategorySubCategory.BulkDelete(Category);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        public async Task<bool> DeleteSubSubCategory(List<SubCategoryMaster> Category, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repository.BulkDelete(Category);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> UpdateAssignedSubCategories(List<ProductAssignedSubSubCategories> AssignedSubCategories, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryProductAssignedSubCategories.BulkUpdate(AssignedSubCategories);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> UpdateCategorySubCategory(List<CategorySubCategories> subcategory, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryCategorySubCategory.BulkUpdate(subcategory);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }


        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var Data = _repository.GetAllList(i => i.Id == input.Id).FirstOrDefault();
            if (Data != null)
            {
                var Assignments = _repositoryProductAssignedSubCategories.GetAllList(i => i.SubSubCategoryId == input.Id).ToList();
                if (Assignments.Count > 0)
                {
                    _repositoryProductAssignedSubCategories.BulkDelete(Assignments);
                }
                await _repository.DeleteAsync(input.Id);
            }
        }

        private async Task<List<ProductListViewDto>> ApplyDtoSortingListForProduct(List<ProductListViewDto> query, ProductDataRequestBySubSubCategoryDto input)
        {
            if (input.Sorting.HasValue)
            {
                switch (input.Sorting)
                {
                    case 1:
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.Id).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Id).ToList();
                        }

                        break;
                    case 2://
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.ProductTitle).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle).ToList();
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.ProductTitle).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle).ToList();
                        }
                        break;

                    case 4://price
                        if (input.FilterBy == 1)
                        {
                            query = query.OrderBy(x => x.UnitPrice).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.UnitPrice).ToList();
                        }
                        break;

                    default:
                        {
                            query = query.OrderBy(x => x.ProductTitle).ToList();

                        }
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.ProductTitle).ToList();
            }
            return query;
        }
    }


}
