using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.Currency;
using CaznerMarketplaceBackendApp.Product.Dto;
using CaznerMarketplaceBackendApp.Product.Importing;
using CaznerMarketplaceBackendApp.Product.Masters;
using CaznerMarketplaceBackendApp.Storage;
using FimApp.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
using CaznerMarketplaceBackendApp.WareHouse;
using Microsoft.AspNetCore.Mvc;
using NUglify.Helpers;
using System.IO;
using System.Diagnostics;
using System.Net.Mail;

namespace CaznerMarketplaceBackendApp.Product
{
    [AbpAuthorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProductBulkUploadAppService : CaznerMarketplaceBackendAppAppServiceBase, IProductBulkUploadAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        protected readonly IBinaryObjectManager _BinaryObjectManager;
        private readonly IProductListExcelDataReader _productExcelDataReader;
        private readonly IProductVariantExcelDataReader _productVariantExcelDataReader;
        private readonly IRepository<ProductMaster, long> _productMasterRepository;
        private readonly IRepository<ProductMaterialMaster, long> _productMaterialMasterRepository;
        private readonly IRepository<BrandingMethodMaster, long> _brandingMethodMasterRepository;
        private readonly IRepository<ProductOptionsMaster, long> _productOptionsMasterRepository;
        private readonly IRepository<ProductVariantOptionValues, long> _productVariantOptionValuesRepository;
        private readonly IRepository<ProductBulkUploadVariations, long> _productBulkUploadVariationsRepository;
        private readonly IRepository<ProductVariantsData, long> _productVariantsDataRepository;
        private readonly IRepository<CurrencyMaster, long> _currencyMasterRepository;
        private readonly IRepository<CategoryMaster, long> _categoryMasterRepository;
        private readonly IRepository<CategoryCollections, long> _categoryCollectionsRepository;
        private readonly IRepository<ProductTypeMaster, long> _productTypeMasterRepository;
        private readonly IRepository<ProductBrandMaster, long> _productBrandMasterRepository;
        private readonly IRepository<ProductTagMaster, long> _productTagMasterRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ProductDimensionsInventory, long> _productDimensionsInventoryRepository;
      
        private readonly IRepository<ProductMediaImages, long> _productMediaImagesRepository;
        private readonly IRepository<ProductImages, long> _productImagesRepository;
        private readonly IRepository<ProductBrandingPriceVariants, long> _productBrandingPriceVariantsRepository;
        private readonly IRepository<ProductMediaImageTypeMaster, long> _productMediaImageTypeMasterRepository;
        private readonly IRepository<ProductStockLocation, long> _productStockLocationRepository;
        private readonly IProductStockLocationDataReader _productStockLocationDataReader;
        private readonly IRepository<WareHouseMaster, long> _wareHouseMasterRepository;
        private readonly IRepository<ProductBrandingPosition, long> _productBrandingPositionRepository;
        private readonly IProductBrandingLocationDataReader _productBrandingLocationDataReader;
        private readonly IVariantStockLocationDataReader _productVariantStockLocReader;
        private readonly IRepository<ProductVariantWarehouse, long> _productVariantWarehouseRepository;
        private readonly IRepository<ProductVariantdataImages, long> _productVariantdataImagesRepository;
        private readonly IRepository<CollectionMaster, long> _collectionMasterRepository;
        private readonly IRepository<CategoryGroupMaster, long> _categoryGroupMasterRepository;
        private readonly IRepository<CategoryGroups, long> _categoryGroupRepository;
        private readonly IRepository<ProductVolumeDiscountVariant, long> _productVolumeDiscountRepository;
        private readonly IRepository<ProductSizeMaster, long> _productSizeMasterRepository;
        private readonly IRepository<TurnAroundTime, long> _TurnAroundTimeRepository;

        private readonly IRepository<ProductAssignedBrands, long> _productAssignedBrandsRepository;
        private readonly IRepository<ProductAssignedMaterials, long> _productAssignedMaterialsRepository;
        private readonly IRepository<ProductAssignedCollections, long> _productAssignedCollectionsRepository;
        private readonly IRepository<ProductAssignedTags, long> _productAssignedTagsRepository;
        private readonly IRepository<ProductAssignedSubCategories, long> _productAssignedCategoriesRepository;
        private readonly IRepository<ProductAssignedTypes, long> _productAssignedTypesRepository;
        private readonly IRepository<ProductAssignedVendors, long> _productAssignedVendorsRepository;
        private readonly IRepository<ProductViewImages, long> _productViewImagesRepository;
        private readonly IRepository<AlternativeProducts, long> _alternativeProductsRepository;
        private readonly IRepository<RelativeProducts, long> _relativeProductsRepository;
        private readonly IRepository<ProductBrandingMethods, long> _productBrandingMethodsRepository;
        private readonly IRepository<ProductVariantQuantityPrices, long> _productVariantQuantityPricesRepository;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _Environment;
        private readonly IUserEmailer _userEmailerService;

        public ProductBulkUploadAppService(IUnitOfWorkManager unitOfWorkManager, IBinaryObjectManager BinaryObjectManager, IProductListExcelDataReader productExcelDataReader,
            IRepository<ProductMaster, long> productMasterRepository, IRepository<ProductMaterialMaster, long> productMaterialMasterRepository,
           IRepository<BrandingMethodMaster, long> brandingMethodMasterRepository, IProductVariantExcelDataReader productVariantExcelDataReader, IRepository<ProductOptionsMaster, long> productOptionsMasterRepository
           , IRepository<ProductVariantOptionValues, long> productVariantOptionValuesRepository, IRepository<ProductBulkUploadVariations, long> productBulkUploadVariationsRepository,
             IRepository<ProductVariantsData, long> productVariantsDataRepository, IRepository<CurrencyMaster, long> currencyMasterRepository, IRepository<CategoryMaster, long> categoryMasterRepository, IRepository<CategoryCollections, long> categoryCollectionsRepository,
             IRepository<ProductTypeMaster, long> productTypeMasterRepository, IRepository<ProductBrandMaster, long> productBrandMasterRepository, IRepository<ProductTagMaster, long> productTagMasterRepository, IRepository<User, long> userRepository, IRepository<ProductDimensionsInventory, long> productDimensionsInventoryRepository,
             IRepository<ProductMediaImages, long> productMediaImagesRepository, IRepository<ProductImages, long> productImagesRepository, IRepository<ProductBrandingPriceVariants, long> productBrandingPriceVariantsRepository
            , IRepository<ProductMediaImageTypeMaster, long> productMediaImageTypeMasterRepository, IProductStockLocationDataReader productStockLocationDataReader, IRepository<WareHouseMaster, long> wareHouseMasterRepository, IRepository<ProductStockLocation, long> productStockLocationRepository,
             IRepository<ProductBrandingPosition, long> productBrandingPositionRepository, IProductBrandingLocationDataReader productBrandingLocationDataReader, IVariantStockLocationDataReader productVariantStockLocReader, IRepository<ProductVariantWarehouse, long> productVariantWarehouseRepository, IRepository<ProductVariantdataImages, long> productVariantdataImagesRepository, IRepository<CollectionMaster, long> collectionMasterRepository,
             IRepository<CategoryGroupMaster, long> categoryGroupMasterRepository, IRepository<CategoryGroups, long> categoryGroupsRepository, IRepository<ProductVolumeDiscountVariant, long> productVolumeDiscountRepository, IRepository<ProductSizeMaster, long> productSizeMasterRepository, IRepository<TurnAroundTime, long> TurnAroundTimeRepository,
             IRepository<ProductAssignedBrands, long> productAssignedBrandsRepository, IRepository<ProductAssignedMaterials, long> productAssignedMaterialsRepository, IRepository<ProductAssignedCollections, long> productAssignedCollectionsRepository, IRepository<ProductAssignedSubCategories, long> productAssignedCategoriesRepository, IRepository<ProductAssignedTypes, long> productAssignedTypesRepository
             , IRepository<ProductAssignedTags, long> productAssignedTagsRepository, IRepository<ProductAssignedVendors, long> productAssignedVendorsRepository, IRepository<ProductViewImages, long> productViewImagesRepository, IRepository<AlternativeProducts, long> alternativeProductsRepository, IRepository<RelativeProducts, long> relativeProductsRepository, IRepository<ProductBrandingMethods, long> productBrandingMethodsRepository, Microsoft.AspNetCore.Hosting.IWebHostEnvironment Environment
            , IRepository<ProductVariantQuantityPrices, long> productVariantQuantityPricesRepository, IUserEmailer userEmailerService)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _BinaryObjectManager = BinaryObjectManager;
            _productExcelDataReader = productExcelDataReader;
            _productMasterRepository = productMasterRepository;
            _productMaterialMasterRepository = productMaterialMasterRepository;
           
            _brandingMethodMasterRepository = brandingMethodMasterRepository;
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
            _productVariantExcelDataReader = productVariantExcelDataReader;
            _productOptionsMasterRepository = productOptionsMasterRepository;
         
            _productVariantOptionValuesRepository = productVariantOptionValuesRepository;
            _productBulkUploadVariationsRepository = productBulkUploadVariationsRepository;
            _productVariantsDataRepository = productVariantsDataRepository;
            _currencyMasterRepository = currencyMasterRepository;
            _categoryMasterRepository = categoryMasterRepository;
            _categoryCollectionsRepository = categoryCollectionsRepository;
            _productTypeMasterRepository = productTypeMasterRepository;
            _productBrandMasterRepository = productBrandMasterRepository;
            _productTagMasterRepository = productTagMasterRepository;
            _userRepository = userRepository;
            _productDimensionsInventoryRepository = productDimensionsInventoryRepository;
          
            _productMediaImagesRepository = productMediaImagesRepository;
            _productImagesRepository = productImagesRepository;
            _productBrandingPriceVariantsRepository = productBrandingPriceVariantsRepository;
            _productMediaImageTypeMasterRepository = productMediaImageTypeMasterRepository;
            _productStockLocationDataReader = productStockLocationDataReader;
            _wareHouseMasterRepository = wareHouseMasterRepository;
            _productStockLocationRepository = productStockLocationRepository;
            _productBrandingPositionRepository = productBrandingPositionRepository;
            _productBrandingLocationDataReader = productBrandingLocationDataReader;
            _productVariantStockLocReader = productVariantStockLocReader;
            _productVariantWarehouseRepository = productVariantWarehouseRepository;
            _productVariantdataImagesRepository = productVariantdataImagesRepository;
            _collectionMasterRepository = collectionMasterRepository;
            _categoryGroupMasterRepository = categoryGroupMasterRepository;
            _categoryGroupRepository = categoryGroupsRepository;
            _productVolumeDiscountRepository = productVolumeDiscountRepository;
            _productSizeMasterRepository = productSizeMasterRepository;
            _TurnAroundTimeRepository = TurnAroundTimeRepository;
            _productAssignedBrandsRepository = productAssignedBrandsRepository;
            _productAssignedMaterialsRepository = productAssignedMaterialsRepository;
            _productAssignedCollectionsRepository = productAssignedCollectionsRepository;
            _productAssignedTagsRepository = productAssignedTagsRepository;
            _productAssignedCategoriesRepository = productAssignedCategoriesRepository;
            _productAssignedTypesRepository = productAssignedTypesRepository;
            _productAssignedVendorsRepository = productAssignedVendorsRepository;
            _productViewImagesRepository = productViewImagesRepository;
            _alternativeProductsRepository = alternativeProductsRepository;
            _relativeProductsRepository = relativeProductsRepository;
            _productBrandingMethodsRepository = productBrandingMethodsRepository;
            _productVariantQuantityPricesRepository = productVariantQuantityPricesRepository;
            _Environment = Environment;
            _userEmailerService = userEmailerService;
        }


        [UnitOfWork]
        public virtual async Task ProductImport(Guid ObjectId)
        {
            ImportProductModel metaData = new ImportProductModel(); //new List<ImportBulkProductDto>();

            metaData = AsyncHelper.RunSync(() => GetMetadataFromExcelOrNull(ObjectId));


            if (metaData == null || !metaData.BulkProducts.Any())
            {
                return;
            }

            AsyncHelper.RunSync(() => CreateProductData(metaData.BulkProducts));

        }


        [UnitOfWork]
        public async Task ProductImportUsingPath()
        {
            ImportProductModel metaData = new ImportProductModel(); 

            metaData = AsyncHelper.RunSync(() => GetMetadataFromExcelOrNullUsingPath());


            if (metaData == null || !metaData.BulkProducts.Any())
            {
                return;
            }

            AsyncHelper.RunSync(() => CreateProductData(metaData.BulkProducts));

        }



        [UnitOfWork]
        public virtual async Task ProductVariantImport(Guid ObjectId)
        {
            var metaData = new List<ImportColorVariantsDto>();

            metaData = AsyncHelper.RunSync(() => GetVariantsFromExcelOrNull(ObjectId));
            if (metaData == null || !metaData.Any())
            {
                return;
            }

            AsyncHelper.RunSync(() => CreateProductVariantData(metaData));
        }

        [UnitOfWork]
        public virtual async Task ProductStockLocationImport(Guid ObjectId)
        {
            var metaData = new List<ImportBulkProductStockLocationDto>();

            metaData = AsyncHelper.RunSync(() => GetStockLocationDataFromExcelOrNull(ObjectId));
            if (metaData == null || !metaData.Any())
            {
                return;
            }

            AsyncHelper.RunSync(() => CreateProductStockLocationDataImport(metaData));
        }

        [UnitOfWork]
        public virtual async Task ProductBrandingPositionImport(Guid ObjectId)
        {
            var metaData = new List<ImportBulkBrandingLocationDto>();

            metaData = AsyncHelper.RunSync(() => GetBrandingPositionDataFromExcelOrNull(ObjectId));
            if (metaData == null || !metaData.Any())
            {
                return;
            }

            AsyncHelper.RunSync(() => CreateProductBrandingPositionDataImport(metaData));
        }

        [UnitOfWork]
        public virtual async Task ProductVariantStockLocationImport(Guid ObjectId)
        {
            var metaData = new List<ImportVariantBulkStockLocDto>();

            metaData = AsyncHelper.RunSync(() => GetVariantStockLocFromExcelOrNull(ObjectId));
            if (metaData == null || !metaData.Any())
            {
                return;
            }

            AsyncHelper.RunSync(() => CreateVariantStockLocationDataImport(metaData));
        }


