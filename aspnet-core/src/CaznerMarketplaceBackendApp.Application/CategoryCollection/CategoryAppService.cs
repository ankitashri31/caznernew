using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CaznerMarketplaceBackendApp.AppConsts;
using Abp.Authorization;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using FimApp.EntityFrameworkCore;
using CaznerMarketplaceBackendApp.Product.Dto;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.MultiTenancy;
using Microsoft.Data.SqlClient;
using System.Data;
using CaznerMarketplaceBackendApp.Connections;
using NUglify.Helpers;
using Dapper;
using CaznerMarketplaceBackendApp.SubCategory;
using Abp.UI;

namespace CaznerMarketplaceBackendApp.CategoryCollection
{
    public class CategoryAppService :
         AsyncCrudAppService<CategoryMaster, CategoryDto, long, CategoryResultRequestDto, CreateCategoryDto, CategoryDto>, ICategoryAppService
    {
        private readonly IRepository<CategoryMaster, long> _repository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        string FolderName = string.Empty;
        string LogoFolderName = string.Empty;
        string CurrentWebsiteUrl = string.Empty;
        private IConfiguration _configuration;
        private readonly IRepository<CollectionMaster, long> _repositoryCollection;
        private readonly IRepository<ProductImages, long> _productImagesRepository;
        private readonly IRepository<CategoryCollections, long> _repositoryCategoryCollection;
        private readonly IRepository<SubCategoryMaster, long> _repositorySubCategoryMaster;
        private readonly IRepository<CategorySubCategories, long> _repositorySubSubCategory;
        private readonly IRepository<CategoryGroups, long> _repositoryCategoryGroups;
        private readonly IRepository<ProductMaster, long> _ProductMasterRepository;
        private readonly IRepository<ProductAssignedSubSubCategories, long> _repositoryAssignedSubSubCategories;
        string AzureProductFolder = string.Empty;
        string AzureStorageUrl = string.Empty;
        private readonly IRepository<Tenant, int> _tenantManager;
        private readonly DbConnectionUtility _connectionUtility;
        private IDbConnection _db;
        private readonly IRepository<ProductAssignedSubCategories, long> _repositoryProductAssignedCategories;
        public CategoryAppService(IRepository<CategoryMaster, long> repository, IUnitOfWorkManager unitOfWorkManager,
             IConfiguration configuration, IRepository<ProductMaster, long> ProductMasterRepository,
             IRepository<ProductAssignedSubSubCategories, long> repositoryAssignedSubSubCategories,
        IRepository<CollectionMaster, long> repositoryCollection, IRepository<CategoryCollections, long> repositoryCategoryCollection,
             IRepository<CategoryGroups, long> repositoryCategoryGroups, IRepository<Tenant, int> tenantManager,
              IRepository<ProductImages, long> productImagesRepository,
        IRepository<SubCategoryMaster, long> repositorySubCategoryMaster, IRepository<CategorySubCategories, long> repositorySubSubCategory,
        DbConnectionUtility connectionUtility, IRepository<ProductAssignedSubCategories, long> repositoryProductAssignedCategories) : base(repository)
        {
            _repository = repository;
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = configuration;
            FolderName = _configuration["FileUpload:FolderName"];
            LogoFolderName = _configuration["FileUpload:FrontEndLogoFolderName"];
            CurrentWebsiteUrl = _configuration["FileUpload:WebsireDomainPath"];
            _repositoryCollection = repositoryCollection;
            _repositoryCategoryCollection = repositoryCategoryCollection;
            _repositoryCategoryGroups = repositoryCategoryGroups;
            _repositorySubCategoryMaster = repositorySubCategoryMaster;
            _tenantManager = tenantManager;
            AzureProductFolder = _configuration["FileUpload:AzureProductFolder"];
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];
            _repositoryCategoryGroups = repositoryCategoryGroups;
            _connectionUtility = connectionUtility;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);
            _repositoryProductAssignedCategories = repositoryProductAssignedCategories;
            _repositorySubSubCategory = repositorySubSubCategory;
            _ProductMasterRepository = ProductMasterRepository;
            _repositoryAssignedSubSubCategories = repositoryAssignedSubSubCategories;
            _productImagesRepository = productImagesRepository;
        }


        public override async Task<PagedResultDto<CategoryDto>> GetAllAsync(CategoryResultRequestDto Input)
        {
            int TotalCount = 0;
            Utility utility = new Utility();
            string TenantId = string.Empty;
            List<CategoryDto> CategoryList = new List<CategoryDto>();
            List<CategoryDto> CategoryListDistinct = new List<CategoryDto>(); var Collections = new List<CollectionModel>();
            IQueryable<CategoryDto> CategoryFinalList = Enumerable.Empty<CategoryDto>().AsQueryable();
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

                    await _connectionUtility.EnsureConnectionOpenAsync();

                    IEnumerable<CategoryDto> Categories = await _db.QueryAsync<CategoryDto>("usp_GetCategorieslist", new
                    {
                        TenantId = Convert.ToInt32(TenantId),
                        SearchText = Input.SearchText,
                        GroupId = Input.GroupId
                    }, commandType: System.Data.CommandType.StoredProcedure);


                    CategoryListDistinct = Categories.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).Distinct().ToList();
                    CategoryFinalList = (from Categorymaster in CategoryListDistinct
                                         select new CategoryDto
                                         {
                                             Id = Categorymaster.Id,
                                             CategoryTitle = Categorymaster.CategoryTitle,
                                             CategoryGroupId = Categorymaster.CategoryGroupId,
                                             CategoryGroupTitle = Categorymaster.CategoryGroupTitle,
                                             CategoryImageUrl = Categorymaster.ImagePath,
                                             SubCategory = (from item in Categories.ToList()
                                                            where item.Id == Categorymaster.Id
                                                            select new SubCategoryModel
                                                            {
                                                                Id = item.SubCategoryId,
                                                                CategorySubCategoryId = item.CategorySubCategoryId,
                                                                Title = item.SubCategoryTitle
                                                            }).GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).ToList(),//.DistinctBy(x => x.Id).ToList(),
                                             ImageObj = new ProductImageType { Ext = Categorymaster.Ext, Name = Categorymaster.Name, Type = Categorymaster.Type, Url = Categorymaster.Url, Size = Categorymaster.Size, FileName = Categorymaster.Name }
                                         }).AsQueryable();

                    TotalCount = CategoryFinalList.Count();
                    if(Input.SkipCount ==0)
                    {
                        Input.MaxResultCount = TotalCount;
                    }
                    CategoryFinalList = await ApplyDtoSorting(CategoryFinalList.AsQueryable(), Input);
                }
            }
            catch (Exception ex)
            { }
            return new PagedResultDto<CategoryDto>(TotalCount, ObjectMapper.Map<List<CategoryDto>>(CategoryFinalList));
        }
        public async Task<List<CategoryMaster>> GetSubCategoryListForSubSubCategory(CategoryResultRequestDto Input)
        {
            int TotalCount = 0;
            Utility utility = new Utility();
            string TenantId = string.Empty;
            var categoryList = new List<CategoryMaster>();


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

                    await _connectionUtility.EnsureConnectionOpenAsync();
                    categoryList = _repository.GetAllList().OrderBy(x => x.CategoryTitle).ToList();

                    TotalCount = categoryList.Count();

                }
            }
            catch (Exception ex)
            { }
            return categoryList;
        }


        public async Task<CategoryCustomlistDto> GetCategoryList(CategoryResultRequestDto Input)
        {
            Utility utility = new Utility();
            int TotalCount = 0; List<CategoryDto> CategoryList = new List<CategoryDto>();
            List<CategoryDto> CategoryListDistinct = new List<CategoryDto>(); var Collections = new List<CollectionModel>();
            IQueryable<CategoryDto> CategoryFinalList = Enumerable.Empty<CategoryDto>().AsQueryable();

            bool IsLoadMore = false;
            string TenantId = string.Empty;
            CategoryCustomlistDto response = new CategoryCustomlistDto();
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

                    await _connectionUtility.EnsureConnectionOpenAsync();


                    IEnumerable<CategoryDto> Categories = await _db.QueryAsync<CategoryDto>("usp_GetCategorieslist", new
                    {
                        TenantId = Convert.ToInt32(TenantId),
                        SearchText = Input.SearchText,
                        GroupId = Input.GroupId
                    }, commandType: System.Data.CommandType.StoredProcedure);


                    CategoryListDistinct = Categories.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).Distinct().ToList();
                    CategoryFinalList = (from Categorymaster in CategoryListDistinct
                                         select new CategoryDto
                                         {
                                             Id = Categorymaster.Id,
                                             CategoryTitle = Categorymaster.CategoryTitle,
                                             CategoryGroupId = Categorymaster.CategoryGroupId,
                                             CategoryGroupTitle = Categorymaster.CategoryGroupTitle,
                                             CategoryImageUrl = Categorymaster.ImagePath,
                                             SubCategory = (from item in Categories.ToList()
                                                            where item.Id == Categorymaster.Id
                                                            select new SubCategoryModel
                                                            {
                                                                Id = item.SubCategoryId,
                                                                CategorySubCategoryId = item.CategorySubCategoryId,
                                                                Title = item.SubCategoryTitle
                                                            }).GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).ToList(),//.DistinctBy(x => x.Id).ToList(),
                                             ImageObj = new ProductImageType { Ext = Categorymaster.Ext, Name = Categorymaster.Name, Type = Categorymaster.Type, Url = Categorymaster.Url, Size = Categorymaster.Size, FileName = Categorymaster.Name }
                                         }).AsQueryable();

                    TotalCount = CategoryFinalList.Count();
                    CategoryFinalList = await ApplyDtoSorting(CategoryFinalList.AsQueryable(), Input);
                    CategoryFinalList = CategoryFinalList.Skip(Input.SkipCount).Take(Input.MaxResultCount);
                
                    if (Input.SkipCount + Input.MaxResultCount < TotalCount)
                    {
                        IsLoadMore = true;
                    }
                    response.SkipCount = Input.SkipCount;
                    response.items = ObjectMapper.Map<List<CategoryDto>>(CategoryFinalList);
                    response.TotalCount = TotalCount;
                    response.IsLoadMore = IsLoadMore;
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return response;
        }

        public async Task<List<SubCategoryCustomDataDto>> GetSubSubCategoryListBySubCategory(SubCategoryRequestDto Input)
        {
            Utility utility = new Utility();
            int TotalCount = 0; List<CategoryDto> CategoryList = new List<CategoryDto>();
            List<SubCategoryDataDto> CategoryListDistinct = new List<SubCategoryDataDto>();
            var Collections = new List<CollectionModel>();
            List<SubCategoryCustomDataDto> CategoryFinalList = new List<SubCategoryCustomDataDto>();

            bool IsLoadMore = false;
            string TenantId = string.Empty;
            List<SubCategoryCustomDataDto> response = new List<SubCategoryCustomDataDto>();
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

                    await _connectionUtility.EnsureConnectionOpenAsync();


                    IEnumerable<SubCategoryDataDto> Categories = await _db.QueryAsync<SubCategoryDataDto>("usp_GetSubSubCategorieslist", new
                    {
                        TenantId = Convert.ToInt32(TenantId),
                        SearchText = Input.SearchText,
                        CategortId = Input.CategoryId
                    }, commandType: System.Data.CommandType.StoredProcedure);


                    CategoryListDistinct = Categories.GroupBy(g => g.SubCategoryId).Select(x => x.FirstOrDefault()).Distinct().ToList();
                    CategoryFinalList = (from Categorymaster in CategoryListDistinct
                                         select new SubCategoryCustomDataDto
                                         {
                                             SubCategoryId = Categorymaster.SubCategoryId,
                                             SubCategoryTitle = Categorymaster.SubCategoryTitle,
                                             CategoryTitle = Categorymaster.CategoryTitle,
                                             IsActive = Categorymaster.IsActive,
                                             CategoryList = (from item in Categories
                                                             where item.SubCategoryId == Categorymaster.SubCategoryId
                                                             select new CategorySubSubModel
                                                             {
                                                                 SubSubCategoryTitle = item.SubSubCategoryTitle,
                                                                 SubSubCategoryMasterId = item.SubSubCategoryId,
                                                                 SubCategoryImageObj = new ProductImageType { Ext = item.ImageExt, Name = item.ImageName, Type = item.ImageType, Url = item.ImageUrl, Size = item.ImageSize, FileName = item.ImageName },
                                                                 ProductList = (from data in _ProductMasterRepository.GetAll()
                                                                                join grp in _repositoryAssignedSubSubCategories.GetAll() on data.Id equals grp.ProductId
                                                                                where grp.SubSubCategoryId == item.SubSubCategoryId
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

                                                             }).GroupBy(g => g.SubSubCategoryMasterId).Select(x => x.FirstOrDefault()).ToList(),//.DistinctBy(x => x.Id).ToList(),

                                         }).ToList();

                    response = CategoryFinalList;
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return response;
        }
        public override async Task<CategoryDto> CreateAsync(CreateCategoryDto Model)
        {
            CategoryMaster Category = new CategoryMaster();
            CategoryDto Response = new CategoryDto();
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            bool IsSuccess = false;
            try
            {
                if (Model != null)
                {
                    var isExists = _repository.GetAllList(i => i.CategoryTitle.ToLower() == Model.CategoryTitle.ToLower()).FirstOrDefault();
                    if (isExists == null)
                    {
                       
                        Category = ObjectMapper.Map<CategoryMaster>(Model);
                        Response = ObjectMapper.Map<CategoryDto>(Category);

                        Category.ImageName = Model.CategoryImageName;
                        if (Model.ImageObj != null && !string.IsNullOrEmpty(Model.ImageObj.FileName))
                        {

                            string ImageLocation = AzureStorageUrl + folderPath + Model.ImageObj.FileName;

                            Category.CategoryImageUrl = ImageLocation;
                            Category.Name = Model.ImageObj.FileName;
                            Category.Ext = Model.ImageObj.Ext;
                            Category.Url = ImageLocation;
                            Category.Size = Model.ImageObj.Size;
                            Category.Type = Model.ImageObj.Type;
                        }
                        long Id = await _repository.InsertAndGetIdAsync(Category);
                        IsSuccess = true;
                        if (Model.CategoryGroupId > 0)
                        {
                            CategoryGroups grps = new CategoryGroups();
                            grps.CategoryGroupId = Model.CategoryGroupId;
                            grps.CategoryMasterId = Id;
                            grps.IsActive = true;
                            await _repositoryCategoryGroups.InsertAsync(grps);
                        }


                    }
                    else
                    {
                        IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (IsSuccess == true)
            {
                return Response;
            }
            else
            {
                throw new UserFriendlyException(L("SubCategoryAlreadyExists"), L("SubCategoryAlreadyExists"));
            }
        }

        public override async Task<CategoryDto> UpdateAsync(CategoryDto Model)
        {
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            bool IsSuccess = false;
            try
            {
                var Category = _repository.GetAllList(i => i.Id == Model.Id).FirstOrDefault();
                if (Category != null)
                {
                    string TiTle = _repository.GetAllList(i => i.CategoryTitle.ToLower() == Model.CategoryTitle.ToLower() && i.Id != Model.Id).Select(i => i.CategoryTitle).FirstOrDefault();
                    if (string.IsNullOrEmpty(TiTle))
                    {

                        if (Model.ImageObj != null && !string.IsNullOrEmpty(Model.ImageObj.FileName))
                        {

                            string ImageLocation = AzureStorageUrl + folderPath + Model.ImageObj.FileName;

                            Category.CategoryImageUrl = ImageLocation;
                            Category.Name = Model.ImageObj.FileName;
                            Category.Ext = Model.ImageObj.Ext;
                            Category.Url = ImageLocation;
                            Category.Size = Model.ImageObj.Size;
                            Category.Type = Model.ImageObj.Type;
                        }

                        Category.CategoryTitle = Model.CategoryTitle;
                        await _repository.UpdateAsync(Category);
                        if (Model.CategoryGroupId > 0)
                        {
                            var ExistingData = _repositoryCategoryGroups.GetAllList(i => i.CategoryMasterId == Model.Id);
                            var OldData = Model.CategoryGroupId;
                            var DeletedData = ExistingData.Where(p => OldData != p.Id).ToList();

                            if (DeletedData.Count > 0)
                            {
                                await DeleteCategoryGroup(DeletedData, AbpSession.TenantId.Value);
                            }
                            if (Model.CategoryGroupId > 0)
                            {
                                var ChkCategory = _repositoryCategoryGroups.GetAllList(i => i.CategoryMasterId == Model.Id && i.CategoryGroupId == Model.CategoryGroupId).FirstOrDefault();
                                if (ChkCategory == null)
                                {
                                    CategoryGroups category = new CategoryGroups();
                                    category.CategoryGroupId = Model.CategoryGroupId;
                                    category.CategoryMasterId = Model.Id;
                                    category.IsActive = true;
                                    await _repositoryCategoryGroups.InsertAsync(category);
                                }
                            }
                        }
                        else
                        {
                            var IsDataExists = _repositoryCategoryGroups.GetAllList(i => i.CategoryMasterId == Model.Id).ToList();
                            if (IsDataExists.Count > 0)
                            {
                                await DeleteCategoryGroup(IsDataExists, AbpSession.TenantId.Value);
                            }
                        }
                        IsSuccess = true;
                    }
                    else
                    {
                        IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (IsSuccess)
            {
                return Model;
            }
            else
            {
                throw new UserFriendlyException(L("SubCategoryAlreadyExists"), L("SubCategoryAlreadyExists"));
            }
        }

        private async Task<bool> DeleteCategoryCollections(List<CategoryCollections> CategoryCollections, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryCategoryCollection.BulkDelete(CategoryCollections);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> UpdateAssignedCategory(List<ProductAssignedSubCategories> Categories, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryProductAssignedCategories.BulkUpdate(Categories);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> UpdateCategoryCollections(List<CategorySubCategories> CategoryCollections, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositorySubSubCategory.BulkUpdate(CategoryCollections);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        public async Task<CategoryDto> GetCategoryById(long Id)
        {
            CategoryDto Response = new CategoryDto();
            try
            {

                var Category = _repository.GetAllList(i => i.Id == Id).FirstOrDefault();

                if (Category != null)
                {
                    Response.CategoryGroupId = _repositoryCategoryGroups.GetAllList(i => i.CategoryMasterId == Id).Select(i => i.CategoryGroupId).FirstOrDefault();
                    Response.Id = Category.Id;
                    Response.CategoryImageUrl = Category.CategoryImageUrl;
                    Response.CategoryTitle = Category.CategoryTitle;
                    Response.SubCategory = (from item in _repositorySubCategoryMaster.GetAll()
                                            join category in _repositorySubSubCategory.GetAll() on item.Id equals category.SubCategoryId
                                            where category.CategoryId == Category.Id
                                            select new SubCategoryModel
                                            {
                                                Id = item.Id,
                                                CategorySubCategoryId = category.Id,
                                                Title = item.Title
                                            }).ToList();
                    //Response.Collections = (from item in _repositoryCollection.GetAll()
                    //                        join collection in _repositoryCategoryCollection.GetAllList(i => i.CategoryId == Category.Id) on item.Id equals collection.CollectionId
                    //                        select new CollectionModel
                    //                        {
                    //                            Id = item.Id,
                    //                            CategoryCollectionId = collection.Id,
                    //                            CollectionName = item.CollectionName,

                    //                        }).ToList();
                    Response.ImagePath = Category.CategoryImageUrl;
                    if (Category.Url != null)
                    {
                        Response.ImageObj = new ProductImageType { Ext = Category.Ext, Name = Category.Name, Type = Category.Type, Url = Category.Url, Size = Category.Size, FileName = Category.ImageName };
                    }
                    else
                    {
                        Response.ImageObj = null;
                    }

                    ProductImageType type = new ProductImageType();

                    return Response;
                }
            }
            catch (Exception ex)
            {

            }

            return Response;

        }

        public async Task DeleteCategoryById(long Id)
        {
            try
            {
                var Group = _repository.GetAllList(i => i.Id == Id).FirstOrDefault();

                var CategoryList = new List<CategorySubCategories>();

                // update all collections with unassigned category if its deleted
                if (Group != null)
                {
                    CategoryList = _repositorySubSubCategory.GetAllList(i => i.CategoryId == Group.Id).ToList();

                    if (CategoryList.Count > 0)
                    {
                        await DeleteGroupCategory(CategoryList, AbpSession.TenantId.Value);
                    }

                    var Assignments = _repositoryProductAssignedCategories.GetAllList(i => i.CategoryId == Id).ToList();
                    if(Assignments.Count > 0)
                    {
                        _repositoryProductAssignedCategories.BulkDelete(Assignments);
                    }
                    // delete category from master
                    await _repository.DeleteAsync(Group);
                }
            }
            catch (Exception ex)
            {

            }

        }
        private async Task<bool> DeleteCategoryGroup(List<CategoryGroups> CategoryGroups, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryCategoryGroups.BulkDelete(CategoryGroups);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteGroupCategory(List<CategorySubCategories> CategoryGroups, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositorySubSubCategory.BulkDelete(CategoryGroups);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        #region sorting 
        private async Task<IQueryable<CategoryDto>> ApplyDtoSorting(IQueryable<CategoryDto> query, CategoryResultRequestDto input)
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
                            query = query.OrderBy(x => x.CategoryTitle);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CategoryTitle);
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.CategoryTitle);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CategoryTitle);
                        }
                        break;
                    default:
                        {
                            query = query.OrderBy(x => x.CategoryTitle);

                        }
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.CategoryTitle);
            }
            return query;
        }
        #endregion
    }
}
