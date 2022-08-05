using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.CategoryCollection;
using CaznerMarketplaceBackendApp.Connections;
using CaznerMarketplaceBackendApp.MultiTenancy;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using Abp.Authorization;
using System.Linq;
using FimApp.EntityFrameworkCore;
using Dapper;
using CaznerMarketplaceBackendApp.Product.Dto;
using static CaznerMarketplaceBackendApp.AppConsts;

namespace CaznerMarketplaceBackendApp.ThemeBuilder
{
    public class CategoryHomePageAppService : CaznerMarketplaceBackendAppAppServiceBase, ICategoryHomePageAppService
    {
        private readonly IRepository<CategoryHomePage, long> _masterRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly DbConnectionUtility _connectionUtility;
        string AzureProductFolder = string.Empty;
        string AzureStorageUrl = string.Empty; 
        private IConfiguration _configuration;
        private IDbConnection _db;
        private readonly IRepository<Tenant, int> _tenantManager;
        public CategoryHomePageAppService(
            IConfiguration configuration,
            IRepository<CategoryHomePage, long> masterRepository, IRepository<Tenant, int> tenantManager,
            IUnitOfWorkManager unitOfWorkManager)


        {
            _masterRepository = masterRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = configuration;
            _tenantManager = tenantManager;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];

        }
        public async Task<CategoryHomePageDto> CreateCategoryHomePage(CategoryHomePageDto input)
        {
            CategoryHomePageDto response = new CategoryHomePageDto();
            
            string TenantId = string.Empty;
            Utility utility = new Utility();
            string videoUrl = string.Empty;
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
            try
            {
                if (input.CategoryId != null)
                {
                    var Tenant = await _tenantManager.FirstOrDefaultAsync(Convert.ToInt32(TenantId));

                    if (input.CategoryId != null & input.CategoryId.Count <= 11)
                    {
                        foreach (var listData in input.CategoryId)
                        {
                            CategoryHomePage master = new CategoryHomePage();
                            master.IsActive = input.IsActive;
                            master.TenantId = Convert.ToInt32(TenantId);
                            master.CategoryId = listData;
                            master.SequenceNumber = input.SequenceNumber;
                            await _masterRepository.InsertAsync(master);
                        }
                    }
                    else
                    {
                        throw new AbpAuthorizationException("Maximum 11 categories can be selected");
                    }
                }
            }
            catch (Exception ex)
            {

            }
       
            return response;
        }


        public async Task UpdateCategoryHomePage(CategoryHomePageDto input)
        {
            // ProductStockLocation
            if (input.CategoryId != null & input.CategoryId.Count <= 11) {
                if (input.CategoryId != null)
                {

                    var ExistingData = _masterRepository.GetAll().ToList();

                    if (ExistingData.Count >0)
                    {
                        await DeleteCategoryHomepage(ExistingData.ToList(), AbpSession.TenantId.Value);
                    }

                    foreach (var category in input.CategoryId)
                    {
                        if (category > 0)
                        {
                            CategoryHomePage homePage = new CategoryHomePage();
                            homePage.CategoryId = category;
                            homePage.IsActive = input.IsActive;
                            homePage.SequenceNumber = input.SequenceNumber;
                            await _masterRepository.InsertAsync(homePage);
                        }

                    }
                }
                else
                {
                    var ExistingData = _masterRepository.GetAll().ToList();

                    if (ExistingData.Count>0)
                    {
                        await DeleteCategoryHomepage(ExistingData.ToList(), AbpSession.TenantId.Value);
                    }
                }
            }
            else
            {
                throw new AbpAuthorizationException("Maximum 11 categories can be selected");
            }


        }
        private async Task<bool> DeleteCategoryHomepage(List<CategoryHomePage> category, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _masterRepository.BulkDelete(category);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        public async Task DeleteCategory(int id)
        {
            try
            {
                var home = _masterRepository.GetAllList(i => i.Id == id).FirstOrDefault();
                if (home != null)
                {
                    await _masterRepository.DeleteAsync(home);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public async Task<List<CategoryCustomDataDto>> GetCategoryGroupDataHomePage(GroupRequestDto Model)
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
                    IEnumerable<CategoryDataDto> Categories = await _db.QueryAsync<CategoryDataDto>("ups_GetCategoryHomePageData", new
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
                                                             where item.CategoryId == CategoryGroupMaster.CategoryId & item.SubCategoryMasterId > 0
                                                             select new CategorySubModel
                                                             {
                                                                 SubCategoryMasterId = item.SubCategoryMasterId,
                                                                 SubCategoryTitle = string.IsNullOrEmpty(item.SubCategoryTitle) ? string.Empty : item.SubCategoryTitle,
                                                                 SubCategoryImageObj = new ProductImageType { Ext = item.SubCategoryExt, Name = item.SubcategoryImageName, Type = item.SubCategoryImageType, Url = item.SubCategoryImageUrl, Size = item.SubCategoryImageSize, FileName = item.SubcategoryImageName }
                                                             }).OrderBy(i => i.SubCategoryTitle).GroupBy(g => g.SubCategoryMasterId).Select(x => x.FirstOrDefault()).ToList(),
                                             ImagePath = CategoryGroupMaster.GroupImageUrl,
                                             ImageObj = new ProductImageType { Ext = CategoryGroupMaster.Ext, Name = CategoryGroupMaster.Name, Type = CategoryGroupMaster.Type, Url = CategoryGroupMaster.Url, Size = CategoryGroupMaster.Size, FileName = CategoryGroupMaster.Name },
                                             AssignmentId = CategoryGroupMaster.AssignmentId

                                         }).AsQueryable();
                
            }

            catch (Exception ex)
            {
            }
            CategoryData = await ApplyDtoSorting(CategoryFinalList.ToList(), Model);
            return CategoryData;
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
    }
}