        [UnitOfWork]
        public virtual async Task<ImportProductModel> GetMetadataFromExcelOrNull(Guid BinaryObjectId)
        {
            var file = new BinaryObject();
            ImportProductModel Response = new ImportProductModel();
            try
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        file = await _BinaryObjectManager.GetOrNullAsync(BinaryObjectId);
                    }
                }

                Response.BulkProducts = _productExcelDataReader.GetProductsFromExcel(file.Bytes);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Response;
        }


        [UnitOfWork]
        public virtual async Task<ImportProductModel> GetMetadataFromExcelOrNullUsingPath()
        {
            var file = new BinaryObject();
            ImportProductModel Response = new ImportProductModel();
            try
            {
                byte[] FileBytes = File.ReadAllBytes(_Environment.WebRootPath + "/"+"ankita.xlsx"); 

                Response.BulkProducts = _productExcelDataReader.GetProductsFromExcel(FileBytes);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Response;
        }

        [UnitOfWork]
        public virtual async Task<List<ImportColorVariantsDto>> GetVariantsFromExcelOrNull(Guid BinaryObjectId)
        {
            var file = new BinaryObject();
            List<ImportColorVariantsDto> Response = new List<ImportColorVariantsDto>();
            try
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        file = await _BinaryObjectManager.GetOrNullAsync(BinaryObjectId);
                    }
                }
                Response = _productVariantExcelDataReader.GetProductsVariantFromExcel(file.Bytes);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Response;
        }

        [UnitOfWork]
        public virtual async Task<List<ImportBulkProductStockLocationDto>> GetStockLocationDataFromExcelOrNull(Guid BinaryObjectId)
        {
            var file = new BinaryObject();
            List<ImportBulkProductStockLocationDto> Response = new List<ImportBulkProductStockLocationDto>();
            try
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        file = await _BinaryObjectManager.GetOrNullAsync(BinaryObjectId);
                    }
                }

                Response = _productStockLocationDataReader.GetStockLocationDataFromExcel(file.Bytes);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Response;
        }

        [UnitOfWork]
        public virtual async Task<List<ImportBulkBrandingLocationDto>> GetBrandingPositionDataFromExcelOrNull(Guid BinaryObjectId)
        {
            var file = new BinaryObject();
            List<ImportBulkBrandingLocationDto> Response = new List<ImportBulkBrandingLocationDto>();
            try
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        file = await _BinaryObjectManager.GetOrNullAsync(BinaryObjectId);
                    }
                }

                Response = _productBrandingLocationDataReader.GetStockLocationDataFromExcel(file.Bytes);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Response;
        }


        [UnitOfWork]
        public virtual async Task<List<ImportVariantBulkStockLocDto>> GetVariantStockLocFromExcelOrNull(Guid BinaryObjectId)
        {
            var file = new BinaryObject();
            List<ImportVariantBulkStockLocDto> Response = new List<ImportVariantBulkStockLocDto>();
            try
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        file = await _BinaryObjectManager.GetOrNullAsync(BinaryObjectId);
                    }
                }

                Response = _productVariantStockLocReader.GetStockLocationDataFromExcel(file.Bytes);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Response;
        }


        public virtual async Task CreateProductData(List<ImportBulkProductDto> productData)
        {
            int NumberOfExcelRows = productData.Count();
            int TenantId = AbpSession.TenantId.Value;
            try
            {

                List<ProductBulkImportDataHistory> productToAddList = new List<ProductBulkImportDataHistory>();

                var UserMasterData = _userRepository.GetAll();
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    UserMasterData = _userRepository.GetAll();
                }
             
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    var allProducts = await _productMasterRepository.GetAllListAsync(); 

                    List<ImportBulkProductDto> distinctExcelSKU = productData.Where(i => !string.IsNullOrEmpty(i.ParentSKU) && i.MainorVariant.ToLower() == "main").GroupBy(p => p.ParentSKU.Trim()).Select(g => g.LastOrDefault()).ToList();

                    var ProductVariantData = productData.Where(i => !string.IsNullOrEmpty(i.VariantSKU) && i.MainorVariant.ToLower() == "variant").GroupBy(p => p.ParentSKU.Trim()).ToList();

                    //--------------------master product data-------------------------
                    var dataToInsert = distinctExcelSKU.Select(v => v.ParentSKU).ToList().Except(allProducts.Where(c => c.IsActive).Select(b => b.ProductSKU).ToList()).ToList();
                    var DataToUpdate = distinctExcelSKU.Where(x => !dataToInsert.Contains(x.ParentSKU)).ToList();
                    productData = distinctExcelSKU.Where(x => dataToInsert.Contains(x.ParentSKU)).ToList();
                    //----------------------------------------------------------------

                    var MaterialMasterData = _productMaterialMasterRepository.GetAll();
                    var BrandingMethodData = _brandingMethodMasterRepository.GetAll();
                    var CategoryMasterData = _categoryMasterRepository.GetAll();
                    var CollectionMasterData = _collectionMasterRepository.GetAll();
                    var GroupMasterData = _categoryGroupMasterRepository.GetAll();
                    var CategoryCollectionData = _collectionMasterRepository.GetAll();     
                    var TypesMasterData = _productTypeMasterRepository.GetAll();
                    var TagsMasterData = _productTagMasterRepository.GetAll();
                    var BrandMasterData = _productBrandMasterRepository.GetAll();
                    var MediaType = _productMediaImageTypeMasterRepository.GetAll();
                    var CollectionsData = _collectionMasterRepository.GetAll();
                    var SizeMasterData = _productSizeMasterRepository.GetAll();
                    var TurnAroundTimeData = _TurnAroundTimeRepository.GetAll();
                    List<ProductDimensionsInventory> InventoryFinalList = new List<ProductDimensionsInventory>();
                    List<ProductMaster> ProductMasterFinalList = new List<ProductMaster>();
                    List<ProductMediaImages> MediaImagesFinalList = new List<ProductMediaImages>();
                    List<ProductImages> ProductImagesFinalList = new List<ProductImages>();
                    List<ProductVolumeDiscountVariant> priceVariantsFinalList = new List<ProductVolumeDiscountVariant>();
                    List<ProductBrandingPosition> brandingPositionFinalList = new List<ProductBrandingPosition>();
                    List<ProductBrandingMethods> ProductBrandingMethodsFinalList = new List<ProductBrandingMethods>();
                    List<ProductTagMaster> ProductTagsFinalList = new List<ProductTagMaster>();
                    List<ProductBrandMaster> ProductBrandsFinalList = new List<ProductBrandMaster>();
                    List<ProductMaterialMaster> ProductMaterialFinalList = new List<ProductMaterialMaster>();
                    List<ProductTypeMaster> ProductTypeFinalList = new List<ProductTypeMaster>();
                    List<BrandingMethodMaster> BrandingMethodFinalList = new List<BrandingMethodMaster>();
                    List<CategoryMaster> Catgorymasterlist = new List<CategoryMaster>();
                    List<CategoryGroupMaster> CategoryGroupMasterList = new List<CategoryGroupMaster>();
                    List<CategoryGroups> CategoryGroupsList = new List<CategoryGroups>();
                    List<CategoryCollections> collectionCategoryList = new List<CategoryCollections>();
                    List<ProductAssignedMaterials> AssignedMaterials = new List<ProductAssignedMaterials>();
                    List<ProductAssignedBrands> AssignedBrands = new List<ProductAssignedBrands>();
                    List<ProductAssignedTypes> AssignedTypes = new List<ProductAssignedTypes>();
                    List<ProductAssignedTags> AssignedTags = new List<ProductAssignedTags>();
                    List<ProductAssignedVendors> AssignedVendors = new List<ProductAssignedVendors>();
                    List<ProductAssignedSubCategories> AssignedCategories = new List<ProductAssignedSubCategories>();
                    List<ProductAssignedCollections> AssignedCollections = new List<ProductAssignedCollections>();
                    List<AlternativeProducts> ProductAlternativesFinalList = new List<AlternativeProducts>();
                    List<RelativeProducts> RelativeProductsFinalList = new List<RelativeProducts>();
                    List<ProductViewImages> ProductViewImagesFinalList = new List<ProductViewImages>();
                    List<CollectionMaster> collectionListFinalList = new List<CollectionMaster>();
                    List<CategoryGroups> CategoryGroupsRawData = new List<CategoryGroups>();
                    List<CategoryCollections> CategoryCollectionsRawData = new List<CategoryCollections>();

                    // applying check to skip dirty data sets

                    productData = productData.Where(i => !string.IsNullOrEmpty(i.ParentSKU) && !string.IsNullOrEmpty(i.Name)).ToList();


                    #region Master table bindings integration

                    List<BrandingMethodMaster> BrandingMethodList = new List<BrandingMethodMaster>();
                    List<CollectionMaster> collectionList = new List<CollectionMaster>();
                    CategoryMaster categorymaster = new CategoryMaster();
                    CategoryGroupMaster GroupMaster = new CategoryGroupMaster();
                    CollectionMaster collectionMaster = new CollectionMaster();
                    ProductMaterialMaster material = new ProductMaterialMaster();
                    ProductTypeMaster typemaster = new ProductTypeMaster();
                    ProductBrandMaster Brand = new ProductBrandMaster();
                    ProductTagMaster Tag = new ProductTagMaster();


                    int DivisonNumber = 0;

                    if (productData.Count <= 10000)
                    {
                        DivisonNumber = 5;
                    }
                    else if (productData.Count <= 20000)
                    {
                        DivisonNumber = 10;
                    }
                    else if (productData.Count <= 30000)
                    {
                        DivisonNumber = 15;
                    }
                    else if (productData.Count <= 40000)
                    {
                        DivisonNumber = 20;
                    }
                    else
                    {
                        DivisonNumber = 25;
                    }

                    var ProductListDataChunks = StaticHelperUtility.Split(productData, DivisonNumber);

                    foreach (var products in ProductListDataChunks)
                    {

                        foreach (var detail in products/*productData*/)
                        {
                            categorymaster = new CategoryMaster();
                            BrandingMethodList = new List<BrandingMethodMaster>();
                            collectionList = new List<CollectionMaster>();
                            long CategoryId = 0;
                            long GroupMasterId = 0;
                            GroupMaster = new CategoryGroupMaster();
                            if (!string.IsNullOrEmpty(detail.Groupcategory))
                            {
                                //---------------Group Master -----------------------
                                GroupMasterId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == detail.Groupcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                CategoryGroupMaster groupObj = CategoryGroupMasterList.Where(i => i.GroupTitle.ToLower().Trim() == detail.Groupcategory.Trim()).FirstOrDefault();

                                if (GroupMasterId == 0 && groupObj == null)
                                {
                                    GroupMaster.GroupTitle = detail.Groupcategory;
                                    GroupMaster.TenantId = TenantId;
                                    CategoryGroupMasterList.Add(GroupMaster);
                                }
                            }
                            //-------------------------Group Master ends-----------------


                            if (!string.IsNullOrEmpty(detail.Productcategory))
                            {
                                CategoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == detail.Productcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                var IsExists = Catgorymasterlist.Where(i => i.CategoryTitle.ToLower() == detail.Productcategory.Trim()).FirstOrDefault();


                                if (CategoryId == 0 && IsExists == null)
                                {
                                    categorymaster.CategoryTitle = detail.Productcategory.Trim();
                                    categorymaster.TenantId = TenantId;

                                    if (IsExists == null)
                                    {
                                        Catgorymasterlist.Add(categorymaster);

                                    }
                                }
                            }
                            //-----------collection master
                            if (!string.IsNullOrEmpty(detail.ProductCollections))
                            {
                                string[] Collections = detail.ProductCollections.Split(',');
                                foreach (var collection in Collections)
                                {
                                    var CategoryCollectionId = CategoryCollectionData.Where(i => i.CollectionName.ToLower().Trim() == collection.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                    var IsCollectionExists = collectionList.Where(i => i.CollectionName.ToLower().Trim() == collection.ToLower().Trim()).FirstOrDefault();
                                    collectionMaster = new CollectionMaster();

                                    if (CategoryCollectionId == 0 && IsCollectionExists == null)
                                    {
                                        collectionMaster.CollectionName = collection.Trim();
                                        collectionMaster.IsActive = true;
                                        collectionMaster.TenantId = TenantId;
                                        collectionList.Add(collectionMaster);
                                    }
                                }
                                if (collectionList.Count > 0)
                                {
                                    collectionListFinalList.AddRange(collectionList);
                                }
                            }
                            if (!string.IsNullOrEmpty(detail.ProductMaterial))
                            {
                                string[] Materials = detail.ProductMaterial.Split(',');
                                foreach (var mat in Materials)
                                {

                                    var MaterialId = MaterialMasterData.Where(i => i.ProductMaterialName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();
                                    if (MaterialId == null)
                                    {
                                        material = new ProductMaterialMaster();
                                        material.ProductMaterialName = mat.ToLower().Trim();
                                        material.IsActive = true;
                                        ProductMaterialFinalList.Add(material);
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(detail.Caznerproducttypes))
                            {
                                string[] Producttypes = detail.Caznerproducttypes.Split(',');
                                foreach (var type in Producttypes)
                                {

                                    var TypeId = TypesMasterData.Where(i => i.ProductTypeName.ToLower().Trim() == type.ToLower().Trim()).FirstOrDefault();
                                    if (TypeId == null)
                                    {
                                        typemaster = new ProductTypeMaster();
                                        typemaster.ProductTypeName = type.ToLower().Trim();
                                        typemaster.IsActive = true;
                                        ProductTypeFinalList.Add(typemaster);
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(detail.ProductBrands))
                            {
                                string[] Brands = detail.ProductBrands.Split(',');
                                foreach (var mat in Brands)
                                {
                                    var BrandId = BrandMasterData.Where(i => i.ProductBrandName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();

                                    if (BrandId == null)
                                    {
                                        Brand = new ProductBrandMaster();
                                        Brand.ProductBrandName = mat.ToLower().Trim();
                                        Brand.IsActive = true;
                                        ProductBrandsFinalList.Add(Brand);
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(detail.ProductTags))
                            {
                                string[] Tags = detail.ProductTags.Split(',');
                                string TagValue = string.Empty;
                                foreach (var mat in Tags)
                                {
                                    var TagId = TagsMasterData.Where(i => i.ProductTagName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();

                                    if (TagId == null)
                                    {
                                        Tag = new ProductTagMaster();
                                        Tag.ProductTagName = mat.ToLower().Trim();
                                        Tag.IsActive = true;
                                        ProductTagsFinalList.Add(Tag);

                                    }
                                }
                            }

                        }
                        #endregion

                    #region Master data creation

                        if (CategoryGroupMasterList.Count > 0)
                        {
                            await CreateGroupMaster(CategoryGroupMasterList.GroupBy(x => x.GroupTitle.ToLower().Trim()).Select(g => g.First()).ToList(), TenantId);
                        }
                        if (Catgorymasterlist.Count > 0)
                        {
                            await CreateCategoryMaster(Catgorymasterlist.GroupBy(x => x.CategoryTitle.ToLower().Trim()).Select(g => g.First()).ToList(), TenantId);
                        }
                        if (collectionListFinalList.Count > 0)
                        {
                            await CreateCollectionMaster(collectionListFinalList.GroupBy(x => x.CollectionName.ToLower().Trim()).Select(g => g.First()).ToList(), TenantId);
                        }
                        if (ProductTagsFinalList.Count > 0)
                        {
                            await CreateProductTags(ProductTagsFinalList.GroupBy(x => x.ProductTagName.ToLower().Trim()).Select(g => g.First()).ToList(), TenantId);
                        }
                        if (ProductMaterialFinalList.Count > 0)
                        {
                            await CreateProductMaterial(ProductMaterialFinalList.GroupBy(x => x.ProductMaterialName.ToLower().Trim()).Select(g => g.First()).ToList(), TenantId);
                        }
                        if (ProductBrandsFinalList.Count > 0)
                        {
                            await CreateProductBrands(ProductBrandsFinalList.GroupBy(x => x.ProductBrandName.ToLower().Trim()).Select(g => g.First()).ToList(), TenantId);
                        }
                        if (BrandingMethodFinalList.Count > 0)
                        {
                            await CreateBrandingMethods(BrandingMethodFinalList.GroupBy(x => x.MethodName.ToLower().Trim()).Select(g => g.First()).ToList(), TenantId);
                        }
                        if (ProductTypeFinalList.Count > 0)
                        {
                            await CreateTypeMethods(ProductTypeFinalList.GroupBy(x => x.ProductTypeName.ToLower().Trim()).Select(g => g.First()).ToList(), TenantId);
                        }

                        #endregion
                    }


                    #region Main product integration and binding process

                    //----------------getting latest master data which has been inserted by sheet.
                     CategoryMasterData = _categoryMasterRepository.GetAll();
                     CollectionMasterData = _collectionMasterRepository.GetAll();
                     GroupMasterData = _categoryGroupMasterRepository.GetAll();


                    CategoryGroups CategoryGroup = new CategoryGroups();
                    ProductBulkImportDataHistory history = new ProductBulkImportDataHistory();
                    ProductDimensionsInventory inventory = new ProductDimensionsInventory();
                    ProductMaster master = new ProductMaster();
                    ProductDimension dimension = new ProductDimension();
                    List<ProductImages> images = new List<ProductImages>();
                    List<ProductMediaImages> MediaImages = new List<ProductMediaImages>();
                    List<ProductViewImages> ProductViewImages = new List<ProductViewImages>();
                    List<ProductVolumeDiscountVariant> priceVariants = new List<ProductVolumeDiscountVariant>();
                    List<ProductBrandingPosition> ProductBrandingLocData = new List<ProductBrandingPosition>();
                    List<AlternativeProducts> ProductAlternatives = new List<AlternativeProducts>();
                    List<RelativeProducts> RelativeProducts = new List<RelativeProducts>();
                    CategoryMaster Catgorymaster = new CategoryMaster();
                    ProductAssignedMaterials assignedMaterials = new ProductAssignedMaterials();
                    ProductAssignedBrands AssignedBrand = new ProductAssignedBrands();
                    ProductAssignedTags AssignedTag = new ProductAssignedTags();
                    ProductAssignedVendors AssignedVendor = new ProductAssignedVendors();
                    ProductAssignedSubCategories AssignedCategory = new ProductAssignedSubCategories();
                    ProductAssignedCollections AssignedCollection = new ProductAssignedCollections();
                    ProductImages img = new ProductImages();
                    ProductImages imgObj = new ProductImages();
                    ProductMediaImages imgLineArts = new ProductMediaImages();
                    ProductViewImages productViewImages = new ProductViewImages();
                    AlternativeProducts alternativeProducts = new AlternativeProducts();
                    RelativeProducts relativeProducts = new RelativeProducts();
                    ProductBrandingMethods Method = new ProductBrandingMethods();
                    int DivisonNumberForMaster = 0;

                    if(productData.Count <=10000)
                    {
                        DivisonNumberForMaster = 5;
                    }
                    else if(productData.Count <= 20000)
                    {
                        DivisonNumberForMaster = 10;
                    }
                    else if (productData.Count <= 30000)
                    {
                        DivisonNumberForMaster = 15;
                    }
                    else if (productData.Count <= 40000)
                    {
                        DivisonNumberForMaster = 20;
                    }
                    else
                    {
                        DivisonNumberForMaster = 25;
                    }

                    var ProductDataChunks = StaticHelperUtility.Split(productData, DivisonNumberForMaster);

                    foreach (var products in ProductDataChunks)
                    {
                        foreach (var product in products.ToList()/*productData*/)
                        {
                            #region CategoryCollections and CategoryGroups assignments insertion

                            #region Category Groups
                            //------------category group assignment

                            if (!string.IsNullOrEmpty(product.Productcategory) && !string.IsNullOrEmpty(product.Groupcategory))
                            {
                                long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.Productcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                long groupId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == product.Groupcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

                                if (groupId > 0 && categoryId > 0)
                                {
                                    CategoryGroups IsGroupAlreadyExists = CategoryGroupsList.Where(i => i.CategoryGroupId == groupId && i.CategoryMasterId == categoryId).FirstOrDefault();

                                    if (IsGroupAlreadyExists == null)
                                    {
                                        CategoryGroup = new CategoryGroups();
                                        CategoryGroup.CategoryGroupId = groupId;
                                        CategoryGroup.CategoryMasterId = categoryId;
                                        CategoryGroup.IsActive = true;
                                        CategoryGroupsList.Add(CategoryGroup);
                                    }
                                }
                            }

                            #endregion


                            #region Category collection assignment

                            if (!string.IsNullOrEmpty(product.ProductCollections) && !string.IsNullOrEmpty(product.Productcategory))
                            {

                                string[] Collections = product.ProductCollections.Split(',');
                                foreach (var collection in Collections)
                                {
                                    CategoryCollections categoryCollection = new CategoryCollections();

                                    long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.Productcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                    long CollectionId = CollectionMasterData.Where(i => i.CollectionName.ToLower().Trim() == collection.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

                                    if (categoryId > 0 && CollectionId > 0)
                                    {
                                        CategoryCollections IsCollectionExists = collectionCategoryList.Where(i => i.CategoryId == categoryId && i.CollectionId == CollectionId).FirstOrDefault();
                                        if (IsCollectionExists == null)
                                        {
                                            categoryCollection.CategoryId = categoryId;
                                            categoryCollection.CollectionId = CollectionId;
                                            categoryCollection.TenantId = TenantId;
                                            categoryCollection.IsActive = true;
                                            collectionCategoryList.Add(categoryCollection);
                                        }

                                    }
                                }
                            }
                            #endregion

                            #endregion


                            history = new ProductBulkImportDataHistory();
                            inventory = new ProductDimensionsInventory();
                            master = new ProductMaster();
                         
                            dimension = new ProductDimension();
                            images = new List<ProductImages>();
                            MediaImages = new List<ProductMediaImages>();
                            ProductViewImages = new List<ProductViewImages>();
                            priceVariants = new List<ProductVolumeDiscountVariant>();
                            ProductBrandingLocData = new List<ProductBrandingPosition>();
                            ProductAlternatives = new List<AlternativeProducts>();
                            RelativeProducts = new List<RelativeProducts>();

                            Catgorymaster = new CategoryMaster();
                            master.ProductSKU = !string.IsNullOrEmpty(product.ParentSKU) ? product.ParentSKU : "";
                            master.ProductTitle = !string.IsNullOrEmpty(product.Name) ? product.Name : "";
                            master.IsActive = true;
                            master.ShortDescripition = !string.IsNullOrEmpty(product.ShortDescription) ? product.ShortDescription : "";
                            master.ProductDescripition = !string.IsNullOrEmpty(product.Description) ? product.Description : "";
                            master.ColorsAvailable = !string.IsNullOrEmpty(product.ColorsAvailable) ? product.ColorsAvailable : "";
                            master.Features = !string.IsNullOrEmpty(product.Features) ? product.Features : "";
                            master.UnitOfMeasure = string.IsNullOrEmpty(product.UnitOfMeasure) ? 0 : Convert.ToInt32(product.UnitOfMeasure);
                            master.MinimumOrderQuantity = string.IsNullOrEmpty(product.MOQ) ? 0 : Convert.ToInt32(product.MOQ);
                            master.UnitPrice = !string.IsNullOrEmpty(product.UnitPrice) ? Convert.ToDecimal(product.UnitPrice) : 0;
                            master.CostPerItem = !string.IsNullOrEmpty(product.CostPerItem) ? Convert.ToDecimal(product.CostPerItem) : 0;
                            master.Profit = !string.IsNullOrEmpty(product.Profit) ? Convert.ToDecimal(product.Profit) : 0;
                            master.IsPhysicalProduct = !string.IsNullOrEmpty(product.IsPhysicalProduct) ? product.IsPhysicalProduct.ToLower() == "true" ? true : false : false;
                            master.OnSale = !string.IsNullOrEmpty(product.IsOnSale) ? product.IsOnSale.ToLower() == "true" ? true : false : false;
                            master.ChargeTaxOnThis = !string.IsNullOrEmpty(product.IsChargeTax) ? product.IsChargeTax.ToLower() == "true" ? true : false : false;
                            master.SalePrice = !string.IsNullOrEmpty(product.SalePrice) ? Convert.ToDecimal(product.SalePrice) : 0;
                            master.MinimumOrderQuantity = !string.IsNullOrEmpty(product.MinimumOrderQuantity) ? Convert.ToInt32(product.MinimumOrderQuantity) : 0;
                            master.DepositRequired = !string.IsNullOrEmpty(product.DepositRequired) ? Convert.ToDecimal(product.DepositRequired) : 0;
                            master.CostPerItem = !string.IsNullOrEmpty(product.CostPerItem) ? Convert.ToDecimal(product.CostPerItem) : 0;
                            master.ProductHasPriceVariant = !string.IsNullOrEmpty(product.IsProductHasPriceVariant) ? product.IsProductHasPriceVariant.ToLower() == "true" ? true : false : false;

                            master.DiscountPercentage = !string.IsNullOrEmpty(product.DiscountPercentage) ? Convert.ToDouble(product.DiscountPercentage) : 0;
                            master.DiscountPercentageDraft = !string.IsNullOrEmpty(product.DiscountPercentage) ? Convert.ToDouble(product.DiscountPercentage) : 0;

                            if (!string.IsNullOrEmpty(product.TurnAroundTime))
                            {
                                master.TurnAroundTimeId = TurnAroundTimeData.Where(i => i.Time.ToLower() == product.TurnAroundTime.ToLower()).Select(i => i.Id).FirstOrDefault();
                            }
                            master.FrightNote = product.Freigthnote;
                            master.VolumeValue = product.VolumeValue;
                            master.VolumeUOM = product.VolumeUOM;
                            master.IfSetOtherProductTitleAndDimensoionsInThisSet = product.IfsetOtherproducttitleanddimensoionsinthisset;
                            master.CounrtyOfOrigin = product.Counrtyoforigin;
                            master.NumberOfPieces = product.NumberofPieces;
                            master.IsIndentOrder = product.IsIndentOrder == "true" ? true : false;
                            master.ColourFamily = product.ColourFamily;
                            master.PMSColourCode = product.PMSColourCode;
                            master.VideoURL = product.VideoURL;
                            master.Image360Degrees = product.Image360Degrees;
                            master.NextShipmentDate = product.NextShipmentDate;
                            master.NextShipmentQuantity = product.NextShipmentQuantity;
                            master.ExtraSetUpFee = product.ExtraSetupFee;
                            master.BrandingMethodNote = product.BrandingMethodNote;
                            master.BrandingUOM = product.BrandingUOM;

                            #region ProductDetails assignments




                            #region Product Material
                            if (!string.IsNullOrEmpty(product.ProductMaterial))
                            {
                                string[] Materials = product.ProductMaterial.Split(',');
                                string Material = string.Empty;
                                foreach (var mat in Materials)
                                {
                                    assignedMaterials = new ProductAssignedMaterials();
                                    var MaterialId = MaterialMasterData.Where(i => i.ProductMaterialName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();

                                    if (MaterialId != null)
                                    {
                                        assignedMaterials.MaterialId = MaterialId.Id;
                                        assignedMaterials.ProductMaster = master;
                                        AssignedMaterials.Add(assignedMaterials);
                                    }

                                }
                            }
                            #endregion

                            #region Product Brand
                            if (!string.IsNullOrEmpty(product.ProductBrands))
                            {
                                string[] Brands = product.ProductBrands.Split(',');
                                string BrandValue = string.Empty;
                                foreach (var mat in Brands)
                                {
                                    AssignedBrand = new ProductAssignedBrands();
                                    var BrandId = BrandMasterData.Where(i => i.ProductBrandName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();

                                    if (BrandId != null)
                                    {
                                        AssignedBrand.ProductBrandId = BrandId.Id;
                                        AssignedBrand.ProductMaster = master;
                                        AssignedBrands.Add(AssignedBrand);
                                    }
                                }
                            }
                            #endregion

                            #region Product Tags
                            if (!string.IsNullOrEmpty(product.ProductTags))
                            {
                                string[] Tags = product.ProductTags.Split(',');
                                string TagValue = string.Empty;
                                foreach (var mat in Tags)
                                {
                                    AssignedTag = new ProductAssignedTags();
                                    var TagId = TagsMasterData.Where(i => i.ProductTagName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();

                                    if (TagId != null)
                                    {
                                        AssignedTag.TagId = TagId.Id;
                                        AssignedTag.ProductMaster = master;
                                        AssignedTags.Add(AssignedTag);
                                    }
                                }
                            }
                            #endregion

                            #region Product Types
                            if (!string.IsNullOrEmpty(product.Caznerproducttypes))
                            {
                                string[] Types = product.Caznerproducttypes.Split(',');
                                string TagValue = string.Empty;
                                foreach (var mat in Types)
                                {
                                    ProductAssignedTypes AssignedType = new ProductAssignedTypes();
                                    var TypeId = TypesMasterData.Where(i => i.ProductTypeName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();

                                    if (TypeId != null)
                                    {
                                        AssignedType.TypeId = TypeId.Id;
                                        AssignedType.ProductMaster = master;
                                        AssignedTypes.Add(AssignedType);
                                    }
                                }
                            }
                            #endregion

                            #region Product Vendors
                            if (!string.IsNullOrEmpty(product.ProductVendors))
                            {
                                string[] Users = product.ProductVendors.Split(',');
                                string UserValue = string.Empty;
                                foreach (var mat in Users)
                                {
                                    AssignedVendor = new ProductAssignedVendors();
                                    var UserId = UserMasterData.ToList().Where(i => i.FullName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();
                                    if (UserId != null)
                                    {
                                        AssignedVendor.VendorUserId = UserId.Id;
                                        AssignedVendor.ProductMaster = master;
                                        AssignedVendors.Add(AssignedVendor);
                                    }
                                }
                            }
                            #endregion


                            #region Product Categories
                            if (!string.IsNullOrEmpty(product.Productcategory))
                            {

                                AssignedCategory = new ProductAssignedSubCategories();
                                var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.Productcategory.ToLower().Trim()).FirstOrDefault();
                                if (CategoryMasterId != null)
                                {
                                    AssignedCategory.SubCategoryId = CategoryMasterId.Id;
                                    AssignedCategory.ProductMaster = master;
                                    AssignedCategories.Add(AssignedCategory);
                                }
                            }
                            #endregion

                            #region Product Collections

                            if (!string.IsNullOrEmpty(product.ProductCollections))
                            {
                                string[] Collections = product.ProductCollections.Split(',');
                                foreach (var collection in Collections)
                                {
                                    AssignedCollection = new ProductAssignedCollections();
                                    var CollectionId = CollectionMasterData.ToList().Where(i => i.CollectionName.ToLower().Trim() == collection.ToLower().Trim()).FirstOrDefault();
                                    if (CollectionId != null)
                                    {
                                        AssignedCollection.CollectionId = CollectionId.Id;
                                        AssignedCollection.ProductMaster = master;
                                        AssignedCollections.Add(AssignedCollection);
                                    }

                                }
                            }
                            #endregion


                            #endregion

                            #region Product Inventory
                            inventory.ProductHeight = product.ProductHeight;
                            inventory.ProductLength = product.Productlength;
                            inventory.ProductWidth = product.ProductWidth;
                            inventory.CartonHeight = product.CartonHeight;
                            inventory.CartonLength = product.CartonLength;
                            inventory.CartonWidth = product.CartonWidth;
                            inventory.CartonWeight = product.CartonWeight;
                            inventory.CartonPackaging = product.CartonPackaging;
                            inventory.CartonNote = product.CartonNote;
                            inventory.PalletWeight = product.PalletWeight;
                            inventory.CartonPerPallet = !string.IsNullOrEmpty(product.CartonsPerPallet) ? Convert.ToDouble(product.CartonsPerPallet) : 0;
                            inventory.UnitPerPallet = !string.IsNullOrEmpty(product.UnitsPerPallet) ? Convert.ToDouble(product.UnitsPerPallet) : 0;
                            inventory.PalletNote = !string.IsNullOrEmpty(product.PalletNote) ? product.PalletNote : "";
                            inventory.TotalNumberAvailable = !string.IsNullOrEmpty(product.TotalNoAvailable) ? Convert.ToInt64(product.TotalNoAvailable) : 0;
                            inventory.AlertRestockNumber = !string.IsNullOrEmpty(product.AlertRestockAtThisNumber) ? Convert.ToInt32(product.AlertRestockAtThisNumber) : 0;
                            inventory.IsTrackQuantity = !string.IsNullOrEmpty(product.IsTrackQuantity) ? product.IsTrackQuantity.ToLower() == "true" ? true : false : false;
                            inventory.IsStopSellingStockZero = !string.IsNullOrEmpty(product.IsStopSellingStockZero) ? product.IsStopSellingStockZero.ToLower() == "true" ? true : false : false;
                            inventory.Barcode = !string.IsNullOrEmpty(product.Barcode) ? product.Barcode : "";
                            inventory.ProductPackaging = !string.IsNullOrEmpty(product.ProductPackaging) ? product.ProductPackaging : "";
                            inventory.UnitPerProduct = !string.IsNullOrEmpty(product.UnitsPerProduct) ? Convert.ToDouble(product.UnitsPerProduct) : 0;
                            inventory.UnitWeight = !string.IsNullOrEmpty(product.ProductUnitWeight) ? product.ProductUnitWeight : "";
                            inventory.CartonQuantity = !string.IsNullOrEmpty(product.CartonQuantity) ? product.CartonQuantity : "";
                            inventory.CartonWeight = !string.IsNullOrEmpty(product.CartonWeight) ? product.CartonWeight : "";
                            inventory.CartonCubicWeightKG = product.CartonCubicWeight;
                            inventory.ProductDiameter = product.ProductDiameter;
                            inventory.ProductDimensionNotes = product.ProductDimensionNotes;

                            if (!string.IsNullOrEmpty(product.UnitOfMeasure))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.UnitOfMeasure.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.ProductUnitMeasureId = MeasureId;
                                }
                            }
                            if (!string.IsNullOrEmpty(product.WeightUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.WeightUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.ProductWeightMeasureId = MeasureId;
                                }
                            }
                            if (!string.IsNullOrEmpty(product.CartonUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.CartonUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.CartonUnitOfMeasureId = MeasureId;
                                }

                            }
                            if (!string.IsNullOrEmpty(product.CartonWeightUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.CartonWeightUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.CartonWeightMeasureId = MeasureId;
                                }
                            }

                            inventory.ProductMaster = master;

                            InventoryFinalList.Add(inventory);
                            #endregion

                            #region Product Images
                            Guid MainImageName = Guid.NewGuid();
                            img = new ProductImages();
                            img.ImagePath = product.MainProductImage;
                            img.ImageName = MainImageName.ToString();
                            img.ProductMaster = master;
                            img.TenantId = TenantId;
                            img.IsDefaultImage = true;
                            images.Add(img);

                            if (!string.IsNullOrEmpty(product.ProductImages))
                            {
                                string[] ProductImages = product.ProductImages.Split(',');
                                imgObj = new ProductImages();
                                foreach (var image in ProductImages)
                                {
                                    string ext = Path.GetExtension(image);
                                    string type = "image/" + ext;
                                    imgObj = new ProductImages();
                                    Guid ImageName = Guid.NewGuid();
                                    imgObj.TenantId = TenantId;
                                    imgObj.ImagePath = image;
                                    imgObj.Url = image;
                                    imgObj.Ext = ext;
                                    imgObj.Type = type;
                                    imgObj.Name = ImageName + "." + ext;
                                    imgObj.ImageName = ImageName.ToString();
                                    imgObj.ProductMaster = master;

                                    images.Add(imgObj);
                                }
                            }
                            if (images.Count > 0)
                            {
                                ProductImagesFinalList.AddRange(images);
                            }
                            #endregion

                            #region Product Media Images
                            if (!string.IsNullOrEmpty(product.LineMediaArtImages))
                            {
                                string[] ProductImages = product.LineMediaArtImages.Split(',');
                                imgLineArts = new ProductMediaImages();
                                foreach (var image in ProductImages)
                                {
                                    string ext = Path.GetExtension(image);
                                    string type = "image/" + ext;
                                    imgLineArts = new ProductMediaImages();
                                    Guid ImageName = Guid.NewGuid();
                                    imgLineArts.TenantId = TenantId;
                                    imgLineArts.ImageUrl = image;
                                    imgLineArts.Url = image;
                                    imgLineArts.Ext = ext;
                                    imgLineArts.Type = type;
                                    imgLineArts.Name = ImageName + "." + ext;
                                    imgLineArts.ProductMediaImageTypeId = MediaType.Where(i => i.ProductMediaImageTypeName.ToLower() == "lineart").Select(i => i.Id).FirstOrDefault();
                                    imgLineArts.ImageName = ImageName.ToString();
                                    imgLineArts.ProductMaster = master;
                                    MediaImages.Add(imgLineArts);
                                }
                            }

                            if (!string.IsNullOrEmpty(product.LifeStyleImages))
                            {
                                string[] ProductImages = product.LifeStyleImages.Split(',');
                                ProductMediaImages lifeStyleImages = new ProductMediaImages();
                                foreach (var image in ProductImages)
                                {
                                    string ext = Path.GetExtension(image);
                                    string type = "image/" + ext;
                                    lifeStyleImages = new ProductMediaImages();
                                    Guid ImageName = Guid.NewGuid();
                                    lifeStyleImages.TenantId = TenantId;
                                    lifeStyleImages.ImageUrl = image;
                                    lifeStyleImages.Ext = ext;
                                    lifeStyleImages.Url = image;
                                    lifeStyleImages.Type = type;
                                    lifeStyleImages.Name = ImageName + "." + ext;
                                    lifeStyleImages.ProductMediaImageTypeId = MediaType.Where(i => i.ProductMediaImageTypeName.ToLower() == "lifestyleimages").Select(i => i.Id).FirstOrDefault();
                                    lifeStyleImages.ImageName = ImageName.ToString();
                                    lifeStyleImages.ProductMaster = master;
                                    MediaImages.Add(lifeStyleImages);
                                }
                            }

                            if (!string.IsNullOrEmpty(product.OtherMediaImages))
                            {
                                string[] ProductImages = product.OtherMediaImages.Split(',');
                                ProductMediaImages otherMediaImages = new ProductMediaImages();
                                foreach (var image in ProductImages)
                                {
                                    string ext = Path.GetExtension(image);
                                    string type = "image/" + ext;
                                    otherMediaImages = new ProductMediaImages();
                                    Guid ImageName = Guid.NewGuid();
                                    otherMediaImages.TenantId = TenantId;
                                    otherMediaImages.ImageUrl = image;
                                    otherMediaImages.Ext = ext;
                                    otherMediaImages.Url = image;
                                    otherMediaImages.Type = type;
                                    otherMediaImages.Name = ImageName + "." + ext;
                                    otherMediaImages.ProductMediaImageTypeId = MediaType.Where(i => i.ProductMediaImageTypeName.ToLower() == "othermediatype").Select(i => i.Id).FirstOrDefault();
                                    otherMediaImages.ImageName = ImageName.ToString();
                                    otherMediaImages.ProductMaster = master;
                                    MediaImages.Add(otherMediaImages);
                                }
                            }

                            if (MediaImages.Count > 0)
                            {
                                MediaImagesFinalList.AddRange(MediaImages);
                            }

                            #endregion

                            #region product view images

                            if (!string.IsNullOrEmpty(product.ProductViews))
                            {
                                string[] ProductImages = product.ProductViews.Split(',');
                                productViewImages = new ProductViewImages();
                                foreach (var image in ProductImages)
                                {
                                    string ext = Path.GetExtension(image);
                                    string type = "image/" + ext;

                                    productViewImages = new ProductViewImages();
                                    Guid ImageName = Guid.NewGuid();
                                    productViewImages.TenantId = TenantId;
                                    productViewImages.ImagePath = image;
                                    productViewImages.Ext = ext;
                                    productViewImages.Type = type;
                                    productViewImages.Name = ImageName + "." + ext;
                                    productViewImages.ImageName = ImageName.ToString();
                                    productViewImages.ProductMaster = master;
                                    ProductViewImages.Add(productViewImages);
                                }
                                if (ProductViewImages.Count > 0)
                                {
                                    ProductViewImagesFinalList.AddRange(ProductViewImages);
                                }
                            }

                            #endregion

                            #region product alternatives and relative products sku

                            if (!string.IsNullOrEmpty(product.AlternativeProducts))
                            {
                                string[] AlternativeProduct = product.AlternativeProducts.Split(',');
                                alternativeProducts = new AlternativeProducts();
                                foreach (var sku in AlternativeProduct)
                                {
                                    alternativeProducts = new AlternativeProducts();
                                    alternativeProducts.ProductMaster = master;
                                    alternativeProducts.ProductSKU = sku;
                                    alternativeProducts.IsActive = true;
                                    ProductAlternatives.Add(alternativeProducts);
                                }

                                if (ProductAlternatives.Count > 0)
                                {
                                    ProductAlternativesFinalList.AddRange(ProductAlternatives);
                                }
                            }

                            if (!string.IsNullOrEmpty(product.RelatedProducts))
                            {
                                string[] RelativeProduct = product.RelatedProducts.Split(',');
                                relativeProducts = new RelativeProducts();
                                foreach (var sku in RelativeProduct)
                                {
                                    relativeProducts = new RelativeProducts();
                                    relativeProducts.ProductMaster = master;
                                    relativeProducts.ProductSKU = sku;
                                    relativeProducts.IsActive = true;
                                    RelativeProducts.Add(relativeProducts);
                                }
                                if (RelativeProducts.Count > 0)
                                {
                                    RelativeProductsFinalList.AddRange(RelativeProducts);
                                }
                            }

                            #endregion


                            #region Product variant combinations

                            try
                            {
                                var VolumeVariantPrices = product.QuantityPriceVariantModel.Where(x => !string.IsNullOrEmpty(x.Quantity) && !string.IsNullOrEmpty(x.Price)).ToList();
                                priceVariants = (from item in VolumeVariantPrices
                                                 select new ProductVolumeDiscountVariant
                                                 {
                                                     QuantityFrom = !string.IsNullOrEmpty(item.Quantity) ? Convert.ToInt32(item.Quantity) : 0,
                                                     Price = !string.IsNullOrEmpty(item.Price) ? Convert.ToDecimal(item.Price) : 0,
                                                     IsActive = true,
                                                     ProductMaster = master
                                                 }).ToList();
                                if (priceVariants.Count > 0)
                                {
                                    priceVariantsFinalList.AddRange(priceVariants);
                                }
                            }
                            catch (Exception ex)
                            {
                                string sku = product.ParentSKU;
                            }

                            #endregion

                            #region branding locations 

                            var BrandingPositionData = product.BrandingLocationModel.Where(i => !string.IsNullOrEmpty(i.Position_Max_Height_) && !string.IsNullOrEmpty(i.Position_Max_Width_) && !string.IsNullOrEmpty(i.Branding_Location_Title_) && !string.IsNullOrEmpty(i.Branding_Location_Image_)).ToList();
                            ProductBrandingLocData = (from item in BrandingPositionData
                                                      select new ProductBrandingPosition
                                                      {
                                                          ProductMaster = master,
                                                          LayerTitle = item.Branding_Location_Title_,
                                                          PostionMaxHeight = !string.IsNullOrEmpty(item.Position_Max_Height_) ? Convert.ToDouble(item.Position_Max_Height_) : 0,
                                                          PostionMaxwidth = !string.IsNullOrEmpty(item.Position_Max_Width_) ? Convert.ToDouble(item.Position_Max_Width_) : 0,
                                                          ImageFileURL = item.Branding_Location_Image_,
                                                          ImageName = Guid.NewGuid().ToString()

                                                      }).ToList();

                            if (ProductBrandingLocData.Count > 0)
                            {
                                brandingPositionFinalList.AddRange(ProductBrandingLocData);
                            }


                            #endregion

                            #region Branding method assignments
                            if (product.BrandingMethodsseparatedbyacommarelevanttothisproduct != null)
                            {
                                string[] MethodIds = product.BrandingMethodsseparatedbyacommarelevanttothisproduct.Split(',');

                                foreach (var brandingmethodid in MethodIds)
                                {
                                    long Id = _brandingMethodMasterRepository.GetAll().Where(i => i.Id == Convert.ToInt64(brandingmethodid)).Select(i => i.Id).FirstOrDefault();
                                    if (Id > 0)
                                    {
                                        Method = new ProductBrandingMethods();
                                        Method.ProductMaster = master;
                                        Method.BrandingMethodId = Convert.ToInt64(brandingmethodid);
                                        Method.IsActive = true;
                                        ProductBrandingMethodsFinalList.Add(Method);
                                    }
                                }
                            }
                            #endregion

                            // binding product master object in list
                            ProductMasterFinalList.Add(master);

                        }

                        #endregion

                        #region Update Products
                        if (DataToUpdate.Count > 0)
                        {
                            AsyncHelper.RunSync(() => Task.Run(() => UpdateProductData(DataToUpdate, TenantId)));
                        }
                        #endregion

                        #region Master product creation process

                        if (productData.Count > 0)
                        {


                            if (CategoryGroupsList.Count > 0)
                            {
                                await CreateCategoryGroups(CategoryGroupsList.GroupBy(x => (x.CategoryGroupId, x.CategoryMasterId)).Select(i => i.First()).ToList(), TenantId);
                            }

                            if (collectionCategoryList.Count > 0)
                            {
                                await CreateCategoryCollection(collectionCategoryList.GroupBy(x => (x.CategoryId, x.CollectionId)).Select(i => i.First()).ToList(), TenantId);
                            }
                            //await CreateProductAsync(productToAddList, TenantId);
                            if (ProductMasterFinalList.Count > 0)
                            {
                                await CreateProductMasterAsync(ProductMasterFinalList, TenantId);
                            }
                           
                            if (InventoryFinalList.Count > 0)
                            {
                                await CreateProductDimensionsInventoryAsync(InventoryFinalList, TenantId);
                            }
                            if (ProductImagesFinalList.Count > 0)
                            {
                               await CreateProductImagesAsync(ProductImagesFinalList, TenantId);
                            }
                            //--------------------------
                            if (ProductViewImagesFinalList.Count > 0)
                            {
                                await CreateProductViewImagesAsync(ProductViewImagesFinalList, TenantId);
                            }
                            if (RelativeProductsFinalList.Count > 0)
                            {
                                await CreateRelativeProductsAsync(RelativeProductsFinalList, TenantId);
                            }
                            if (ProductAlternativesFinalList.Count > 0)
                            {
                                await CreateAlternativeProductsAsync(ProductAlternativesFinalList, TenantId);
                            }
                            //--------------------------
                            if (MediaImagesFinalList.Count > 0)
                            {
                                await CreateProducMediatImagesAsync(MediaImagesFinalList, TenantId);
                            }
                            if (priceVariantsFinalList.Count > 0)
                            {
                                await CreateProducVolumePriceAsync(priceVariantsFinalList, TenantId);
                            }
                            if (brandingPositionFinalList.Count > 0)
                            {
                                await CreateProducBrandingLocAsync(brandingPositionFinalList, TenantId);
                            }

                            // create product details assignment

                            if (AssignedBrands.Count > 0)
                            {
                                await CreateProductAssignedBrandsAsync(AssignedBrands, TenantId);
                            }
                            if (AssignedMaterials.Count > 0)
                            {
                                await CreateProductAssignedMaterialsAsync(AssignedMaterials, TenantId);
                            }
                            if (AssignedCollections.Count > 0)
                            {
                                await CreateProductAssignedCollectionsAsync(AssignedCollections, TenantId);
                            }
                            if (AssignedTags.Count > 0)
                            {
                                await CreateProductAssignedTagsAsync(AssignedTags, TenantId);
                            }
                            if (AssignedTypes.Count > 0)
                            {
                                await CreateProductAssignedTypesAsync(AssignedTypes, TenantId);
                            }
                            if (AssignedCategories.Count > 0)
                            {
                                await CreateProductAssignedCategoriesAsync(AssignedCategories, TenantId);
                            }
                            if (AssignedVendors.Count > 0)
                            {
                                await CreateProductAssignedVendorsAsync(AssignedVendors, TenantId);
                            }
                            if (ProductBrandingMethodsFinalList.Count > 0)
                            {
                                await CreateProductBrandingMethodsAsync(ProductBrandingMethodsFinalList, TenantId);
                            }
                        }
                        #endregion


                        #region product variant data process

                        List<ImportColorVariantsDto> VariantModel = new List<ImportColorVariantsDto>();


                        var result = (from variant in ProductVariantData
                                      select new ImportColorVariantsDto
                                      {
                                          ParentProductId = variant.Key,
                                          VariantsData = (from colorvariant in variant
                                                          select new ColorVariantsModel
                                                          {
                                                              ParentProductSKU = colorvariant.ParentSKU,
                                                              BarCode = colorvariant.Barcode,
                                                              Color = colorvariant.Colour,
                                                              Style = colorvariant.Style,
                                                              Size = colorvariant.ProductSize,
                                                              Material = colorvariant.ProductMaterial,
                                                              SKU = colorvariant.VariantSKU,
                                                              Price = colorvariant.UnitPrice,
                                                              Images = colorvariant.MainProductImage,
                                                              NextShipment = colorvariant.NextShipmentDate,
                                                              IncomingQuantity = colorvariant.NextShipmentQuantity,
                                                              IsChargeTaxVariant = colorvariant.IsChargeTax,
                                                              CostPerItem = colorvariant.CostPerItem,
                                                              IsTrackQuantity = colorvariant.IsTrackQuantity,
                                                              PriceVariantModel = colorvariant.QuantityPriceVariantModel,
                                                              DiscountPercentage = colorvariant.DiscountPercentage,
                                                              DiscountPercentageDraft = colorvariant.DiscountPercentage,
                                                              Profit = colorvariant.Profit,
                                                              OnSale = colorvariant.IsOnSale,
                                                              SaleUnitPrice = colorvariant.SalePrice,
                                                              SalePrice = colorvariant.SalePrice
                                                          }).ToList()

                                      }).ToList();

                        await CreateVariantChildData(result);


                        #endregion


                        var objEmail = new MailMessage
                        {
                            To = { "ankita.shrivastava@systematixindia.com" },
                            Subject = "Products Bulk Import successfully done",
                            Body = "File imported successfully!",
                            IsBodyHtml = true,
                        };

                        await _userEmailerService.SendEmail(objEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                string exception = ex.StackTrace  +"________________________________"+ ex.Message;
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                CreateErrorLogsWithException(line.ToString(), ex.Message, ex.StackTrace);

                var objEmail = new MailMessage
                {
                    To = { "ankita.shrivastava@systematixindia.com" },
                    Subject = "Products Bulk Import Failed",
                    Body = "Products Bulk Import Failed!",
                    IsBodyHtml = true,
                };

                //objEmail.Attachments.Add(objAttachment);
                await _userEmailerService.SendEmail(objEmail);
                //
            }
        }

       

        public virtual async Task UpdateProductData(List<ImportBulkProductDto> productDataToUpdate, int TenantId)
        {

            try
            {
                List<ProductBulkImportDataHistory> productToAddList = new List<ProductBulkImportDataHistory>();

                var UserMasterData = _userRepository.GetAll();
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    UserMasterData = _userRepository.GetAll();
                }

                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    var allProducts = _productMasterRepository.GetAll();
                    var allDimensionsInventory = _productDimensionsInventoryRepository.GetAll();
                    var allProductImages = _productImagesRepository.GetAll();
                    var allProductMediaImages = _productMediaImagesRepository.GetAll();
                    var allProductBrandingPosition = _productBrandingPositionRepository.GetAll();
                    var allProductBrandingMethods = _productBrandingMethodsRepository.GetAll();
                    var allProductPriceVariants = _productVolumeDiscountRepository.GetAll();
                    var MaterialMasterData = _productMaterialMasterRepository.GetAll();
                    var BrandingMethodData = _brandingMethodMasterRepository.GetAll();
                    var CategoryMasterData = _categoryMasterRepository.GetAll();
                    var TypesMasterData = _productTypeMasterRepository.GetAll();
                    var TagsMasterData = _productTagMasterRepository.GetAll();
                    var BrandMasterData = _productBrandMasterRepository.GetAll();
                    var MediaType = _productMediaImageTypeMasterRepository.GetAll();
                    var CollectionsData = _collectionMasterRepository.GetAll();
                    var SizeMasterData = _productSizeMasterRepository.GetAll();
                    var TurnAroundTime = _TurnAroundTimeRepository.GetAll();
                    var ProductViewImagesData = _productViewImagesRepository.GetAll();
                    var RelativeProductsData = _relativeProductsRepository.GetAll();
                    var AlternativeProductsData = _alternativeProductsRepository.GetAll();
                    List<ProductDimensionsInventory> InventoryListToBeUpdated = new List<ProductDimensionsInventory>();
                    List<ProductDimensionsInventory> InventoryListToBeInserted = new List<ProductDimensionsInventory>();
                    List<ProductMaster> ProductMasterFinalList = new List<ProductMaster>();
                    
                    List<ProductDimension> ProductDimensionListToBeUpdated = new List<ProductDimension>();
                    List<ProductMediaImages> MediaImagesListToBeInserted = new List<ProductMediaImages>();
                    List<ProductImages> ProductImagesListToBeInserted = new List<ProductImages>();
                    List<ProductVolumeDiscountVariant> priceVariantsListToBeInserted = new List<ProductVolumeDiscountVariant>();
                    List<ProductMediaImages> MediaImagesListToBeDeleted = new List<ProductMediaImages>();
                    List<ProductImages> ProductImagesListToBeDeleted = new List<ProductImages>();
                    List<ProductImages> ProductImagesListToBeUpdated = new List<ProductImages>();
                    List<ProductVolumeDiscountVariant> priceVariantsToBeDeleted = new List<ProductVolumeDiscountVariant>();
                    List<ProductAssignedMaterials> AssignedMaterialsToBeDeleted = new List<ProductAssignedMaterials>();
                    List<ProductAssignedBrands> AssignedBrandsToBeDeleted = new List<ProductAssignedBrands>();
                    List<ProductAssignedTypes> AssignedTypesToBeDeleted = new List<ProductAssignedTypes>();
                    List<ProductAssignedTags> AssignedTagsToBeDeleted = new List<ProductAssignedTags>();
                    List<ProductAssignedVendors> AssignedVendorsToBeDeleted = new List<ProductAssignedVendors>();
                    List<ProductAssignedSubCategories> AssignedCategoriesToBeDeleted = new List<ProductAssignedSubCategories>();
                    List<ProductAssignedCollections> AssignedCollectionsToBeDeleted = new List<ProductAssignedCollections>();
                    List<ProductBrandingPosition> ProductBrandingPositionFinalList  = new List<ProductBrandingPosition>();
                    List<ProductBrandingMethods> ProductBrandingMethodsFinalList = new List<ProductBrandingMethods>();
                    List<ProductBrandingMethods> ProductBrandingMethodsToBeDeleted = new List<ProductBrandingMethods>();
                    List<ProductBrandingPosition> ProductBrandingPositionToBeDeleted = new List<ProductBrandingPosition>();

                    List<ProductAssignedMaterials> AssignedMaterials = new List<ProductAssignedMaterials>();
                    List<ProductAssignedBrands> AssignedBrands = new List<ProductAssignedBrands>();
                    List<ProductAssignedTypes> AssignedTypes = new List<ProductAssignedTypes>();
                    List<ProductAssignedTags> AssignedTags = new List<ProductAssignedTags>();
                    List<ProductAssignedVendors> AssignedVendors = new List<ProductAssignedVendors>();
                    List<ProductAssignedSubCategories> AssignedCategories = new List<ProductAssignedSubCategories>();
                    List<ProductAssignedCollections> AssignedCollections = new List<ProductAssignedCollections>();
                    List<AlternativeProducts> ProductAlternativesFinalList = new List<AlternativeProducts>();
                    List<RelativeProducts> RelativeProductsFinalList = new List<RelativeProducts>();
                    List<ProductViewImages> ProductViewImagesFinalList = new List<ProductViewImages>();
                    List<AlternativeProducts> ProductAlternativesToBeDeleted = new List<AlternativeProducts>();
                    List<RelativeProducts> RelativeProductsFinalListToBeDeleted = new List<RelativeProducts>();
                    List<ProductViewImages> ProductViewImagesFinalListToBeDeleted = new List<ProductViewImages>();

                    foreach (var product in productDataToUpdate)
                    { 

                        List<ProductImages> images = new List<ProductImages>();
                        List<ProductMediaImages> MediaImages = new List<ProductMediaImages>();
                        List<ProductVolumeDiscountVariant> priceVariants = new List<ProductVolumeDiscountVariant>();
                        List<ProductBrandingPosition> ProductBrandingLocData = new List<ProductBrandingPosition>();
                        List<ProductViewImages> ProductViewImages = new List<ProductViewImages>();
                        List<AlternativeProducts> ProductAlternatives = new List<AlternativeProducts>();
                        List<RelativeProducts> RelativeProducts = new List<RelativeProducts>();


                        ProductMaster master = allProducts.Where(i => i.ProductSKU.Trim() == product.ParentSKU.Trim()).FirstOrDefault();

                        if (master == null)
                        {
                            break;
                        }

                        master.ProductSKU = product.ParentSKU;
                        master.ProductTitle = product.Name;
                        master.IsActive = true;
                        master.ShortDescripition = product.ShortDescription;
                        master.ProductDescripition = product.Description;
                        master.ColorsAvailable = product.ColorsAvailable;
                        master.Features = product.Features;
                        master.UnitOfMeasure = string.IsNullOrEmpty(product.UnitOfMeasure) ? 0 : Convert.ToInt32(product.UnitOfMeasure);
                        master.MinimumOrderQuantity = string.IsNullOrEmpty(product.MOQ) ? 0 : Convert.ToInt32(product.MOQ);
                        master.UnitPrice = !string.IsNullOrEmpty(product.UnitPrice) ? Convert.ToDecimal(product.UnitPrice) : 0;
                        master.CostPerItem = !string.IsNullOrEmpty(product.CostPerItem) ? Convert.ToDecimal(product.CostPerItem) : 0;
                        master.Profit = !string.IsNullOrEmpty(product.Profit) ? Convert.ToDecimal(product.Profit) : 0;
                        master.IsPhysicalProduct = !string.IsNullOrEmpty(product.IsPhysicalProduct) ? product.IsPhysicalProduct.ToLower() == "true" ? true : false : false;
                        master.OnSale = !string.IsNullOrEmpty(product.IsOnSale) ? product.IsOnSale.ToLower() == "true" ? true : false : false;
                        master.ChargeTaxOnThis = !string.IsNullOrEmpty(product.IsChargeTax) ? product.IsChargeTax.ToLower() == "true" ? true : false : false;
                        master.ChargeTaxOnThis = !string.IsNullOrEmpty(product.IsChargeTax) ? product.IsChargeTax.ToLower() == "true" ? true : false : false;
                        master.SalePrice = !string.IsNullOrEmpty(product.SalePrice) ? Convert.ToDecimal(product.SalePrice) : 0;
                        master.MinimumOrderQuantity = !string.IsNullOrEmpty(product.MinimumOrderQuantity) ? Convert.ToInt32(product.MinimumOrderQuantity) : 0;
                        master.DepositRequired = !string.IsNullOrEmpty(product.DepositRequired) ? Convert.ToDecimal(product.DepositRequired) : 0;
                        master.CostPerItem = !string.IsNullOrEmpty(product.CostPerItem) ? Convert.ToDecimal(product.CostPerItem) : 0;
                        master.ProductHasPriceVariant = !string.IsNullOrEmpty(product.IsProductHasPriceVariant) ? product.IsProductHasPriceVariant.ToLower() == "true" ? true : false : false;
                        master.DiscountPercentage = !string.IsNullOrEmpty(product.DiscountPercentage) ? Convert.ToDouble(product.DiscountPercentage) : 0;
                        if (!string.IsNullOrEmpty(product.TurnAroundTime))
                        {
                            master.TurnAroundTimeId = TurnAroundTime.Where(i => i.Time.ToLower() == product.TurnAroundTime.ToLower()).Select(i => i.Id).FirstOrDefault();
                        }
                        master.FrightNote = product.Freigthnote;
                        master.VolumeValue = product.VolumeValue;
                        master.VolumeUOM = product.VolumeUOM;
                        master.IfSetOtherProductTitleAndDimensoionsInThisSet = product.IfsetOtherproducttitleanddimensoionsinthisset;
                        master.CounrtyOfOrigin = product.Counrtyoforigin;
                        master.NumberOfPieces = product.NumberofPieces;
                        master.IsIndentOrder = product.IsIndentOrder == "true" ? true : false;
                        master.ColourFamily = product.ColourFamily;
                        master.PMSColourCode = product.PMSColourCode;
                        master.VideoURL = product.VideoURL;
                        master.Image360Degrees = product.Image360Degrees;
                        master.NextShipmentDate = product.NextShipmentDate;
                        master.NextShipmentQuantity = product.NextShipmentQuantity;
                        master.ExtraSetUpFee = product.ExtraSetupFee;
                        master.BrandingMethodNote = product.BrandingMethodNote;
                        master.BrandingUOM = product.BrandingUOM;

                        #region ProductDetails




                        #region Product Material
                        if (!string.IsNullOrEmpty(product.ProductMaterial))
                        {

                            var ExistingData = _productAssignedMaterialsRepository.GetAll().Where(i => i.ProductId == master.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                AssignedMaterialsToBeDeleted.AddRange(ExistingData);
                            }

                            string[] Materials = product.ProductMaterial.Split(',');
                            string Material = string.Empty;
                            foreach (var mat in Materials)
                            {
                                ProductAssignedMaterials assignedMaterials = new ProductAssignedMaterials();
                                var MaterialId = MaterialMasterData.Where(i => i.ProductMaterialName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();

                                if (MaterialId != null)
                                {
                                    assignedMaterials.MaterialId = MaterialId.Id;
                                    assignedMaterials.ProductMaster = master;
                                    AssignedMaterials.Add(assignedMaterials);
                                }
                              
                            }
                        }
                        #endregion

                        #region Product Brand
                        if (!string.IsNullOrEmpty(product.ProductBrands))
                        {

                            var ExistingData = _productAssignedBrandsRepository.GetAll().Where(i => i.ProductId == master.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                AssignedBrandsToBeDeleted.AddRange(ExistingData);
                            }

                            string[] Brands = product.ProductBrands.Split(',');
                            string BrandValue = string.Empty;
                            foreach (var mat in Brands)
                            {
                                ProductAssignedBrands AssignedBrand = new ProductAssignedBrands();
                                var BrandId = BrandMasterData.Where(i => i.ProductBrandName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();

                                if (BrandId != null)
                                {
                                    AssignedBrand.ProductBrandId = BrandId.Id;
                                    AssignedBrand.ProductMaster = master;
                                    AssignedBrands.Add(AssignedBrand);
                                }
                               
                            }
                        }
                        #endregion

                        #region Product Tags
                        if (!string.IsNullOrEmpty(product.ProductTags))
                        {
                            var ExistingData = _productAssignedTagsRepository.GetAll().Where(i => i.ProductId == master.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                AssignedTagsToBeDeleted.AddRange(ExistingData);
                            }

                            string[] Tags = product.ProductTags.Split(',');
                            string TagValue = string.Empty;
                            foreach (var mat in Tags)
                            {
                                ProductAssignedTags AssignedTag = new ProductAssignedTags();
                                var TagId = TagsMasterData.Where(i => i.ProductTagName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();

                                if (TagId != null)
                                {
                                    AssignedTag.TagId = TagId.Id;
                                    AssignedTag.ProductMaster = master;
                                    AssignedTags.Add(AssignedTag);
                                }
                              
                            }
                        }
                        #endregion

                        #region Product Types
                        if (!string.IsNullOrEmpty(product.Caznerproducttypes))
                        {
                            var ExistingData = _productAssignedTypesRepository.GetAll().Where(i => i.ProductId == master.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                AssignedTypesToBeDeleted.AddRange(ExistingData);
                            }

                            string[] Tags = product.Caznerproducttypes.Split(',');
                            string TagValue = string.Empty;
                            foreach (var mat in Tags)
                            {
                                ProductAssignedTypes AssignedType = new ProductAssignedTypes();
                                var TypeId = TypesMasterData.Where(i => i.ProductTypeName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();

                                if (TypeId != null)
                                {
                                    AssignedType.TypeId = TypeId.Id;
                                    AssignedType.ProductMaster = master;
                                    AssignedTypes.Add(AssignedType);
                                }

                            }
                        }
                        #endregion

                        #region Product Vendors
                        if (!string.IsNullOrEmpty(product.ProductVendors))
                        {
                            var ExistingData = _productAssignedVendorsRepository.GetAll().Where(i => i.ProductId == master.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                AssignedVendorsToBeDeleted.AddRange(ExistingData);
                            }

                            string[] Users = product.ProductVendors.Split(',');
                            string UserValue = string.Empty;
                            foreach (var mat in Users)
                            {
                                ProductAssignedVendors AssignedVendor = new ProductAssignedVendors();
                                var UserId = UserMasterData.ToList().Where(i => i.FullName.ToLower().Trim() == mat.ToLower().Trim()).FirstOrDefault();
                                if (UserId != null)
                                {
                                    AssignedVendor.VendorUserId = UserId.Id;
                                    AssignedVendor.ProductMaster = master;
                                    AssignedVendors.Add(AssignedVendor);
                                }
                             
                            }
                        }
                        #endregion


                        #region Product Categories
                        if (!string.IsNullOrEmpty(product.Productcategory))
                        {
                            var ExistingData = _productAssignedCategoriesRepository.GetAll().Where(i => i.ProductId == master.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                AssignedCategoriesToBeDeleted.AddRange(ExistingData);
                            }

                            ProductAssignedSubCategories AssignedCategory = new ProductAssignedSubCategories();
                            var CategoryId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.Productcategory.ToLower().Trim()).FirstOrDefault();
                            if (CategoryId != null)
                            {
                                AssignedCategory.SubCategoryId = CategoryId.Id;
                                AssignedCategory.ProductMaster = master;
                                AssignedCategories.Add(AssignedCategory);
                            }
                          
                        }
                        #endregion

                        #region Product Collections

                        if (!string.IsNullOrEmpty(product.ProductCollections))
                        {
                            var ExistingData = _productAssignedCollectionsRepository.GetAll().Where(i => i.ProductId == master.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                AssignedCollectionsToBeDeleted.AddRange(ExistingData);
                            }
                            string[] Collections = product.ProductCollections.Split(',');
                            foreach (var collection in Collections)
                            {
                                ProductAssignedCollections AssignedCollection = new ProductAssignedCollections();
                                var CollectionId = CollectionsData.ToList().Where(i => i.CollectionName.ToLower().Trim() == collection.ToLower().Trim()).FirstOrDefault();
                                if (CollectionId != null)
                                {
                                    AssignedCollection.CollectionId = CollectionId.Id;
                                    AssignedCollection.ProductMaster = master;
                                    AssignedCollections.Add(AssignedCollection);
                                }
                               
                            }
                        }
                        #endregion

                        #endregion

                        #region Product Inventory

                        ProductDimensionsInventory inventory = allDimensionsInventory.Where(i => i.ProductId == master.Id).FirstOrDefault();
                        if (inventory != null)
                        {
                            inventory.CartonHeight = product.CartonHeight;
                            inventory.CartonLength = product.CartonLength;
                            inventory.CartonWidth = product.CartonWidth;
                            inventory.CartonPackaging = product.CartonPackaging;
                            inventory.CartonNote = product.CartonNote;
                            inventory.PalletWeight = product.PalletWeight;
                            inventory.CartonPerPallet = !string.IsNullOrEmpty(product.CartonsPerPallet) ? Convert.ToDouble(product.CartonsPerPallet) : 0;
                            inventory.UnitPerPallet = !string.IsNullOrEmpty(product.UnitsPerPallet) ? Convert.ToDouble(product.UnitsPerPallet) : 0;
                            inventory.PalletNote = !string.IsNullOrEmpty(product.PalletNote) ? product.PalletNote : "";
                            inventory.TotalNumberAvailable = !string.IsNullOrEmpty(product.TotalNoAvailable) ? Convert.ToInt64(product.TotalNoAvailable) : 0;
                            inventory.AlertRestockNumber = !string.IsNullOrEmpty(product.AlertRestockAtThisNumber) ? Convert.ToInt32(product.AlertRestockAtThisNumber) : 0;
                            inventory.IsTrackQuantity = !string.IsNullOrEmpty(product.IsTrackQuantity) ? product.IsTrackQuantity.ToLower() == "true" ? true : false : false;
                            inventory.IsStopSellingStockZero = !string.IsNullOrEmpty(product.IsStopSellingStockZero) ? product.IsStopSellingStockZero.ToLower() == "true" ? true : false : false;
                            inventory.Barcode = !string.IsNullOrEmpty(product.Barcode) ? product.Barcode : "";
                            inventory.ProductPackaging = !string.IsNullOrEmpty(product.ProductPackaging) ? product.ProductPackaging : "";
                            inventory.UnitPerProduct = !string.IsNullOrEmpty(product.UnitsPerProduct) ? Convert.ToDouble(product.UnitsPerProduct) : 0;
                            inventory.UnitWeight = !string.IsNullOrEmpty(product.ProductUnitWeight) ? product.ProductUnitWeight : "";
                            inventory.CartonWeight = product.CartonWeight;
                            inventory.CartonQuantity = !string.IsNullOrEmpty(product.CartonQuantity) ? product.CartonQuantity : "";
                            inventory.CartonWeight = !string.IsNullOrEmpty(product.CartonWeight) ? product.CartonWeight : "";
                            inventory.CartonCubicWeightKG = product.CartonCubicWeight;
                            inventory.ProductDiameter = product.ProductDiameter;
                            inventory.ProductDimensionNotes = product.ProductDimensionNotes;
                            if (!string.IsNullOrEmpty(product.UnitOfMeasure))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.UnitOfMeasure.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.ProductUnitMeasureId = MeasureId;
                                }
                            }
                            if (!string.IsNullOrEmpty(product.WeightUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.WeightUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.ProductWeightMeasureId = MeasureId;
                                }
                            }
                            if (!string.IsNullOrEmpty(product.CartonUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.CartonUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.CartonUnitOfMeasureId = MeasureId;
                                }

                            }
                            if (!string.IsNullOrEmpty(product.CartonWeightUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.CartonWeightUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.CartonWeightMeasureId = MeasureId;
                                }
                            }
                            InventoryListToBeUpdated.Add(inventory);
                        }
                        else
                        {
                            ProductDimensionsInventory Newinventory = new ProductDimensionsInventory();
                            Newinventory.CartonHeight = product.CartonHeight;
                            Newinventory.CartonLength = product.CartonLength;
                            Newinventory.CartonWidth = product.CartonWidth;
                            Newinventory.CartonPackaging = product.CartonPackaging;
                            Newinventory.CartonNote = product.CartonNote;
                            Newinventory.PalletWeight = product.PalletWeight;
                            Newinventory.CartonPerPallet = !string.IsNullOrEmpty(product.CartonsPerPallet) ? Convert.ToDouble(product.CartonsPerPallet) : 0;
                            Newinventory.UnitPerPallet = !string.IsNullOrEmpty(product.UnitsPerPallet) ? Convert.ToDouble(product.UnitsPerPallet) : 0;
                            Newinventory.PalletNote = !string.IsNullOrEmpty(product.PalletNote) ? product.PalletNote : "";
                            Newinventory.TotalNumberAvailable = !string.IsNullOrEmpty(product.TotalNoAvailable) ? Convert.ToInt64(product.TotalNoAvailable) : 0;
                            Newinventory.AlertRestockNumber = !string.IsNullOrEmpty(product.AlertRestockAtThisNumber) ? Convert.ToInt32(product.AlertRestockAtThisNumber) : 0;
                            Newinventory.IsTrackQuantity = !string.IsNullOrEmpty(product.IsTrackQuantity) ? product.IsTrackQuantity.ToLower() == "true" ? true : false : false;
                            Newinventory.IsStopSellingStockZero = !string.IsNullOrEmpty(product.IsStopSellingStockZero) ? product.IsStopSellingStockZero.ToLower() == "true" ? true : false : false;
                            Newinventory.Barcode = !string.IsNullOrEmpty(product.Barcode) ? product.Barcode : "";
                            Newinventory.ProductPackaging = !string.IsNullOrEmpty(product.ProductPackaging) ? product.ProductPackaging : "";
                            Newinventory.UnitPerProduct = !string.IsNullOrEmpty(product.UnitsPerProduct) ? Convert.ToDouble(product.UnitsPerProduct) : 0;
                            Newinventory.UnitWeight = !string.IsNullOrEmpty(product.ProductUnitWeight) ? product.ProductUnitWeight : "";
                            Newinventory.CartonWeight = product.CartonWeight;
                            Newinventory.CartonQuantity = !string.IsNullOrEmpty(product.CartonQuantity) ? product.CartonQuantity : "";
                            inventory.CartonWeight = !string.IsNullOrEmpty(product.CartonWeight) ? product.CartonWeight : "";
                            inventory.CartonCubicWeightKG = product.CartonCubicWeight;
                            inventory.ProductDiameter = product.ProductDiameter;
                            inventory.ProductDimensionNotes = product.ProductDimensionNotes;
                            inventory.ProductUnitMeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.UnitOfMeasure.ToLower()).Select(i => i.Id).FirstOrDefault();
                            inventory.ProductWeightMeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.WeightUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                            inventory.CartonUnitOfMeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.CartonUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                            inventory.CartonWeightMeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.CartonWeightUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                            if (!string.IsNullOrEmpty(product.UnitOfMeasure))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.UnitOfMeasure.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.ProductUnitMeasureId = MeasureId;
                                }
                            }
                            if (!string.IsNullOrEmpty(product.WeightUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.WeightUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.ProductWeightMeasureId = MeasureId;
                                }
                            }
                            if (!string.IsNullOrEmpty(product.CartonUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.CartonUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.CartonUnitOfMeasureId = MeasureId;
                                }

                            }
                            if (!string.IsNullOrEmpty(product.CartonWeightUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.CartonWeightUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    inventory.CartonWeightMeasureId = MeasureId;
                                }
                            }
                            Newinventory.ProductId = master.Id;
                            InventoryListToBeInserted.Add(Newinventory);
                        }
                        #endregion

                        #region Product Images
                        if (!string.IsNullOrEmpty(product.MainProductImage))
                        {
                            var ProductImage = allProductImages.Where(i => i.ProductId == master.Id && i.IsDefaultImage == true).FirstOrDefault();
                            if (ProductImage != null)
                            {
                                ProductImage.ImagePath = product.MainProductImage;
                                ProductImagesListToBeUpdated.Add(ProductImage);
                            }
                            else
                            {
                                string ext = Path.GetExtension(product.MainProductImage);
                                string type = "image/" + ext;
                                ProductImages imgDefault = new ProductImages();
                                Guid ImageName = Guid.NewGuid();
                                imgDefault.TenantId = TenantId;
                                imgDefault.Ext = ext;
                                imgDefault.Name = ImageName + "." + ext;
                                imgDefault.Type = type;
                                imgDefault.Name = ImageName+"."+ ext;
                                imgDefault.ImagePath = product.MainProductImage;
                                imgDefault.ImageName = ImageName.ToString();
                                imgDefault.Url = product.MainProductImage;
                                imgDefault.ProductId = master.Id;
                                images.Add(imgDefault);
                            }
                        }

                        var ExistingProductImages = allProductImages.Where(i => i.ProductId == master.Id && i.IsDefaultImage == false).ToList();
                        if (ExistingProductImages.Count > 0)
                        {
                            ProductImagesListToBeDeleted.AddRange(ExistingProductImages);
                        }

                        if (!string.IsNullOrEmpty(product.ProductImages))
                        {
                            string[] ProductImages = product.ProductImages.Split(',');
                            ProductImages imgObj = new ProductImages();
                            foreach (var image in ProductImages)
                            {
                                imgObj = new ProductImages();
                                Guid ImageName = Guid.NewGuid();
                                imgObj.TenantId = TenantId;
                                imgObj.Url = image;
                                imgObj.ImagePath = image;
                                imgObj.ImageName = ImageName.ToString();
                                imgObj.ProductId = master.Id;
                                images.Add(imgObj);
                            }
                        }
                        if (images.Count > 0)
                        {
                            ProductImagesListToBeInserted.AddRange(images);
                        }
                        #endregion


                        #region product view images


                        var ExistingProductViewImages = ProductViewImagesData.Where(i => i.ProductId == master.Id).ToList();
                        if (ExistingProductViewImages.Count > 0)
                        {
                            ProductViewImagesFinalListToBeDeleted.AddRange(ExistingProductViewImages);
                        }

                        if (!string.IsNullOrEmpty(product.ProductViews))
                        {
                            string[] ProductImages = product.ProductViews.Split(',');
                            ProductViewImages productViewImages = new ProductViewImages();
                            foreach (var image in ProductImages)
                            {
                                string ext = Path.GetExtension(product.MainProductImage);
                                string type = "image/" + ext;
                                productViewImages = new ProductViewImages();
                                Guid ImageName = Guid.NewGuid();
                                productViewImages.TenantId = TenantId;
                                productViewImages.ImagePath = image;
                                productViewImages.Ext = ext;
                                productViewImages.Type = type;
                                productViewImages.Name = ImageName+"."+ ext;
                                productViewImages.ImageName = ImageName.ToString();
                                productViewImages.ProductMaster = master;
                                ProductViewImages.Add(productViewImages);
                            }
                            if (ProductViewImages.Count > 0)
                            {
                                ProductViewImagesFinalList.AddRange(ProductViewImages);
                            }
                        }

                        #endregion

                        #region product alternatives and relative products sku

                        var ExistingAlternative = AlternativeProductsData.Where(i => i.ProductId == master.Id).ToList();
                        if (ExistingAlternative.Count > 0)
                        {
                            ProductAlternativesToBeDeleted.AddRange(ExistingAlternative);
                        }

                        if (!string.IsNullOrEmpty(product.AlternativeProducts))
                        {
                            string[] AlternativeProduct = product.AlternativeProducts.Split(',');
                            AlternativeProducts alternativeProducts = new AlternativeProducts();
                            foreach (var sku in AlternativeProduct)
                            {
                                alternativeProducts = new AlternativeProducts();
                                alternativeProducts.ProductMaster = master;
                                alternativeProducts.ProductSKU = sku;
                                alternativeProducts.IsActive = true;
                                ProductAlternatives.Add(alternativeProducts);
                            }

                            if (ProductAlternatives.Count > 0)
                            {
                                ProductAlternativesFinalList.AddRange(ProductAlternatives);
                            }
                        }
                        //---------------------------------------related

                        var ExistingRelative = RelativeProductsData.Where(i => i.ProductId == master.Id).ToList();
                        if (ExistingRelative.Count > 0)
                        {
                            RelativeProductsFinalListToBeDeleted.AddRange(ExistingRelative);
                        }

                        if (!string.IsNullOrEmpty(product.RelatedProducts))
                        {
                            string[] RelativeProduct = product.RelatedProducts.Split(',');
                            RelativeProducts relativeProducts = new RelativeProducts();
                            foreach (var sku in RelativeProduct)
                            {
                                relativeProducts = new RelativeProducts();
                                relativeProducts.ProductMaster = master;
                                relativeProducts.ProductSKU = sku;
                                relativeProducts.IsActive = true;
                                RelativeProducts.Add(relativeProducts);
                            }
                            if (RelativeProducts.Count > 0)
                            {
                                RelativeProductsFinalList.AddRange(RelativeProducts);
                            }
                        }

                        #endregion



                        #region Product Media Images

                        #region product media images delete 

                        var lineArtTypeId = MediaType.Where(i => i.ProductMediaImageTypeName.ToLower().Trim() == "lineart").Select(i => i.Id).FirstOrDefault();
                        var ExistinglineArtImages = allProductMediaImages.Where(i => i.ProductId == master.Id && i.ProductMediaImageTypeId == lineArtTypeId).ToList();
                        if (ExistinglineArtImages.Count > 0)
                        {
                            MediaImagesListToBeDeleted.AddRange(ExistinglineArtImages);

                        }


                        var lifeStyleTypeId = MediaType.Where(i => i.ProductMediaImageTypeName.ToLower().Trim() == "lifestyleimages").Select(i => i.Id).FirstOrDefault();
                        var ExistingLifeStyleImages = allProductMediaImages.Where(i => i.ProductId == master.Id && i.ProductMediaImageTypeId == lifeStyleTypeId).ToList();
                        if (ExistingLifeStyleImages.Count > 0)
                        {
                            MediaImagesListToBeDeleted.AddRange(ExistingLifeStyleImages);

                        }

                        var OtherMediaTypeId = MediaType.Where(i => i.ProductMediaImageTypeName.ToLower().Trim() == "othermediatype").Select(i => i.Id).FirstOrDefault();
                        var ExistingOtherMediaImages = allProductMediaImages.Where(i => i.ProductId == master.Id && i.ProductMediaImageTypeId == OtherMediaTypeId).ToList();
                        if (ExistingOtherMediaImages.Count > 0)
                        {
                            MediaImagesListToBeDeleted.AddRange(ExistingOtherMediaImages);

                        }
                        #endregion

                        if (!string.IsNullOrEmpty(product.LineMediaArtImages))
                        {
                            string[] ProductImages = product.LineMediaArtImages.Split(',');
                            ProductMediaImages imgLineArts = new ProductMediaImages();
                            foreach (var image in ProductImages)
                            {
                                string ext = Path.GetExtension(product.MainProductImage);
                                string type = "image/" + ext;
                                imgLineArts = new ProductMediaImages();
                                Guid ImageName = Guid.NewGuid();
                                imgLineArts.TenantId = TenantId;
                                imgLineArts.ImageUrl = image;
                                imgLineArts.Ext = ext;
                                imgLineArts.Type = type;
                                imgLineArts.Url = image;
                                imgLineArts.Name = ImageName +"."+ ext;
                                imgLineArts.ProductMediaImageTypeId = lineArtTypeId;
                                imgLineArts.ImageName = ImageName.ToString();
                                imgLineArts.ProductId = master.Id;
                                MediaImages.Add(imgLineArts);
                            }
                        }

                        if (!string.IsNullOrEmpty(product.LifeStyleImages))
                        {

                            string[] ProductImages = product.LifeStyleImages.Split(',');
                            ProductMediaImages lifeStyleImages = new ProductMediaImages();
                            foreach (var image in ProductImages)
                            {
                                string ext = Path.GetExtension(product.MainProductImage);
                                string type = "image/" + ext;
                                lifeStyleImages = new ProductMediaImages();
                                Guid ImageName = Guid.NewGuid();
                                lifeStyleImages.TenantId = TenantId;
                                lifeStyleImages.ImageUrl = image;
                                lifeStyleImages.Ext = ext;
                                lifeStyleImages.Type = type;
                                lifeStyleImages.Name = ImageName+"."+ ext;
                                lifeStyleImages.Url = image;
                                lifeStyleImages.ProductMediaImageTypeId = lifeStyleTypeId;
                                lifeStyleImages.ImageName = ImageName.ToString();
                                lifeStyleImages.ProductId = master.Id;
                                MediaImages.Add(lifeStyleImages);
                            }
                        }

                        if (!string.IsNullOrEmpty(product.OtherMediaImages))
                        {
                            string[] ProductImages = product.OtherMediaImages.Split(',');
                            ProductMediaImages otherMediaImages = new ProductMediaImages();
                            foreach (var image in ProductImages)
                            {
                                string ext = Path.GetExtension(product.MainProductImage);
                                string type = "image/" + ext;
                                otherMediaImages = new ProductMediaImages();
                                Guid ImageName = Guid.NewGuid();
                                otherMediaImages.TenantId = TenantId;
                                otherMediaImages.ImageUrl = image;
                                otherMediaImages.Ext = ext;
                                otherMediaImages.Url = image;
                                otherMediaImages.Type = type;
                                otherMediaImages.Name = ImageName+"."+ ext;
                                otherMediaImages.ProductMediaImageTypeId = OtherMediaTypeId;
                                otherMediaImages.ImageName = ImageName.ToString();
                                otherMediaImages.ProductId = master.Id;
                                MediaImages.Add(otherMediaImages);
                            }
                        }
                        if (MediaImages.Count > 0)
                        {
                            MediaImagesListToBeInserted.AddRange(MediaImages);
                        }

                        #endregion

                        #region Product variant combinations

                        var PriceVariantsToBeDeleted = allProductPriceVariants.Where(i => i.ProductId == master.Id).ToList();
                        if(PriceVariantsToBeDeleted.Count > 0)
                        {
                            PriceVariantsToBeDeleted.AddRange(PriceVariantsToBeDeleted);
                        }
                        var VolumeVariantPrices = product.QuantityPriceVariantModel.Where(x => !string.IsNullOrEmpty(x.Quantity) && !string.IsNullOrEmpty(x.Price)).ToList();

                        priceVariants = (from item in VolumeVariantPrices
                                         select new ProductVolumeDiscountVariant
                                         {
                                             QuantityFrom = !string.IsNullOrEmpty(item.Quantity) ? Convert.ToInt32(item.Quantity) : 0,
                                             Price = !string.IsNullOrEmpty(item.Price) ? Convert.ToDecimal(item.Price) : 0,
                                             IsActive = true,
                                             ProductMaster = master
                                         }).ToList();
                        if (priceVariants.Count > 0)
                        {
                            priceVariantsListToBeInserted.AddRange(priceVariants);
                        }
                        #endregion


                        #region branding locations 

                        var BrandingLocToBeDeleted = allProductBrandingPosition.Where(i => i.ProductId == master.Id).ToList();
                        if (BrandingLocToBeDeleted.Count > 0)
                        {
                            ProductBrandingPositionToBeDeleted.AddRange(BrandingLocToBeDeleted);
                        }
                        var BrandingPositionData = product.BrandingLocationModel.Where(i => !string.IsNullOrEmpty(i.Position_Max_Height_) && !string.IsNullOrEmpty(i.Position_Max_Width_) && !string.IsNullOrEmpty(i.Branding_Location_Title_) && !string.IsNullOrEmpty(i.Branding_Location_Image_)).ToList();

                        ProductBrandingLocData = (from item in BrandingPositionData
                                                  select new ProductBrandingPosition
                                                  {
                                                      ProductMaster = master,
                                                      LayerTitle = item.Branding_Location_Title_,
                                                      PostionMaxHeight = !string.IsNullOrEmpty(item.Position_Max_Height_) ? Convert.ToDouble(item.Position_Max_Height_) : 0,
                                                      PostionMaxwidth = !string.IsNullOrEmpty(item.Position_Max_Width_) ? Convert.ToDouble(item.Position_Max_Width_) : 0,
                                                      ImageFileURL = item.Branding_Location_Image_,
                                                      ImageName = Guid.NewGuid().ToString()

                                                  }).ToList();
                        if (ProductBrandingLocData.Count > 0)
                        {
                            ProductBrandingPositionFinalList.AddRange(ProductBrandingLocData);
                        }
                        #endregion

                        #region Branding method assignments


                        var BrandingMethodsToBeDeleted = allProductBrandingMethods.Where(i => i.ProductMasterId == master.Id).ToList();
                        if (BrandingMethodsToBeDeleted.Count > 0)
                        {
                            ProductBrandingMethodsToBeDeleted.AddRange(BrandingMethodsToBeDeleted);
                        }
                        if (product.BrandingMethodsseparatedbyacommarelevanttothisproduct != null)
                        {
                            string[] MethodIds = product.BrandingMethodsseparatedbyacommarelevanttothisproduct.Split(',');

                            foreach (var brandingmethodid in MethodIds)
                            {
                                long Id = _brandingMethodMasterRepository.GetAll().Where(i => i.Id == Convert.ToInt64(brandingmethodid)).Select(i => i.Id).FirstOrDefault();
                                if (Id > 0)
                                {
                                    ProductBrandingMethods Method = new ProductBrandingMethods();
                                    Method.ProductMaster = master;
                                    Method.BrandingMethodId = Convert.ToInt64(brandingmethodid);
                                    Method.IsActive = true;
                                    ProductBrandingMethodsFinalList.Add(Method);
                                }
                            }
                        }
                        #endregion

                    }

                    if (productDataToUpdate.Count > 0)
                    {

                        //delete cases
                        if (ProductImagesListToBeDeleted.Count > 0)
                        {
                            await DeleteProductImagesAsync(ProductImagesListToBeDeleted, TenantId);
                        }
                        if (MediaImagesListToBeDeleted.Count > 0)
                        {
                            await DeleteProducMediatImagesAsync(MediaImagesListToBeDeleted, TenantId);
                        }
                        if (priceVariantsToBeDeleted.Count > 0)
                        {
                            await DeleteProducVolumePriceAsync(priceVariantsToBeDeleted, TenantId);
                        }
                        if (ProductBrandingPositionToBeDeleted.Count > 0)
                        {
                            await DeleteProductBrandingLocAsync(ProductBrandingPositionToBeDeleted, TenantId);
                        }
                        if (AssignedCategoriesToBeDeleted.Count > 0)
                        {
                            await DeleteProductAssignedCategoriesAsync(AssignedCategoriesToBeDeleted, TenantId);
                        }
                        if (AssignedMaterialsToBeDeleted.Count > 0)
                        {
                            await DeleteProductAssignedMaterialsAsync(AssignedMaterialsToBeDeleted, TenantId);
                        }
                        if (AssignedBrandsToBeDeleted.Count > 0)
                        {
                            await DeleteProductAssignedBrandsAsync(AssignedBrandsToBeDeleted, TenantId);
                        }
                        if (AssignedVendorsToBeDeleted.Count > 0)
                        {
                            await DeleteProductAssignedVendorsAsync(AssignedVendorsToBeDeleted, TenantId);
                        }
                        if (AssignedTagsToBeDeleted.Count > 0)
                        {
                            await DeleteProductAssignedTagsAsync(AssignedTagsToBeDeleted, TenantId);
                        }
                        if (AssignedTypesToBeDeleted.Count > 0)
                        {
                            await DeleteProductAssignedTypesAsync(AssignedTypesToBeDeleted, TenantId);
                        }
                        if (AssignedCollectionsToBeDeleted.Count > 0)
                        {
                            await DeleteProductAssignedCollectionsAsync(AssignedCollectionsToBeDeleted, TenantId);
                        }
                        if (AssignedCollectionsToBeDeleted.Count > 0)
                        {
                            await DeleteProductAssignedCollectionsAsync(AssignedCollectionsToBeDeleted, TenantId);
                        }

                        if (ProductViewImagesFinalListToBeDeleted.Count > 0)
                        {
                            await DeleteProductViewImagesAsync(ProductViewImagesFinalListToBeDeleted, TenantId);
                        }
                        if (RelativeProductsFinalListToBeDeleted.Count > 0)
                        {
                            await DeleteRelativeProductsAsync(RelativeProductsFinalListToBeDeleted, TenantId);
                        }
                        if (ProductAlternativesToBeDeleted.Count > 0)
                        {
                            await DeleteAlternativeProductsAsync(ProductAlternativesToBeDeleted, TenantId);
                        }
                        if (ProductBrandingMethodsToBeDeleted.Count > 0)
                        {
                            await DeleteProductBrandingMethodsAsync(ProductBrandingMethodsToBeDeleted, TenantId);
                        }

                        // update single row table data
                        if (ProductMasterFinalList.Count > 0)
                        {
                            await UpdateProductMasterAsync(ProductMasterFinalList, TenantId);
                        }
                        if (InventoryListToBeUpdated.Count > 0)
                        {
                            await UpdateProductDimensionsInventoryAsync(InventoryListToBeUpdated, TenantId);
                        }
                        if (ProductImagesListToBeUpdated.Count > 0)
                        {
                            await UpdateProductImagesAsync(ProductImagesListToBeUpdated, TenantId);
                        }
                        //create cases
                        if (ProductImagesListToBeInserted.Count > 0)
                        {
                            await CreateProductImagesAsync(ProductImagesListToBeInserted, TenantId);
                        }
                        if (MediaImagesListToBeInserted.Count > 0)
                        {
                            await CreateProducMediatImagesAsync(MediaImagesListToBeInserted, TenantId);
                        }
                        if (priceVariantsListToBeInserted.Count > 0)
                        {
                            await CreateProducVolumePriceAsync(priceVariantsListToBeInserted, TenantId);
                        }
                        if (ProductBrandingPositionFinalList.Count > 0)
                        {
                            await CreateProducBrandingLocAsync(ProductBrandingPositionFinalList, TenantId);
                        }
                        if (InventoryListToBeInserted.Count > 0)
                        {
                            await CreateProductDimensionsInventoryAsync(InventoryListToBeInserted, TenantId);
                        }
                       
                        if (ProductBrandingMethodsFinalList.Count > 0)
                        {
                            await CreateProductBrandingMethodsAsync(ProductBrandingMethodsFinalList, TenantId);
                        }
                        // create product details assignment

                        if (AssignedBrands.Count > 0)
                        {
                            await CreateProductAssignedBrandsAsync(AssignedBrands, TenantId);
                        }
                        if (AssignedMaterials.Count > 0)
                        {
                            await CreateProductAssignedMaterialsAsync(AssignedMaterials, TenantId);
                        }
                        if (AssignedCollections.Count > 0)
                        {
                            await CreateProductAssignedCollectionsAsync(AssignedCollections, TenantId);
                        }
                        if (AssignedTags.Count > 0)
                        {
                           await CreateProductAssignedTagsAsync(AssignedTags, TenantId);
                        }
                        if (AssignedTypes.Count > 0)
                        {
                            await CreateProductAssignedTypesAsync(AssignedTypes, TenantId);
                        }
                        if (AssignedCategories.Count > 0)
                        {
                            await CreateProductAssignedCategoriesAsync(AssignedCategories, TenantId);
                        }
                        if (AssignedVendors.Count > 0)
                        {
                            await CreateProductAssignedVendorsAsync(AssignedVendors, TenantId);
                        }
                        if (ProductViewImagesFinalList.Count > 0)
                        {
                            await CreateProductViewImagesAsync(ProductViewImagesFinalList, TenantId);
                        }
                        if (RelativeProductsFinalList.Count > 0)
                        {
                            await CreateRelativeProductsAsync(RelativeProductsFinalList, TenantId);
                        }
                        if (ProductAlternativesFinalList.Count > 0)
                        {
                            await CreateAlternativeProductsAsync(ProductAlternativesFinalList, TenantId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                CreateErrorLogsWithException(line.ToString(), ex.Message, ex.StackTrace);
            }
        }

        public virtual async Task CreateVariantChildData(List<ImportColorVariantsDto> productData)
        {
            int NumberOfExcelRows = productData.Count();
            int TenantId = AbpSession.TenantId.Value;
            try
            {
                List<ProductBulkImportDataHistory> productToAddList = new List<ProductBulkImportDataHistory>();
                List<ProductBulkUploadVariations> BulkVariationsFinalList = new List<ProductBulkUploadVariations>();
                List<ProductVariantOptionValues> VariantOptionValuesFinalList = new List<ProductVariantOptionValues>();
                List<ProductVariantsData> VariantsDataListFinalList = new List<ProductVariantsData>();


                List<ProductVariantdataImages> VariantImagesFinalList = new List<ProductVariantdataImages>();

                var MasterOptions = _productOptionsMasterRepository.GetAll();
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    var AllProducts = _productVariantsDataRepository.GetAll();
                    var ProductMasterData = _productMasterRepository.GetAll();

                    long CurId = _currencyMasterRepository.GetAll().Where(i => i.CurrencyName.Trim() == "$").Select(i => i.Id).FirstOrDefault();
                    List<ColorVariantsModel> VariantListExistsInDB = new List<ColorVariantsModel>();

                    List<ProductVariantQuantityPrices> priceVariantsListToBeInserted = new List<ProductVariantQuantityPrices>();
                    List<ProductVariantQuantityPrices> priceVariantsList = new List<ProductVariantQuantityPrices>();
                    List<ProductVariantsData> ProductVariantsDataList = new List<ProductVariantsData>();
                    List<ProductVariantdataImages> ProductVariantImagesList = new List<ProductVariantdataImages>();
                    foreach (var item in productData)
                    {
                        priceVariantsList = new List<ProductVariantQuantityPrices>();
                        ProductVariantsDataList = new List<ProductVariantsData>();
                        ProductVariantImagesList = new List<ProductVariantdataImages>();

                        var IsProductExists = ProductMasterData.Where(i => i.ProductSKU == item.ParentProductId).FirstOrDefault();
                        var ColorId = MasterOptions.Where(i => i.OptionName.ToLower() == "color").Select(i => i.Id).FirstOrDefault();
                        var SizeId = MasterOptions.Where(i => i.OptionName.ToLower() == "size").Select(i => i.Id).FirstOrDefault();
                        var MaterialId = MasterOptions.Where(i => i.OptionName.ToLower() == "material").Select(i => i.Id).FirstOrDefault();
                        var StyleId = MasterOptions.Where(i => i.OptionName.ToLower() == "style").Select(i => i.Id).FirstOrDefault();


                        if (IsProductExists != null)
                        {
                            string Colors = string.Empty;
                            string Size = string.Empty;
                            string Style = string.Empty;
                            string Material = string.Empty;

                            List<ColorVariantsModel> distictExcelSKU = item.VariantsData.GroupBy(p => p.SKU.Trim()).Select(g => g.LastOrDefault()).ToList();
                            var dataToInsert = distictExcelSKU.Select(v => v.SKU.Trim()).ToList().Except(AllProducts.ToList().Where(c => c.IsActive).Select(b => b.SKU).ToList()).ToList();
                            var variantDataList = distictExcelSKU;
                            var variantDataListToBeUpdated = distictExcelSKU.Where(x => !dataToInsert.Contains(x.SKU)).ToList();
                            if (variantDataListToBeUpdated.Count > 0)
                            {
                                VariantListExistsInDB.AddRange(variantDataListToBeUpdated);
                            }

                            if (variantDataList.Count > 0)
                            {
                                Colors = "'" + string.Join("','", variantDataList.Where(e => !string.IsNullOrEmpty(e.Color) && e.Color != null && e.Color != "null").Select(i => i.Color).ToArray()) + "'";
                                Size = "'" + string.Join("','", variantDataList.Where(e => !string.IsNullOrEmpty(e.Size) && e.Size != null).Select(i => i.Size).ToArray()) + "'";
                                Style = "'" + string.Join("','", variantDataList.Where(e => !string.IsNullOrEmpty(e.Style) && e.Style != null).Select(i => i.Style).ToArray()) + "'";
                                Material = "'" + string.Join("','", variantDataList.Where(e => !string.IsNullOrEmpty(e.Material) && e.Material != null).Select(i => i.Material).ToArray()) + "'";
                            }

                            variantDataList = variantDataList.Where(i => !string.IsNullOrWhiteSpace(i.SKU)).ToList();

                            foreach (var variant in variantDataList/*item.VariantsData*/)
                            {

                                if (!string.IsNullOrEmpty(variant.SKU) && (!string.IsNullOrEmpty(variant.Color) || !string.IsNullOrEmpty(variant.Size) || !string.IsNullOrEmpty(variant.Material) || !string.IsNullOrEmpty(variant.Style)))
                                {

                                    ProductVariantsData VariantData = new ProductVariantsData();
                                    List<ProductVariantOptionValues> VariantOptionValuesList = new List<ProductVariantOptionValues>();

                                    string Variant = string.Empty;
                                    string VariantMasterIds = string.Empty;
                                    if (!string.IsNullOrEmpty(variant.Color))
                                    {
                                        Variant = variant.Color;
                                        VariantMasterIds = ColorId.ToString();
                                    }
                                    if (!string.IsNullOrEmpty(variant.Size))
                                    {
                                        Variant = string.IsNullOrEmpty(Variant) ? variant.Size : Variant + "/" + variant.Size;
                                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? SizeId.ToString() : VariantMasterIds + "/" + SizeId;
                                    }
                                    if (!string.IsNullOrEmpty(variant.Material))
                                    {
                                        Variant = string.IsNullOrEmpty(Variant) ? variant.Material : Variant + "/" + variant.Material;
                                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? MaterialId.ToString() : VariantMasterIds + "/" + MaterialId;
                                    }
                                    if (!string.IsNullOrEmpty(variant.Style))
                                    {
                                        Variant = string.IsNullOrEmpty(Variant) ? variant.Style : Variant + "/" + variant.Style;
                                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? StyleId.ToString() : VariantMasterIds + "/" + StyleId;
                                    }

                                    //-----------------------------------------------------------//

                                    VariantData.ProductId = IsProductExists.Id;
                                    VariantData.VariantMasterIds = VariantMasterIds;
                                    VariantData.TenantId = TenantId;
                                    VariantData.Variant = Variant;
                                    VariantData.QuantityStockUnit = !string.IsNullOrEmpty(variant.QuantityStockUnit) ? Convert.ToDouble(variant.QuantityStockUnit) : 0;
                                    VariantData.Price = variant.Price ==  ""? 0 : Convert.ToDecimal(variant.Price);

                                    #region extra fields
                                    VariantData.ComparePrice = string.IsNullOrEmpty(variant.ComparePrice) ? 0 : Convert.ToDecimal(variant.ComparePrice);
                                    VariantData.CostPerItem = string.IsNullOrEmpty(variant.CostPerItem) ? 0 : Convert.ToDecimal(variant.CostPerItem);
                                    VariantData.Margin = string.IsNullOrEmpty(variant.Margin) ?  "": variant.Margin;
                                    VariantData.SalePrice = string.IsNullOrEmpty(variant.SalePrice) ? 0 : Convert.ToDecimal(variant.SalePrice);
                                    VariantData.DiscountPercentage = string.IsNullOrEmpty(variant.DiscountPercentage) ? 0 : Convert.ToDouble(variant.DiscountPercentage);
                                    VariantData.DiscountPercentageDraft = string.IsNullOrEmpty(variant.DiscountPercentageDraft) ? 0 : Convert.ToDouble(variant.DiscountPercentageDraft);
                                    VariantData.OnSale = variant.OnSale=="1" ? true : false;

                                    if (!string.IsNullOrEmpty(variant.ProfitCurrencySymbol))
                                    {
                                        if (variant.ProfitCurrencySymbol == "$")
                                        {
                                            VariantData.ProfitCurrencySymbol = CurId > 0 ? Convert.ToInt32(CurId) : 0;
                                        }
                                        else
                                        {
                                            VariantData.ProfitCurrencySymbol = 0;
                                        }
                                    }
                                    else
                                    {
                                        VariantData.ProfitCurrencySymbol = 0;
                                    }

                                    VariantData.Profit = string.IsNullOrEmpty(variant.Profit) ? 0 : Convert.ToDecimal(variant.Profit);
                                    VariantData.IsChargeTaxVariant = string.IsNullOrEmpty(variant.IsChargeTaxVariant) ? false : variant.IsChargeTaxVariant.ToLower().Trim() == "true" ? true : false;
                                    VariantData.IsTrackQuantity = string.IsNullOrEmpty(variant.IsTrackQuantity) ? false : variant.IsTrackQuantity.ToLower().Trim() == "true" ? true : false;
                                    VariantData.Shape = string.IsNullOrEmpty(variant.Shape) ?  "": variant.Shape;
                                    #endregion

                                    VariantData.SKU = variant.SKU;
                                    VariantData.BarCode = string.IsNullOrEmpty(variant.BarCode) ?  "": variant.BarCode;
                                    VariantData.IsActive = true;

                                    ProductVariantsDataList.Add(VariantData);
                                    if (ProductVariantsDataList.Count > 0)
                                    {
                                        VariantsDataListFinalList.AddRange(ProductVariantsDataList);
                                    }

                                    string[] VariantIds = VariantData.VariantMasterIds.Split('/');
                                    string[] VariantValues = Variant.Split('/');


                                    for (int i = 0; i < VariantIds.Length; i++)
                                    {
                                      
                                        if (VariantIds[i] !="" && VariantValues[i] !="")
                                        {
                                            ProductVariantOptionValues ModelVariantOptionValues = new ProductVariantOptionValues();
                                            ModelVariantOptionValues.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                            ModelVariantOptionValues.VariantOptionId = Convert.ToInt32(VariantIds[i]);
                                            ModelVariantOptionValues.VariantOptionValue = VariantValues[i].ToString();
                                            ModelVariantOptionValues.ProductVariantsData = VariantData;
                                            VariantOptionValuesList.Add(ModelVariantOptionValues);
                                        }
                                        //------------------------------------------
                                    }
                                    
                                    if (VariantOptionValuesList.Count > 0)
                                    {
                                        VariantOptionValuesFinalList.AddRange(VariantOptionValuesList);
                                      
                                    }

                                    #region variants images

                                    if (!string.IsNullOrEmpty(variant.Images))
                                    {

                                        string[] ProductImages = variant.Images.Split(',');
                                        foreach (var image in ProductImages)
                                        {
                                            ProductVariantdataImages Image = new ProductVariantdataImages();

                                            Guid ImageName = Guid.NewGuid();
                                            Image.TenantId = TenantId;
                                            Image.ImageURL = image;
                                            Image.ImageFileName = ImageName.ToString();
                                            Image.ProductVariantsData = VariantData;
                                            Image.ProductId = IsProductExists.Id;
                                            ProductVariantImagesList.Add(Image);
                                        }

                                        if (ProductVariantImagesList.Count > 0)
                                        {
                                            VariantImagesFinalList.AddRange(ProductVariantImagesList);
                                        }
                                    }

                                    #endregion

                                    #region price quantity variants

                                    var VolumeVariantPrices = variant.PriceVariantModel.Where(i => !string.IsNullOrEmpty(i.Quantity) && !string.IsNullOrEmpty(i.Price)).ToList();
                                    priceVariantsList = (from price in VolumeVariantPrices
                                                      select new ProductVariantQuantityPrices
                                                     {
                                                         QuantityFrom = !string.IsNullOrEmpty(price.Quantity) ? Convert.ToInt32(price.Quantity) : 0,
                                                         Price = !string.IsNullOrEmpty(price.Price) ? Convert.ToDecimal(price.Price) : 0,
                                                         IsActive = true,
                                                         ProductVariantsData = VariantData
                                                      }).ToList();

                                    if (priceVariantsList.Count > 0)
                                    {
                                        priceVariantsListToBeInserted.AddRange(priceVariantsList);
                                    }

                                    #endregion
                                }
                            }

                            if (variantDataList.Count > 0)
                            {

                                List<ProductBulkUploadVariations> BulkUploadVariationsList = new List<ProductBulkUploadVariations>();
                                foreach (var option in MasterOptions)
                                {
                                    // area for ProductBulkUploadVariations
                                    ProductBulkUploadVariations Variation = new ProductBulkUploadVariations();
                                    Variation.TenantId = TenantId;
                                    Variation.ProductId = IsProductExists.Id;
                                    Variation.productOptionId = option.Id;
                                    if (option.OptionName.ToLower().Trim() == "size")
                                    {
                                        Variation.ProductOptionValue = Size;
                                    }
                                    else if (option.OptionName.ToLower().Trim() == "material")
                                    {
                                        Variation.ProductOptionValue = Material;
                                    }
                                    else if (option.OptionName.ToLower().Trim() == "color")
                                    {
                                        Variation.ProductOptionValue = Colors;
                                    }
                                    else if (option.OptionName.ToLower().Trim() == "style")
                                    {
                                        Variation.ProductOptionValue = Style;
                                    }
                                    if (!string.IsNullOrEmpty(Variation.ProductOptionValue) && Variation.ProductOptionValue != "''")
                                    {
                                        BulkUploadVariationsList.Add(Variation);
                                    }
                                }
                                if (BulkUploadVariationsList.Count > 0)
                                {
                                    BulkVariationsFinalList.AddRange(BulkUploadVariationsList);
                                }
                            }
                        }
                    }
                    if (VariantListExistsInDB.Count > 0)
                    {
                        // method to delete existing data
                        await UpdateBulkProductVariant(VariantListExistsInDB, TenantId);
                    }

                    if (VariantsDataListFinalList.Count > 0)
                    {
                        await CreateVariantsDataAsync(VariantsDataListFinalList, TenantId);
                    }
                    if (VariantOptionValuesFinalList.Count > 0)
                    {
                        await CreateVariantsDataValuesAsync(VariantOptionValuesFinalList, TenantId);
                    }
                    if (BulkVariationsFinalList.Count > 0)
                    {
                        await CreateVariantionsAsync(BulkVariationsFinalList, TenantId);
                    }
                    if (VariantImagesFinalList.Count > 0)
                    {
                        await CreateVariantImagessAsync(VariantImagesFinalList, TenantId);
                    }
                    if (priceVariantsListToBeInserted.Count > 0)
                    {
                        await CreateVariantsPriceDataAsync(priceVariantsListToBeInserted, TenantId);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }


        public virtual async Task CreateProductVariantData(List<ImportColorVariantsDto> productData)
        {
            int NumberOfExcelRows = productData.Count();
            int TenantId = AbpSession.TenantId.Value;
            try
            {
                List<ProductBulkImportDataHistory> productToAddList = new List<ProductBulkImportDataHistory>();
                List<ProductBulkUploadVariations> BulkVariationsFinalList = new List<ProductBulkUploadVariations>();
                List<ProductVariantOptionValues> VariantOptionValuesFinalList = new List<ProductVariantOptionValues>();
                List<ProductVariantsData> VariantsDataListFinalList = new List<ProductVariantsData>();


                List<ProductVariantdataImages> VariantImagesFinalList = new List<ProductVariantdataImages>();

                var MasterOptions = _productOptionsMasterRepository.GetAll();
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    var AllProducts = _productVariantsDataRepository.GetAll();
                    var ProductMasterData = _productMasterRepository.GetAll();

                    long CurId = _currencyMasterRepository.GetAll().Where(i => i.CurrencyName.Trim() == "$").Select(i => i.Id).FirstOrDefault();
                    List<ColorVariantsModel> VariantListExistsInDB = new List<ColorVariantsModel>();

                    List<ImportColorVariantsDto> distictExcelParentSKU = productData.Where(i => !string.IsNullOrEmpty(i.ParentProductId) && i.ParentProductId != null).GroupBy(p => p.ParentProductId.Trim()).Select(g => g.LastOrDefault()).ToList();
                    List<ProductVariantsData> ProductVariantsDataList = new List<ProductVariantsData>();
                    foreach (var item in distictExcelParentSKU)
                    {
                        ProductVariantsDataList = new List<ProductVariantsData>();

                        List<ProductVariantdataImages> ProductVariantImagesList = new List<ProductVariantdataImages>();

                        var IsProductExists = ProductMasterData.Where(i => i.ProductSKU == item.ParentProductId).FirstOrDefault();
                        var ColorId = MasterOptions.Where(i => i.OptionName.ToLower() == "color").Select(i => i.Id).FirstOrDefault();
                        var SizeId = MasterOptions.Where(i => i.OptionName.ToLower() == "size").Select(i => i.Id).FirstOrDefault();
                        var MaterialId = MasterOptions.Where(i => i.OptionName.ToLower() == "material").Select(i => i.Id).FirstOrDefault();
                        var StyleId = MasterOptions.Where(i => i.OptionName.ToLower() == "style").Select(i => i.Id).FirstOrDefault();

                        if (IsProductExists != null)
                        {
                            string Colors = string.Empty;
                            string Size = string.Empty;
                            string Style = string.Empty;
                            string Material = string.Empty;

                            List<ColorVariantsModel> distictExcelSKU = item.VariantsData.GroupBy(p => p.SKU.Trim()).Select(g => g.LastOrDefault()).ToList();
                            var dataToInsert = distictExcelSKU.Select(v => v.SKU.Trim()).ToList().Except(AllProducts.ToList().Where(c => c.IsActive).Select(b => b.SKU).ToList()).ToList();
                            var variantDataList = distictExcelSKU;
                            var variantDataListToBeUpdated = distictExcelSKU.Where(x => !dataToInsert.Contains(x.SKU)).ToList();
                            if (variantDataListToBeUpdated.Count > 0)
                            {
                                VariantListExistsInDB.AddRange(variantDataListToBeUpdated);
                            }

                            if (variantDataList.Count > 0)
                            {
                                Colors = "'" + string.Join("','", variantDataList.Where(e => !string.IsNullOrEmpty(e.Color) && e.Color != null && e.Color != "null").Select(i => i.Color).ToArray()) + "'";
                                Size = "'" + string.Join("','", variantDataList.Where(e => !string.IsNullOrEmpty(e.Size) && e.Size != null).Select(i => i.Size).ToArray()) + "'";
                                Style = "'" + string.Join("','", variantDataList.Where(e => !string.IsNullOrEmpty(e.Style) && e.Style != null).Select(i => i.Style).ToArray()) + "'";
                                Material = "'" + string.Join("','", variantDataList.Where(e => !string.IsNullOrEmpty(e.Material) && e.Material != null).Select(i => i.Material).ToArray()) + "'";
                            }

                            foreach (var variant in variantDataList)
                            {

                                if (!string.IsNullOrEmpty(variant.SKU) && (!string.IsNullOrEmpty(variant.Color) || !string.IsNullOrEmpty(variant.Size) || !string.IsNullOrEmpty(variant.Material) || !string.IsNullOrEmpty(variant.Style)))
                                {

                                    ProductVariantsData VariantData = new ProductVariantsData();
                                    List<ProductVariantOptionValues> VariantOptionValuesList = new List<ProductVariantOptionValues>();

                                    string Variant = string.Empty;
                                    string VariantMasterIds = string.Empty;
                                    if (!string.IsNullOrEmpty(variant.Color))
                                    {
                                        Variant = variant.Color;
                                        VariantMasterIds = ColorId.ToString();
                                    }
                                    if (!string.IsNullOrEmpty(variant.Size))
                                    {
                                        Variant = string.IsNullOrEmpty(Variant) ? variant.Size : Variant + "/" + variant.Size;
                                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? SizeId.ToString() : VariantMasterIds + "/" + SizeId;
                                    }
                                    if (!string.IsNullOrEmpty(variant.Material))
                                    {
                                        Variant = string.IsNullOrEmpty(Variant) ? variant.Material : Variant + "/" + variant.Material;
                                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? MaterialId.ToString() : VariantMasterIds + "/" + MaterialId;
                                    }
                                    if (!string.IsNullOrEmpty(variant.Style))
                                    {
                                        Variant = string.IsNullOrEmpty(Variant) ? variant.Style : Variant + "/" + variant.Style;
                                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? StyleId.ToString() : VariantMasterIds + "/" + StyleId;
                                    }

                                    //-----------------------------------------------------------//

                                    VariantData.ProductId = IsProductExists.Id;
                                    VariantData.VariantMasterIds = VariantMasterIds;
                                    VariantData.TenantId = TenantId;
                                    VariantData.Variant = Variant;
                                    VariantData.QuantityStockUnit = !string.IsNullOrEmpty(variant.QuantityStockUnit) ? Convert.ToDouble(variant.QuantityStockUnit) : 0;
                                    VariantData.Price = variant.Price ==  ""? 0 : Convert.ToDecimal(variant.Price);

                                    #region extra fields
                                    VariantData.ComparePrice = string.IsNullOrEmpty(variant.ComparePrice) ? 0 : Convert.ToDecimal(variant.ComparePrice);
                                    VariantData.CostPerItem = string.IsNullOrEmpty(variant.CostPerItem) ? 0 : Convert.ToDecimal(variant.CostPerItem);
                                    VariantData.Margin = string.IsNullOrEmpty(variant.Margin) ?  "": variant.Margin;

                                    if (!string.IsNullOrEmpty(variant.ProfitCurrencySymbol))
                                    {
                                        if (variant.ProfitCurrencySymbol == "$")
                                        {
                                            VariantData.ProfitCurrencySymbol = CurId > 0 ? Convert.ToInt32(CurId) : 0;
                                        }
                                        else
                                        {
                                            VariantData.ProfitCurrencySymbol = 0;
                                        }
                                    }
                                    else
                                    {
                                        VariantData.ProfitCurrencySymbol = 0;
                                    }

                                    VariantData.Profit = string.IsNullOrEmpty(variant.Profit) ? 0 : Convert.ToDecimal(variant.Profit);
                                    VariantData.IsChargeTaxVariant = string.IsNullOrEmpty(variant.IsChargeTaxVariant) ? false : variant.IsChargeTaxVariant.ToLower().Trim() == "true" ? true : false;
                                    VariantData.IsTrackQuantity = string.IsNullOrEmpty(variant.IsTrackQuantity) ? false : variant.IsTrackQuantity.ToLower().Trim() == "true" ? true : false;
                                    VariantData.Shape = string.IsNullOrEmpty(variant.Shape) ? "" : variant.Shape;
                                    #endregion

                                    VariantData.SKU = variant.SKU;
                                    VariantData.BarCode = string.IsNullOrEmpty(variant.BarCode) ?  "": variant.BarCode;
                                    VariantData.IsActive = true;

                                    ProductVariantsDataList.Add(VariantData);
                                    if (ProductVariantsDataList.Count > 0)
                                    {
                                        VariantsDataListFinalList.AddRange(ProductVariantsDataList);
                                    }

                                    string[] VariantIds = VariantData.VariantMasterIds.Split('/');
                                    string[] VariantValues = Variant.Split('/');



                                    for (int i = 0; i < VariantIds.Length; i++)
                                    {
                                        if (VariantIds[i] != "" && VariantValues[i] != "")
                                        {
                                            // area for ProductVariantOptionValues
                                            ProductVariantOptionValues ModelVariantOptionValues = new ProductVariantOptionValues();
                                            ModelVariantOptionValues.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                            ModelVariantOptionValues.VariantOptionId = Convert.ToInt32(VariantIds[i]);
                                            ModelVariantOptionValues.VariantOptionValue = VariantValues[i].ToString();
                                            ModelVariantOptionValues.ProductVariantsData = VariantData;
                                            VariantOptionValuesList.Add(ModelVariantOptionValues);
                                        }
                                        //------------------------------------------
                                    }
                                    if (VariantOptionValuesList.Count > 0)
                                    {
                                        VariantOptionValuesFinalList.AddRange(VariantOptionValuesList);
                                      
                                    }

                                    #region variants images

                                    if (!string.IsNullOrEmpty(variant.Images))
                                    {

                                        string[] ProductImages = variant.Images.Split(',');
                                        foreach (var image in ProductImages)
                                        {
                                            ProductVariantdataImages Image = new ProductVariantdataImages();

                                            Guid ImageName = Guid.NewGuid();
                                            Image.TenantId = TenantId;
                                            Image.ImageURL = image;
                                            Image.ImageFileName = ImageName.ToString();
                                            Image.ProductVariantsData = VariantData;
                                            Image.ProductId = IsProductExists.Id;
                                            ProductVariantImagesList.Add(Image);
                                        }

                                        if (ProductVariantImagesList.Count > 0)
                                        {
                                            VariantImagesFinalList.AddRange(ProductVariantImagesList);
                                        }
                                    }

                                    #endregion
                                }
                            }

                            if (variantDataList.Count > 0)
                            {

                                List<ProductBulkUploadVariations> BulkUploadVariationsList = new List<ProductBulkUploadVariations>();
                                foreach (var option in MasterOptions)
                                {
                                    // area for ProductBulkUploadVariations
                                    ProductBulkUploadVariations Variation = new ProductBulkUploadVariations();
                                    Variation.TenantId = TenantId;
                                    Variation.ProductId = IsProductExists.Id;
                                    Variation.productOptionId = option.Id;
                                    if (option.OptionName.ToLower().Trim() == "size")
                                    {
                                        Variation.ProductOptionValue = Size;
                                    }
                                    else if (option.OptionName.ToLower().Trim() == "material")
                                    {
                                        Variation.ProductOptionValue = Material;
                                    }
                                    else if (option.OptionName.ToLower().Trim() == "color")
                                    {
                                        Variation.ProductOptionValue = Colors;
                                    }
                                    else if (option.OptionName.ToLower().Trim() == "style")
                                    {
                                        Variation.ProductOptionValue = Style;
                                    }
                                    if (!string.IsNullOrEmpty(Variation.ProductOptionValue) && Variation.ProductOptionValue != "''")
                                    {
                                        BulkUploadVariationsList.Add(Variation);
                                    }
                                }
                                if (BulkUploadVariationsList.Count > 0)
                                {
                                    BulkVariationsFinalList.AddRange(BulkUploadVariationsList);
                                }
                            }
                        }
                    }
                    if (VariantListExistsInDB.Count > 0)
                    {
                        // method to delete existing data
                        await UpdateBulkProductVariant(VariantListExistsInDB, TenantId);
                    }

                 
                    if (VariantOptionValuesFinalList.Count > 0)
                    {
                        await CreateVariantsDataValuesAsync(VariantOptionValuesFinalList, TenantId);
                    }
                    if (BulkVariationsFinalList.Count > 0)
                    {
                        await CreateVariantionsAsync(BulkVariationsFinalList, TenantId);
                    }
                    if (VariantImagesFinalList.Count > 0)
                    {
                        await CreateVariantImagessAsync(VariantImagesFinalList, TenantId);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public async Task UpdateBulkProductVariant(List<ColorVariantsModel> VariantData, int TenantId)
        {
            try
            {
                List<ProductVariantsData> VariantsDataListToBeDeleted = new List<ProductVariantsData>();
                List<ProductVariantOptionValues> VariantsOptionsToBeDeleted = new List<ProductVariantOptionValues>();
                List<ProductVariantdataImages> ProductVariantImagesToBeDeleted = new List<ProductVariantdataImages>();
                List<ProductBulkUploadVariations> BulkUploadVariationsToBeDeleted = new List<ProductBulkUploadVariations>();

                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    var AllProducts = _productVariantsDataRepository.GetAll();
                    var AllVariantValues = _productVariantOptionValuesRepository.GetAll();
                    var AllVariantImages = _productVariantdataImagesRepository.GetAll();
                    var AllBulkUploadVariations = _productBulkUploadVariationsRepository.GetAll();
                    var AllVariantPriceQtyData  = _productVariantQuantityPricesRepository.GetAll();
                    var ProductMasterData = _productMasterRepository.GetAll();
                    var IsProductExists = new ProductMaster();
                    List<ProductVariantQuantityPrices> VariantQuantityPricesToBeInserted = new List<ProductVariantQuantityPrices>();
                    List<ProductVariantQuantityPrices> VariantQuantityPricesToBeDeleted = new List<ProductVariantQuantityPrices>();
                    foreach (var variant in VariantData.Where(i => !string.IsNullOrEmpty(i.SKU))/*item.VariantsData*/)
                    {

                        if (!string.IsNullOrEmpty(variant.SKU) && !string.IsNullOrEmpty(variant.ParentProductSKU))
                        {
                            IsProductExists = ProductMasterData.Where(i => i.ProductSKU == variant.ParentProductSKU).FirstOrDefault();

                            List<ProductVariantOptionValues> VariantOptionValuesList = new List<ProductVariantOptionValues>();

                            var ExistingVariants = AllProducts.Where(i => i.SKU.Trim() == variant.SKU.Trim()).ToList();
                            if (ExistingVariants != null)
                            {
                                VariantsDataListToBeDeleted.AddRange(ExistingVariants);
                            }


                            if (ExistingVariants.Count > 0)
                            {
                                var ExistingVariantsOptionVal = AllVariantValues.Where(i => i.ProductVariantId == ExistingVariants.Select(i => i.Id).FirstOrDefault()).ToList();
                                if (ExistingVariantsOptionVal.Count > 0)
                                {
                                    VariantsOptionsToBeDeleted.AddRange(ExistingVariantsOptionVal);
                                }
                            }

                            #region variants images

                            if (!string.IsNullOrEmpty(variant.Images))
                            {
                                if (ExistingVariants != null)
                                {
                                    var IsImagesExists = AllVariantImages.Where(i => i.ProductVariantId == ExistingVariants.Select(i => i.Id).FirstOrDefault()).ToList();
                                    if (IsImagesExists.Count > 0)
                                    {
                                        ProductVariantImagesToBeDeleted.AddRange(IsImagesExists);
                                    }
                                }
                            }

                            #endregion

                            #region variants price qty

                            if (variant.PriceVariantModel != null)
                            {
                                var IsImagesExists = AllVariantPriceQtyData.Where(i => i.ProductVariantId == ExistingVariants.Select(i => i.Id).FirstOrDefault()).ToList();
                                if (IsImagesExists.Count > 0)
                                {
                                    VariantQuantityPricesToBeDeleted.AddRange(IsImagesExists);
                                }
                            }
                            #endregion

                        }

                        if (VariantData.Count > 0)
                        {
                            var IsVariantsExists = AllBulkUploadVariations.Where(i => i.ProductId == IsProductExists.Id).ToList();
                            if (IsVariantsExists.Count > 0)
                            {
                                BulkUploadVariationsToBeDeleted.AddRange(IsVariantsExists);
                            }

                        }

          
                    }

                    if (VariantsDataListToBeDeleted.Count > 0)
                    {
                        await DeleteProductVariantDataAsync(VariantsDataListToBeDeleted, TenantId);
                    }
                    if (VariantsOptionsToBeDeleted.Count > 0)
                    {
                        await DeleteVariantOptionValuessAsync(VariantsOptionsToBeDeleted, TenantId);
                    }
                    if (BulkUploadVariationsToBeDeleted.Count > 0)
                    {
                        await DeleteProductBulkUploadVariantsAsync(BulkUploadVariationsToBeDeleted, TenantId);
                    }
                    if (ProductVariantImagesToBeDeleted.Count > 0)
                    {
                        await DeleteProductVariantImagesAsync(ProductVariantImagesToBeDeleted, TenantId);
                    }
                    if (VariantQuantityPricesToBeDeleted.Count > 0)
                    {
                        await DeleteVariantsPriceDataAsync(VariantQuantityPricesToBeDeleted, TenantId);
                    }
                }
            }

            catch (Exception ex)
            {

            }

        }

        public virtual async Task CreateProductStockLocationDataImport(List<ImportBulkProductStockLocationDto> stockLocation)
        {
            int NumberOfExcelRows = stockLocation.Count();
            int TenantId = AbpSession.TenantId.Value;
            try
            {
                List<ProductStockLocation> VariationsListToDeleted = new List<ProductStockLocation>();
                List<ProductStockLocation> VariationsListToInserted = new List<ProductStockLocation>();

                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    var AllProducts = _productMasterRepository.GetAll();
                    var WareHouseData = _wareHouseMasterRepository.GetAll();
                    var StockLocationData = _productStockLocationRepository.GetAll();

                    foreach (var item in stockLocation)
                    {

                        List<ProductStockLocation> BulkVariationsListToInserted = new List<ProductStockLocation>();

                        var IsProductExists = AllProducts.Where(i => i.ProductSKU == item.ProductParentSKU).FirstOrDefault();
                        if (IsProductExists != null)
                        {
                            var IsStockLocationExists = StockLocationData.Where(i => i.ProductId == IsProductExists.Id).ToList();
                            if (IsStockLocationExists.Count > 0)
                            {
                                //delete case
                                VariationsListToDeleted.AddRange(IsStockLocationExists);
                            }

                            foreach (var location in item.StockLocationList)
                            {

                                if (!string.IsNullOrEmpty(location.StockKeepingUnit))
                                {
                                    ProductStockLocation model = new ProductStockLocation();
                                    model.TenantId = TenantId;
                                    model.QuantityAtLocation = !string.IsNullOrEmpty(location.QuantityAtLocation) ? Convert.ToInt32(location.QuantityAtLocation) : 0;
                                    model.StockAlertQty = !string.IsNullOrEmpty(location.StockAlertQty) ? Convert.ToInt32(location.StockAlertQty) : 0;
                                    model.LocationA = !string.IsNullOrEmpty(location.LocationA) ? location.LocationA : "";
                                    model.LocationB = !string.IsNullOrEmpty(location.LocationB) ? location.LocationB : "";
                                    model.LocationC = !string.IsNullOrEmpty(location.LocationC) ? location.LocationC : "";
                                    model.ProductId = IsProductExists.Id;
                                    var WareHouse = WareHouseData.Where(i => i.WarehouseTitle.ToLower().Trim() == location.StockKeepingUnit.ToLower().Trim()).FirstOrDefault();
                                    if (WareHouse != null)
                                    {
                                        model.WareHouseId = WareHouse.Id;
                                        BulkVariationsListToInserted.Add(model);
                                        VariationsListToInserted.AddRange(BulkVariationsListToInserted);
                                    }
                                }
                            }
                        }
                    }

                    if (VariationsListToDeleted.Count > 0)
                    {
                         DeleteProductStockLocationsAsync(VariationsListToDeleted, TenantId);
                    }
                    if (VariationsListToInserted.Count > 0)
                    {
                         CreateProductStockLocationAsync(VariationsListToInserted, TenantId);
                    }

                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public virtual async Task CreateVariantStockLocationDataImport(List<ImportVariantBulkStockLocDto> stockLocation)
        {
            int NumberOfExcelRows = stockLocation.Count();
            int TenantId = AbpSession.TenantId.Value;
            try
            {
                List<ProductVariantWarehouse> VariationsListToDeleted = new List<ProductVariantWarehouse>();
                List<ProductVariantWarehouse> VariationsListToInserted = new List<ProductVariantWarehouse>();

                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    var WareHouseMasterData = _wareHouseMasterRepository.GetAll();
                    var AllProducts = _productMasterRepository.GetAll();
                    var WareHouseData = _productVariantWarehouseRepository.GetAll();
                    var VariantData = _productVariantsDataRepository.GetAll();

                    foreach (var item in stockLocation)
                    {

                        List<ProductVariantWarehouse> BulkVariationsListToInserted = new List<ProductVariantWarehouse>();

                        var IsProductExists = AllProducts.Where(i => i.ProductSKU == item.ParentProductSKU).FirstOrDefault();
                        if (IsProductExists != null)
                        {

                            var IsProductVariantExists = VariantData.Where(i => i.SKU == item.VariantProductSKU).FirstOrDefault();

                            if (IsProductVariantExists != null)
                            {
                                var IsStockLocationExists = WareHouseData.Where(i => i.ProductVariantId == IsProductVariantExists.Id).ToList();
                                if (IsStockLocationExists.Count > 0)
                                {
                                    //delete case
                                    VariationsListToDeleted.AddRange(IsStockLocationExists);
                                }

                                foreach (var location in item.WareHouseList)
                                {

                                    if (!string.IsNullOrEmpty(location.StockKeepingUnit))
                                    {
                                        ProductVariantWarehouse model = new ProductVariantWarehouse();
                                        model.TenantId = TenantId;
                                        model.QuantityThisLocation = !string.IsNullOrEmpty(location.QuantityAtLocation) ? Convert.ToInt32(location.QuantityAtLocation) : 0;
                                        model.StockAlertQuantity = !string.IsNullOrEmpty(location.StockAlertQty) ? Convert.ToInt32(location.StockAlertQty) : 0;
                                        model.LocationA = !string.IsNullOrEmpty(location.LocationA) ? location.LocationA : "";
                                        model.LocationB = !string.IsNullOrEmpty(location.LocationB) ? location.LocationB : "";
                                        model.LocationC = !string.IsNullOrEmpty(location.LocationC) ? location.LocationC : "";
                                        model.ProductVariantId = IsProductVariantExists.Id;
                                        var WareHouse = WareHouseMasterData.Where(i => i.WarehouseTitle.ToLower().Trim() == location.StockKeepingUnit.ToLower().Trim()).FirstOrDefault();
                                        if (WareHouse != null)
                                        {
                                            model.WarehouseId = WareHouse.Id;
                                            BulkVariationsListToInserted.Add(model);
                                            VariationsListToInserted.AddRange(BulkVariationsListToInserted);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (VariationsListToDeleted.Count > 0)
                    {
                        await DeleteVariantStockLocAsync(VariationsListToDeleted, TenantId);
                    }
                    if (VariationsListToInserted.Count > 0)
                    {
                       await  CreateVariantStockLocAsync(VariationsListToInserted, TenantId);
                    }

                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public virtual async Task CreateProductBrandingPositionDataImport(List<ImportBulkBrandingLocationDto> BrandingPositionData)
        {
            int NumberOfExcelRows = BrandingPositionData.Count();
            int TenantId = AbpSession.TenantId.Value;
            try
            {
                List<ProductBrandingPosition> PositionsListToDeleted = new List<ProductBrandingPosition>();
                List<ProductBrandingPosition> PositionsListToInserted = new List<ProductBrandingPosition>();

                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    var AllProducts = _productMasterRepository.GetAll();
                    var BrandingPositionDBData = _productBrandingPositionRepository.GetAll();

                    foreach (var item in BrandingPositionData)
                    {

                        List<ProductBrandingPosition> BulkPositionListToInserted = new List<ProductBrandingPosition>();

                        var IsProductExists = AllProducts.Where(i => i.ProductSKU == item.ProductParentSKU).FirstOrDefault();
                        if (IsProductExists != null)
                        {
                            var IsStockLocationExists = BrandingPositionDBData.Where(i => i.ProductId == IsProductExists.Id).ToList();
                            if (IsStockLocationExists.Count > 0)
                            {
                                //delete case
                                PositionsListToDeleted.AddRange(IsStockLocationExists);
                            }

                            foreach (var location in item.BrandingLocationList)
                            {

                                if (!string.IsNullOrEmpty(location.LayerTitle) && !string.IsNullOrEmpty(location.ImageFileURL))
                                {
                                    Guid ImageName = Guid.NewGuid();
                                    ProductBrandingPosition model = new ProductBrandingPosition();
                                    model.TenantId = TenantId;
                                    model.LayerTitle = !string.IsNullOrEmpty(location.LayerTitle) ? location.LayerTitle : "";
                                    model.PostionMaxHeight = !string.IsNullOrEmpty(location.PositionMaxHeight) ? Convert.ToInt32(location.PositionMaxHeight) : 0;
                                    model.PostionMaxwidth = !string.IsNullOrEmpty(location.PositionMaxwidth) ? Convert.ToInt32(location.PositionMaxwidth) : 0;
                                    model.ImageFileURL = location.ImageFileURL;
                                    model.ImageName = ImageName.ToString();
                                    model.ProductId = IsProductExists.Id;
                                    BulkPositionListToInserted.Add(model);
                                    PositionsListToInserted.AddRange(BulkPositionListToInserted);

                                }
                            }
                        }
                    }

                    if (PositionsListToDeleted.Count > 0)
                    {
                        await DeleteProductBrandingPositionAsync(PositionsListToDeleted, TenantId);
                    }
                    if (PositionsListToInserted.Count > 0)
                    {
                       await  CreateProductBrandingPositionAsync(PositionsListToInserted, TenantId);
                    }

                }
            }
            catch (Exception ex)
            {
                
            }
        }



        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        private async Task CreateVariantsDataValuesAsync(List<ProductVariantOptionValues> productsVariant, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productVariantOptionValuesRepository.BulkInsertAsync(productsVariant);
                 
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        private async Task CreateVariantsDataAsync(List<ProductVariantsData> productsVariant, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {

                    await _productVariantsDataRepository.BulkInsertAsync(productsVariant);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteVariantOptionValuessAsync(List<ProductVariantOptionValues> productsOptionValues, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productVariantOptionValuesRepository.BulkDelete(productsOptionValues);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        private async Task CreateVariantionsAsync(List<ProductBulkUploadVariations> productsVariant, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productBulkUploadVariationsRepository.BulkInsertAsync(productsVariant);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProductBulkUploadVariantsAsync(List<ProductBulkUploadVariations> productsBulkVariants, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productBulkUploadVariationsRepository.BulkDelete(productsBulkVariants);
                    unitOfWork.Complete();
                }
                }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProductVariantImagesAsync(List<ProductVariantdataImages> productsVariantsImages, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productVariantdataImagesRepository.BulkDelete(productsVariantsImages);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProductVariantDataAsync(List<ProductVariantsData> productVariantsData, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productVariantsDataRepository.BulkDelete(productVariantsData);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        private async Task CreateProductStockLocationAsync(List<ProductStockLocation> products, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productStockLocationRepository.BulkInsertAsync(products);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }


        #region Product CRUD

        private async Task CreateCategoryMaster(List<CategoryMaster> categoryMaster, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _categoryMasterRepository.BulkInsertAsync(categoryMaster);
                await _unitOfWorkManager.Current.SaveChangesAsync();

                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateGroupMaster(List<CategoryGroupMaster> groupMasterData, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _categoryGroupMasterRepository.BulkInsertAsync(groupMasterData);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private async Task CreateCollectionMaster(List<CollectionMaster> CollectionMaster, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _collectionMasterRepository.BulkInsertAsync(CollectionMaster);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateCategoryCollection(List<CategoryCollections> categoryCollections, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _categoryCollectionsRepository.BulkInsertAsync(categoryCollections);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateCategoryGroups(List<CategoryGroups> categoryGroupsData, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _categoryGroupRepository.BulkInsertAsync(categoryGroupsData);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateProductTags(List<ProductTagMaster> productsTags, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productTagMasterRepository.BulkInsertAsync(productsTags);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateProductMaterial(List<ProductMaterialMaster> productsMaterial, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productMaterialMasterRepository.BulkInsertAsync(productsMaterial);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateProductBrands(List<ProductBrandMaster> productsBrands, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productBrandMasterRepository.BulkInsertAsync(productsBrands);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private async Task CreateProductMasterAsync(List<ProductMaster> productsMaster, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productMasterRepository.BulkInsertAsync(productsMaster);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateBrandingMethods(List<BrandingMethodMaster> brandMethod, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _brandingMethodMasterRepository.BulkInsertAsync(brandMethod);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateTypeMethods(List<ProductTypeMaster> typedata, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {

                   await _productTypeMasterRepository.BulkInsertAsync(typedata);
                   await _unitOfWorkManager.Current.SaveChangesAsync();
                   unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }



        private async Task UpdateProductMasterAsync(List<ProductMaster> productsMaster, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productMasterRepository.BulkUpdate(productsMaster);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region ProductDimensionsInventory CRUD
        private async Task CreateProductDimensionsInventoryAsync(List<ProductDimensionsInventory> productsInventory, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productDimensionsInventoryRepository.BulkInsertAsync(productsInventory);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task UpdateProductDimensionsInventoryAsync(List<ProductDimensionsInventory> productsInventory, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productDimensionsInventoryRepository.BulkUpdate(productsInventory);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region product view images

        private async Task CreateProductViewImagesAsync(List<ProductViewImages> productsImages, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productViewImagesRepository.BulkInsertAsync(productsImages);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private async Task DeleteProductViewImagesAsync(List<ProductViewImages> productsImages, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productViewImagesRepository.BulkDelete(productsImages);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Alternative products

        private async Task CreateAlternativeProductsAsync(List<AlternativeProducts> AlternativeProducts, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _alternativeProductsRepository.BulkInsertAsync(AlternativeProducts);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private async Task DeleteAlternativeProductsAsync(List<AlternativeProducts> AlternativeProducts, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {

                    _alternativeProductsRepository.BulkDelete(AlternativeProducts);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Relative products

        private async Task CreateRelativeProductsAsync(List<RelativeProducts> RelativeProducts, int TenantId)
        {
            try
            {

                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {

                    await _relativeProductsRepository.BulkInsertAsync(RelativeProducts);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }


        private async Task DeleteRelativeProductsAsync(List<RelativeProducts> RelativeProducts, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {

                    _relativeProductsRepository.BulkDelete(RelativeProducts);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Product Images CRUD
        private async Task CreateProductImagesAsync(List<ProductImages> productsImages, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productImagesRepository.BulkInsertAsync(productsImages);
                    unitOfWork.Complete();
                }

                }
            catch (Exception ex)
            {

            }
        }

        private async Task UpdateProductImagesAsync(List<ProductImages> productsImages, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productImagesRepository.BulkUpdate(productsImages);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProductImagesAsync(List<ProductImages> productsImages, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productImagesRepository.BulkDelete(productsImages);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Product Media Images
        private async Task CreateProducMediatImagesAsync(List<ProductMediaImages> productsImages, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productMediaImagesRepository.BulkInsertAsync(productsImages);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProducMediatImagesAsync(List<ProductMediaImages> productsImages, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productMediaImagesRepository.BulkDelete(productsImages);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Product Method Price CRUD

        private async Task CreateProducBrandingLocAsync(List<ProductBrandingPosition> PriceBrandingLocation, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productBrandingPositionRepository.BulkInsertAsync(PriceBrandingLocation);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProductBrandingLocAsync(List<ProductBrandingPosition> PriceBrandingLocation, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productBrandingPositionRepository.BulkDelete(PriceBrandingLocation);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateProducVolumePriceAsync(List<ProductVolumeDiscountVariant> PriceVariants, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productVolumeDiscountRepository.BulkInsertAsync(PriceVariants);
                    unitOfWork.Complete();
                }

                }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProducVolumePriceAsync(List<ProductVolumeDiscountVariant> PriceVariants, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productVolumeDiscountRepository.BulkDelete(PriceVariants);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProducBrandingPriceVariantsAsync(List<ProductBrandingPriceVariants> PriceVariants, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productBrandingPriceVariantsRepository.BulkDelete(PriceVariants);
                    unitOfWork.Complete();
                }

                }
            catch (Exception ex)
            {

            }

        }

        #endregion

        private async Task DeleteProductStockLocationsAsync(List<ProductStockLocation> productsStockLocData, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productStockLocationRepository.BulkDelete(productsStockLocData);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProductBrandingPositionAsync(List<ProductBrandingPosition> productsPositions, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productBrandingPositionRepository.BulkDelete(productsPositions);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }
        private async Task CreateProductBrandingPositionAsync(List<ProductBrandingPosition> BrandingPositionData, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productBrandingPositionRepository.BulkInsertAsync(BrandingPositionData);
                    unitOfWork.Complete();
                }
                }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteVariantStockLocAsync(List<ProductVariantWarehouse> VariantWareHouse, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productVariantWarehouseRepository.BulkDelete(VariantWareHouse);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateVariantStockLocAsync(List<ProductVariantWarehouse> VariantStockLocData, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productVariantWarehouseRepository.BulkInsertAsync(VariantStockLocData);
                    unitOfWork.Complete();
                }
                }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateVariantImagessAsync(List<ProductVariantdataImages> VariantImagesData, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productVariantdataImagesRepository.BulkInsertAsync(VariantImagesData);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private async Task DeleteProductAssignedBrandsAsync(List<ProductAssignedBrands> productAssignedBrands, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productAssignedBrandsRepository.BulkDelete(productAssignedBrands);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProductAssignedCollectionsAsync(List<ProductAssignedCollections> productAssignedCollections, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productAssignedCollectionsRepository.BulkDelete(productAssignedCollections);
                    unitOfWork.Complete();
                }

                }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProductAssignedMaterialsAsync(List<ProductAssignedMaterials> productAssignedMaterials, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productAssignedMaterialsRepository.BulkDelete(productAssignedMaterials);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProductAssignedCategoriesAsync(List<ProductAssignedSubCategories> productAssignedCategories, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productAssignedCategoriesRepository.BulkDelete(productAssignedCategories);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }

        }
        private async Task DeleteProductAssignedTagsAsync(List<ProductAssignedTags> productAssignedTags, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productAssignedTagsRepository.BulkDelete(productAssignedTags);
                    unitOfWork.Complete();
                }
                }
            catch (Exception ex)
            {

            }

        }
        private async Task DeleteProductAssignedTypesAsync(List<ProductAssignedTypes> productAssignedTypes, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productAssignedTypesRepository.BulkDelete(productAssignedTypes);
                    unitOfWork.Complete();
                }
                }
            catch (Exception ex)
            {

            }

        }
        private async Task DeleteProductAssignedVendorsAsync(List<ProductAssignedVendors> productAssignedVendors, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productAssignedVendorsRepository.BulkDelete(productAssignedVendors);
                    unitOfWork.Complete();
                }
                }
            catch (Exception ex)
            {

            }

        }



        private async Task CreateProductAssignedBrandsAsync(List<ProductAssignedBrands> productAssignedBrands, int TenantId)
        {

            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productAssignedBrandsRepository.BulkInsertAsync(productAssignedBrands);
                    unitOfWork.Complete();
                }

                }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateProductAssignedCollectionsAsync(List<ProductAssignedCollections> productAssignedCollections, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productAssignedCollectionsRepository.BulkInsertAsync(productAssignedCollections);
                    unitOfWork.Complete();
                }

                }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateProductAssignedMaterialsAsync(List<ProductAssignedMaterials> productAssignedMaterials, int TenantId)
        {

            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productAssignedMaterialsRepository.BulkInsertAsync(productAssignedMaterials);
                    unitOfWork.Complete();
                }
                }
            catch (Exception ex)
            {

            }
        }
        private async Task CreateProductAssignedCategoriesAsync(List<ProductAssignedSubCategories> productAssignedCategories, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productAssignedCategoriesRepository.BulkInsertAsync(productAssignedCategories);
                    unitOfWork.Complete();
                }

                }
            catch (Exception ex)
            {

            }
        }

        private async Task<bool> CreateProductAssignedTagsAsync(List<ProductAssignedTags> productAssignedTags, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productAssignedTagsRepository.BulkInsertAsync(productAssignedTags);
                    unitOfWork.Complete();
                }
                }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task CreateProductAssignedTypesAsync(List<ProductAssignedTypes> productAssignedTypes, int TenantId)
        {

            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productAssignedTypesRepository.BulkInsertAsync(productAssignedTypes);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }

        }
        private async Task CreateProductAssignedVendorsAsync(List<ProductAssignedVendors> productAssignedVendors, int TenantId)
        {

            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productAssignedVendorsRepository.BulkInsertAsync(productAssignedVendors);
                    unitOfWork.Complete();
                }
                }
            catch (Exception ex)
            {

            }
        }

        #region branding method 

        private async Task CreateProductBrandingMethodsAsync(List<ProductBrandingMethods> ProductBrandingMethods, int TenantId)
        {

            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productBrandingMethodsRepository.BulkInsertAsync(ProductBrandingMethods);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteProductBrandingMethodsAsync(List<ProductBrandingMethods> ProductBrandingMethods, int TenantId)
        {

            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productBrandingMethodsRepository.BulkDelete(ProductBrandingMethods);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region product variant quantity prices 
        
        private async Task CreateVariantsPriceDataAsync(List<ProductVariantQuantityPrices> ProductPriceQtyData, int TenantId)
        {

            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productVariantQuantityPricesRepository.BulkInsertAsync(ProductPriceQtyData);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task DeleteVariantsPriceDataAsync(List<ProductVariantQuantityPrices> ProductPriceQtyData, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productVariantQuantityPricesRepository.BulkDelete(ProductPriceQtyData);
                    unitOfWork.Complete();

                }

            }
            catch (Exception ex)
            {

            }
        }

        #region Save file in www root location of solution
        public string SaveFileInWWWRootLocation(byte[] ImgStr, string ImgName, string WebrootPath)
        {
            string FileLocation = string.Empty;
            string Msg = string.Empty;
            try
            {
                //Check if directory exist
                if (!System.IO.Directory.Exists(WebrootPath))
                {
                    System.IO.Directory.CreateDirectory(WebrootPath); //Create directory if it doesn't exist
                }

                string imageName = ImgName;

                //set the image path
                string imgPath = Path.Combine(WebrootPath, imageName);

                byte[] imageBytes = ImgStr;

                File.WriteAllBytes(imgPath, imageBytes);
                FileLocation = WebrootPath + "//" + imageName;

            }
            catch (Exception ex)
            {
                Msg = ex.Message;

            }
            return FileLocation;
        }
        #endregion

        #endregion
        public void CreateErrorLogsWithException(string Path, string message, string Ex)
        {
            try
            {
                var fileSavePath = _Environment.WebRootPath + "//swagger//ErrorLogs.txt";
                //Creating the path where pdf format of forms will be save.
                DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);



                // check if file exists and date is same then write error log in same file
                if (File.Exists(fileSavePath))
                {



                    //write to same file
                    using (StreamWriter SW = File.AppendText(fileSavePath))
                    {
                        try
                        {
                            SW.WriteLine(DateTime.Now + " ------------------------------------------------------------------------------------------------------------------");



                            SW.WriteLine(message);
                            if (!string.IsNullOrEmpty(Path))
                            {
                                SW.WriteLine("Tesseract path: " + Path);
                            }
                            if (!string.IsNullOrEmpty(Ex))
                            {
                                SW.WriteLine("Error: " + Ex);
                            }



                            //SW.Close();
                        }
                        catch (Exception exc)
                        {
                            Logger.Error("INFO: " + DateTime.UtcNow + " || Error: " + exc.StackTrace);
                        }
                        finally
                        {
                            if (SW != null)
                            {
                                SW.Flush();
                                SW.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                //CreateErrorLogs("line no 415", exc.Message);
            }
        }
    }
}
