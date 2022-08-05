using Abp.Application.Services;
using System;
using System.Collections.Generic;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using System.Linq;
using static CaznerMarketplaceBackendApp.AppConsts;
using Abp.Authorization;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using FimApp.EntityFrameworkCore;
using CaznerMarketplaceBackendApp.Product.Dto;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.MultiTenancy;
using CaznerMarketplaceBackendApp.AzureBlobStorage;
using Microsoft.Data.SqlClient;
using System.Data;
using CaznerMarketplaceBackendApp.Connections;
using Dapper;
using CaznerMarketplaceBackendApp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FastMember;
using Abp.UI;

namespace CaznerMarketplaceBackendApp.CategoryCollection
{
    public class CategoryCollectionAppService :
         AsyncCrudAppService<CategoryCollections, CategoryCollectionsDto, long, CategoryCollectionsResultRequestDto, CreateCategoryCollectionDto, CategoryCollectionsDto>, ICategoryCollectionAppService
    {
        private readonly IRepository<CategoryCollections, long> _repository;
        private readonly IRepository<CollectionMaster, long> _repositoryCollectionMaster;
        private readonly IRepository<CategoryMaster, long> _repositoryCategoryMaster;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<CollectionCalculations, long> _repositoryCalculations;
        private readonly IRepository<CalculationTypeTags, long> _repositoryCalculationTypeTags;
        private readonly IRepository<CalculationTypes, long> _repositoryCalculationType;
        private readonly IRepository<CalculationTypeAttributes, long> _repositoryCalculationTypeAttributes;
        string FolderName = string.Empty;
        string LogoFolderName = string.Empty;
        string CurrentWebsiteUrl = string.Empty;
        private IConfiguration _configuration;
        string AzureProductFolder = string.Empty;
        string AzureStorageUrl = string.Empty;
        private CaznerMarketplaceBackendAppDbContext _dbContext;
        private readonly IRepository<Tenant, int> _tenantManager;
        private readonly DbConnectionUtility _connectionUtility;
        private IDbConnection _db;
        private readonly IRepository<ProductAssignedCollections, long> _repositoryProductAssignedCollections;

        public CategoryCollectionAppService(IRepository<CategoryCollections, long> repository, IUnitOfWorkManager unitOfWorkManager,
            IConfiguration configuration,
            IRepository<CollectionMaster, long> repositoryCollectionMaster, IRepository<CategoryMaster, long> repositoryCategoryMaster,
            IRepository<CollectionCalculations, long> repositoryCalculations,
            IRepository<CalculationTypeTags, long> repositoryCalculationTypeTags,
            IRepository<CalculationTypes, long> repositoryCalculationType,
            IRepository<CalculationTypeAttributes, long> repositoryCalculationTypeAttributes,
            DbConnectionUtility connectionUtility, IRepository<Tenant, int> tenantManager, CaznerMarketplaceBackendAppDbContext dbContext,
            IRepository<ProductAssignedCollections, long> repositoryProductAssignedCollections) : base(repository)
        {
            _repository = repository;
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = configuration;
            FolderName = _configuration["FileUpload:FolderName"];
            LogoFolderName = _configuration["FileUpload:FrontEndLogoFolderName"];
            CurrentWebsiteUrl = _configuration["FileUpload:WebsireDomainPath"];
            _repositoryCollectionMaster = repositoryCollectionMaster;
            _repositoryCategoryMaster = repositoryCategoryMaster;
            _repositoryCalculations = repositoryCalculations;
            _repositoryCalculationTypeTags = repositoryCalculationTypeTags;
            _repositoryCalculationType = repositoryCalculationType;
            _repositoryCalculationTypeAttributes = repositoryCalculationTypeAttributes;
            _tenantManager = tenantManager;
            AzureProductFolder = _configuration["FileUpload:AzureProductFolder"];
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];
            _connectionUtility = connectionUtility;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);
            _dbContext = dbContext;
            _repositoryProductAssignedCollections = repositoryProductAssignedCollections;
        }

        public override async Task<PagedResultDto<CategoryCollectionsDto>> GetAllAsync(CategoryCollectionsResultRequestDto Input)
        {
            Utility utility = new Utility();
            int TotalCount = 0;
            string TenantId = string.Empty;
            List<CategoryCollectionsDto> CategoryList = new List<CategoryCollectionsDto>();
            List<CategoryCollectionsDto> CollectionListDistinct = new List<CategoryCollectionsDto>();
            List<CategoryCollectionsDto> CollectionFinalList = new List<CategoryCollectionsDto>();
            List<CategoryCollectionsDto> CollectionsList = new List<CategoryCollectionsDto>();
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
                    //dynamic category;
                    //if (Input.CategoryId != null)
                    //{
                    //    category = string.Join(",", Input.CategoryId);
                    //}
                    //else
                    //{
                    //    category = string.Empty;
                    //}
                    await _connectionUtility.EnsureConnectionOpenAsync();

                    IEnumerable<CategoryCollectionsDto> collection = await _db.QueryAsync<CategoryCollectionsDto>("usp_CategoryCollectionsGetAll", new
                    {
                        TenantId = Convert.ToInt32(TenantId),
                        SearchText = Input.SearchText
                    }, commandType: System.Data.CommandType.StoredProcedure);


                    CollectionListDistinct = collection.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).Distinct().ToList();

                    CollectionFinalList = (from collectionMaster in CollectionListDistinct
                                           select new CategoryCollectionsDto
                                           {
                                               Id = collectionMaster.Id,
                                               CollectionName = collectionMaster.CollectionName,
                                               ImagePath = collectionMaster.ImagePath,
                                               CreationTime = collectionMaster.CreationTime,
                                               ImageObj = new ProductImageType { Ext = collectionMaster.Ext, Name = collectionMaster.Name, Type = collectionMaster.Type, Url = collectionMaster.Url, Size = collectionMaster.Size, FileName = collectionMaster.Name }

                                           }).ToList();

                    TotalCount = CollectionFinalList.Count();
                    CollectionsList = await ApplyDtoSorting(CollectionFinalList, Input);
                }
            }
            catch (System.Exception ex)
            {
                //throw;
            }
            return new PagedResultDto<CategoryCollectionsDto>(TotalCount, ObjectMapper.Map<List<CategoryCollectionsDto>>(CollectionsList));
        }


        public async Task<CategoryCollectionsCustomDto> GetCategoryCollectionsList(CategoryCollectionsResultRequestDto Input)
        {
            Utility utility = new Utility();
            int TotalCount = 0;
            string TenantId = string.Empty;

            List<CategoryCollectionsDto> CategoryList = new List<CategoryCollectionsDto>();
            List<CategoryCollectionsDto> CategoryListDistinct = new List<CategoryCollectionsDto>();
            List<CategoryCollectionsDto> CategoryFinalList = new List<CategoryCollectionsDto>();
            List<CategoryCollectionsDto> CategoryCollectionList = new List<CategoryCollectionsDto>();
            bool IsLoadMore = false;
            CategoryCollectionsCustomDto response = new CategoryCollectionsCustomDto();
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

                    IEnumerable<CategoryCollectionsDto> collection = await (_db.QueryAsync<CategoryCollectionsDto>("usp_GetCategoryCollectionsList", new
                    {
                        TenantId = Convert.ToInt32(TenantId),
                        SearchText = Input.SearchText

                    }, commandType: System.Data.CommandType.StoredProcedure));


                    CategoryListDistinct = collection.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).Distinct().ToList();
                    CategoryFinalList = (from Categorymaster in CategoryListDistinct
                                         select new CategoryCollectionsDto
                                         {
                                             Id = Categorymaster.Id,
                                             CollectionName = Categorymaster.CollectionName,
                                             ImagePath = Categorymaster.ImagePath,
                                             CreationTime = Categorymaster.CreationTime,
                                             ImageObj = new ProductImageType { Ext = Categorymaster.Ext, Name = Categorymaster.Name, Type = Categorymaster.Type, Url = Categorymaster.Url, Size = Categorymaster.Size, FileName = Categorymaster.Name }
                                            
                                         }).ToList();



                    TotalCount = CategoryFinalList.Count();
                    CategoryCollectionList = await ApplyDtoSorting(CategoryFinalList, Input);
                    CategoryCollectionList = CategoryCollectionList.Skip(Input.SkipCount).Take(Input.MaxResultCount).ToList();

                    if (Input.SkipCount + Input.MaxResultCount < TotalCount)
                    {
                        IsLoadMore = true;
                    }

                    response.SkipCount = Input.SkipCount;
                    response.items = ObjectMapper.Map<List<CategoryCollectionsDto>>(CategoryCollectionList);
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

        public override async Task<CategoryCollectionsDto> CreateAsync(CreateCategoryCollectionDto Model)
        {
            bool isExists = false;
            CategoryCollectionsDto response = new CategoryCollectionsDto();
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
         
            try
            {
                var IsExists = _repositoryCollectionMaster.GetAllList(i => i.CollectionName.ToLower().Trim() == Model.CollectionName.ToLower().Trim()).FirstOrDefault();
                long Id = 0;
                if (IsExists == null)
                {
                    CollectionMaster master = new CollectionMaster();
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
                    master.CollectionName = Model.CollectionName;
                    master.IsManualCalculation = Model.IsManualCalculation;
                    master.IsMatchAnyCondition = Model.IsMatchAnyCondition;
                    master.SeoDescription = Model.SeoDescription;
                    master.SeoPageTitle = Model.SeoPageTitle;
                    master.SeoUrl = Model.SeoUrl;
                    master.IsSeoEnabled = Model.IsSeoEnabled;
                    Id = await _repositoryCollectionMaster.InsertAndGetIdAsync(master);

                    //calculatons

                    if (Model.CalculationsDto.Count > 0)
                    {
                        foreach (var calculation in Model.CalculationsDto)
                        {
                            CollectionCalculations calculations = new CollectionCalculations();
                            calculations.TenantId = AbpSession.TenantId.Value;
                            calculations.TypeId = calculation.TypeId;
                            calculations.TypeMatchId = calculation.TypeMatchId;
                            calculations.EntityValue = calculation.EntityValue;
                            calculations.CollectionId = Id;
                            calculations.IsActive = true;
                            await _repositoryCalculations.InsertAsync(calculations);

                        }
                    }
                    response.Id = Id;
                    response.CollectionName = Model.CollectionName;
                    isExists = false;
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
                throw new UserFriendlyException(L("CollectionAlreadyExists"), L("CollectionAlreadyExists"));
            }
            else
            {
                return response;
            }
        }

        public override async Task<CategoryCollectionsDto> UpdateAsync(CategoryCollectionsDto Model)
        {
            bool IsNameExists = false;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            try
            {
                var IsExists = _repositoryCollectionMaster.GetAllList(i => i.CollectionName.ToLower().Trim() == Model.CollectionName.ToLower().Trim() && i.Id != Model.Id).FirstOrDefault();
                if (IsExists == null)
                {
                    var Category = _repositoryCollectionMaster.GetAllList(i => i.Id == Model.Id).FirstOrDefault();

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

                    Category.CollectionName = Model.CollectionName;
                    Category.IsManualCalculation = Model.IsManualCalculation;
                    Category.IsMatchAnyCondition = Model.IsMatchAnyCondition;
                    Category.SeoDescription = Model.SeoDescription;
                    Category.SeoPageTitle = Model.SeoPageTitle;
                    Category.SeoUrl = Model.SeoUrl;
                    Category.IsSeoEnabled = Model.IsSeoEnabled;

                    await _repositoryCollectionMaster.UpdateAsync(Category);

                    //if (Category != null && Model.CategoryIds != null)
                    //{
                    //    var ExistingData = _repository.GetAllList(i => i.CollectionId == Model.Id);
                    //    var OldData = Model.CategoryIds.Where(i => i.CategoryCollectionId > 0);
                    //    var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.CategoryCollectionId == p.Id)).ToList();

                    //    if (DeletedData.Count > 0)
                    //    {
                    //        await DeleteCategoryCollection(DeletedData, AbpSession.TenantId.Value);
                    //    }

                    //    foreach (var item in Model.CategoryIds)
                    //    {
                    //        var ChkCategory = _repository.GetAllList(i => i.CollectionId == Model.Id && i.CategoryId == item.Id).FirstOrDefault();
                    //        if (ChkCategory == null)
                    //        {
                    //            CategoryCollections collection = new CategoryCollections();
                    //            collection.CategoryId = item.Id;
                    //            collection.CollectionId = Model.Id;
                    //            collection.IsActive = true;
                    //            await _repository.InsertAsync(collection);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    var IsDataExists = _repository.GetAllList(i => i.CategoryId == Model.Id).ToList();
                    //    if (IsDataExists.Count > 0)
                    //    {
                    //        await DeleteCategoryCollection(IsDataExists, AbpSession.TenantId.Value);
                    //    }
                    //}

                    // calculations

                    if (Model.Calculations != null)
                    {
                        if (Model.Calculations.Count > 0)
                        {
                            var ExistingCalculationsData = _repositoryCalculations.GetAllList(i => i.CollectionId == Model.Id);
                            var OldData = Model.Calculations.Where(i => i.Id > 0);
                            var DeletedCalculationData = ExistingCalculationsData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();

                            if (DeletedCalculationData.Count > 0)
                            {
                                await DeleteCollectionCalculations(DeletedCalculationData, AbpSession.TenantId.Value);
                            }

                            foreach (var calculation in Model.Calculations)
                            {
                                CollectionCalculations calculations = new CollectionCalculations();
                                calculations.TenantId = AbpSession.TenantId.Value;
                                calculations.TypeId = calculation.TypeId;
                                calculations.TypeMatchId = calculation.TypeMatchId;
                                calculations.EntityValue = calculation.EntityValue;
                                calculations.CollectionId = Model.Id;
                                calculations.IsActive = true;
                                await _repositoryCalculations.InsertAsync(calculations);

                            }
                        }
                        else
                        {
                            var IsCalculationDataExists = _repositoryCalculations.GetAllList(i => i.CollectionId == Model.Id).ToList();
                            if (IsCalculationDataExists.Count > 0)
                            {
                                await DeleteCollectionCalculations(IsCalculationDataExists, AbpSession.TenantId.Value);
                            }
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
                throw new UserFriendlyException(L("CollectionAlreadyExists"), L("CollectionAlreadyExists"));
            }
            else
            {
                return Model;
            }
        }

        public async Task<CategoryCollectionsCustomlistDto> GetCollectionById(CategoryCollectionsResultRequestDto Input)
        {
            int TotalCount = 0;
            bool IsLoadMore = false;
            string str = "";
            IQueryable<CategoryCollectionsDto> CollectionFinalList = Enumerable.Empty<CategoryCollectionsDto>().AsQueryable();
            CategoryCollectionsDto List = new CategoryCollectionsDto();
            List<CategoryCollectionsDto> CollectionListDistinct = new List<CategoryCollectionsDto>();
            CategoryCollectionsCustomlistDto Collections = new CategoryCollectionsCustomlistDto();
            Utility utility = new Utility();
            if (Input.CollectionId != null)
            {
                str = String.Join(",", Input.CollectionId);
            }

            string TenantId = string.Empty;
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

                    IEnumerable<CategoryCollectionsDto> CollectionData = await _db.QueryAsync<CategoryCollectionsDto>("usp_GetCollectionListById", new
                    {
                        SearchText = Input.SearchText,
                        TenantId = Convert.ToInt32(TenantId),
                        CollectionList = str
                    }, commandType: System.Data.CommandType.StoredProcedure);

                    CollectionListDistinct = CollectionData.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).Distinct().ToList();
                    //Collections = CollectionData.ToList();

                    foreach (var item in CollectionListDistinct)
                    {
                        List.Id = item.Id;
                        List.CollectionName = item.CollectionName;
                        List.ImageObj = new ProductImageType
                        {
                            Ext = item.Ext,
                            Url = item.Url,
                            ImagePath = item.ImagePath,
                            Size = item.Size,
                            Name = item.Name,
                            Type = item.Type
                        };
                        List.IsManualCalculation = item.IsManualCalculation;
                        List.IsMatchAnyCondition = item.IsMatchAnyCondition;
                        List.SeoDescription = item.SeoDescription;
                        List.SeoPageTitle = item.SeoPageTitle;
                        List.SeoUrl = item.SeoUrl;
                        List.IsSeoEnabled = item.IsSeoEnabled;
                        List.Calculations = (from calculation in _repositoryCalculations.GetAll()
                                             where calculation.CollectionId == item.Id
                                             select new CollectionCalculationsDto
                                             {
                                                 Id = calculation.Id,
                                                 TypeId = calculation.TypeId,
                                                 TypeMatchId = calculation.TypeMatchId,
                                                 EntityValue = calculation.EntityValue,
                                                 TypeTitle = calculation.TypeMatchId > 0 ? _repositoryCalculationType.GetAll().Where(i => i.Id == calculation.TypeMatchId).Select(i => i.Type).FirstOrDefault() : "",
                                                 CollectionId = calculation.CollectionId,
                                                 TypeMatchData = new TypeModel
                                                 {
                                                     Id = calculation.TypeMatchId,
                                                     TypeMatchTitle = calculation.TypeMatchId > 0 ? _repositoryCalculationTypeTags.GetAll().Where(i => i.Id == calculation.TypeMatchId).Select(i => i.TypeTitle).FirstOrDefault() : "",
                                                     TypeList = (from type in _repositoryCalculationType.GetAll()
                                                                 select new CalculationTypesDto
                                                                 {
                                                                     Id = type.Id,
                                                                     TypeName = type.Type,
                                                                     IsActive = type.IsActive,
                                                                     IsAssigned = calculation.TypeMatchId > 0 && type.Id > 0 ? _repositoryCalculationTypeAttributes.GetAll().Where(i => i.TypeMatchId == calculation.TypeMatchId && i.TypeId == type.Id).Select(i => i.Id).FirstOrDefault() > 0 ? true : false : false

                                                                 }).ToList()
                                                 },


                                             }).ToList();


                    }


                    Collections.items = List;



                }
            }
            catch (Exception ex)
            {
            }

            return Collections;
        }

        public async Task<CollectionPaginationDto> GetProductListDataByCollectionId(CategoryProductCollectionsResultRequestDto Input)
        {

            string collectionId = "";
            int countData = 0;
            Utility utility = new Utility();
            ProductCollectionDataDto ProductData = new ProductCollectionDataDto();
            ProductCollectionDataDto ProductDataCollection = new ProductCollectionDataDto();
            ProductCollectionDataDto ProductDataList = new ProductCollectionDataDto();
            IQueryable<ProductCollectionDataDto> ProductDataFinal = Enumerable.Empty<ProductCollectionDataDto>().AsQueryable();
            CollectionPaginationDto response = new CollectionPaginationDto();
            string TenantId = string.Empty;
            int limit = 0;
            int limitMannual = 0;
            int TotalCount = 0;
            bool IsLoadMore = false;
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
                await EnsureConnectionOpenAsync();
                var checkId = _repositoryCollectionMaster.GetAllList(i => i.Id == Input.CollectionId).Select(i => i.Id).FirstOrDefault();
                if (checkId > 0)
                {
                    IEnumerable<ProductCollectionListDto> ProductDataa = await _db.QueryAsync<ProductCollectionListDto>("Usp_GetProdcutDetailsByCollectinId", new
                    {
                        collection = Input.CollectionId,
                        TenantId = TenantId,
                        searchText = Input.SearchText,
                        PageSize = 10,
                        PageNum = Input.PageNumber == 0 ? 1 : Input.PageNumber
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    var ListDistinct = ProductDataa.GroupBy(g => g.ProductSKU).Select(x => x.FirstOrDefault()).Distinct().ToList();
                    var countLimit = (from a in ListDistinct
                                      select a.TotalCount).ToList();
                    if (countLimit.Count > 0)
                    {
                        limit = countLimit[0];
                    }
                     
                    ProductData = new ProductCollectionDataDto
                    {  
                        
                       TotalCount = limit,
                       List = (from item in ListDistinct
                                select new ProductColListDto
                                {
                                    Id = item.Id,
                                    ProductSKU = item.ProductSKU,
                                    ProductTitle = item.ProductTitle,
                                    DefaultImagePath = item.DefaultImagePath
                                }).ToList()
                    };
                    IEnumerable<ProductCollectionListDto> ProductDataCol = await _db.QueryAsync<ProductCollectionListDto>("Usp_GetProductDataByMannualCollectionId", new
                    {
                        collection = Input.CollectionId,
                        tenantId = TenantId,
                        @searchText = Input.SearchText,
                        PageSize = 10,
                        PageNum = Input.PageNumber == 0 ? 1 : Input.PageNumber
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    var ListDistinctManual = ProductDataCol.GroupBy(g => g.ProductSKU).Select(x => x.FirstOrDefault()).Distinct().ToList();
                    var countLimitManuual = (from a in ListDistinctManual
                                             select a.TotalCount).ToList();
                    if (countLimitManuual.Count > 0)
                    {
                        limitMannual = countLimitManuual[0];
                    }
                    
                    ProductDataCollection = new ProductCollectionDataDto
                    {
                        TotalCount = limitMannual,
                        List = (from item in ListDistinctManual
                                select new ProductColListDto
                                {
                                    Id = item.Id,
                                    ProductSKU = item.ProductSKU,
                                    ProductTitle = item.ProductTitle,
                                    DefaultImagePath = item.DefaultImagePath
                                }).ToList()
                    };
                    var result = ProductData.List.Union(ProductDataCollection.List).ToList();
                    countData = ProductData.TotalCount + ProductDataCollection.TotalCount;
                    ProductDataFinal = (from collection in _repositoryCollectionMaster.GetAllList()
                                        where collection.Id == Input.CollectionId
                                        select new ProductCollectionDataDto
                                        {
                                            CollectionName = collection.CollectionName,
                                            CollectionId = collection.Id,
                                            List = result,
                                            TotalCount = countData
                                        }
                                        ).AsQueryable();

                    ProductDataList = new ProductCollectionDataDto
                    {
                        List = result
                    };
                }

            }
            catch (Exception ex)
            {
            }
            var data = await ApplyProductDtoSorting(ProductDataList.List.ToList(), Input);
           
            ProductDataFinal = (from collection in _repositoryCollectionMaster.GetAllList()
                                where collection.Id == Input.CollectionId
                                select new ProductCollectionDataDto
                                {
                                    CollectionName = collection.CollectionName,
                                    CollectionId = collection.Id,
                                    List = data

                                }).AsQueryable();
            try
            {

                response.items = ObjectMapper.Map<List<ProductCollectionDataDto>>(ProductDataFinal);
            }
            catch (Exception ex)
            { }
            response.TotalCount = countData;
            if (response.TotalCount > 0)
            { 
                response.PageNumber = Input.PageNumber + 1;
                if (Input.PageNumber == 0)
                {
                    Input.PageNumber  = Input.PageNumber + 1;
                    response.PageNumber = Input.PageNumber + 1;
                }
                if (Input.PageNumber <= countData / 10)
                {
                    response.IsLoadMore = true;
                }
                else {
                    response.PageNumber = 0;
                    response.IsLoadMore = false;
                }               
            }
            else
            { 
                response.PageNumber = 0;
                response.IsLoadMore = false;
            }
            
            
            return response;
        }
        private async Task<bool> DeleteCategoryCollection(List<CategoryCollections> CategoryCollection, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repository.BulkDelete(CategoryCollection);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task EnsureConnectionOpenAsync()
        {
            var connection = _dbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
        }

        private async Task<bool> DeleteCollectionCalculations(List<CollectionCalculations> CollectionCalculations, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryCalculations.BulkDelete(CollectionCalculations);
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
        private async Task<List<ProductColListDto>> ApplyProductDtoSorting(List<ProductColListDto> query, CategoryProductCollectionsResultRequestDto input)
        {
            if (input.Sorting.HasValue)
            {
                switch (input.Sorting)
                {
                    case 1:
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.Id).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Id).ToList();
                        }

                        break;
                    case 2://
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.ProductTitle).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle).ToList();
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.ProductTitle).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle).ToList();
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
                query = query.OrderByDescending(x => x.Id).ToList();
            }
            return query;
        }

        private async Task<List<CategoryCollectionsDto>> ApplyDtoSorting(List<CategoryCollectionsDto> query, CategoryCollectionsResultRequestDto input)
        {
            if (input.Sorting.HasValue)
            {
                switch (input.Sorting)
                {
                    case 1:
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.Id).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Id).ToList();
                        }

                        break;
                    case 2://
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.CollectionName).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CollectionName).ToList();
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.CollectionName).ToList();
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.CollectionName).ToList();
                        }
                        break;

                    default:
                        {
                            query = query.OrderBy(x => x.CollectionName).ToList();
                        }
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(x => x.Id).ToList();
            }
            return query;
        }

        #endregion

        private async Task<bool> UpdateAssignedCollections(List<ProductAssignedCollections> AssignedCollections, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryProductAssignedCollections.BulkUpdate(AssignedCollections);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> UpdateCategoryCollections(List<CategoryCollections> CategoryCollections, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repository.BulkUpdate(CategoryCollections);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }


        public async Task DeleteCollectionById(long Id)
        {
            try
            {
                var Collection = _repositoryCollectionMaster.GetAllList(i => i.Id == Id).FirstOrDefault();

                // update all collections with unassigned category if its deleted
                if (Collection != null)
                {
                    // delete category from master
                    await _repositoryCollectionMaster.DeleteAsync(Collection);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task AssignProductsToCollection(ProductCollectionModel Model)
        {
            try
            {
                List<ProductAssignedCollections> AssignedProducts = new List<ProductAssignedCollections>();
                if (Model.CollectionId > 0)
                {
                    foreach (var item in Model.ProductIds)
                    {
                        ProductAssignedCollections obj = new ProductAssignedCollections();
                        obj.CollectionId = Model.CollectionId;
                        obj.ProductId = item;
                        obj.IsActive = true;
                        AssignedProducts.Add(obj);
                    }
                    await _repositoryProductAssignedCollections.BulkInsertAsync(AssignedProducts);
                }
            }
            catch (Exception ex)
            {

            }

        }


        public async Task RemoveProductsFromCollection(ProductCollectionModel Model)
        {
            try
            {
                List<ProductAssignedCollections> AssignedProducts = new List<ProductAssignedCollections>();
                var ProductsList = _repositoryProductAssignedCollections.GetAllList(i => i.CollectionId == Model.CollectionId).ToList();
                ProductsList = ProductsList.Where(i => Model.ProductIds.Any(p2 => p2 == i.ProductId)).ToList();
                if(ProductsList.Count > 0)
                {
                   _repositoryProductAssignedCollections.BulkDelete(ProductsList);
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}
