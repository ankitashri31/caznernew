using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.CategoryCollection;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using CaznerMarketplaceBackendApp.Connections;
using CaznerMarketplaceBackendApp.MultiTenancy;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.ThemeBuilder
{
    public class CollectionHomePageAppService : CaznerMarketplaceBackendAppAppServiceBase, ICollectionHomePageAppService
    {
        private readonly IRepository<CollectionHomePage, long> _masterRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly DbConnectionUtility _connectionUtility;
        string AzureProductFolder = string.Empty;
        string AzureStorageUrl = string.Empty;
        private IConfiguration _configuration;
        private IDbConnection _db;
        private readonly IRepository<Tenant, int> _tenantManager;
        public CollectionHomePageAppService(
            IConfiguration configuration,
            IRepository<CollectionHomePage, long> masterRepository)

        {
            _masterRepository = masterRepository;
            _configuration = configuration;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];

        }
        public async Task CreateCollection(CollectionHomePageDto input)
        {
            CollectionHomePageDto response = new CollectionHomePageDto();
            CollectionHomePage master = new CollectionHomePage();
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
            try {
                if (input.CollectionId != null)
                {
                    var Tenant = await _tenantManager.FirstOrDefaultAsync(Convert.ToInt32(TenantId));
                    string folderPath = $"{Tenant.TenancyName + "/" + AzureProductFolder}";
                    var collectionExists = _masterRepository.GetAllList(x => x.CollectionId == input.CollectionId).FirstOrDefault();
                    if (collectionExists == null)
                    {
                        if (input.VideoFileName != null) 
                        {
                            videoUrl = AzureStorageUrl + folderPath + input.VideoFileName;
                        }
                        master.CollectionId = input.CollectionId;
                        master.SequenceNumber = input.SequenceNumber;
                        master.NumberOfProducts = input.NumberOfProducts;
                        master.IsActive = input.IsActive;
                        master.TenantId = Convert.ToInt32(TenantId);
                        master.VideoUrl = videoUrl;
                        long collectionId = await _masterRepository.InsertAndGetIdAsync(master);

                    }
                    else
                    {
                        if (input.VideoFileName != null)
                        {
                            videoUrl = AzureStorageUrl + folderPath + input.VideoFileName;
                        }
                        
                        collectionExists.TenantId = Convert.ToInt32(TenantId);
                        collectionExists.NumberOfProducts = input.NumberOfProducts;
                        collectionExists.VideoUrl = videoUrl;
                        collectionExists.SequenceNumber = input.SequenceNumber;
                        collectionExists.IsActive = input.IsActive;
                        await _masterRepository.UpdateAsync(collectionExists);

                    }

                }
            
            }
            catch (Exception ex)
            { 
            
            }

        }
        
            public async Task<List<CollectionRibbonResponseDto>> GetRibbonCollections(CollectionRequestDto Input)
        {

            Utility utility = new Utility();
            string TenantId = string.Empty;
            IQueryable<CollectionRibbonResponseDto> CollectionFinalList = Enumerable.Empty<CollectionRibbonResponseDto>().AsQueryable();
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
                //  using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
                // {

                // await _connectionUtility.EnsureConnectionOpenAsync();
               
                    IEnumerable<CollectionRibbonResponseDto> collect = await _db.QueryAsync<CollectionRibbonResponseDto>("GetCollectionRibbonList", new
                    {
                        TenantId = Convert.ToInt32(TenantId)
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    CollectionFinalList = (from col in collect
                                           select new CollectionRibbonResponseDto
                                           {

                                               CollectionName = col.CollectionName,
                                               Id = col.Id
                                           }).AsQueryable();
            }
            catch (Exception ex)
            { }
            return CollectionFinalList.ToList();
        }
        public  async Task<List<CollectionResponseProductDto>> GetAll(CollectionRequestDto Input)
        {
            
            Utility utility = new Utility();
            string TenantId = string.Empty;
            List<CollectionHomePageDto> collection = new List<CollectionHomePageDto>();
            List<CollectionResponseDto> collectionDistinct = new List<CollectionResponseDto>(); 
            IQueryable<CollectionResponseProductDto> CollectionFinalList = Enumerable.Empty<CollectionResponseProductDto>().AsQueryable();
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
              //  using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
               // {

                   // await _connectionUtility.EnsureConnectionOpenAsync();
                   var NumberOfProduct =  _masterRepository.GetAllList(i => i.CollectionId == Input.CollectionId).Select(i => i.NumberOfProducts).FirstOrDefault();
                if (NumberOfProduct != null)
                {
                    IEnumerable<CollectionResponseDto> collect = await _db.QueryAsync<CollectionResponseDto>("Usp_GetProductBycollectionHomePage", new
                    {
                        TenantId = Convert.ToInt32(TenantId),
                        collection = Input.CollectionId,
                        NumberOfProduct = NumberOfProduct
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    collectionDistinct = collect.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).Distinct().ToList();
                    CollectionFinalList = (from col in collectionDistinct
                                           select new CollectionResponseProductDto
                                           {

                                               CollectionName = col.CollectionName,
                                               CollectionId = col.CollectionId,
                                               ProductList = (from item in collectionDistinct
                                                              select new ProductColListDto
                                                              {
                                                                  Id = item.Id,
                                                                  ProductSKU = item.ProductSKU,
                                                                  ProductTitle = item.ProductTitle,
                                                                  DefaultImagePath = item.DefaultImagePath
                                                              }).ToList(),
                                               VideoUrl = col.VideoUrl
                                           }).AsQueryable();
                }
                    
              //  }
            }
            catch (Exception ex)
            { }
            return CollectionFinalList.ToList();
        }

        public async Task<List<CollectionResponseProductDto>> GetAllCollectionHomePage(CollectionRequestDto Input)
        {

            Utility utility = new Utility();
            string TenantId = string.Empty;
            List<CollectionHomePageDto> collection = new List<CollectionHomePageDto>();
            List<CollectionResponseDto> collectionDistinct = new List<CollectionResponseDto>();
            IQueryable<CollectionResponseProductDto> CollectionFinalList = Enumerable.Empty<CollectionResponseProductDto>().AsQueryable();
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
                //  using (_unitOfWorkManager.Current.SetTenantId(Convert.ToInt32(TenantId)))
                // {

                // await _connectionUtility.EnsureConnectionOpenAsync();
               // var NumberOfProduct = _masterRepository.GetAllList(i => i.CollectionId == Input.CollectionId).Select(i => i.NumberOfProducts).FirstOrDefault();
               // if (NumberOfProduct != null)
               // {
                    IEnumerable<CollectionResponseDto> collect = await _db.QueryAsync<CollectionResponseDto>("GetAllCollectionHomePage", new
                    {
                        TenantId = Convert.ToInt32(TenantId)
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    collectionDistinct = collect.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).Distinct().ToList();
                    CollectionFinalList = (from col in collectionDistinct
                                           select new CollectionResponseProductDto
                                           {

                                               CollectionName = col.CollectionName,
                                               CollectionId = col.CollectionId,
                                               ProductList = (from item in collectionDistinct
                                                              select new ProductColListDto
                                                              {
                                                                  Id = item.Id,
                                                                  ProductSKU = item.ProductSKU,
                                                                  ProductTitle = item.ProductTitle,
                                                                  DefaultImagePath = item.DefaultImagePath
                                                              }).ToList(),
                                               VideoUrl = col.VideoUrl
                                           }).AsQueryable();
               // }

                //  }
            }
            catch (Exception ex)
            { }
            return CollectionFinalList.ToList();
        }
        private async Task<bool> DeleteCollectionHomePage(int Id)
        {
            var collectionExists = _masterRepository.GetAllList(x => x.CollectionId == Id).FirstOrDefault();
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(AbpSession.TenantId))
                {
                    _masterRepository.DeleteAsync(collectionExists);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
    }
}
