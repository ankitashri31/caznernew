using System;
using System.Collections.Generic;
using System.Text;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using Abp.Domain.Uow;
using Abp.Domain.Repositories;
using System.Threading.Tasks;
using Abp.Authorization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using FimApp.EntityFrameworkCore;
using Abp.Collections.Extensions;
using CaznerMarketplaceBackendApp.Product.Dto;
using static CaznerMarketplaceBackendApp.AppConsts;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.MultiTenancy;
using CaznerMarketplaceBackendApp.AzureBlobStorage;
using Microsoft.Data.SqlClient;
using System.Data;
using CaznerMarketplaceBackendApp.Connections;
using Dapper;
using CaznerMarketplaceBackendApp.SubCategory;
using Abp.Linq.Extensions;
using Abp.UI;
using CaznerMarketplaceBackendApp.Product.Masters;

namespace CaznerMarketplaceBackendApp.CategoryGroupService
{
    public class CategoryGroupAppService : CaznerMarketplaceBackendAppAppServiceBase, ICategoryGroupAppService
    {
        private readonly IRepository<CategoryGroupMaster, long> _categoryGroupRepository;
        private readonly IRepository<SubCategoryMaster, long> _repositorySubCategoryMaster;

        private readonly IRepository<CategoryGroups, long> _categoryGroupsRepository;
        private readonly DbConnectionUtility _connectionUtility;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        string FolderName = string.Empty;
        string LogoFolderName = string.Empty;
        string CurrentWebsiteUrl = string.Empty;
        private IConfiguration _configuration;
        string AzureProductFolder = string.Empty;
        string AzureStorageUrl = string.Empty;
        private readonly IRepository<Tenant, int> _tenantManager;
        private IDbConnection _db;
        private int commandTimeout = (1 * 60 * 5);
        private readonly IRepository<CategoryMaster, long> _categoryMasterRepository;
        private readonly IRepository<CategorySubCategories, long> _repositorySubSubCategory;
        private readonly IRepository<ProductAssignedCategoryMaster, long> _repositoryAssignedCategoryMaster;
        public CategoryGroupAppService(IRepository<CategoryGroupMaster, long> categoryGroupRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<CategoryGroups,
                long> categoryGroupsRepository, IConfiguration configuration, IRepository<SubCategoryMaster, long> repositorySubCategoryMaster,
            IRepository<CategorySubCategories, long> repositorySubSubCategory, IRepository<Tenant, int> tenantManager, DbConnectionUtility connectionUtility, IRepository<CategoryMaster, long> categoryMasterRepository, IRepository<ProductAssignedCategoryMaster, long> repositoryAssignedCategoryMaster)
        {
            _categoryGroupRepository = categoryGroupRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _categoryGroupsRepository = categoryGroupsRepository;
            _configuration = configuration;
            FolderName = _configuration["FileUpload:FolderName"];
            LogoFolderName = _configuration["FileUpload:FrontEndLogoFolderName"];
            CurrentWebsiteUrl = _configuration["FileUpload:WebsireDomainPath"];
            _tenantManager = tenantManager;
            AzureProductFolder = _configuration["FileUpload:AzureProductFolder"];
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];
            _connectionUtility = connectionUtility;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);
            _categoryMasterRepository = categoryMasterRepository;
            _repositorySubSubCategory = repositorySubSubCategory;
            _repositorySubCategoryMaster = repositorySubCategoryMaster;
            _repositoryAssignedCategoryMaster = repositoryAssignedCategoryMaster;
        }

        public async Task<List<CategoryCustomDataDto>> GetCategoryGroupData(GroupRequestDto Model)
        {
            List<CategoryCustomDataDto> CategoryData = new List<CategoryCustomDataDto>();
            List<CategoryDataDto> CategoryListDistinct = new List<CategoryDataDto>();

            IQueryable<CategoryCustomDataDto> CategoryFinalList = Enumerable.Empty<CategoryCustomDataDto>().AsQueryable();

            string TenantId = string.Empty;
            try
            {
                Utility utility = new Utility();
                if (string.IsNullOrEmpty(Model.EncryptedTenantId))
                {
                    int? SessionTenantId = AbpSession.TenantId;
                    if (SessionTenantId.HasValue)
                    {
                        TenantId = SessionTenantId.ToString();
                    }
                }
                else
                {
                    TenantId = utility.DecryptString(Model.EncryptedTenantId);
                }

                if (string.IsNullOrEmpty(TenantId))
                {
                    throw new AbpAuthorizationException("Tenant user is unauthorized");
                }


                using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
                {
                    await _connectionUtility.EnsureConnectionOpenAsync();

                    IEnumerable<CategoryDataDto> Categories = await _db.QueryAsync<CategoryDataDto>("ups_GetCategoryGroupData", new
                    {
                        TenantId = Convert.ToInt32(TenantId),
                        SearchText = Model.SearchText
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    CategoryListDistinct = Categories.ToList();

                    CategoryListDistinct = Categories.GroupBy(g => g.CategoryId).Select(x => x.FirstOrDefault()).Distinct().ToList();

                    var list = String.Join(",", CategoryListDistinct);
                    CategoryFinalList = (from CategoryGroupMaster in CategoryListDistinct
                                         select new CategoryCustomDataDto
                                         {
                                             CategoryId = CategoryGroupMaster.CategoryId,
                                             CategoryTitle = string.IsNullOrEmpty(CategoryGroupMaster.CategoryTitle) ? string.Empty : CategoryGroupMaster.CategoryTitle,
                                             IsActive = CategoryGroupMaster.IsActive,
                                             GroupImageUrl = CategoryGroupMaster.GroupImageUrl,
                                             SequenceNumber = CategoryGroupMaster.SequenceNumber,
                                             CreationTime = CategoryGroupMaster.CreationTime,
                                             CategoryList =(from item in Categories.ToList()
                                                            where item.CategoryId == CategoryGroupMaster.CategoryId & item.SubCategoryMasterId > 0
                                                            select new CategorySubModel
                                                             {
                                                                 SubCategoryMasterId = item.SubCategoryMasterId,
                                                                 SubCategoryTitle = string.IsNullOrEmpty(item.SubCategoryTitle)? string.Empty : item.SubCategoryTitle,
                                                                 SubCategoryImageObj = new ProductImageType { Ext = item.SubCategoryExt, Name = item.SubcategoryImageName, Type = item.SubCategoryImageType, Url = item.SubCategoryImageUrl, Size = item.SubCategoryImageSize, FileName = item.SubcategoryImageName }
                                                             }).OrderBy(i => i.SubCategoryTitle).GroupBy(g => g.SubCategoryMasterId).Select(x => x.FirstOrDefault()).ToList(),
                                             ImagePath = CategoryGroupMaster.GroupImageUrl,
                                             ImageObj = new ProductImageType { Ext = CategoryGroupMaster.Ext, Name = CategoryGroupMaster.Name, Type = CategoryGroupMaster.Type, Url = CategoryGroupMaster.Url, Size = CategoryGroupMaster.Size, FileName = CategoryGroupMaster.Name }
                                             
                                         }).AsQueryable();
                }
            }

            catch (Exception ex)
            {
            }
            CategoryData = await ApplyDtoSorting(CategoryFinalList.ToList(), Model);
            return CategoryData;
        }

        public async Task<CategoryCustomDto> GetCategoryGroupDataWithPage(GroupRequestDto Model)
        {
            CategoryCustomDto response = new CategoryCustomDto();
            List<CategoryCustomDataDto> CategoryData = new List<CategoryCustomDataDto>();
            List<CategoryDataDto> CategoryListDistinct = new List<CategoryDataDto>();

            IQueryable<CategoryCustomDataDto> CategoryFinalList = Enumerable.Empty<CategoryCustomDataDto>().AsQueryable();
            int TotalCount = 0;
            string TenantId = string.Empty;
            bool IsLoadMore = false;
            try
            {
                Utility utility = new Utility();
                if (string.IsNullOrEmpty(Model.EncryptedTenantId))
                {
                    int? SessionTenantId = AbpSession.TenantId;
                    if (SessionTenantId.HasValue)
                    {
                        TenantId = SessionTenantId.ToString();
                    }
                }
                else
                {
                    TenantId = utility.DecryptString(Model.EncryptedTenantId);
                }

                if (string.IsNullOrEmpty(TenantId))
                {
                    throw new AbpAuthorizationException("Tenant user is unauthorized");
                }


                using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
                {
                    await _connectionUtility.EnsureConnectionOpenAsync();

                    IEnumerable<CategoryDataDto> Categories = await _db.QueryAsync<CategoryDataDto>("ups_GetCategoryGroupData", new
                    {
                        TenantId = Convert.ToInt32(TenantId),
                        SearchText = Model.SearchText
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    CategoryListDistinct = Categories.ToList();

                    CategoryListDistinct = Categories.GroupBy(g => g.CategoryId).Select(x => x.FirstOrDefault()).Distinct().ToList();

                    var list = String.Join(",", CategoryListDistinct);
                    CategoryFinalList = (from CategoryGroupMaster in CategoryListDistinct
                                         select new CategoryCustomDataDto
                                         {
                                             CategoryId = CategoryGroupMaster.CategoryId,
                                             CategoryTitle = string.IsNullOrEmpty(CategoryGroupMaster.CategoryTitle) ? string.Empty : CategoryGroupMaster.CategoryTitle,
                                             IsActive = CategoryGroupMaster.IsActive,
                                             GroupImageUrl = CategoryGroupMaster.GroupImageUrl,
                                             SequenceNumber = CategoryGroupMaster.SequenceNumber,
                                             CreationTime = CategoryGroupMaster.CreationTime,
                                             CategoryList = (from item in Categories.ToList()
                                                             where item.CategoryId == CategoryGroupMaster.CategoryId
                                                             select new CategorySubModel
                                                             {
                                                                 SubCategoryMasterId = item.SubCategoryMasterId,
                                                                 SubCategoryTitle = string.IsNullOrEmpty(item.SubCategoryTitle) ? string.Empty : item.SubCategoryTitle,
                                                                 SubCategoryImageObj = new ProductImageType { Ext = item.SubCategoryExt, Name = item.SubcategoryImageName, Type = item.SubCategoryImageType, Url = item.SubCategoryImageUrl, Size = item.SubCategoryImageSize, FileName = item.SubcategoryImageName }
                                                             }).OrderBy(i => i.SubCategoryTitle).GroupBy(g => g.SubCategoryMasterId).Select(x => x.FirstOrDefault()).ToList(),
                                             ImagePath = CategoryGroupMaster.GroupImageUrl,
                                             ImageObj = new ProductImageType { Ext = CategoryGroupMaster.Ext, Name = CategoryGroupMaster.Name, Type = CategoryGroupMaster.Type, Url = CategoryGroupMaster.Url, Size = CategoryGroupMaster.Size, FileName = CategoryGroupMaster.Name }

                                         }).AsQueryable().OrderBy(i => i.CategoryTitle);
                    TotalCount = CategoryFinalList.Count();
                }
            }

            catch (Exception ex)
            {
            }

            CategoryData = await ApplyDtoSorting(CategoryFinalList.ToList(), Model);
            //if (Model.SkipCount > 0 & Model.MaxResultCount > 0)
            //{
                CategoryData = CategoryData.Skip(Model.SkipCount).Take(Model.MaxResultCount).ToList();

                if (Model.SkipCount + Model.MaxResultCount < TotalCount)
                {
                    IsLoadMore = true;
                }

           // }          

            response.SkipCount = Model.SkipCount;
            response.items = ObjectMapper.Map<List<CategoryCustomDataDto>>(CategoryData);
            response.TotalCount = TotalCount;
            response.IsLoadMore = IsLoadMore;
            return response;
        }


        public async Task<List<AllCategoryTypeMatserData>> GetCategoryGroupMasterData(GroupRequestDto Model)
        {
            List<AllCategoryTypeMatserData> CategoryData = new List<AllCategoryTypeMatserData>();
            List<AllCategoryResponseDto> CategoryListDistinct = new List<AllCategoryResponseDto>();

            IQueryable<AllCategoryTypeMatserData> CategoryFinalList = Enumerable.Empty<AllCategoryTypeMatserData>().AsQueryable();

            string TenantId = string.Empty;
            try
            {
                Utility utility = new Utility();
                if (string.IsNullOrEmpty(Model.EncryptedTenantId))
                {
                    int? SessionTenantId = AbpSession.TenantId;
                    if (SessionTenantId.HasValue)
                    {
                        TenantId = SessionTenantId.ToString();
                    }
                }
                else
                {
                    TenantId = utility.DecryptString(Model.EncryptedTenantId);
                }

                if (string.IsNullOrEmpty(TenantId))
                {
                    throw new AbpAuthorizationException("Tenant user is unauthorized");
                }


                using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
                {
                    await _connectionUtility.EnsureConnectionOpenAsync();

                    IEnumerable<AllCategoryResponseDto> Categories = await _db.QueryAsync<AllCategoryResponseDto>("ups_GetCategoryGroupMasterData", new
                    {
                        TenantId = Convert.ToInt32(TenantId)
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    CategoryListDistinct = Categories.ToList();

                    CategoryListDistinct = Categories.GroupBy(g => g.CategoryId).Select(x => x.FirstOrDefault()).Distinct().ToList();

                    var list = String.Join(",", CategoryListDistinct);
                    CategoryFinalList = (from CategoryGroupMaster in CategoryListDistinct
                                         select new AllCategoryTypeMatserData
                                         {
                                             Id = CategoryGroupMaster.CategoryId,
                                             categoryTitle = CategoryGroupMaster.CategoryTitle,
                                             IsActive = CategoryGroupMaster.IsActive,
                                             SequenceNumber = CategoryGroupMaster.SequenceNumber,
                                             categorySelectedTitle = new int[0],
                                             CategoryList = (from item in Categories.ToList()
                                                             where item.CategoryId == CategoryGroupMaster.CategoryId & item.SubCategoryId > 0
                                                             select new CategoryDataModel
                                                             {
                                                                 SubCategoryId = item.SubCategoryId,
                                                                 SubCategoryTitle = item.SubCategoryTitle,
                                                                 SubCategorySelectedTitle = new int[0],
                                                                 SubSubCategory = (from data in Categories.ToList()
                                                                                   where data.SubCategoryId == item.SubCategoryId & data.SubSubCategoryId > 0
                                                                                   select new SubSubCategoryData
                                                                                   {
                                                                                       SubSubCategoryId = data.SubSubCategoryId,
                                                                                       SubSubCategoryTitle = data.SubSubCategoryTitle,
                                                                                       SubSubCategorySelectedTitle = new int[0]

                                                                                   }).OrderBy(i => i.SubSubCategoryTitle).GroupBy(g => g.SubSubCategoryId).Select(x => x.FirstOrDefault()).ToList(),
                                                             }).OrderBy(i => i.SubCategoryTitle).GroupBy(g => g.SubCategoryId).Select(x => x.FirstOrDefault()).ToList()

                                         }).AsQueryable();
                }
            }

            catch (Exception ex)
            {
            }
            CategoryData = CategoryFinalList.ToList();
            return CategoryData;
        }
        public async Task<List<CategoryCustomDataDto>> GetCategoryGroupDataBySearchText(GroupRequestDto Model)
        {
            List<CategoryCustomDataDto> CategoryData = new List<CategoryCustomDataDto>();
            List<CategoryDataDto> CategoryListDistinct = new List<CategoryDataDto>();

            IQueryable<CategoryCustomDataDto> CategoryFinalList = Enumerable.Empty<CategoryCustomDataDto>().AsQueryable();

            string TenantId = string.Empty;
            try
            {
                Utility utility = new Utility();
                if (string.IsNullOrEmpty(Model.EncryptedTenantId))
                {
                    int? SessionTenantId = AbpSession.TenantId;
                    if (SessionTenantId.HasValue)
                    {
                        TenantId = SessionTenantId.ToString();
                    }
                }
                else
                {
                    TenantId = utility.DecryptString(Model.EncryptedTenantId);
                }

                if (string.IsNullOrEmpty(TenantId))
                {
                    throw new AbpAuthorizationException("Tenant user is unauthorized");
                }


                using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
                {
                    await _connectionUtility.EnsureConnectionOpenAsync();

                    IEnumerable<CategoryDataDto> Categories = await _db.QueryAsync<CategoryDataDto>("ups_GetCategoryGroupData", new
                    {
                        TenantId = Convert.ToInt32(TenantId),
                        SearchText = Model.SearchText
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    CategoryListDistinct = Categories.ToList();

                    CategoryListDistinct = Categories.GroupBy(g => g.CategoryId).Select(x => x.FirstOrDefault()).Distinct().ToList();

                    var list = String.Join(",", CategoryListDistinct);
                    CategoryFinalList = (from CategoryGroupMaster in CategoryListDistinct
                                         select new CategoryCustomDataDto
                                         {
                                             CategoryId = CategoryGroupMaster.CategoryId,
                                             CategoryTitle = CategoryGroupMaster.CategoryTitle,
                                             IsActive = CategoryGroupMaster.IsActive,
                                             GroupImageUrl = CategoryGroupMaster.GroupImageUrl,
                                             CategoryList = (from item in Categories.ToList()
                                                             where item.CategoryId == CategoryGroupMaster.CategoryId
                                                             select new CategorySubModel
                                                             {
                                                                 SubCategoryMasterId = item.SubCategoryMasterId,
                                                                 SubCategoryTitle = item.SubCategoryTitle,
                                                                 SubCategoryImageObj = new ProductImageType { Ext = item.SubCategoryExt, Name = item.SubcategoryImageName, Type = item.SubCategoryImageType, Url = item.SubCategoryImageUrl, Size = item.SubCategoryImageSize, FileName = item.SubcategoryImageName }
                                                             }).OrderBy(i => i.SubCategoryTitle).GroupBy(g => g.SubCategoryMasterId).Select(x => x.FirstOrDefault()).ToList(),
                                             ImagePath = CategoryGroupMaster.GroupImageUrl,
                                             ImageObj = new ProductImageType { Ext = CategoryGroupMaster.Ext, Name = CategoryGroupMaster.Name, Type = CategoryGroupMaster.Type, Url = CategoryGroupMaster.Url, Size = CategoryGroupMaster.Size, FileName = CategoryGroupMaster.Name }

                                         }).AsQueryable();
                }
            }

            catch (Exception ex)
            {
            }
            CategoryData = await ApplyDtoSorting(CategoryFinalList.ToList(), Model);
            return CategoryData;
        }

        public async Task<List<CategoryGroupMaster>> GetAllCategoryMasterGroups()
        {
            Utility utility = new Utility();
            string TenantId = string.Empty;
            List<CategoryGroupMaster> CategoryGroup = new List<CategoryGroupMaster>();

            try
            {
                int? SessionTenantId = AbpSession.TenantId;
                if (SessionTenantId.HasValue)
                {
                    TenantId = SessionTenantId.ToString();
                }

                await _connectionUtility.EnsureConnectionOpenAsync();

                IEnumerable<CategoryGroupMaster> CategoryList = await _db.QueryAsync<CategoryGroupMaster>("usp_GetAllCategoryMasterGroups", new
                {
                    TenantId = TenantId

                }, commandType: System.Data.CommandType.StoredProcedure);

                CategoryGroup = CategoryList.ToList();

            }
            catch (Exception ex)
            {

            }
            return CategoryGroup;
        }




        #region sorting 

        private async Task<List<CategoryCustomDataDto>> ApplyDtoSorting(List<CategoryCustomDataDto> query, GroupRequestDto input)
        {
            if (input.Sorting.HasValue)
            {
                switch (input.Sorting)
                {
                    case 1:
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.CategoryId).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CategoryId).ToList();
                        }

                        break;
                    case 2://
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.CategoryTitle).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CategoryTitle).ToList();
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.CategoryTitle).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CategoryTitle).ToList();
                        }
                        break;
                    case 4://SequenceNumber
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.SequenceNumber).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.SequenceNumber).ToList();
                        }
                        break;
                    default:
                        {
                            query = query.OrderBy(x => x.CategoryTitle).ToList();
                        }
                        break;
                }

            }
            else
            {
                // query = query.OrderBy(x => x.CategoryTitle).ToList();
                query = query.OrderByDescending(x => x.CategoryId).ToList();
            }
            return query;
        }
        #endregion

        public async Task<CategoryGroupDto> CreateAsync(CategoryGroupDto Model)
        {
            CategoryGroupMaster Group = new CategoryGroupMaster();
            CategoryGroupDto Response = new CategoryGroupDto();
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            bool IsNameExists = false;
            if (Model != null)
            {
                var isExists = _categoryGroupRepository.GetAllList(i => i.GroupTitle.ToLower() == Model.GroupTitle.ToLower()).FirstOrDefault();
                if (isExists == null)
                {

                    Group = ObjectMapper.Map<CategoryGroupMaster>(Model);
                    Response = ObjectMapper.Map<CategoryGroupDto>(Group);

                    if (Model.ImageObj != null && !string.IsNullOrEmpty(Model.ImageObj.FileName))
                    {

                        string ImageLocation = AzureStorageUrl + folderPath + Model.ImageObj.FileName;

                        Group.GroupImageUrl = ImageLocation;
                        Group.ImageName = Model.ImageName;
                        Group.Ext = Model.ImageObj.Ext;
                        Group.Url = ImageLocation;
                        Group.Size = Model.ImageObj.Size;
                        Group.Type = Model.ImageObj.Type;
                        Group.Name = Model.ImageObj.Name;
                    }

                    long Id = await _categoryGroupRepository.InsertAndGetIdAsync(Group);

                    if (Model.CategoryList != null)
                    {
                        foreach (var coll in Model.CategoryList)
                        {
                            var IsExists = _categoryGroupsRepository.GetAllList(i => i.CategoryMasterId == coll.Id && i.CategoryGroupId == Id).FirstOrDefault();
                            if (IsExists == null)
                            {
                                CategoryGroups category = new CategoryGroups();
                                category.CategoryGroupId = Id;
                                category.CategoryMasterId = coll.Id;
                                category.IsActive = true;
                                await _categoryGroupsRepository.InsertAsync(category);
                            }
                        }
                    }
                }
                else
                {
                    IsNameExists = true;
                }
            }
            if (IsNameExists)
            {
                throw new UserFriendlyException(L("CategoryAlreadyExists"), L("CategoryAlreadyExists"));
            }
            else
            {
                return Response;
            }
        }

        public async Task<CategoryGroupDto> UpdateAsync(CategoryGroupDto Model)
        {
            bool IsSuccess = false;
            try
            {
                int TenantId = AbpSession.TenantId.Value;
                var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
                string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
                var Group = _categoryGroupRepository.GetAllList(i => i.Id == Model.Id).FirstOrDefault();
              
                if (Group != null)
                {
                    var TitleExists = _categoryGroupRepository.GetAllList(i =>i.GroupTitle.ToLower().Trim() == Model.GroupTitle.ToLower().Trim() && i.Id != Model.Id).FirstOrDefault();
                    if (TitleExists == null)
                    {

                        if (Model.ImageObj != null && !string.IsNullOrEmpty(Model.ImageObj.FileName))
                        {

                            string ImageLocation = AzureStorageUrl + folderPath + Model.ImageObj.FileName;

                            Group.GroupImageUrl = ImageLocation;
                            Group.ImageName = Model.ImageName;
                            Group.Ext = Model.ImageObj.Ext;
                            Group.Url = ImageLocation;
                            Group.Size = Model.ImageObj.Size;
                            Group.Type = Model.ImageObj.Type;
                            Group.Name = Model.ImageObj.Name;
                        }

                        Group.GroupTitle = Model.GroupTitle;
                        await _categoryGroupRepository.UpdateAsync(Group);

                        //if (Model.CategoryList != null)
                        //{
                        //    var ExistingData = _categoryGroupsRepository.GetAllList(i => i.CategoryGroupId == Group.Id);
                        //    var OldData = Model.CategoryList.Where(i => i.CategoryGroupId > 0);
                        //    var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.CategoryGroupId == p.Id)).ToList();

                        //    if (DeletedData.Count > 0)
                        //    {
                        //        await DeleteCategoryGroups(DeletedData, AbpSession.TenantId.Value);
                        //    }

                        //    foreach (var coll in Model.CategoryList)
                        //    {
                        //        var IsExists = _categoryGroupsRepository.GetAllList(i => i.CategoryMasterId == coll.Id && i.CategoryGroupId == Group.Id).FirstOrDefault();
                        //        if (IsExists == null)
                        //        {
                        //            CategoryGroups category = new CategoryGroups();
                        //            category.CategoryGroupId = Group.Id;
                        //            category.CategoryMasterId = coll.Id;
                        //            category.IsActive = true;
                        //            await _categoryGroupsRepository.InsertAsync(category);
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    var IsExists = _categoryGroupsRepository.GetAllList(i => i.CategoryGroupId == Group.Id).ToList();
                        //    if (IsExists.Count > 0)
                        //    {
                        //        await DeleteCategoryGroups(IsExists, AbpSession.TenantId.Value);
                        //    }
                        //}
                    }
                    else
                    {
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (IsSuccess)
            {
                throw new UserFriendlyException(L("CategoryAlreadyExists"), L("CategoryAlreadyExists"));
            }
            else
            {
                return Model;
            }
        }

        private async Task<bool> DeleteCategoryGroups(List<CategoryGroups> CategoryGroups, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _categoryGroupsRepository.BulkDelete(CategoryGroups);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }



        public async Task<CategoryGroupDto> GetSubCategoriesByCategoryId(CategoryResultRequestDto input)
        {
            Utility utility = new Utility();
            string TenantId = string.Empty;
            CategoryGroupDto Response = new CategoryGroupDto();
            if (string.IsNullOrEmpty(input.EncryptedTenantId))
            {
                int? SessionTenantId = AbpSession.TenantId;
                if (SessionTenantId.HasValue)
                {
                    TenantId = SessionTenantId.ToString();
                }
            }
            else
            {
                TenantId = utility.DecryptString(input.EncryptedTenantId);
            }
            if (string.IsNullOrEmpty(TenantId))
            {
                throw new AbpAuthorizationException("Tenant user is unauthorized");
            }

            using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
            {
                var Group = _categoryGroupRepository.GetAllList(i => i.Id == input.GroupId).FirstOrDefault();

                if (Group != null)
                {
                    Response.Id = Group.Id;
                    Response.GroupTitle = Group.GroupTitle;
                    Response.GroupImageUrl = Group.GroupImageUrl;
                    var Data = (from item in _categoryMasterRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.SearchText), x => x.CategoryTitle.ToLower().Trim().Contains(input.SearchText.ToLower().Trim()))
                                             join collection in _categoryGroupsRepository.GetAll() on item.Id equals collection.CategoryMasterId
                                             where collection.CategoryGroupId == Group.Id

                                             select new CategoryGroupModel
                                             {
                                                 Id = item.Id,
                                                 CategoryGroupId = collection.Id,
                                                 CategoryTitle = item.CategoryTitle,
                                                 ImageObj = new ProductImageType { Ext = item.Ext, Name = item.Name, Size = item.Size, Url = item.Url, Type = item.Type },
                                                 SubSubCategory = (from data in _repositorySubCategoryMaster.GetAll()
                                                                   join category in _repositorySubSubCategory.GetAll() on data.Id equals category.SubCategoryId
                                                                   where category.CategoryId == item.Id
                                                                   select new CategorySubSubModel
                                                                   {
                                                                       Id= data.Id,
                                                                       SubSubCategoryMasterId = data.Id,
                                                                       SubCategoryImageObj = new ProductImageType { Ext = data.Ext, ImagePath = data.ImageUrl, Name = data.Name, Size = data.Size, Url = data.Url, Type = data.Type },
                                                                       SubSubCategoryTitle = data.Title
                                                                   }).OrderBy(i => i.SubSubCategoryTitle).ToList()

                                             }).OrderBy(i => i.CategoryTitle).ToList();

                    Response.CategoryList = Data.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).ToList();
                    Response.ImagePath = Group.GroupImageUrl;
                    if (Response.ImagePath != null)
                    {
                        Response.ImageObj = new ProductImageType { Ext = Group.Ext, Name = Group.Name, Type = Group.Type, Url = Group.Url, Size = Group.Size, FileName = Group.ImageName };
                    }
                    else
                    {
                        Response.ImageObj = null;
                    }
                }
            }
            return Response;
        }


        public async Task<List<CategoryGroupDataDto>> GetCategoryByCategoryGroupId(long Id)
        {
            List<CategoryGroupDataDto> Response = new List<CategoryGroupDataDto>();
            try
            {
                List<CategoryGroupDataDto> CategoryFinalList = new List<CategoryGroupDataDto>();
                List<AllCategoryTypeData> list = new List<AllCategoryTypeData>();
                int TenantId = AbpSession.TenantId.Value;

                IEnumerable<AllCategoryTypeData> CategoryList = await _db.QueryAsync<AllCategoryTypeData>("usp_GetCategoryListByCategoryGroupId", new
                {
                    tenantId = TenantId,
                    categoryId = Id

                }, commandType: System.Data.CommandType.StoredProcedure);

                if (CategoryList != null)
                {

                    CategoryFinalList = (from CategoryGroupMaster in CategoryList
                                         select new CategoryGroupDataDto
                                         {
                                             CategoryId = CategoryGroupMaster.CategoryId,
                                             CategoryTitle = CategoryGroupMaster.CategoryTitle,
                                             CategoryImageUrl = CategoryGroupMaster.ImageUrl,
                                             CategoryList = (from mster in CategoryList
                                                             select new CategoryAllModel
                                                             {
                                                                 SubCategoryId = mster.SubCategoryId,
                                                                 SubCategoryTitle = mster.SubCategoryTitle,
                                                                 SubSubCategoryId = mster.SubSubCategoryId,
                                                                 SubSubCategoryTitle = mster.SubSubCategoryTitle
                                                             }).ToList(),
                                             ImagePath = CategoryGroupMaster.ImageUrl,
                                             ImageObj = new ProductImageType { Ext = CategoryGroupMaster.Ext, Name = CategoryGroupMaster.Name, Url = CategoryGroupMaster.Url, Size = CategoryGroupMaster.Size, FileName = CategoryGroupMaster.Name }

                                         }).AsQueryable().ToList();

                }
                Response = CategoryFinalList;
            }
            catch (Exception ex)
            { }


            return Response;
        }

        private async Task<bool> DeleteGroupCategory(List<CategoryGroups> CategoryGroups, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _categoryGroupsRepository.BulkDelete(CategoryGroups);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        public async Task DeleteGroupById(long Id)
        {
            try
            {
                var Group = _categoryGroupRepository.GetAllList(i => i.Id == Id).FirstOrDefault();
                //long GroupId = _categoryGroupRepository.GetAllList(i => i.GroupTitle.ToLower() == "Uncategorizedgroup").Select(i => i.Id).FirstOrDefault();

                var CategoryList = new List<CategoryGroups>();


                // update all collections with unassigned category if its deleted
                if (Group != null)
                {
                    CategoryList = _categoryGroupsRepository.GetAllList(i => i.CategoryGroupId == Group.Id).ToList();
                    if (CategoryList.Count > 0)
                    {
                       // CategoryList = CategoryList.Select(w => { w.CategoryGroupId = GroupId; return w; }).ToList();
                        await DeleteGroupCategory(CategoryList, AbpSession.TenantId.Value);
                    }

                    var Assignments = _repositoryAssignedCategoryMaster.GetAllList(i => i.CategoryId == Id).ToList();
                    if (Assignments.Count > 0)
                    {
                        _repositoryAssignedCategoryMaster.BulkDelete(Assignments);
                    }

                    // delete category from master
                    await _categoryGroupRepository.DeleteAsync(Group);
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
