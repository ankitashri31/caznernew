
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.Currency;
using CaznerMarketplaceBackendApp.Product.Dto;
using CaznerMarketplaceBackendApp.Product.Masters;
using FimApp.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
using System.IO;
using System.Diagnostics;
using CaznerMarketplaceBackendApp.Product.Importing.Dto;
using CaznerMarketplaceBackendApp.Storage;
using Abp.Dependency;
using Abp.Runtime.Session;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using CaznerMarketplaceBackendApp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Abp.Data;
using Microsoft.Data.SqlClient;
using CaznerMarketplaceBackendApp.Users;
using CaznerMarketplaceBackendApp.Users.Dto;
using System.Text.RegularExpressions;
using Dapper;
using CaznerMarketplaceBackendApp.Connections;
using FastMember;
using CaznerMarketplaceBackendApp.SubCategory;

namespace CaznerMarketplaceBackendApp.Product.Importing
{

    #region header constructor callings

    public class ImportProductsFromExcelJob : BackgroundJob<ImportProductFromExcelJobArgs>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        protected readonly IBinaryObjectManager _BinaryObjectManager;
        private readonly IProductListExcelDataReader _productExcelDataReader;
        private readonly IRepository<ProductMaster, long> _productMasterRepository;
        private readonly IRepository<ProductMaterialMaster, long> _productMaterialMasterRepository;
        private readonly IRepository<ProductDetails, long> _productDetailsRepository;
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
        private readonly IRepository<ProductBrandingPosition, long> _productBrandingPositionRepository;
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
        private IConfiguration _configuration;
        string DBConnection = string.Empty;
        private CaznerMarketplaceBackendAppDbContext _dbContext;
        private readonly IActiveTransactionProvider _transactionProvider;
        private readonly IRepository<UserBusinessSettings, long> _userBusinessSettings;
        private IDbConnection _db;
        private readonly DbConnectionUtility _connectionUtility;
        private readonly IRepository<SubCategoryMaster, long> _SubCategoryMasterRepository;
        private readonly IRepository<CategorySubCategories, long> _categorySubCategoriesRepository;
        private readonly IRepository<ProductAssignedCategoryMaster, long> _ProductAssignedCategoryMasterRepository;
        private readonly IRepository<ProductAssignedSubSubCategories, long> _ProductAssignedSubSubCategories;
        public ImportProductsFromExcelJob(IUnitOfWorkManager unitOfWorkManager, IBinaryObjectManager BinaryObjectManager, IProductListExcelDataReader productExcelDataReader,
            IRepository<ProductMaster, long> productMasterRepository, IRepository<ProductMaterialMaster, long> productMaterialMasterRepository, IRepository<ProductDetails, long> productDetailsRepository,
           IRepository<BrandingMethodMaster, long> brandingMethodMasterRepository, IRepository<ProductOptionsMaster, long> productOptionsMasterRepository
           , IRepository<ProductVariantOptionValues, long> productVariantOptionValuesRepository, IRepository<ProductBulkUploadVariations, long> productBulkUploadVariationsRepository,
             IRepository<ProductVariantsData, long> productVariantsDataRepository, IRepository<CurrencyMaster, long> currencyMasterRepository, IRepository<CategoryMaster, long> categoryMasterRepository, IRepository<CategoryCollections, long> categoryCollectionsRepository,
             IRepository<ProductTypeMaster, long> productTypeMasterRepository, IRepository<ProductBrandMaster, long> productBrandMasterRepository, IRepository<ProductTagMaster, long> productTagMasterRepository, IRepository<User, long> userRepository, IRepository<ProductDimensionsInventory, long> productDimensionsInventoryRepository,
             IRepository<ProductMediaImages, long> productMediaImagesRepository, IRepository<ProductImages, long> productImagesRepository, IRepository<ProductBrandingPriceVariants, long> productBrandingPriceVariantsRepository
            , IRepository<ProductMediaImageTypeMaster, long> productMediaImageTypeMasterRepository, IRepository<ProductStockLocation, long> productStockLocationRepository,
             IRepository<ProductBrandingPosition, long> productBrandingPositionRepository, IRepository<ProductVariantWarehouse, long> productVariantWarehouseRepository, IRepository<ProductVariantdataImages, long> productVariantdataImagesRepository, IRepository<CollectionMaster, long> collectionMasterRepository,
             IRepository<CategoryGroupMaster, long> categoryGroupMasterRepository, IRepository<CategoryGroups, long> categoryGroupsRepository, IRepository<ProductVolumeDiscountVariant, long> productVolumeDiscountRepository, IRepository<ProductSizeMaster, long> productSizeMasterRepository, IRepository<TurnAroundTime, long> TurnAroundTimeRepository,
             IRepository<ProductAssignedBrands, long> productAssignedBrandsRepository, IRepository<ProductAssignedMaterials, long> productAssignedMaterialsRepository, IRepository<ProductAssignedCollections, long> productAssignedCollectionsRepository, IRepository<ProductAssignedSubCategories, long> productAssignedCategoriesRepository, IRepository<ProductAssignedTypes, long> productAssignedTypesRepository
             , IRepository<ProductAssignedTags, long> productAssignedTagsRepository, IRepository<ProductAssignedVendors, long> productAssignedVendorsRepository, IRepository<ProductViewImages, long> productViewImagesRepository, IRepository<AlternativeProducts, long> alternativeProductsRepository, IRepository<RelativeProducts, long> relativeProductsRepository, IRepository<ProductBrandingMethods, long> productBrandingMethodsRepository, Microsoft.AspNetCore.Hosting.IWebHostEnvironment Environment
            , IRepository<ProductVariantQuantityPrices, long> productVariantQuantityPricesRepository, IUserEmailer userEmailerService, IConfiguration configuration, CaznerMarketplaceBackendAppDbContext dbContext, IActiveTransactionProvider transactionProvider, IRepository<UserBusinessSettings, long> userBusinessSettings, DbConnectionUtility connectionUtility, IRepository<SubCategoryMaster, long> SubCategoryMasterRepository, IRepository<CategorySubCategories, long> categorySubCategoriesRepository,
             IRepository<ProductAssignedCategoryMaster, long> ProductAssignedCategoryMasterRepository, IRepository<ProductAssignedSubSubCategories, long> ProductAssignedSubSubCategories)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _BinaryObjectManager = BinaryObjectManager;
            _productExcelDataReader = productExcelDataReader;
            _productMasterRepository = productMasterRepository;
            _productMaterialMasterRepository = productMaterialMasterRepository;
            _productDetailsRepository = productDetailsRepository;

            _brandingMethodMasterRepository = brandingMethodMasterRepository;
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
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
            _productStockLocationRepository = productStockLocationRepository;
            _productBrandingPositionRepository = productBrandingPositionRepository;
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
            _configuration = configuration;
            DBConnection = _configuration["ConnectionStrings:Default"];
            _dbContext = dbContext;
            _transactionProvider = transactionProvider;
            _userBusinessSettings = userBusinessSettings;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);
            _connectionUtility = connectionUtility;
            _SubCategoryMasterRepository = SubCategoryMasterRepository;
            _categorySubCategoriesRepository = categorySubCategoriesRepository;
            _ProductAssignedCategoryMasterRepository = ProductAssignedCategoryMasterRepository;
            _ProductAssignedSubSubCategories = ProductAssignedSubSubCategories;
        }
        #endregion

        [UnitOfWork]
        public override void Execute(ImportProductFromExcelJobArgs args)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                    {
                        CreateErrorLogsWithException("Line no 180", "CreateProductData executing started", "");
                        AsyncHelper.RunSync(() => CreateProductDataUsingTemp(args.TenantId.Value));
                    }
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex) 
            {

            }

        } 


        [UnitOfWork]
        public virtual async Task<List<ImportBulkProductDto>> GetMetadataFromExcelOrNull(ImportProductFromExcelJobArgs args)
        {
            var file = new BinaryObject();
            List<ImportBulkProductDto> Response = new List<ImportBulkProductDto>();
            try
            {
                CreateErrorLogsWithException("Line no 193", "GetMetadataFromExcelOrNull executed", "");

                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        file = await _BinaryObjectManager.GetOrNullAsync(args.BinaryObjectId);

                        if (args.BinaryObjectId != null)
                        {
                            await _BinaryObjectManager.DeleteAsync(args.BinaryObjectId);
                            await _unitOfWorkManager.Current.SaveChangesAsync();
                        }
                    }
                    Response = _productExcelDataReader.GetProductsFromExcel(file.Bytes);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            CreateErrorLogsWithException("Line no 193", "GetMetadataFromExcelOrNull Response executed", "");
            return Response;
        }

        public virtual async Task CreateProductData(List<ImportBulkProductDto> productData, int TenantId)
        {

            int NumberOfExcelRows = productData.Count();
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
                    List<ProductDetails> ProductDetailsFinalList = new List<ProductDetails>();
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

                    #region trial


                    //var OldGroupData = productData.Select(i => i.Groupcategory).Distinct().ToList();
                    //var ToBeInsertedGroups = OldGroupData.Where(p => !GroupMasterData.Any(p2 => p2.GroupTitle == p)).ToList();
                    //if (ToBeInsertedGroups.Count > 0)
                    //{

                    //    CategoryGroupMasterList = (from item in ToBeInsertedGroups
                    //                               select new CategoryGroupMaster
                    //                               {
                    //                                   GroupTitle = item,
                    //                                   IsActive = true,
                    //                                   TenantId = TenantId
                    //                               }).ToList();

                    //}

                    //var OldCategoryData = productData.Select(i => i.Productcategory).Distinct().ToList();
                    //var ToBeInsertedCategories = OldCategoryData.Where(p => !CategoryMasterData.Any(p2 => p2.CategoryTitle == p)).ToList();
                    //if (ToBeInsertedCategories.Count > 0)
                    //{

                    //    Catgorymasterlist = (from item in ToBeInsertedCategories
                    //                         select new CategoryMaster
                    //                               {
                    //                                   CategoryTitle = item,
                    //                                   IsActive = true,
                    //                                   TenantId = TenantId
                    //                               }).ToList();

                    //}

                    #endregion

                    foreach (var detail in productData)
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
                  


                    #region Main product integration and binding process

                    //----------------getting latest master data which has been inserted by sheet.
                    CategoryMasterData = _categoryMasterRepository.GetAll();
                    CollectionMasterData = _collectionMasterRepository.GetAll();
                    GroupMasterData = _categoryGroupMasterRepository.GetAll();


                    CategoryGroups CategoryGroup = new CategoryGroups();
                    ProductBulkImportDataHistory history = new ProductBulkImportDataHistory();
                    ProductDimensionsInventory inventory = new ProductDimensionsInventory();
                    ProductMaster master = new ProductMaster();
                    ProductDetails details = new ProductDetails();
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

                    foreach (var product in productData /*products.ToList()*/)
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
                        details = new ProductDetails();
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
                            master.TurnAroundTimeId = TurnAroundTimeData.Where(i => i.NumberOfDays.ToLower() == product.TurnAroundTime.ToLower()).Select(i => i.Id).FirstOrDefault();
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
                        if (!string.IsNullOrEmpty(product.MainProductImage))
                        {
                            string ext = Path.GetExtension(product.MainProductImage);
                            string type = "image/" + ext;
                            Guid MainImageName = Guid.NewGuid();
                            img = new ProductImages();
                            img.ImagePath = product.MainProductImage;
                            img.ImageName = MainImageName.ToString();
                            img.ProductMaster = master;
                            img.TenantId = TenantId;
                            img.IsDefaultImage = true;
                            imgObj.Url = product.MainProductImage;
                            imgObj.Ext = ext;
                            imgObj.Type = type;
                            imgObj.Name = MainImageName.ToString() + "." + ext;
                            images.Add(img);
                        }

                        if (!string.IsNullOrEmpty(product.ProductImages))
                        {
                            string[] ProductImages = product.ProductImages.Split(',');
                           
                            foreach (var image in ProductImages)
                            {
                                imgObj = new ProductImages();
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
                           
                            foreach (var image in ProductImages)
                            {
                                imgLineArts = new ProductMediaImages();
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
                           
                            foreach (var image in ProductImages)
                            {
                                ProductMediaImages lifeStyleImages = new ProductMediaImages();
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
                           
                            foreach (var image in ProductImages)
                            {
                                ProductMediaImages otherMediaImages = new ProductMediaImages();
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
                           
                            foreach (var image in ProductImages)
                            {
                                productViewImages = new ProductViewImages();
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
                          
                            foreach (var sku in AlternativeProduct)
                            {
                                alternativeProducts = new AlternativeProducts();
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
                        long? BrandingMeasureId = null;
                        if (!string.IsNullOrEmpty(product.BrandingUOM))
                        {
                            BrandingMeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.BrandingUOM.ToLower()).Select(i => i.Id).FirstOrDefault();

                        }
                        ProductBrandingLocData = (from item in BrandingPositionData
                                                  select new ProductBrandingPosition
                                                  {
                                                      ProductMaster = master,
                                                      LayerTitle = item.Branding_Location_Title_,
                                                      PostionMaxHeight = !string.IsNullOrEmpty(item.Position_Max_Height_) ? Convert.ToDouble(item.Position_Max_Height_) : 0,
                                                      PostionMaxwidth = !string.IsNullOrEmpty(item.Position_Max_Width_) ? Convert.ToDouble(item.Position_Max_Width_) : 0,
                                                      ImageFileURL = item.Branding_Location_Image_,
                                                      ImageName = Guid.NewGuid().ToString(),
                                                      UnitOfMeasureId = BrandingMeasureId.HasValue ? BrandingMeasureId : null
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
                                long Id = _brandingMethodMasterRepository.GetAllList(i => i.UniqueNumber == Convert.ToInt64(brandingmethodid)).Select(i => i.Id).FirstOrDefault();
                                if (Id > 0)
                                {
                                    Method = new ProductBrandingMethods();
                                    Method.ProductMaster = master;
                                    Method.BrandingMethodId = Id;
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
                   // commented this code to avoid product update / delete, import must always work for new products not for existing products otherwise it will create a mess in tables.
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
                    
                        if (ProductDetailsFinalList.Count > 0)
                        {
                            await CreateProductDetailsAsync(ProductDetailsFinalList, TenantId);
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
                        if (ProductMasterFinalList.Count > 0)
                        {
                            await CreateProductMasterAsync(ProductMasterFinalList, TenantId);
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
                                                          // Shape = variant.shape
                                                          IsChargeTaxVariant = colorvariant.IsChargeTax,
                                                          CostPerItem = colorvariant.CostPerItem,
                                                          IsTrackQuantity = colorvariant.IsTrackQuantity,
                                                          PriceVariantModel = colorvariant.QuantityPriceVariantModel,
                                                          DiscountPercentage = colorvariant.DiscountPercentage,
                                                          DiscountPercentageDraft = colorvariant.DiscountPercentage,
                                                          Profit = colorvariant.Profit,
                                                          OnSale = colorvariant.IsOnSale,
                                                          SaleUnitPrice = colorvariant.UnitPrice,
                                                          SalePrice = colorvariant.SalePrice
                                                      }).ToList()

                                  }).ToList();

                    await CreateVariantChildData(result, TenantId);

                    #endregion

                    CreateErrorLogsWithException("1334", "CreateProducts successfully executed", "");

                }

            }
            catch (Exception ex)
            {
                string exception = ex.StackTrace + "________________________________" + ex.Message;
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                CreateErrorLogsWithException(line.ToString(), ex.Message, ex.StackTrace);
            }
        }

        private async Task EnsureConnectionOpenAsync()
        {
            var connection = _dbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
        }
        private DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = _dbContext.Database.GetDbConnection().CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }
        private DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
                {
                {"ContextType", typeof(CaznerMarketplaceBackendAppDbContext) },
                {"MultiTenancySide", null }
                });
        }

        public async Task<DataTable> GetProductBulkData()
        {
            await EnsureConnectionOpenAsync();
            var dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(DBConnection))
                {
                    connection.Open();

                    using (var command = CreateCommand("SELECT * FROM ProductBulkImportRawData", CommandType.Text))
                    {
                        using (var scope = new TransactionScope())
                        {
                            using (var dataReader = await command.ExecuteReaderAsync())
                            {
                                var result = new List<ProductBulkImportRawData>();

                                dt.Load(dataReader);


                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dt;

        }

        public async Task DeleteProductBulkData()
        {
            await EnsureConnectionOpenAsync();
           
            try
            {
                using (var connection = new SqlConnection(DBConnection))
                {
                    connection.Open();

                    using (var command = CreateCommand("Delete FROM ProductBulkImportRawData", CommandType.Text))
                    {
                        using (var scope = new TransactionScope())
                        {
                            using (var dataReader = await command.ExecuteReaderAsync())
                            {

                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<QuantityPriceVariantModel>> ConvertPriceDataToList(RawPriceModel obj)
        {
            List<QuantityPriceVariantModel> PriceQuantityData = new List<QuantityPriceVariantModel>();
            for (int i = 1; i <= 8; i++)
            {
                QuantityPriceVariantModel model = new QuantityPriceVariantModel();
                string Qty = string.Empty;
                string Price = string.Empty;
                switch (i)
                {
                    case 1:
                        Qty = obj.Q1;
                        Price = obj.P1;
                        if (string.IsNullOrEmpty(Qty) && string.IsNullOrEmpty(Price))
                        {
                            break;
                        }
                        else
                        {
                            model.Quantity = Qty;
                            model.Price = Price;
                            PriceQuantityData.Add(model);
                        }
                        break;

                    case 2:
                        Qty = obj.Q2;
                        Price = obj.P2;
                        if (string.IsNullOrEmpty(Qty) && string.IsNullOrEmpty(Price))
                        {
                            break;
                        }
                        else
                        {
                            model.Quantity = Qty;
                            model.Price = Price;
                            PriceQuantityData.Add(model);
                        }
                        break;

                    case 3:
                        Qty = obj.Q3;
                        Price = obj.P3;
                        if (string.IsNullOrEmpty(Qty) && string.IsNullOrEmpty(Price))
                        {
                            break;
                        }
                        else
                        {
                            model.Quantity = Qty;
                            model.Price = Price;
                            PriceQuantityData.Add(model);
                        }
                        break;

                    case 4:
                        Qty = obj.Q4;
                        Price = obj.P4;
                        if (string.IsNullOrEmpty(Qty) && string.IsNullOrEmpty(Price))
                        {
                            break;
                        }
                        else
                        {
                            model.Quantity = Qty;
                            model.Price = Price;
                            PriceQuantityData.Add(model);
                        }
                        break;

                    case 5:
                        Qty = obj.Q5;
                        Price = obj.P5;
                        if (string.IsNullOrEmpty(Qty) && string.IsNullOrEmpty(Price))
                        {
                            break;
                        }
                        else
                        {
                            model.Quantity = Qty;
                            model.Price = Price;
                            PriceQuantityData.Add(model);
                        }
                        break;


                    case 6:
                        Qty = obj.Q6;
                        Price = obj.P6;
                        if (string.IsNullOrEmpty(Qty) && string.IsNullOrEmpty(Price))
                        {
                            break;
                        }
                        else
                        {
                            model.Quantity = Qty;
                            model.Price = Price;
                            PriceQuantityData.Add(model);
                        }
                        break;

                    case 7:
                        Qty = obj.Q7;
                        Price = obj.P7;
                        if (string.IsNullOrEmpty(Qty) && string.IsNullOrEmpty(Price))
                        {
                            break;
                        }
                        else
                        {
                            model.Quantity = Qty;
                            model.Price = Price;
                            PriceQuantityData.Add(model);
                        }
                        break;

                    case 8:
                        Qty = obj.Q8;
                        Price = obj.P8;
                        if (string.IsNullOrEmpty(Qty) && string.IsNullOrEmpty(Price))
                        {
                            break;
                        }
                        else
                        {
                            model.Quantity = Qty;
                            model.Price = Price;
                            PriceQuantityData.Add(model);
                        }
                        break;
                }
            }

            return PriceQuantityData;

        }

        public async Task<List<BrandingLocationModel>> ConvertBrandingDataToList(RawBrandingModel obj)
        {
            List<BrandingLocationModel> BrandingLocations = new List<BrandingLocationModel>();

            for (int i = 1; i <= 20; i++)
            {
                BrandingLocationModel model = new BrandingLocationModel();
                string Title = string.Empty;
                switch (i)
                {
                    case 1:

                        Title = obj.Branding_Location_Title_1;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_1;
                            model.Position_Max_Width_ = obj.Position_Max_Width_1;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_1;
                            BrandingLocations.Add(model);
                        }

                        break;
                    case 2:
                        Title = obj.Branding_Location_Title_2;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_2;
                            model.Position_Max_Width_ = obj.Position_Max_Width_2;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_2;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 3:
                        Title = obj.Branding_Location_Title_3;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_3;
                            model.Position_Max_Width_ = obj.Position_Max_Width_3;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_3;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 4:
                        Title = obj.Branding_Location_Title_4;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_4;
                            model.Position_Max_Width_ = obj.Position_Max_Width_4;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_4;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 5:
                        Title = obj.Branding_Location_Title_5;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_5;
                            model.Position_Max_Width_ = obj.Position_Max_Width_5;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_5;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 6:
                        Title = obj.Branding_Location_Title_6;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_6;
                            model.Position_Max_Width_ = obj.Position_Max_Width_6;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_6;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 7:
                        Title = obj.Branding_Location_Title_7;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_7;
                            model.Position_Max_Width_ = obj.Position_Max_Width_7;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_7;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 8:
                        Title = obj.Branding_Location_Title_8;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_8;
                            model.Position_Max_Width_ = obj.Position_Max_Width_8;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_8;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 9:
                        Title = obj.Branding_Location_Title_9;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_9;
                            model.Position_Max_Width_ = obj.Position_Max_Width_9;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_9;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 10:
                        Title = obj.Branding_Location_Title_10;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_10;
                            model.Position_Max_Width_ = obj.Position_Max_Width_10;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_10;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 11:
                        Title = obj.Branding_Location_Title_11;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_11;
                            model.Position_Max_Width_ = obj.Position_Max_Width_11;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_11;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 12:
                        Title = obj.Branding_Location_Title_12;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_12;
                            model.Position_Max_Width_ = obj.Position_Max_Width_12;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_12;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 13:
                        Title = obj.Branding_Location_Title_13;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_13;
                            model.Position_Max_Width_ = obj.Position_Max_Width_13;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_13;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 14:
                        Title = obj.Branding_Location_Title_14;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_14;
                            model.Position_Max_Width_ = obj.Position_Max_Width_14;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_14;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 15:
                        Title = obj.Branding_Location_Title_15;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_15;
                            model.Position_Max_Width_ = obj.Position_Max_Width_15;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_15;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 16:
                        Title = obj.Branding_Location_Title_16;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_16;
                            model.Position_Max_Width_ = obj.Position_Max_Width_16;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_16;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 17:
                        Title = obj.Branding_Location_Title_17;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_17;
                            model.Position_Max_Width_ = obj.Position_Max_Width_17;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_17;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 18:
                        Title = obj.Branding_Location_Title_18;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_18;
                            model.Position_Max_Width_ = obj.Position_Max_Width_18;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_18;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 19:
                        Title = obj.Branding_Location_Title_19;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_19;
                            model.Position_Max_Width_ = obj.Position_Max_Width_19;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_19;
                            BrandingLocations.Add(model);
                        }
                        break;
                    case 20:
                        Title = obj.Branding_Location_Title_20;
                        if (string.IsNullOrEmpty(Title))
                        {
                            break;
                        }
                        else
                        {
                            model.Branding_Location_Title_ = Title;
                            model.Position_Max_Height_ = obj.Position_Max_Height_20;
                            model.Position_Max_Width_ = obj.Position_Max_Width_20;
                            model.Branding_Location_Image_ = obj.Branding_Location_Image_20;
                            BrandingLocations.Add(model);
                        }
                        break;

                    //default:
                    //    // code block
                    //    break;
                }


            }
            return BrandingLocations;
        }



        public async Task<List<ImportBulkProductDto>> ConvertToImportModel(DataTable obj)
        {
            List<ImportBulkProductDto> Data = new List<ImportBulkProductDto>();

            try
            {
                Data = (from DataRow P in obj.Rows
                        select new ImportBulkProductDto
                        {

                            MainorVariant = P["MainorVariant"].ToString(),
                            ParentSKU = P["ParentSKU"].ToString(),
                            VariantSKU = P["VariantSKU"].ToString(),
                            Name = P["Name"].ToString(),
                            CategoryImage = "",
                            //Groupcategory = P["Groupcategory"].ToString(),
                            //Productcategory = P["Productcategory"].ToString(),
                            GroupCategory1 = P["GroupCategory1"].ToString(),
                            SubCategory1 = P["SubCategory1"].ToString(),
                            SubSubCategory1 = P["SubSubCategory1"].ToString(),
                            GroupCategory2 = P["GroupCategory2"].ToString(),
                            SubCategory2 = P["SubCategory2"].ToString(),
                            SubSubCategory2 = P["SubSubCategory2"].ToString(),
                            GroupCategory3 = P["GroupCategory3"].ToString(),
                            SubCategory3 = P["SubCategory3"].ToString(),
                            SubSubCategory3 = P["SubSubCategory3"].ToString(),
                            Description = P["Description"].ToString(),
                            Features = P["Features"].ToString(),
                            Caznerproducttypes = P["Caznerproducttypes"].ToString(),
                            ProductSize = P["ProductSize"].ToString(),
                            Colour = P["Colour"].ToString(),
                            ProductMaterial = P["ProductMaterial"].ToString(),
                            Style = P["Style"].ToString(),
                            ProductBrands = P["ProductBrands"].ToString(),

                            ProductCollections = P["ProductCollections"].ToString(),
                            ProductTags = P["ProductTags"].ToString(),
                            ProductVendors = P["ProductVendors"].ToString(),
                            ColorsAvailable = P["Colour"].ToString(),
                            UnitPrice = P["UnitPrice"].ToString(),
                            CostPerItem = P["CostPerItem"].ToString(),
                            Profit = "",
                            IsOnSale = P["IsOnSale"].ToString(),
                            DiscountPercentage = P["DiscountPercentage"].ToString(),
                            SalePrice = P["SalePrice"].ToString(),
                            // Freigthnote = P["Freigthnote"].ToString(),
                            MinimumOrderQuantity = P["MOQ"].ToString(),
                            DepositRequired = P["DepositRequired"].ToString(),
                            IsChargeTax = P["IsChargeTax"].ToString(),
                            IsProductHasPriceVariant = P["IsProductHasPriceVariant"].ToString(),
                            Freigthnote = P["Freigtnote"].ToString(),
                            Barcode = P["Barcode"].ToString(),
                            TotalNoAvailable = P["TotalNoAvailable"].ToString(),
                            AlertRestockAtThisNumber = P["AlertRestockAtThisNumber"].ToString(),
                            IsTrackQuantity = P["IsTrackQuantity"].ToString(),
                            IsStopSellingStockZero = P["IsStopSellingStockZero"].ToString(),
                            ProductHeight = P["ProductHeight"].ToString(),
                            ProductWidth = P["ProductWidth"].ToString(),
                            Productlength = P["Productlength"].ToString(),
                            ProductDiameter = P["ProductDiameter"].ToString(),
                            ProductUOM = P["ProductUOM"].ToString(),
                            ProductUnitWeight = P["ProductUnitWeight"].ToString(),
                            //ProductPackaging = "",
                            WeightUOM = P["WeightUOM"].ToString(),
                            VolumeValue = P["VolumeValue"].ToString(),
                            VolumeUOM = P["VolumeUOM"].ToString(),
                            ProductDimensionNotes = P["ProductDimensionNotes"].ToString(),
                            IfsetOtherproducttitleanddimensoionsinthisset = P["IfsetOtherproducttitleanddimensoionsinthisset"].ToString(),
                            ProductPackaging = P["ProductPackaging"].ToString(),
                            Counrtyoforigin = P["Counrtyoforigin"].ToString(),
                            IsPhysicalProduct = P["IsPhysicalProduct"].ToString(),
                            CartonQuantity = P["CartonQuantity"].ToString(),
                            CartonLength = P["CartonLength"].ToString(),
                            CartonWidth = P["CartonWidth"].ToString(),
                            CartonHeight = P["CartonHeight"].ToString(),
                            UnitsPerProduct = "",
                            //Productpackaging = P["Productpackaging"].ToString(),
                            CartonUOM = P["CartonUOM"].ToString(),
                            CartonWeight = P["CartonWeight"].ToString(),
                            CartonWeightUOM = P["CartonWeightUOM"].ToString(),
                            CartonPackaging = P["CartonPackaging"].ToString(),
                            CartonNote = P["CartonNote"].ToString(),
                            CartonCubicWeight = P["CartonCubicWeight"].ToString(),
                            PalletWeight = P["PalletWeight"].ToString(),

                            CartonsPerPallet = P["CartonsPerPallet"].ToString(),
                            UnitsPerPallet = P["UnitsPerPallet"].ToString(),
                            PalletNote = P["PalletNote"].ToString(),
                            MainProductImage = P["MainProductImage"].ToString(),
                            ProductImages = P["ProductImages"].ToString(),
                            LineMediaArtImages = P["LineMediaArtImages"].ToString(),
                            LifeStyleImages = P["LifeStyleImages"].ToString(),
                            OtherMediaImages = P["OtherMediaImages"].ToString(),
                            NumberofPieces = P["NumberofPieces"].ToString(),
                            UnitOfMeasure = P["ProductUOM"].ToString(),
                            MOQ = P["MOQ"].ToString(),
                            status = P["status"].ToString(),
                            IsIndentOrder = P["IsIndentOrder"].ToString(),
                            Published = P["Published"].ToString(),
                            TurnAroundTime = P["TurnAroundTime"].ToString(),
                            RelatedProducts = P["RelatedProducts"].ToString(),
                            AlternativeProducts = P["AlternativeProducts"].ToString(),
                            ColourFamily = P["ColourFamily"].ToString(),
                            PMSColourCode = P["PMSColourCode"].ToString(),
                            VideoURL = P["VideoURL"].ToString(),
                            Image360Degrees = P["Image360Degrees"].ToString(),
                            ProductViews = P["ProductViews"].ToString(),
                            NextShipmentDate = P["NextShipmentDate"].ToString(),
                            NextShipmentQuantity = P["NextShipmentQuantity"].ToString(),
                            ExtraSetupFee = P["ExtraSetupFee"].ToString(),
                            BrandingMethodsseparatedbyacommarelevanttothisproduct = P["BrandingMethodsseparatedbyacommarelevanttothisproduct"].ToString(),
                            BrandingMethodNote = P["BrandingMethodNote"].ToString(),
                            BrandingUOM = P["BrandingUOM"].ToString(),
                            QuantityPriceVariantModel = AsyncHelper.RunSync(() => ConvertPriceDataToList(new RawPriceModel
                            {
                                Q1 = P["Q1"].ToString(),
                                Q2 = P["Q2"].ToString(),
                                Q3 = P["Q3"].ToString(),
                                Q4 = P["Q4"].ToString(),
                                Q5 = P["Q5"].ToString(),
                                Q6 = P["Q6"].ToString(),
                                Q7 = P["Q7"].ToString(),
                                Q8 = P["Q8"].ToString(),
                                P1 = P["P1"].ToString(),
                                P2 = P["P2"].ToString(),
                                P3 = P["P3"].ToString(),
                                P4 = P["P4"].ToString(),
                                P5 = P["P5"].ToString(),
                                P6 = P["P6"].ToString(),
                                P7 = P["P7"].ToString(),
                                P8 = P["P8"].ToString(),
                            })),
                            BrandingLocationModel = AsyncHelper.RunSync(() => ConvertBrandingDataToList(new RawBrandingModel
                            {
                                Branding_Location_Title_1 = P["Branding_Location_Title_1"].ToString(),
                                Position_Max_Width_1 = P["Position_Max_Width_1"].ToString(),
                                Position_Max_Height_1 = P["Position_Max_Height_1"].ToString(),
                                Branding_Location_Image_1 = P["Branding_Location_Image_1"].ToString(),

                                Branding_Location_Title_2 = P["Branding_Location_Title_2"].ToString(),
                                Position_Max_Width_2 = P["Position_Max_Width_2"].ToString(),
                                Position_Max_Height_2 = P["Position_Max_Height_2"].ToString(),
                                Branding_Location_Image_2 = P["Branding_Location_Image_2"].ToString(),

                                Branding_Location_Title_3 = P["Branding_Location_Title_3"].ToString(),
                                Position_Max_Width_3 = P["Position_Max_Width_3"].ToString(),
                                Position_Max_Height_3 = P["Position_Max_Height_3"].ToString(),
                                Branding_Location_Image_3 = P["Branding_Location_Image_3"].ToString(),

                                Branding_Location_Title_4 = P["Branding_Location_Title_4"].ToString(),
                                Position_Max_Width_4 = P["Position_Max_Width_4"].ToString(),
                                Position_Max_Height_4 = P["Position_Max_Height_4"].ToString(),
                                Branding_Location_Image_4 = P["Branding_Location_Image_4"].ToString(),

                                Branding_Location_Title_5 = P["Branding_Location_Title_5"].ToString(),
                                Position_Max_Width_5 = P["Position_Max_Width_5"].ToString(),
                                Position_Max_Height_5 = P["Position_Max_Height_5"].ToString(),
                                Branding_Location_Image_5 = P["Branding_Location_Image_5"].ToString(),

                                Branding_Location_Title_6 = P["Branding_Location_Title_6"].ToString(),
                                Position_Max_Width_6 = P["Position_Max_Width_6"].ToString(),
                                Position_Max_Height_6 = P["Position_Max_Height_6"].ToString(),
                                Branding_Location_Image_6 = P["Branding_Location_Image_6"].ToString(),

                                Branding_Location_Title_7 = P["Branding_Location_Title_7"].ToString(),
                                Position_Max_Width_7 = P["Position_Max_Width_7"].ToString(),
                                Position_Max_Height_7 = P["Position_Max_Height_7"].ToString(),
                                Branding_Location_Image_7 = P["Branding_Location_Image_7"].ToString(),

                                Branding_Location_Title_8 = P["Branding_Location_Title_8"].ToString(),
                                Position_Max_Width_8 = P["Position_Max_Width_8"].ToString(),
                                Position_Max_Height_8 = P["Position_Max_Height_8"].ToString(),
                                Branding_Location_Image_8 = P["Branding_Location_Image_8"].ToString(),

                                Branding_Location_Title_9 = P["Branding_Location_Title_9"].ToString(),
                                Position_Max_Width_9 = P["Position_Max_Width_9"].ToString(),
                                Position_Max_Height_9 = P["Position_Max_Height_9"].ToString(),
                                Branding_Location_Image_9 = P["Branding_Location_Image_9"].ToString(),

                                Branding_Location_Title_10 = P["Branding_Location_Title_10"].ToString(),
                                Position_Max_Width_10 = P["Position_Max_Width_10"].ToString(),
                                Position_Max_Height_10 = P["Position_Max_Height_10"].ToString(),
                                Branding_Location_Image_10 = P["Branding_Location_Image_10"].ToString(),

                                Branding_Location_Title_11 = P["Branding_Location_Title_11"].ToString(),
                                Position_Max_Width_11 = P["Position_Max_Width_11"].ToString(),
                                Position_Max_Height_11 = P["Position_Max_Height_11"].ToString(),
                                Branding_Location_Image_11 = P["Branding_Location_Image_11"].ToString(),

                                Branding_Location_Title_12 = P["Branding_Location_Title_12"].ToString(),
                                Position_Max_Width_12 = P["Position_Max_Width_12"].ToString(),
                                Position_Max_Height_12 = P["Position_Max_Height_12"].ToString(),
                                Branding_Location_Image_12 = P["Branding_Location_Image_12"].ToString(),

                                Branding_Location_Title_13 = P["Branding_Location_Title_13"].ToString(),
                                Position_Max_Width_13 = P["Position_Max_Width_13"].ToString(),
                                Position_Max_Height_13 = P["Position_Max_Height_13"].ToString(),
                                Branding_Location_Image_13 = P["Branding_Location_Image_13"].ToString(),

                                Branding_Location_Title_14 = P["Branding_Location_Title_14"].ToString(),
                                Position_Max_Width_14 = P["Position_Max_Width_14"].ToString(),
                                Position_Max_Height_14 = P["Position_Max_Height_14"].ToString(),
                                Branding_Location_Image_14 = P["Branding_Location_Image_14"].ToString(),

                                Branding_Location_Title_15 = P["Branding_Location_Title_15"].ToString(),
                                Position_Max_Width_15 = P["Position_Max_Width_15"].ToString(),
                                Position_Max_Height_15 = P["Position_Max_Height_15"].ToString(),
                                Branding_Location_Image_15 = P["Branding_Location_Image_15"].ToString(),

                                Branding_Location_Title_16 = P["Branding_Location_Title_16"].ToString(),
                                Position_Max_Width_16 = P["Position_Max_Width_16"].ToString(),
                                Position_Max_Height_16 = P["Position_Max_Height_16"].ToString(),
                                Branding_Location_Image_16 = P["Branding_Location_Image_16"].ToString(),

                                Branding_Location_Title_17 = P["Branding_Location_Title_17"].ToString(),
                                Position_Max_Width_17 = P["Position_Max_Width_17"].ToString(),
                                Position_Max_Height_17 = P["Position_Max_Height_17"].ToString(),
                                Branding_Location_Image_17 = P["Branding_Location_Image_17"].ToString(),

                                Branding_Location_Title_18 = P["Branding_Location_Title_18"].ToString(),
                                Position_Max_Width_18 = P["Position_Max_Width_18"].ToString(),
                                Position_Max_Height_18 = P["Position_Max_Height_18"].ToString(),
                                Branding_Location_Image_18 = P["Branding_Location_Image_18"].ToString(),

                                Branding_Location_Title_19 = P["Branding_Location_Title_19"].ToString(),
                                Position_Max_Width_19 = P["Position_Max_Width_19"].ToString(),
                                Position_Max_Height_19 = P["Position_Max_Height_19"].ToString(),
                                Branding_Location_Image_19 = P["Branding_Location_Image_19"].ToString(),

                                Branding_Location_Title_20 = P["Branding_Location_Title_20"].ToString(),
                                Position_Max_Width_20 = P["Position_Max_Width_20"].ToString(),
                                Position_Max_Height_20 = P["Position_Max_Height_20"].ToString(),
                                Branding_Location_Image_20 = P["Branding_Location_Image_20"].ToString(),

                            })),


                        }).ToList();
            }
            catch (Exception ex)
            {

            }
            return Data;

        }
        public virtual async Task CreateProductDataUsingTemp(int TenantId)
        {
            DataTable ProductDataDt = new DataTable();
            ProductDataDt = await GetProductBulkData();


            List<ImportBulkProductDto> productData = new List<ImportBulkProductDto>();
            productData = await ConvertToImportModel(ProductDataDt);

            int NumberOfExcelRows = productData.Count();
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
                    var SubCategoryData = _SubCategoryMasterRepository.GetAll();
                   
                    List<ProductDimensionsInventory> InventoryFinalList = new List<ProductDimensionsInventory>();
                    List<ProductMaster> ProductMasterFinalList = new List<ProductMaster>();
                    List<ProductDetails> ProductDetailsFinalList = new List<ProductDetails>();
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
                    List<SubCategoryMaster> SubCatgorymasterlist = new List<SubCategoryMaster>();
                    List<CategoryGroupMaster> CategoryGroupMasterList = new List<CategoryGroupMaster>();
                    List<CategoryGroups> CategoryGroupsList = new List<CategoryGroups>();
                    List<CategorySubCategories> CategorySubCategoriesList = new List<CategorySubCategories>();
                    List<CategoryCollections> collectionCategoryList = new List<CategoryCollections>();
                    List<ProductAssignedMaterials> AssignedMaterials = new List<ProductAssignedMaterials>();
                    List<ProductAssignedBrands> AssignedBrands = new List<ProductAssignedBrands>();
                    List<ProductAssignedTypes> AssignedTypes = new List<ProductAssignedTypes>();
                    List<ProductAssignedTags> AssignedTags = new List<ProductAssignedTags>();
                    List<ProductAssignedVendors> AssignedVendors = new List<ProductAssignedVendors>();
                    List<ProductAssignedSubCategories> AssignedCategories = new List<ProductAssignedSubCategories>();
                    List<ProductAssignedSubSubCategories> AssignedSubSubCategories = new List<ProductAssignedSubSubCategories>();
                    List<ProductAssignedCategoryMaster> CategoryMasters = new List<ProductAssignedCategoryMaster>();
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
                    SubCategoryMaster SubCategorymaster = new SubCategoryMaster();
                    CategoryGroupMaster GroupMaster = new CategoryGroupMaster();
                    CollectionMaster collectionMaster = new CollectionMaster();
                    ProductMaterialMaster material = new ProductMaterialMaster();
                    ProductTypeMaster typemaster = new ProductTypeMaster();
                    ProductBrandMaster Brand = new ProductBrandMaster();
                    ProductTagMaster Tag = new ProductTagMaster();
                    ProductMaster master = new ProductMaster();
                    foreach (var detail in productData)
                    {
                        master = new ProductMaster();
                       
                        BrandingMethodList = new List<BrandingMethodMaster>();
                        collectionList = new List<CollectionMaster>();
                       //  long CategoryId = 0;
                       // long GroupMasterId = 0;
                      

                        if (!string.IsNullOrEmpty(detail.GroupCategory1))
                        {
                            //---------------Group Master -----------------------
                            long GroupMasterId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory1.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            CategoryGroupMaster groupObj = CategoryGroupMasterList.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory1.Trim()).FirstOrDefault();

                            if (GroupMasterId == 0 && groupObj == null)
                            {
                                GroupMaster = new CategoryGroupMaster();
                                GroupMaster.GroupTitle = detail.GroupCategory1;
                                GroupMaster.TenantId = TenantId;
                                CategoryGroupMasterList.Add(GroupMaster);
                            }
                        }

                        if (!string.IsNullOrEmpty(detail.GroupCategory2))
                        {
                            //---------------Group Master -----------------------
                            long GroupMasterId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory2.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            CategoryGroupMaster groupObj = CategoryGroupMasterList.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory2.Trim()).FirstOrDefault();

                            if (GroupMasterId == 0 && groupObj == null)
                            {
                                GroupMaster = new CategoryGroupMaster();
                                GroupMaster.GroupTitle = detail.GroupCategory2;
                                GroupMaster.TenantId = TenantId;
                                CategoryGroupMasterList.Add(GroupMaster);
                            }
                        }


                        if (!string.IsNullOrEmpty(detail.GroupCategory3))
                        {
                            //---------------Group Master -----------------------
                            long GroupMasterId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory3.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            CategoryGroupMaster groupObj = CategoryGroupMasterList.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory3.Trim()).FirstOrDefault();

                            if (GroupMasterId == 0 && groupObj == null)
                            {
                                GroupMaster = new CategoryGroupMaster();
                                GroupMaster.GroupTitle = detail.GroupCategory3;
                                GroupMaster.TenantId = TenantId;
                                CategoryGroupMasterList.Add(GroupMaster);
                            }
                        }

                        if (!string.IsNullOrEmpty(detail.SubCategory1))
                        {
                                long CategoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == detail.SubCategory1.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                var IsExists = Catgorymasterlist.Where(i => i.CategoryTitle.ToLower() == detail.SubCategory1.Trim()).FirstOrDefault();

                                if (CategoryId == 0 && IsExists == null)
                                {
                                    categorymaster = new CategoryMaster();
                                    categorymaster.CategoryTitle = detail.SubCategory1.Trim();
                                    categorymaster.TenantId = TenantId;

                                    if (IsExists == null)
                                    {
                                        Catgorymasterlist.Add(categorymaster);

                                    }
                                }
                        }

                        if (!string.IsNullOrEmpty(detail.SubCategory2))
                        {
                          
                                long CategoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == detail.SubCategory2.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                var IsExists = Catgorymasterlist.Where(i => i.CategoryTitle.ToLower() == detail.SubCategory2.Trim()).FirstOrDefault();

                                if (CategoryId == 0 && IsExists == null)
                                {
                                    categorymaster = new CategoryMaster();
                                    categorymaster.CategoryTitle = detail.SubCategory2.Trim();
                                    categorymaster.TenantId = TenantId;

                                    if (IsExists == null)
                                    {
                                        Catgorymasterlist.Add(categorymaster);

                                    }
                                }
                        }

                        if (!string.IsNullOrEmpty(detail.SubCategory3))
                        {
                           
                                long CategoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == detail.SubCategory3.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                var IsExists = Catgorymasterlist.Where(i => i.CategoryTitle.ToLower() == detail.SubCategory3.Trim()).FirstOrDefault();

                                if (CategoryId == 0 && IsExists == null)
                                {
                                    categorymaster = new CategoryMaster();
                                    categorymaster.CategoryTitle = detail.SubCategory3.Trim();
                                    categorymaster.TenantId = TenantId;

                                    if (IsExists == null)
                                    {
                                        Catgorymasterlist.Add(categorymaster);

                                    }
                                }
                            
                        }

                        if (!string.IsNullOrEmpty(detail.SubSubCategory1))
                        {

                            string[] SubCategories = detail.SubSubCategory1.Split(',');
                            foreach (var sub in SubCategories)
                            {
                                long CategoryId = SubCategoryData.Where(i => i.Title.ToLower().Trim() == sub.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                var IsExists = SubCatgorymasterlist.Where(i => i.Title.ToLower() == sub.Trim()).FirstOrDefault();


                                if (CategoryId == 0 && IsExists == null)
                                {
                                    SubCategorymaster = new SubCategoryMaster();
                                    SubCategorymaster.Title = sub.Trim();
                                    SubCategorymaster.TenantId = TenantId;

                                    if (IsExists == null)
                                    {
                                        SubCatgorymasterlist.Add(SubCategorymaster);

                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(detail.SubSubCategory2))
                        {
                            string[] SubCategories = detail.SubSubCategory2.Split(',');
                            foreach (var sub in SubCategories)
                            {
                                long CategoryId = SubCategoryData.Where(i => i.Title.ToLower().Trim() == sub.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                var IsExists = SubCatgorymasterlist.Where(i => i.Title.ToLower() == sub.Trim()).FirstOrDefault();


                                if (CategoryId == 0 && IsExists == null)
                                {
                                    SubCategorymaster = new SubCategoryMaster();
                                    SubCategorymaster.Title = sub.Trim();
                                    SubCategorymaster.TenantId = TenantId;

                                    if (IsExists == null)
                                    {
                                        SubCatgorymasterlist.Add(SubCategorymaster);

                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(detail.SubSubCategory3))
                        {
                            string[] SubCategories = detail.SubSubCategory3.Split(',');
                            foreach (var sub in SubCategories)
                            {

                                long CategoryId = SubCategoryData.Where(i => i.Title.ToLower().Trim() == sub.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                var IsExists = SubCatgorymasterlist.Where(i => i.Title.ToLower() == sub.Trim()).FirstOrDefault();


                                if (CategoryId == 0 && IsExists == null)
                                {
                                    SubCategorymaster = new SubCategoryMaster();
                                    SubCategorymaster.Title = sub.Trim();
                                    SubCategorymaster.TenantId = TenantId;

                                    if (IsExists == null)
                                    {
                                        SubCatgorymasterlist.Add(SubCategorymaster);

                                    }
                                }
                            }
                        }


                        #region old block of code of groupmaster
                        //if (!string.IsNullOrEmpty(detail.Groupcategory))
                        //{
                        //    //---------------Group Master -----------------------
                        //    GroupMasterId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == detail.Groupcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                        //    CategoryGroupMaster groupObj = CategoryGroupMasterList.Where(i => i.GroupTitle.ToLower().Trim() == detail.Groupcategory.Trim()).FirstOrDefault();

                        //    if (GroupMasterId == 0 && groupObj == null)
                        //    {
                        //        GroupMaster.GroupTitle = detail.Groupcategory;
                        //        GroupMaster.TenantId = TenantId;
                        //        CategoryGroupMasterList.Add(GroupMaster);
                        //    }
                        //}
                        ////-------------------------Group Master ends-----------------
                        #endregion

                        #region category master old block of code
                        //if (!string.IsNullOrEmpty(detail.Productcategory))
                        //{
                        //    CategoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == detail.Productcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                        //    var IsExists = Catgorymasterlist.Where(i => i.CategoryTitle.ToLower() == detail.Productcategory.Trim()).FirstOrDefault();


                        //    if (CategoryId == 0 && IsExists == null)
                        //    {
                        //        categorymaster.CategoryTitle = detail.Productcategory.Trim();
                        //        categorymaster.TenantId = TenantId;

                        //        if (IsExists == null)
                        //        {
                        //            Catgorymasterlist.Add(categorymaster);

                        //        }
                        //    }
                        //}
                        #endregion



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
                                    collectionMaster.IsManualCalculation = true;
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

                        #region product main logic

                        master.ProductSKU = !string.IsNullOrEmpty(detail.ParentSKU) ? detail.ParentSKU : "";
                        master.ProductTitle = !string.IsNullOrEmpty(detail.Name) ? detail.Name : "";
                        master.IsActive = true;
                        master.ShortDescripition = !string.IsNullOrEmpty(detail.ShortDescription) ? detail.ShortDescription : "";
                        master.ProductDescripition = !string.IsNullOrEmpty(detail.Description) ? detail.Description : "";
                        master.ColorsAvailable = !string.IsNullOrEmpty(detail.ColorsAvailable) ? detail.ColorsAvailable : "";
                        master.Features = !string.IsNullOrEmpty(detail.Features) ? detail.Features : "";
                       // master.UnitOfMeasure = string.IsNullOrEmpty(detail.UnitOfMeasure) ? 0 : Convert.ToInt32(detail.UnitOfMeasure);
                        master.MinimumOrderQuantity = string.IsNullOrEmpty(detail.MOQ) ? 0 : Convert.ToInt32(detail.MOQ);
                        master.UnitPrice = !string.IsNullOrEmpty(detail.UnitPrice) ? Convert.ToDecimal(detail.UnitPrice) : 0;
                        master.CostPerItem = !string.IsNullOrEmpty(detail.CostPerItem) ? Convert.ToDecimal(detail.CostPerItem) : 0;
                        master.Profit = !string.IsNullOrEmpty(detail.Profit) ? Convert.ToDecimal(detail.Profit) : 0;
                        master.IsPhysicalProduct = !string.IsNullOrEmpty(detail.IsPhysicalProduct) ? detail.IsPhysicalProduct.ToLower() == "true" ? true : false : false;
                        master.OnSale = !string.IsNullOrEmpty(detail.IsOnSale) ? detail.IsOnSale.ToLower() == "true" ? true : false : false;
                        master.ChargeTaxOnThis = !string.IsNullOrEmpty(detail.IsChargeTax) ? detail.IsChargeTax.ToLower() == "true" ? true : false : false;
                        master.SalePrice = !string.IsNullOrEmpty(detail.SalePrice) ? Convert.ToDecimal(detail.SalePrice) : 0;
                        master.MinimumOrderQuantity = !string.IsNullOrEmpty(detail.MinimumOrderQuantity) ? Convert.ToInt32(detail.MinimumOrderQuantity) : 0;
                        master.DepositRequired = !string.IsNullOrEmpty(detail.DepositRequired) ? Convert.ToDecimal(detail.DepositRequired) : 0;
                        master.CostPerItem = !string.IsNullOrEmpty(detail.CostPerItem) ? Convert.ToDecimal(detail.CostPerItem) : 0;
                        master.ProductHasPriceVariant = !string.IsNullOrEmpty(detail.IsProductHasPriceVariant) ? detail.IsProductHasPriceVariant.ToLower() == "true" ? true : false : false;

                        master.DiscountPercentage = !string.IsNullOrEmpty(detail.DiscountPercentage) ? Convert.ToDouble(detail.DiscountPercentage) : 0;
                        master.DiscountPercentageDraft = !string.IsNullOrEmpty(detail.DiscountPercentage) ? Convert.ToDouble(detail.DiscountPercentage) : 0;

                        if (!string.IsNullOrEmpty(detail.TurnAroundTime))
                        {
                            master.TurnAroundTimeId = TurnAroundTimeData.Where(i => i.NumberOfDays.ToLower() == detail.TurnAroundTime.ToLower()).Select(i => i.Id).FirstOrDefault();
                        }
                        master.FrightNote = detail.Freigthnote;
                        master.VolumeValue = detail.VolumeValue;
                        master.VolumeUOM = detail.VolumeUOM;
                        master.IfSetOtherProductTitleAndDimensoionsInThisSet = detail.IfsetOtherproducttitleanddimensoionsinthisset;
                        master.CounrtyOfOrigin = detail.Counrtyoforigin;
                        master.NumberOfPieces = detail.NumberofPieces;
                        master.IsIndentOrder = detail.IsIndentOrder == "true" ? true : false;
                        master.ColourFamily = detail.ColourFamily;
                        master.PMSColourCode = detail.PMSColourCode;
                        master.VideoURL = detail.VideoURL;
                        master.Image360Degrees = detail.Image360Degrees;
                        master.NextShipmentDate = detail.NextShipmentDate;
                        master.NextShipmentQuantity = detail.NextShipmentQuantity;
                        master.ExtraSetUpFee = detail.ExtraSetupFee;
                        master.BrandingMethodNote = detail.BrandingMethodNote;
                        master.BrandingUOM = detail.BrandingUOM;

                        if(detail.QuantityPriceVariantModel != null)
                        {
                            if (detail.QuantityPriceVariantModel.Count > 0)
                            {
                                master.ProductHasPriceVariant = true;
                            }
                        }

                        ProductMasterFinalList.Add(master);
                        #endregion

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
                    if (SubCatgorymasterlist.Count > 0)
                    {
                        await CreateSubCategoryMaster(SubCatgorymasterlist.GroupBy(x => x.Title.ToLower().Trim()).Select(g => g.First()).ToList(), TenantId);
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
                    if (ProductMasterFinalList.Count > 0)
                    {
                        await CreateProductMasterAsync(ProductMasterFinalList, TenantId);
                    }
                    #endregion
                     
                    #region Main product integration and binding process

                    //----------------getting latest master data which has been inserted by sheet.
                    CategoryMasterData = _categoryMasterRepository.GetAll();
                    CollectionMasterData = _collectionMasterRepository.GetAll();
                    GroupMasterData = _categoryGroupMasterRepository.GetAll();
                    SubCategoryData = _SubCategoryMasterRepository.GetAll();
                    var CategoryGroups = _categoryGroupRepository.GetAll();
                    var CategorySubCategories = _categorySubCategoriesRepository.GetAll();
                    var ProductDBData = _productMasterRepository.GetAll();

                    CategoryGroups CategoryGroup = new CategoryGroups();
                    ProductBulkImportDataHistory history = new ProductBulkImportDataHistory();
                    ProductDimensionsInventory inventory = new ProductDimensionsInventory();
                    //ProductMaster master = new ProductMaster();
                    ProductDetails details = new ProductDetails();
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

                    foreach (var product in productData)
                    {
                        #region CategoryCollections and CategoryGroups assignments insertion

                        #region Category Groups
                        //------------category group assignment

                        //if (!string.IsNullOrEmpty(product.Productcategory) && !string.IsNullOrEmpty(product.Groupcategory))
                        //{
                        //    long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.Productcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                        //    long groupId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == product.Groupcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

                        //    if (groupId > 0 && categoryId > 0)
                        //    {
                        //        CategoryGroups IsGroupAlreadyExists = CategoryGroupsList.Where(i => i.CategoryGroupId == groupId && i.CategoryMasterId == categoryId).FirstOrDefault();

                        //        if (IsGroupAlreadyExists == null)
                        //        {
                        //            CategoryGroup = new CategoryGroups();
                        //            CategoryGroup.CategoryGroupId = groupId;
                        //            CategoryGroup.CategoryMasterId = categoryId;
                        //            CategoryGroup.IsActive = true;
                        //            CategoryGroupsList.Add(CategoryGroup);
                        //        }
                        //    }
                        //}


                        if (!string.IsNullOrEmpty(product.GroupCategory1) && !string.IsNullOrEmpty(product.SubCategory1))
                        {
                            long groupId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory1.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory1.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

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

                            if (!string.IsNullOrEmpty(product.SubSubCategory1) && categoryId > 0)
                            {
                                string[] SubCategories = product.SubSubCategory1.Split(',');
                                foreach (var cat in SubCategories)
                                {
                                    var IsSubCategoryExists = SubCategoryData.Where(i => i.Title.ToLower() == cat.ToLower()).FirstOrDefault();
                                    if (IsSubCategoryExists != null)
                                    {
                                        CategorySubCategories IsCatAlreadyExists = CategorySubCategories.Where(i => i.CategoryId == categoryId && i.SubCategoryId == IsSubCategoryExists.Id).FirstOrDefault();

                                        if(IsCatAlreadyExists == null)
                                        {
                                            CategorySubCategories subcategory = new CategorySubCategories();
                                            subcategory.CategoryId = categoryId;
                                            subcategory.SubCategoryId = IsSubCategoryExists.Id;
                                            subcategory.IsActive = true;
                                            subcategory.TenantId = TenantId;
                                            CategorySubCategoriesList.Add(subcategory);
                                        }
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(product.GroupCategory2) && !string.IsNullOrEmpty(product.SubCategory2))
                        {
                            long groupId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory2.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory2.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

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

                            if (!string.IsNullOrEmpty(product.SubSubCategory2) && categoryId > 0)
                            {
                                string[] SubCategories = product.SubSubCategory2.Split(',');
                                foreach (var cat in SubCategories)
                                {
                                    var IsSubCategoryExists = SubCategoryData.Where(i => i.Title.ToLower() == cat.ToLower()).FirstOrDefault();
                                    if (IsSubCategoryExists != null)
                                    {
                                        CategorySubCategories IsCatAlreadyExists = CategorySubCategories.Where(i => i.CategoryId == categoryId && i.SubCategoryId == IsSubCategoryExists.Id).FirstOrDefault();

                                        if (IsCatAlreadyExists == null)
                                        {
                                            CategorySubCategories subcategory = new CategorySubCategories();
                                            subcategory.CategoryId = categoryId;
                                            subcategory.SubCategoryId = IsSubCategoryExists.Id;
                                            subcategory.IsActive = true;
                                            subcategory.TenantId = TenantId;
                                            CategorySubCategoriesList.Add(subcategory);
                                        }
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(product.GroupCategory3) && !string.IsNullOrEmpty(product.SubCategory3))
                        {
                            long groupId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory3.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory3.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

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

                            if (!string.IsNullOrEmpty(product.SubSubCategory3) && categoryId > 0)
                            {
                                string[] SubCategories = product.SubSubCategory3.Split(',');
                                foreach (var cat in SubCategories)
                                {
                                    var IsSubCategoryExists = SubCategoryData.Where(i => i.Title.ToLower() == cat.ToLower()).FirstOrDefault();
                                    if (IsSubCategoryExists != null)
                                    {
                                        CategorySubCategories IsCatAlreadyExists = CategorySubCategories.Where(i => i.CategoryId == categoryId && i.SubCategoryId == IsSubCategoryExists.Id).FirstOrDefault();

                                        if (IsCatAlreadyExists == null)
                                        {
                                            CategorySubCategories subcategory = new CategorySubCategories();
                                            subcategory.CategoryId = categoryId;
                                            subcategory.SubCategoryId = IsSubCategoryExists.Id;
                                            subcategory.IsActive = true;
                                            subcategory.TenantId = TenantId;
                                            CategorySubCategoriesList.Add(subcategory);
                                        }
                                    }
                                }
                            }
                        }

                        #endregion


                        #region Category collection assignment

                        //if (!string.IsNullOrEmpty(product.ProductCollections) && !string.IsNullOrEmpty(product.Productcategory))
                        //{

                        //    string[] Collections = product.ProductCollections.Split(',');
                        //    foreach (var collection in Collections)
                        //    {
                        //        CategoryCollections categoryCollection = new CategoryCollections();

                        //        long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.Productcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                        //        long CollectionId = CollectionMasterData.Where(i => i.CollectionName.ToLower().Trim() == collection.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

                        //        if (categoryId > 0 && CollectionId > 0)
                        //        {
                        //            CategoryCollections IsCollectionExists = collectionCategoryList.Where(i => i.CategoryId == categoryId && i.CollectionId == CollectionId).FirstOrDefault();
                        //            if (IsCollectionExists == null)
                        //            {
                        //                categoryCollection.CategoryId = categoryId;
                        //                categoryCollection.CollectionId = CollectionId;
                        //                categoryCollection.TenantId = TenantId;
                        //                categoryCollection.IsActive = true;
                        //                collectionCategoryList.Add(categoryCollection);
                        //            }

                        //        }
                        //    }
                        //}
                        #endregion

                        #endregion


                        history = new ProductBulkImportDataHistory();
                        inventory = new ProductDimensionsInventory();
                        master = new ProductMaster();
                        details = new ProductDetails();
                        dimension = new ProductDimension();
                        images = new List<ProductImages>();
                        MediaImages = new List<ProductMediaImages>();
                        ProductViewImages = new List<ProductViewImages>();
                        priceVariants = new List<ProductVolumeDiscountVariant>();
                        ProductBrandingLocData = new List<ProductBrandingPosition>();
                        ProductAlternatives = new List<AlternativeProducts>();
                        RelativeProducts = new List<RelativeProducts>();

                        Catgorymaster = new CategoryMaster();

                        long ProductId = ProductDBData.Where(i => i.ProductSKU.Trim() == product.ParentSKU.Trim()).Select(i=>i.Id).FirstOrDefault();
                        if (ProductId > 0)
                        {

                            #region product master commented
                            //master.ProductSKU = !string.IsNullOrEmpty(product.ParentSKU) ? product.ParentSKU : "";
                            //master.ProductTitle = !string.IsNullOrEmpty(product.Name) ? product.Name : "";
                            //master.IsActive = true;
                            //master.ShortDescripition = !string.IsNullOrEmpty(product.ShortDescription) ? product.ShortDescription : "";
                            //master.ProductDescripition = !string.IsNullOrEmpty(product.Description) ? product.Description : "";
                            //master.ColorsAvailable = !string.IsNullOrEmpty(product.ColorsAvailable) ? product.ColorsAvailable : "";
                            //master.Features = !string.IsNullOrEmpty(product.Features) ? product.Features : "";
                            //master.UnitOfMeasure = string.IsNullOrEmpty(product.UnitOfMeasure) ? 0 : Convert.ToInt32(product.UnitOfMeasure);
                            //master.MinimumOrderQuantity = string.IsNullOrEmpty(product.MOQ) ? 0 : Convert.ToInt32(product.MOQ);
                            //master.UnitPrice = !string.IsNullOrEmpty(product.UnitPrice) ? Convert.ToDecimal(product.UnitPrice) : 0;
                            //master.CostPerItem = !string.IsNullOrEmpty(product.CostPerItem) ? Convert.ToDecimal(product.CostPerItem) : 0;
                            //master.Profit = !string.IsNullOrEmpty(product.Profit) ? Convert.ToDecimal(product.Profit) : 0;
                            //master.IsPhysicalProduct = !string.IsNullOrEmpty(product.IsPhysicalProduct) ? product.IsPhysicalProduct.ToLower() == "true" ? true : false : false;
                            //master.OnSale = !string.IsNullOrEmpty(product.IsOnSale) ? product.IsOnSale.ToLower() == "true" ? true : false : false;
                            //master.ChargeTaxOnThis = !string.IsNullOrEmpty(product.IsChargeTax) ? product.IsChargeTax.ToLower() == "true" ? true : false : false;
                            //master.SalePrice = !string.IsNullOrEmpty(product.SalePrice) ? Convert.ToDecimal(product.SalePrice) : 0;
                            //master.MinimumOrderQuantity = !string.IsNullOrEmpty(product.MinimumOrderQuantity) ? Convert.ToInt32(product.MinimumOrderQuantity) : 0;
                            //master.DepositRequired = !string.IsNullOrEmpty(product.DepositRequired) ? Convert.ToDecimal(product.DepositRequired) : 0;
                            //master.CostPerItem = !string.IsNullOrEmpty(product.CostPerItem) ? Convert.ToDecimal(product.CostPerItem) : 0;
                            //master.ProductHasPriceVariant = !string.IsNullOrEmpty(product.IsProductHasPriceVariant) ? product.IsProductHasPriceVariant.ToLower() == "true" ? true : false : false;

                            //master.DiscountPercentage = !string.IsNullOrEmpty(product.DiscountPercentage) ? Convert.ToDouble(product.DiscountPercentage) : 0;
                            //master.DiscountPercentageDraft = !string.IsNullOrEmpty(product.DiscountPercentage) ? Convert.ToDouble(product.DiscountPercentage) : 0;

                            //if (!string.IsNullOrEmpty(product.TurnAroundTime))
                            //{
                            //    master.TurnAroundTimeId = TurnAroundTimeData.Where(i => i.NumberOfDays.ToLower() == product.TurnAroundTime.ToLower()).Select(i => i.Id).FirstOrDefault();
                            //}
                            //master.FrightNote = product.Freigthnote;
                            //master.VolumeValue = product.VolumeValue;
                            //master.VolumeUOM = product.VolumeUOM;
                            //master.IfSetOtherProductTitleAndDimensoionsInThisSet = product.IfsetOtherproducttitleanddimensoionsinthisset;
                            //master.CounrtyOfOrigin = product.Counrtyoforigin;
                            //master.NumberOfPieces = product.NumberofPieces;
                            //master.IsIndentOrder = product.IsIndentOrder == "true" ? true : false;
                            //master.ColourFamily = product.ColourFamily;
                            //master.PMSColourCode = product.PMSColourCode;
                            //master.VideoURL = product.VideoURL;
                            //master.Image360Degrees = product.Image360Degrees;
                            //master.NextShipmentDate = product.NextShipmentDate;
                            //master.NextShipmentQuantity = product.NextShipmentQuantity;
                            //master.ExtraSetUpFee = product.ExtraSetupFee;
                            //master.BrandingMethodNote = product.BrandingMethodNote;
                            //master.BrandingUOM = product.BrandingUOM;

                            #endregion

                            #region ProductDetails allassignment tables bindings




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
                                        assignedMaterials.ProductId = ProductId;
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
                                        AssignedBrand.ProductId = ProductId;
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
                                        AssignedTag.ProductId = ProductId;
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
                                        AssignedType.ProductId = ProductId;
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
                                        AssignedVendor.ProductId = ProductId;
                                        AssignedVendors.Add(AssignedVendor);
                                    }
                                }
                            }
                            #endregion

                            #region Product Group Categories Master
                            if (!string.IsNullOrEmpty(product.GroupCategory1))
                            {

                                ProductAssignedCategoryMaster AssignCategory = new ProductAssignedCategoryMaster();
                                var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory1.ToLower().Trim()).FirstOrDefault();

                                if (CategoryGroupId != null)
                                {
                                    AssignCategory.CategoryId = CategoryGroupId.Id;
                                    AssignCategory.ProductId = ProductId;
                                    CategoryMasters.Add(AssignCategory);
                                }
                            }

                            if (!string.IsNullOrEmpty(product.GroupCategory2))
                            {

                                ProductAssignedCategoryMaster AssignCategory = new ProductAssignedCategoryMaster();
                                var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory2.ToLower().Trim()).FirstOrDefault();

                                if (CategoryGroupId != null)
                                {
                                    AssignCategory.CategoryId = CategoryGroupId.Id;
                                    AssignCategory.ProductId = ProductId;
                                    CategoryMasters.Add(AssignCategory);
                                }
                            }
                            if (!string.IsNullOrEmpty(product.GroupCategory3))
                            {

                                ProductAssignedCategoryMaster AssignCategory = new ProductAssignedCategoryMaster();
                                var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory3.ToLower().Trim()).FirstOrDefault();

                                if (CategoryGroupId != null)
                                {
                                    AssignCategory.CategoryId = CategoryGroupId.Id;
                                    AssignCategory.ProductId = ProductId;
                                    CategoryMasters.Add(AssignCategory);
                                }
                            }


                            #endregion


                            #region Product Categories
                            if (!string.IsNullOrEmpty(product.SubCategory1))
                            {

                                AssignedCategory = new ProductAssignedSubCategories();
                                var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory1.ToLower().Trim()).FirstOrDefault();
                                var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory1.ToLower().Trim()).FirstOrDefault();

                                if (CategoryMasterId != null  )
                                {
                                    AssignedCategory.SubCategoryId = CategoryMasterId.Id;
                                    AssignedCategory.CategoryId = CategoryGroupId.Id;
                                    AssignedCategory.ProductId = ProductId;
                                    AssignedCategories.Add(AssignedCategory);
                                }
                            }
                            if (!string.IsNullOrEmpty(product.SubCategory2))
                            {

                                AssignedCategory = new ProductAssignedSubCategories();
                                var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory2.ToLower().Trim()).FirstOrDefault();
                                var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory2.ToLower().Trim()).FirstOrDefault();

                                if (CategoryMasterId != null && CategoryGroupId != null)
                                {
                                    AssignedCategory.SubCategoryId = CategoryMasterId.Id;
                                    AssignedCategory.CategoryId = CategoryGroupId.Id;
                                    AssignedCategory.ProductId = ProductId;
                                    AssignedCategories.Add(AssignedCategory);
                                }
                            }
                            if (!string.IsNullOrEmpty(product.SubCategory3))
                            {

                                AssignedCategory = new ProductAssignedSubCategories();
                                var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory3.ToLower().Trim()).FirstOrDefault();
                                var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory3.ToLower().Trim()).FirstOrDefault();

                                if (CategoryMasterId != null && CategoryGroupId != null)
                                {
                                    AssignedCategory.SubCategoryId = CategoryMasterId.Id;
                                    AssignedCategory.CategoryId = CategoryGroupId.Id;
                                    AssignedCategory.ProductId = ProductId;
                                    AssignedCategories.Add(AssignedCategory);
                                }
                            }
                            #endregion

                            #region Product sub sub Categories
                            if (!string.IsNullOrEmpty(product.SubSubCategory1))
                            {
                                var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory1.ToLower().Trim()).FirstOrDefault();
                                if (CategoryMasterId != null)
                                {
                                    string[] SubSubCategories = product.SubSubCategory1.Split(',');
                                    foreach (var subcategory in SubSubCategories)
                                    {
                                        ProductAssignedSubSubCategories SubSubCategory = new ProductAssignedSubSubCategories();
                                        var SubSubCatId = SubCategoryData.ToList().Where(i => i.Title.ToLower().Trim() == subcategory.ToLower().Trim()).FirstOrDefault();

                                        if (CategoryMasterId != null)
                                        {
                                            SubSubCategory.SubCategoryId = CategoryMasterId.Id;
                                            SubSubCategory.SubSubCategoryId = SubSubCatId.Id;
                                            SubSubCategory.ProductId = ProductId;
                                            AssignedSubSubCategories.Add(SubSubCategory);
                                        }
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(product.SubSubCategory2))
                            {
                                var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory2.ToLower().Trim()).FirstOrDefault();
                                if (CategoryMasterId != null)
                                {
                                    string[] SubSubCategories = product.SubSubCategory2.Split(',');
                                    foreach (var subcategory in SubSubCategories)
                                    {
                                        ProductAssignedSubSubCategories SubSubCategory = new ProductAssignedSubSubCategories();
                                        var SubSubCatId = SubCategoryData.ToList().Where(i => i.Title.ToLower().Trim() == subcategory.ToLower().Trim()).FirstOrDefault();

                                        if (CategoryMasterId != null)
                                        {
                                            SubSubCategory.SubCategoryId = CategoryMasterId.Id;
                                            SubSubCategory.SubSubCategoryId = SubSubCatId.Id;
                                            SubSubCategory.ProductId = ProductId;
                                            AssignedSubSubCategories.Add(SubSubCategory);
                                        }
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(product.SubSubCategory3))
                            {
                                var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory3.ToLower().Trim()).FirstOrDefault();
                                if (CategoryMasterId != null)
                                {
                                    string[] SubSubCategories = product.SubSubCategory3.Split(',');
                                    foreach (var subcategory in SubSubCategories)
                                    {
                                        ProductAssignedSubSubCategories SubSubCategory = new ProductAssignedSubSubCategories();
                                        var SubSubCatId = SubCategoryData.ToList().Where(i => i.Title.ToLower().Trim() == subcategory.ToLower().Trim()).FirstOrDefault();

                                        if (CategoryMasterId != null)
                                        {
                                            SubSubCategory.SubCategoryId = CategoryMasterId.Id;
                                            SubSubCategory.SubSubCategoryId = SubSubCatId.Id;
                                            SubSubCategory.ProductId = ProductId;
                                            AssignedSubSubCategories.Add(SubSubCategory);
                                        }
                                    }
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
                                        AssignedCollection.ProductId = ProductId;
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

                            if (!string.IsNullOrEmpty(product.ProductUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.ProductUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
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

                            inventory.ProductId = ProductId;

                            InventoryFinalList.Add(inventory);
                            #endregion

                            #region Product Images
                            if (!string.IsNullOrEmpty(product.MainProductImage))
                            {
                                string ext = Path.GetExtension(product.MainProductImage).Replace(".", "");
                                string type = "image/" + ext;
                                Guid MainImageName = Guid.NewGuid();
                                img = new ProductImages();
                                img.ImagePath = product.MainProductImage;
                                img.ImageName = MainImageName.ToString() + "." + ext;
                                img.ProductId = ProductId;
                                img.TenantId = TenantId;
                                img.IsDefaultImage = true;
                                img.Url = product.MainProductImage;
                                img.Ext = ext;
                                img.Type = type;
                                img.Name = MainImageName.ToString() + "." + ext;
                                images.Add(img);
                            }

                            if (!string.IsNullOrEmpty(product.ProductImages))
                            {
                                string[] ProductImages = product.ProductImages.Split(',');

                                foreach (var image in ProductImages)
                                {
                                    if (!string.IsNullOrEmpty(image))
                                    {
                                        imgObj = new ProductImages();
                                        string ext = Path.GetExtension(image).Replace(".", "");
                                        string type = "image/" + ext;
                                        imgObj = new ProductImages();
                                        Guid ImageName = Guid.NewGuid();
                                        imgObj.TenantId = TenantId;
                                        imgObj.ImagePath = image;
                                        imgObj.Url = image;
                                        imgObj.Ext = ext;
                                        imgObj.IsDefaultImage = false;
                                        imgObj.Type = type;
                                        imgObj.Name = ImageName + "." + ext;
                                        imgObj.ImageName = ImageName + "." + ext;
                                        imgObj.ProductId = ProductId;

                                        images.Add(imgObj);
                                    }
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
                              
                                foreach (var image in ProductImages)
                                {
                                    if (!string.IsNullOrEmpty(image))
                                    {
                                        imgLineArts = new ProductMediaImages();
                                        string ext = Path.GetExtension(image).Replace(".", "");
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
                                        imgLineArts.ImageName = ImageName + "." + ext;
                                        imgLineArts.ProductId = ProductId;
                                        MediaImages.Add(imgLineArts);
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(product.LifeStyleImages))
                            {
                                string[] ProductImages = product.LifeStyleImages.Split(',');
                               
                                foreach (var image in ProductImages)
                                {
                                    if (!string.IsNullOrEmpty(image))
                                    {
                                        ProductMediaImages lifeStyleImages = new ProductMediaImages();
                                        string ext = Path.GetExtension(image).Replace(".", "");
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
                                        lifeStyleImages.ImageName = ImageName + "." + ext;
                                        lifeStyleImages.ProductId = ProductId;
                                        MediaImages.Add(lifeStyleImages);
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(product.OtherMediaImages))
                            {
                                string[] ProductImages = product.OtherMediaImages.Split(',');
                              
                                foreach (var image in ProductImages)
                                {
                                    if (!string.IsNullOrEmpty(image))
                                    {
                                        ProductMediaImages otherMediaImages = new ProductMediaImages();
                                        string ext = Path.GetExtension(image).Replace(".", "");
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
                                        otherMediaImages.ImageName = ImageName + "." + ext;
                                        otherMediaImages.ProductId = ProductId;
                                        MediaImages.Add(otherMediaImages);
                                    }
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
                                foreach (var image in ProductImages)
                                {
                                    if (!string.IsNullOrEmpty(image))
                                    {
                                        productViewImages = new ProductViewImages();
                                        string ext = Path.GetExtension(image).Replace(".", "");
                                        string type = "image/" + ext;

                                        productViewImages = new ProductViewImages();
                                        Guid ImageName = Guid.NewGuid();
                                        productViewImages.TenantId = TenantId;
                                        productViewImages.ImagePath = image;
                                        productViewImages.Ext = ext;
                                        productViewImages.Type = type;
                                        productViewImages.Name = ImageName + "." + ext;
                                        productViewImages.ImageName = ImageName + "." + ext;
                                        productViewImages.ProductId = ProductId;
                                        ProductViewImages.Add(productViewImages);
                                    }
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
                             
                                foreach (var sku in AlternativeProduct)
                                {
                                    alternativeProducts = new AlternativeProducts();
                                    alternativeProducts.ProductId = ProductId;
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
                                foreach (var sku in RelativeProduct)
                                {
                                    relativeProducts = new RelativeProducts();
                                    relativeProducts.ProductId = ProductId;
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
                                                     ProductId = ProductId
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
                            long? BrandingMeasureId = null;
                            if (!string.IsNullOrEmpty(product.BrandingUOM))
                            {
                                BrandingMeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.BrandingUOM.ToLower()).Select(i => i.Id).FirstOrDefault();

                            }

                            ProductBrandingLocData = (from item in BrandingPositionData.Where(i=>i.Position_Max_Height_ != null && i.Position_Max_Width_ != null)
                                                      select new ProductBrandingPosition
                                                      {
                                                          ProductId = ProductId,
                                                          LayerTitle = item.Branding_Location_Title_,
                                                          PostionMaxHeight = !string.IsNullOrEmpty(item.Position_Max_Height_) ? Convert.ToDouble(item.Position_Max_Height_) : 0,
                                                          PostionMaxwidth = !string.IsNullOrEmpty(item.Position_Max_Width_) ? Convert.ToDouble(item.Position_Max_Width_) : 0,
                                                          ImageFileURL = item.Branding_Location_Image_,
                                                          ImageName = Guid.NewGuid().ToString(),
                                                          UnitOfMeasureId = BrandingMeasureId.HasValue ? BrandingMeasureId : null
                                                      }).ToList();

                            if (ProductBrandingLocData.Count > 0)
                            {
                                brandingPositionFinalList.AddRange(ProductBrandingLocData);
                            }


                            #endregion

                            #region Branding method assignments
                            if (product.BrandingMethodsseparatedbyacommarelevanttothisproduct != null)
                            {
                                string[] MethodIds = product.BrandingMethodsseparatedbyacommarelevanttothisproduct.Trim().Split(',');

                                foreach (var brandingmethodid in MethodIds)
                                {

                                    if (brandingmethodid != "" && brandingmethodid != "#N/A")
                                    {
                                        long Id = _brandingMethodMasterRepository.GetAllList(i => i.UniqueNumber == Convert.ToInt64(brandingmethodid)).Select(i => i.Id).FirstOrDefault();
                                        if (Id > 0)
                                        {
                                            Method = new ProductBrandingMethods();
                                            Method.ProductMasterId = ProductId;
                                            Method.BrandingMethodId = Id;
                                            Method.IsActive = true;
                                            ProductBrandingMethodsFinalList.Add(Method);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                    }

                    #endregion

                    #region Update Products
                    //commented this code to avoid product update/delete, import must always work for new products not for existing products otherwise it will create a mess in tables.
                    // For now we will be using Utility class for bulk product price update. commented 4-8-22
                    //if (DataToUpdate.Count > 0)
                    //{
                    //    AsyncHelper.RunSync(() => Task.Run(() => UpdateProductData(DataToUpdate, TenantId)));
                    //}

                    #endregion

                    #region Master product creation process

                    if (productData.Count > 0)
                    {


                        if (CategoryGroupsList.Count > 0)
                        {
                            await CreateCategoryGroups(CategoryGroupsList.GroupBy(x => (x.CategoryGroupId, x.CategoryMasterId)).Select(i => i.First()).ToList(), TenantId);
                        }

                        if(CategorySubCategoriesList.Count > 0)
                        {
                            await CreateCategorySubCategories(CategorySubCategoriesList.GroupBy(x => (x.CategoryId, x.SubCategoryId)).Select(i => i.First()).ToList(), TenantId);

                        }

                        //if (CategoryMasters.Count > 0)
                        //{
                        //    await CreateProductAssignedCategoryMaster(CategoryMasters.GroupBy(x => (x.CategoryId, x.ProductId)).Select(i => i.First()).ToList(), TenantId);

                        //}
                        //if (AssignedSubSubCategories.Count > 0)
                        //{
                        //    await CreateProductAssignedSubSubCategories(AssignedSubSubCategories.GroupBy(x => (x.SubCategoryId, x.SubSubCategoryId, x.ProductId)).Select(i => i.First()).ToList(), TenantId);

                        //}

                        if (CategoryMasters.Count > 0)
                        {
                            await CreateProductAssignedCategoryMaster(CategoryMasters.GroupBy(x => (x.CategoryId, x.ProductId)).Select(i => i.First()).ToList(), TenantId);

                        }
                        if (AssignedCategories.Count > 0)
                        {
                            await CreateProductAssignedCategoriesAsync(AssignedCategories.GroupBy(x => (x.CategoryId, x.SubCategoryId, x.ProductId)).Select(i => i.First()).ToList(), TenantId);

                        }

                        if (AssignedSubSubCategories.Count > 0)
                        {
                            await CreateProductAssignedSubSubCategories(AssignedSubSubCategories.GroupBy(x => (x.SubCategoryId, x.SubSubCategoryId, x.ProductId)).Select(i => i.First()).ToList(), TenantId);

                        }

                        if (collectionCategoryList.Count > 0)
                        {
                            await CreateCategoryCollection(collectionCategoryList.GroupBy(x => (x.CategoryId, x.CollectionId)).Select(i => i.First()).ToList(), TenantId);
                        }

                        if (ProductDetailsFinalList.Count > 0)
                        {
                            await CreateProductDetailsAsync(ProductDetailsFinalList, TenantId);
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
                                      // group variant by variant.ParentSKU into variants
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
                                                          // Shape = variant.shape
                                                          IsChargeTaxVariant = colorvariant.IsChargeTax,
                                                          CostPerItem = colorvariant.CostPerItem,
                                                          IsTrackQuantity = colorvariant.IsTrackQuantity,
                                                          PriceVariantModel = colorvariant.QuantityPriceVariantModel,
                                                          DiscountPercentage = colorvariant.DiscountPercentage,
                                                          DiscountPercentageDraft = colorvariant.DiscountPercentage,
                                                          Profit = colorvariant.Profit,
                                                          OnSale = colorvariant.IsOnSale,
                                                          SaleUnitPrice = colorvariant.UnitPrice,
                                                          SalePrice = colorvariant.SalePrice
                                                      }).ToList()

                                  }).ToList();

                    AsyncHelper.RunSync(() => Task.Run(() => CreateVariantChildData(result, TenantId)));

                    #endregion

                    CreateErrorLogsWithException("1334", "CreateProducts successfully executed", "");

                }


                //delete data from temp table
                #region delete data from temp product bulk table

                await DeleteProductBulkData();

                #endregion

                #region send email to current logged in admin user

                await SendEmailOnUploadSuccess(true, TenantId, "");

                #endregion


            }
            catch (Exception ex)
            {
                string exception = ex.StackTrace + "________________________________" + ex.Message;
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                CreateErrorLogsWithException(line.ToString(), ex.Message, ex.StackTrace);


                #region send email to current logged in admin user

                await SendEmailOnUploadSuccess(false, TenantId, ex.StackTrace);

                #endregion

            }
        }
        
        public virtual async Task SendEmailOnUploadSuccess(bool IsSuccess, int TenantId, string ErrorDetails)
        {
            try
            {
                if (IsSuccess == true)
                {
                    var pathToFile = _Environment.WebRootPath + "//EmailTemplates//BulkProductSuccess.html";
                    var SupplierLogo = _userBusinessSettings.GetAllList(i => i.TenantId == TenantId).FirstOrDefault();
                    var user = _userRepository.GetAllList(i => i.Id == SupplierLogo.UserId).FirstOrDefault();
                    // code to bind HTML content with replace tags
                    StreamReader ObjStream;
                    ObjStream = new StreamReader(pathToFile);
                    string Content = ObjStream.ReadToEnd();
                    ObjStream.Close();
                    Content = Regex.Replace(Content, "{name}", user.Name + " " + user.Surname);
                    if (!string.IsNullOrEmpty(SupplierLogo.LogoUrl))
                    {
                        Content = Regex.Replace(Content, "{logo}", SupplierLogo.LogoUrl);
                    }
                    Content = Regex.Replace(Content, "{body}", "Product bulk import process executed successfully!");
                    string EmailBody = Content;

                    //----------------------------------------------------
                    EmailHelper Emailhelper = new EmailHelper(_configuration);
                   
                    EmailDto email = new EmailDto();
                    email.EmailTo = user.EmailAddress;
                    email.Subject = "Bulk Products imported successfully to the cazner admin portal.";
                    email.Body = EmailBody;
                    await Emailhelper.SendEmail(email);
                }
                else
                {

                    // when file upload flow came up with any error.
                    var pathToFile = _Environment.WebRootPath + "//EmailTemplates//BulkProductFailure.html";
                    var SupplierLogo = _userBusinessSettings.GetAllList(i => i.TenantId == TenantId).FirstOrDefault();
                    var user = _userRepository.GetAllList(i => i.Id == SupplierLogo.UserId).FirstOrDefault();
                    // code to bind HTML content with replace tags
                    StreamReader ObjStream;
                    ObjStream = new StreamReader(pathToFile);
                    string Content = ObjStream.ReadToEnd();
                    ObjStream.Close();
                    Content = Regex.Replace(Content, "{name}", user.Name + " " + user.Surname);
                    if (!string.IsNullOrEmpty(SupplierLogo.LogoUrl))
                    {
                        Content = Regex.Replace(Content, "{logo}", SupplierLogo.LogoUrl);
                    }
                    Content = Regex.Replace(Content, "{body}", ErrorDetails);
                    string EmailBody = Content;

                    //----------------------------------------------------
                    EmailHelper Emailhelper = new EmailHelper(_configuration);
                   
                    EmailDto email = new EmailDto();
                    email.EmailTo = user.EmailAddress;
                    email.Subject = "Something went wrong while uploading bulk products.";
                    email.Body = EmailBody;
                    await Emailhelper.SendEmail(email);

                }
            }
            catch(Exception ex)
            {

            }
        }



        public virtual async Task CreateVariantChildData(List<ImportColorVariantsDto> productData, int TenantId)
        {
            int NumberOfExcelRows = productData.Count();

            try
            {
                List<ProductBulkImportDataHistory> productToAddList = new List<ProductBulkImportDataHistory>();
                List<ProductBulkUploadVariations> BulkVariationsFinalList = new List<ProductBulkUploadVariations>();
                List<ProductVariantOptionValues> VariantOptionValuesFinalList = new List<ProductVariantOptionValues>();
                


                List<ProductVariantdataImages> VariantImagesFinalList = new List<ProductVariantdataImages>();

                var MasterOptions = _productOptionsMasterRepository.GetAll();
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    var AllProducts = _productVariantsDataRepository.GetAll();
                    var ProductMasterData = _productMasterRepository.GetAll();

                    long CurId = _currencyMasterRepository.GetAllList(i => i.CurrencyName.Trim() == "$").Select(i => i.Id).FirstOrDefault();
                    List<ColorVariantsModel> VariantListExistsInDB = new List<ColorVariantsModel>();
                    List<ProductVariantsData> ProductVariantsDataToBeDeleted = new List<ProductVariantsData>();
                    List<ProductVariantQuantityPrices> priceVariantsListToBeInserted = new List<ProductVariantQuantityPrices>();
                    List<ProductVariantQuantityPrices> priceVariantsList = new List<ProductVariantQuantityPrices>();
                    //List<ProductVariantsData> ProductVariantsDataList = new List<ProductVariantsData>();
                    List<ProductVariantdataImages> ProductVariantImagesList = new List<ProductVariantdataImages>();

                    await _connectionUtility.EnsureConnectionOpenAsync();
                    CreateErrorLogsWithException("Line no 3438 first foreach", "Line no 3438 first foreach", "Line no 3438 first foreach");
                    foreach (var item in productData)
                    {
                        priceVariantsList = new List<ProductVariantQuantityPrices>();
                        ProductVariantImagesList = new List<ProductVariantdataImages>();
                        List<ProductVariantsData> VariantsDataListFinalList = new List<ProductVariantsData>();
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
                            var dataToInsert = distictExcelSKU.Select(v => v.SKU.Trim()).ToList().Except(AllProducts.ToList().Where(c => c.ProductId == IsProductExists.Id).Select(b => b.SKU).ToList()).ToList();
                            var variantDataList = distictExcelSKU;
                            var variantDataListToBeInserted = distictExcelSKU.Where(x => dataToInsert.Contains(x.SKU)).ToList();
                            var variantDataListToBeUpdated = distictExcelSKU.Where(x => !dataToInsert.Contains(x.SKU)).ToList();
                           
                        
                                IEnumerable<ProductVariantsData> VariantsToBeDeleted = await _db.QueryAsync<ProductVariantsData>("select * from ProductVariantsData where ProductId = "+ IsProductExists.Id + " and TenantId="+ TenantId +"", new
                                {
                                   
                                }, commandType: System.Data.CommandType.Text);

                                //AllProducts.ToList().Where(i => i.ProductId == IsProductExists.Id && variantDataListToBeUpdated.Any(j => j.SKU.Trim() == i.SKU.Trim())).ToList();
 

                            //var variantDataList = variantDataListToBeInserted;
                            if (variantDataListToBeUpdated.Count > 0)
                            {
                                ProductVariantsDataToBeDeleted.AddRange(VariantsToBeDeleted.ToList());
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
                            List<ProductVariantsData> ProductVariantsDataList = new List<ProductVariantsData>();

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
                                    VariantData.Price = variant.Price == "" ? 0 : Convert.ToDecimal(variant.Price);

                                    #region extra fields
                                    VariantData.ComparePrice = string.IsNullOrEmpty(variant.ComparePrice) ? 0 : Convert.ToDecimal(variant.ComparePrice);
                                    VariantData.CostPerItem = string.IsNullOrEmpty(variant.CostPerItem) ? 0 : Convert.ToDecimal(variant.CostPerItem);
                                    VariantData.Margin = string.IsNullOrEmpty(variant.Margin) ? "" : variant.Margin;
                                    VariantData.SalePrice = string.IsNullOrEmpty(variant.SalePrice) ? 0 : Convert.ToDecimal(variant.SalePrice);
                                    VariantData.DiscountPercentage = string.IsNullOrEmpty(variant.DiscountPercentage) ? 0 : Convert.ToDouble(variant.DiscountPercentage);
                                    VariantData.DiscountPercentageDraft = string.IsNullOrEmpty(variant.DiscountPercentageDraft) ? 0 : Convert.ToDouble(variant.DiscountPercentageDraft);
                                    VariantData.OnSale = variant.OnSale == "1" ? true : false;
                                    VariantData.SaleUnitPrice = string.IsNullOrEmpty(variant.SaleUnitPrice) ? 0 : Convert.ToDecimal(variant.SaleUnitPrice);

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

                                    // VariantData.ProfitCurrencySymbol = string.IsNullOrEmpty(variant.ProfitCurrencySymbol) ? 0 : Convert.ToInt32(variant.ProfitCurrencySymbol);
                                    VariantData.Profit = string.IsNullOrEmpty(variant.Profit)  ? 0 : Convert.ToDecimal(variant.Profit);
                                    VariantData.IsChargeTaxVariant = string.IsNullOrEmpty(variant.IsChargeTaxVariant) ? false : variant.IsChargeTaxVariant.ToLower().Trim() == "true" ? true : false;
                                    VariantData.IsTrackQuantity = string.IsNullOrEmpty(variant.IsTrackQuantity) ? false : variant.IsTrackQuantity.ToLower().Trim() == "true" ? true : false;
                                    VariantData.Shape = string.IsNullOrEmpty(variant.Shape) ? "" : variant.Shape;
                                    #endregion

                                    VariantData.SKU = variant.SKU;
                                    VariantData.BarCode = string.IsNullOrEmpty(variant.BarCode) ? "" : variant.BarCode;
                                    VariantData.IsActive = true;

                                    //ProductVariantsDataList.Add(VariantData);
                                    //if (ProductVariantsDataList.Count > 0)
                                    //{
                                    //    VariantsDataListFinalList.AddRange(ProductVariantsDataList);
                                    //}

                                    if (VariantData != null)
                                    {
                                        VariantsDataListFinalList.Add(VariantData);
                                    }

                                }
                            }
                        

                            // creation of product variant data

                            if (VariantsDataListFinalList.Count > 0)
                            {
                                await CreateVariantsDataAsync(VariantsDataListFinalList, TenantId); 
                            }
                            //--------------------------------------
                            
                            var VariantUpdatedData = await _db.QueryAsync<ProductVariantsData>("select * from ProductVariantsData where ProductId = " + IsProductExists.Id + " and TenantId=" + TenantId + "", new
                            {

                            }, commandType: System.Data.CommandType.Text);
                            //_productVariantsDataRepository.GetAllList(i=>i.ProductId == IsProductExists.Id).ToList();
                            // another loop to make list for child objects
                           
                            foreach (var variant in variantDataList/*item.VariantsData*/)
                            {

                                var VariantData = VariantUpdatedData.Where(i => i.SKU.Trim() == variant.SKU.Trim() && i.ProductId == IsProductExists.Id).FirstOrDefault();
                                if (VariantData != null)
                                {
                                    if (!string.IsNullOrEmpty(variant.SKU) && (!string.IsNullOrEmpty(variant.Color) || !string.IsNullOrEmpty(variant.Size) || !string.IsNullOrEmpty(variant.Material) || !string.IsNullOrEmpty(variant.Style)))
                                    {
                                        List<ProductVariantOptionValues> VariantOptionValuesList = new List<ProductVariantOptionValues>();

                                        string[] VariantIds = VariantData.VariantMasterIds.Split('/');
                                        string[] VariantValues = VariantData.Variant.Split('/');


                                        for (int i = 0; i < VariantIds.Length; i++)
                                        {

                                            if (VariantIds[i] != "" && VariantValues[i] != "")
                                            {
                                                ProductVariantOptionValues ModelVariantOptionValues = new ProductVariantOptionValues();
                                                ModelVariantOptionValues.TenantId = TenantId;
                                                ModelVariantOptionValues.VariantOptionId = Convert.ToInt32(VariantIds[i]);
                                                ModelVariantOptionValues.VariantOptionValue = VariantValues[i].ToString();
                                                ModelVariantOptionValues.ProductVariantId = VariantData.Id;
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
                                                if (!string.IsNullOrEmpty(image))
                                                {
                                                    ProductVariantdataImages Image = new ProductVariantdataImages();
                                                    string ext = Path.GetExtension(image).Replace(".","");
                                                    string type = "image/" + ext;

                                                    Guid ImageName = Guid.NewGuid();
                                                    Image.TenantId = TenantId;
                                                    Image.ImageURL = image;
                                                    Image.ImageFileName = ImageName + "." + ext;
                                                    Image.ProductVariantId = VariantData.Id;
                                                    Image.ProductId = IsProductExists.Id;
                                                    Image.Ext = ext;
                                                    Image.Name = ImageName + "." + ext;
                                                    Image.Type = type;
                                                    ProductVariantImagesList.Add(Image);
                                                }
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
                                                                 ProductVariantId = VariantData.Id
                                                             }).ToList();

                                        if (priceVariantsList.Count > 0)
                                        {
                                            priceVariantsListToBeInserted.AddRange(priceVariantsList);
                                        }

                                        #endregion
                                    }
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
                        //await UpdateBulkProductVariant(VariantListExistsInDB, TenantId);

                        await DeleteBulkProductVariant(ProductVariantsDataToBeDeleted, TenantId);
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
                    CreateErrorLogsWithException("Line no 3759 end of foreach", "Line no 3759 end of foreach", "Line no 3759 end of foreach");
                }
            }
            catch (Exception ex)
            {
                string exception = ex.StackTrace + "________________________________" + ex.Message;
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                CreateErrorLogsWithException(line.ToString(), ex.Message, ex.StackTrace);
            }
        }


        public async Task DeleteBulkProductVariant(List<ProductVariantsData> VariantData, int TenantId)
        {
            try
            {
                CreateErrorLogsWithException("line no 3773 DeleteBulkProductVariant bulk", "line no 3773 DeleteBulkProductVariant bulk", "line no 3773 deletevariant bulk");
                List<ProductVariantsData> VariantsDataListToBeDeleted = new List<ProductVariantsData>();
                List<ProductVariantOptionValues> VariantsOptionsToBeDeleted = new List<ProductVariantOptionValues>();
                List<ProductVariantdataImages> ProductVariantImagesToBeDeleted = new List<ProductVariantdataImages>();
                List<ProductVariantQuantityPrices> VariantQuantityPricesToBeDeleted = new List<ProductVariantQuantityPrices>();
                List<ProductVariantWarehouse> VariantWareHousesToBeDeleted = new List<ProductVariantWarehouse>();
                List<ProductBulkUploadVariations> VariantVariationsToBeDeleted = new List<ProductBulkUploadVariations>();


                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    var AllVariantValues = _productVariantOptionValuesRepository.GetAll();
                    var AllVariantImages = _productVariantdataImagesRepository.GetAll();
                    var AllBulkUploadVariations = _productVariantQuantityPricesRepository.GetAll();
                    var AllVariantPriceQtyData = _productVariantQuantityPricesRepository.GetAll();
                    var AllWarehouseData = _productVariantWarehouseRepository.GetAll();
                    var AllBulkUploadVariationData = _productBulkUploadVariationsRepository.GetAll();
                    //----------------------------------------------------------------------------------//
                    var data = VariantData.Select(e => e.Id).ToList();

                    DataTable table = new DataTable();
                    using (var reader = ObjectReader.Create(VariantData, "Id"))
                    {
                        table.Load(reader);
                    }


                    try
                    {
                        await _connectionUtility.EnsureConnectionOpenAsync();
                        // VariantsOptionsToBeDeleted = AllVariantValues.ToList().Where(i => VariantData.Any(j => j.Id == i.ProductVariantId)).ToList();
                        var dts = await _db.QueryAsync<ProductVariantOptionValues>("usp_GetOptionToBeDeletedById", new
                        {
                            TVP = table
                        }, commandType: System.Data.CommandType.StoredProcedure);

                        VariantsOptionsToBeDeleted = (from listdata in dts
                                                      select new ProductVariantOptionValues
                                                      {
                                                          Id = listdata.Id
                                                      }).ToList();

                        if (VariantsOptionsToBeDeleted.Count > 0)
                        {
                            await DeleteVariantOptionValuessAsync(VariantsOptionsToBeDeleted, TenantId);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    try
                    {

                        // ProductVariantImagesToBeDeleted = AllVariantImages.ToList().Where(i => VariantData.Any(j => j.Id == i.ProductVariantId)).ToList();
                        var ImageData = await _db.QueryAsync<ProductVariantdataImages>("usp_GetDataImageToBeDeletedById", new
                        {
                            TVP = table
                        }, commandType: System.Data.CommandType.StoredProcedure);
                        if (ImageData != null)
                        {
                            ProductVariantImagesToBeDeleted = (from listdata in ImageData
                                                               select new ProductVariantdataImages
                                                               {
                                                                   Id = listdata.Id

                                                               }).ToList();


                            if (ProductVariantImagesToBeDeleted.Count > 0)
                            {
                                await DeleteProductVariantImagesAsync(ProductVariantImagesToBeDeleted, TenantId);
                            }
                        }
                    }

                    catch (Exception ex)
                    {

                    }
                    try
                    {

                        // VariantQuantityPricesToBeDeleted = AllVariantPriceQtyData.ToList().Where(i => VariantData.Any(j => j.Id == i.ProductVariantId)).ToList();
                        var variantQuantity = await _db.QueryAsync<ProductVariantQuantityPrices>("usp_GetQuantityPricesToBeDeletedById", new
                        {
                            TVP = table
                        }, commandType: System.Data.CommandType.StoredProcedure);
                        if (variantQuantity != null)
                        {

                            VariantQuantityPricesToBeDeleted = (from listdata in variantQuantity
                                                                select new ProductVariantQuantityPrices
                                                                {
                                                                    Id = listdata.Id

                                                                }).ToList();


                            if (VariantQuantityPricesToBeDeleted.Count > 0)
                            {
                                await DeleteVariantsPriceDataAsync(VariantQuantityPricesToBeDeleted, TenantId);
                            }
                        }
                    }

                    catch (Exception ex)
                    {

                    }
                    try
                    {

                        // VariantWareHousesToBeDeleted = AllWarehouseData.ToList().Where(i => VariantData.Any(j => j.Id == i.ProductVariantId)).ToList();
                        var warehouse = await _db.QueryAsync<ProductVariantWarehouse>("usp_GetWareHousesToBeDeletedById", new
                        {
                            TVP = table
                        }, commandType: System.Data.CommandType.StoredProcedure);
                        if (warehouse != null)
                        {


                            VariantWareHousesToBeDeleted = (from listdata in warehouse
                                                            select new ProductVariantWarehouse
                                                            {
                                                                Id = listdata.Id

                                                            }).ToList();


                            if (VariantWareHousesToBeDeleted.Count > 0)
                            {
                                await DeleteVariantsWareHouses(VariantWareHousesToBeDeleted, TenantId);
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {

                        //  VariantVariationsToBeDeleted = AllBulkUploadVariationData.ToList().Where(i => VariantData.Any(j => j.ProductId == i.ProductId)).ToList();
                        var ProductData = VariantData.Select(e => e.ProductId).Distinct().ToList();
                        var variant = await _db.QueryAsync<ProductBulkUploadVariations>("usp_GetVariationsToBeDeletedById", new
                        {
                            List = ProductData,
                        }, commandType: System.Data.CommandType.StoredProcedure);
                        if (variant != null)
                        {
                            VariantVariationsToBeDeleted = (from listdata in variant
                                                            select new ProductBulkUploadVariations
                                                            {
                                                                Id = listdata.Id

                                                            }).ToList();

                            if (VariantVariationsToBeDeleted.Count > 0)
                            {
                                await DeleteProductBulkUploadVariantsAsync(VariantVariationsToBeDeleted, TenantId);
                            }

                        }
                    }

                    catch (Exception ex)
                    {

                    }
                    if (VariantData.Count > 0)
                    {
                        await DeleteProductVariantDataAsync(VariantData, TenantId);
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
                    var AllVariantPriceQtyData = _productVariantQuantityPricesRepository.GetAll();
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
                    if (VariantsDataListToBeDeleted.Count > 0)
                    {
                        await DeleteProductVariantDataAsync(VariantsDataListToBeDeleted, TenantId);
                    }
                }
            }

            catch (Exception ex)
            {

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
                    var allProductsDetails = _productDetailsRepository.GetAll();
                    var allDimensionsInventory = _productDimensionsInventoryRepository.GetAll();
                    var allProductImages = _productImagesRepository.GetAll();
                    var allProductMediaImages = _productMediaImagesRepository.GetAll();
                    var allProductBrandingPosition = _productBrandingPositionRepository.GetAll();
                    var allProductBrandingMethods = _productBrandingMethodsRepository.GetAll();
                    var allProductPriceVariants = _productVolumeDiscountRepository.GetAll();
                    var MaterialMasterData = _productMaterialMasterRepository.GetAll();
                    var BrandingMethodData = _brandingMethodMasterRepository.GetAll();
                    var CategoryMasterData = _categoryMasterRepository.GetAll();
                    var GroupMasterData = _categoryGroupMasterRepository.GetAll();
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
                    var SubCategoryData = _SubCategoryMasterRepository.GetAll();
                    List<ProductDimensionsInventory> InventoryListToBeUpdated = new List<ProductDimensionsInventory>();
                    List<ProductDimensionsInventory> InventoryListToBeInserted = new List<ProductDimensionsInventory>();
                    List<ProductMaster> ProductMasterFinalList = new List<ProductMaster>();
                    List<ProductDetails> ProductDetailsListToBeUpdated = new List<ProductDetails>();
                    List<ProductDetails> ProductDetailsListToBeInserted = new List<ProductDetails>();
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
                    List<ProductBrandingPosition> ProductBrandingPositionFinalList = new List<ProductBrandingPosition>();
                    List<ProductBrandingMethods> ProductBrandingMethodsFinalList = new List<ProductBrandingMethods>();
                    List<ProductBrandingMethods> ProductBrandingMethodsToBeDeleted = new List<ProductBrandingMethods>();
                    List<ProductBrandingPosition> ProductBrandingPositionToBeDeleted = new List<ProductBrandingPosition>();

                    List<ProductAssignedCategoryMaster> AssignedCategoryToBeDeleted = new List<ProductAssignedCategoryMaster>();
                    List<ProductAssignedSubCategories> AssignedSubCategoryToBeDeleted = new List<ProductAssignedSubCategories>();
                    List<ProductAssignedSubSubCategories> AssignedSubSubCategoryToBeDeleted = new List<ProductAssignedSubSubCategories>();
                    List<BrandingMethodMaster> BrandingMethodFinalList = new List<BrandingMethodMaster>();
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
                    List<ProductTagMaster> ProductTagsFinalList = new List<ProductTagMaster>();
                    List<ProductBrandMaster> ProductBrandsFinalList = new List<ProductBrandMaster>();
                    List<ProductMaterialMaster> ProductMaterialFinalList = new List<ProductMaterialMaster>();
                    List<ProductTypeMaster> ProductTypeFinalList = new List<ProductTypeMaster>();
                    List<CategoryMaster> Catgorymasterlist = new List<CategoryMaster>();
                    List<SubCategoryMaster> SubCatgorymasterlist = new List<SubCategoryMaster>();
                    List<CategoryGroupMaster> CategoryGroupMasterList = new List<CategoryGroupMaster>();
                    List<CategoryGroups> CategoryGroupsList = new List<CategoryGroups>();
                    List<CategoryCollections> collectionCategoryList = new List<CategoryCollections>();
                    List<CollectionMaster> collectionListFinalList = new List<CollectionMaster>();
                    List<CategorySubCategories> CategorySubCategoriesList = new List<CategorySubCategories>();
                    List<ProductAssignedSubSubCategories> AssignedSubSubCategories = new List<ProductAssignedSubSubCategories>();
                    List<ProductAssignedCategoryMaster> CategoryMasters = new List<ProductAssignedCategoryMaster>();
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
                    SubCategoryMaster SubCategorymaster = new SubCategoryMaster();
                    foreach (var detail in productDataToUpdate)
                    {
                      
                        categorymaster = new CategoryMaster();
                        BrandingMethodList = new List<BrandingMethodMaster>();
                        collectionList = new List<CollectionMaster>();
                        GroupMaster = new CategoryGroupMaster();
                        SubCategorymaster = new SubCategoryMaster();
                        #region old code
                        //if (!string.IsNullOrEmpty(detail.Groupcategory))
                        //{
                        //    //---------------Group Master -----------------------
                        //    GroupMasterId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == detail.Groupcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                        //    CategoryGroupMaster groupObj = CategoryGroupMasterList.Where(i => i.GroupTitle.ToLower().Trim() == detail.Groupcategory.Trim()).FirstOrDefault();

                        //    if (GroupMasterId == 0 && groupObj == null)
                        //    {
                        //        GroupMaster.GroupTitle = detail.Groupcategory;
                        //        GroupMaster.TenantId = TenantId;
                        //        CategoryGroupMasterList.Add(GroupMaster);
                        //    }
                        //}
                        ////-------------------------Group Master ends-----------------


                        //if (!string.IsNullOrEmpty(detail.Productcategory))
                        //{
                        //    CategoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == detail.Productcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                        //    var IsExists = Catgorymasterlist.Where(i => i.CategoryTitle.ToLower() == detail.Productcategory.Trim()).FirstOrDefault();


                        //    if (CategoryId == 0 && IsExists == null)
                        //    {
                        //        categorymaster.CategoryTitle = detail.Productcategory.Trim();
                        //        categorymaster.TenantId = TenantId;

                        //        if (IsExists == null)
                        //        {
                        //            Catgorymasterlist.Add(categorymaster);

                        //        }
                        //    }
                        //}
                        #endregion

                        if (!string.IsNullOrEmpty(detail.GroupCategory1))
                        {
                            //---------------Group Master -----------------------
                            long GroupMasterId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory1.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            CategoryGroupMaster groupObj = CategoryGroupMasterList.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory1.Trim()).FirstOrDefault();

                            if (GroupMasterId == 0 && groupObj == null)
                            {
                                GroupMaster = new CategoryGroupMaster();
                                GroupMaster.GroupTitle = detail.GroupCategory1;
                                GroupMaster.TenantId = TenantId;
                                CategoryGroupMasterList.Add(GroupMaster);
                            }
                        }

                        if (!string.IsNullOrEmpty(detail.GroupCategory2))
                        {
                            //---------------Group Master -----------------------
                            long GroupMasterId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory2.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            CategoryGroupMaster groupObj = CategoryGroupMasterList.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory2.Trim()).FirstOrDefault();

                            if (GroupMasterId == 0 && groupObj == null)
                            {
                                GroupMaster = new CategoryGroupMaster();
                                GroupMaster.GroupTitle = detail.GroupCategory2;
                                GroupMaster.TenantId = TenantId;
                                CategoryGroupMasterList.Add(GroupMaster);
                            }
                        }


                        if (!string.IsNullOrEmpty(detail.GroupCategory3))
                        {
                            //---------------Group Master -----------------------
                            long GroupMasterId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory3.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            CategoryGroupMaster groupObj = CategoryGroupMasterList.Where(i => i.GroupTitle.ToLower().Trim() == detail.GroupCategory3.Trim()).FirstOrDefault();

                            if (GroupMasterId == 0 && groupObj == null)
                            {
                                GroupMaster = new CategoryGroupMaster();
                                GroupMaster.GroupTitle = detail.GroupCategory3;
                                GroupMaster.TenantId = TenantId;
                                CategoryGroupMasterList.Add(GroupMaster);
                            }
                        }

                        if (!string.IsNullOrEmpty(detail.SubCategory1))
                        {
                            long CategoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == detail.SubCategory1.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            var IsExists = Catgorymasterlist.Where(i => i.CategoryTitle.ToLower() == detail.SubCategory1.Trim()).FirstOrDefault();

                            if (CategoryId == 0 && IsExists == null)
                            {
                                categorymaster = new CategoryMaster();
                                categorymaster.CategoryTitle = detail.SubCategory1.Trim();
                                categorymaster.TenantId = TenantId;

                                if (IsExists == null)
                                {
                                    Catgorymasterlist.Add(categorymaster);

                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(detail.SubCategory2))
                        {

                            long CategoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == detail.SubCategory2.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            var IsExists = Catgorymasterlist.Where(i => i.CategoryTitle.ToLower() == detail.SubCategory2.Trim()).FirstOrDefault();

                            if (CategoryId == 0 && IsExists == null)
                            {
                                categorymaster = new CategoryMaster();
                                categorymaster.CategoryTitle = detail.SubCategory2.Trim();
                                categorymaster.TenantId = TenantId;

                                if (IsExists == null)
                                {
                                    Catgorymasterlist.Add(categorymaster);

                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(detail.SubCategory3))
                        {

                            long CategoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == detail.SubCategory3.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            var IsExists = Catgorymasterlist.Where(i => i.CategoryTitle.ToLower() == detail.SubCategory3.Trim()).FirstOrDefault();

                            if (CategoryId == 0 && IsExists == null)
                            {
                                categorymaster = new CategoryMaster();
                                categorymaster.CategoryTitle = detail.SubCategory3.Trim();
                                categorymaster.TenantId = TenantId;

                                if (IsExists == null)
                                {
                                    Catgorymasterlist.Add(categorymaster);

                                }
                            }

                        }

                        if (!string.IsNullOrEmpty(detail.SubSubCategory1))
                        {

                            string[] SubCategories = detail.SubSubCategory1.Split(',');
                            foreach (var sub in SubCategories)
                            {
                                long CategoryId = SubCategoryData.Where(i => i.Title.ToLower().Trim() == sub.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                var IsExists = SubCatgorymasterlist.Where(i => i.Title.ToLower() == sub.Trim()).FirstOrDefault();


                                if (CategoryId == 0 && IsExists == null)
                                {
                                    SubCategorymaster = new SubCategoryMaster();
                                    SubCategorymaster.Title = sub.Trim();
                                    SubCategorymaster.TenantId = TenantId;

                                    if (IsExists == null)
                                    {
                                        SubCatgorymasterlist.Add(SubCategorymaster);

                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(detail.SubSubCategory2))
                        {
                            string[] SubCategories = detail.SubSubCategory2.Split(',');
                            foreach (var sub in SubCategories)
                            {
                                long CategoryId = SubCategoryData.Where(i => i.Title.ToLower().Trim() == sub.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                var IsExists = SubCatgorymasterlist.Where(i => i.Title.ToLower() == sub.Trim()).FirstOrDefault();


                                if (CategoryId == 0 && IsExists == null)
                                {
                                    SubCategorymaster = new SubCategoryMaster();
                                    SubCategorymaster.Title = sub.Trim();
                                    SubCategorymaster.TenantId = TenantId;

                                    if (IsExists == null)
                                    {
                                        SubCatgorymasterlist.Add(SubCategorymaster);

                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(detail.SubSubCategory3))
                        {
                            string[] SubCategories = detail.SubSubCategory3.Split(',');
                            foreach (var sub in SubCategories)
                            {

                                long CategoryId = SubCategoryData.Where(i => i.Title.ToLower().Trim() == sub.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                                var IsExists = SubCatgorymasterlist.Where(i => i.Title.ToLower() == sub.Trim()).FirstOrDefault();


                                if (CategoryId == 0 && IsExists == null)
                                {
                                    SubCategorymaster = new SubCategoryMaster();
                                    SubCategorymaster.Title = sub.Trim();
                                    SubCategorymaster.TenantId = TenantId;

                                    if (IsExists == null)
                                    {
                                        SubCatgorymasterlist.Add(SubCategorymaster);

                                    }
                                }
                            }
                        }

                        //-----------collection master
                        if (!string.IsNullOrEmpty(detail.ProductCollections))
                        {
                            string[] Collections = detail.ProductCollections.Split(',');
                            foreach (var collection in Collections)
                            {
                                var CategoryCollectionId = CollectionsData.Where(i => i.CollectionName.ToLower().Trim() == collection.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
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
                    if (SubCatgorymasterlist.Count > 0)
                    {
                        await CreateSubCategoryMaster(SubCatgorymasterlist.GroupBy(x => x.Title.ToLower().Trim()).Select(g => g.First()).ToList(), TenantId);
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

                    #region product attributes insert logic
                    var CategorySubCategories = _categorySubCategoriesRepository.GetAll();
                    foreach (var product in productDataToUpdate)
                    {

                        List<ProductImages> images = new List<ProductImages>();
                        List<ProductMediaImages> MediaImages = new List<ProductMediaImages>();
                        List<ProductVolumeDiscountVariant> priceVariants = new List<ProductVolumeDiscountVariant>();
                        List<ProductBrandingPosition> ProductBrandingLocData = new List<ProductBrandingPosition>();
                        List<ProductViewImages> ProductViewImages = new List<ProductViewImages>();
                        List<AlternativeProducts> ProductAlternatives = new List<AlternativeProducts>();
                        List<RelativeProducts> RelativeProducts = new List<RelativeProducts>();
                        CategoryMasterData = _categoryMasterRepository.GetAll();
                        CollectionsData = _collectionMasterRepository.GetAll();
                        GroupMasterData = _categoryGroupMasterRepository.GetAll();

                        #region CategoryCollections and CategoryGroups assignments insertion

                        #region Category Groups
                        //------------category group assignment

                        //if (!string.IsNullOrEmpty(product.Productcategory) && !string.IsNullOrEmpty(product.Groupcategory))
                        //{
                        //    long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.Productcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                        //    long groupId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == product.Groupcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

                        //    if (groupId > 0 && categoryId > 0)
                        //    {
                        //        CategoryGroups IsGroupAlreadyExists = CategoryGroupsList.Where(i => i.CategoryGroupId == groupId && i.CategoryMasterId == categoryId).FirstOrDefault();

                        //        if (IsGroupAlreadyExists == null)
                        //        {
                        //            CategoryGroups CategoryGroup = new CategoryGroups();
                        //            CategoryGroup.CategoryGroupId = groupId;
                        //            CategoryGroup.CategoryMasterId = categoryId;
                        //            CategoryGroup.IsActive = true;
                        //            CategoryGroupsList.Add(CategoryGroup);
                        //        }
                        //    }
                        //}

                        if (!string.IsNullOrEmpty(product.GroupCategory1) && !string.IsNullOrEmpty(product.SubCategory1))
                        {
                            long groupId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory1.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory1.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

                            if (groupId > 0 && categoryId > 0)
                            {
                                CategoryGroups IsGroupAlreadyExists = CategoryGroupsList.Where(i => i.CategoryGroupId == groupId && i.CategoryMasterId == categoryId).FirstOrDefault();

                                if (IsGroupAlreadyExists == null)
                                {
                                    CategoryGroups CategoryGroup = new CategoryGroups();
                                    CategoryGroup.CategoryGroupId = groupId;
                                    CategoryGroup.CategoryMasterId = categoryId;
                                    CategoryGroup.IsActive = true;
                                    CategoryGroupsList.Add(CategoryGroup);
                                }
                            }

                            if (!string.IsNullOrEmpty(product.SubSubCategory1) && categoryId > 0)
                            {
                                string[] SubCategories = product.SubSubCategory1.Split(',');
                                foreach (var cat in SubCategories)
                                {
                                    var IsSubCategoryExists = SubCategoryData.Where(i => i.Title.ToLower() == cat.ToLower()).FirstOrDefault();
                                    if (IsSubCategoryExists != null)
                                    {
                                        CategorySubCategories IsCatAlreadyExists = CategorySubCategories.Where(i => i.CategoryId == categoryId && i.SubCategoryId == IsSubCategoryExists.Id).FirstOrDefault();

                                        if (IsCatAlreadyExists == null)
                                        {
                                            CategorySubCategories subcategory = new CategorySubCategories();
                                            subcategory.CategoryId = categoryId;
                                            subcategory.SubCategoryId = IsSubCategoryExists.Id;
                                            subcategory.IsActive = true;
                                            subcategory.TenantId = TenantId;
                                            CategorySubCategoriesList.Add(subcategory);
                                        }
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(product.GroupCategory2) && !string.IsNullOrEmpty(product.SubCategory2))
                        {
                            long groupId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory2.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory2.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

                            if (groupId > 0 && categoryId > 0)
                            {
                                CategoryGroups IsGroupAlreadyExists = CategoryGroupsList.Where(i => i.CategoryGroupId == groupId && i.CategoryMasterId == categoryId).FirstOrDefault();

                                if (IsGroupAlreadyExists == null)
                                {
                                    CategoryGroups CategoryGroup = new CategoryGroups();
                                    CategoryGroup.CategoryGroupId = groupId;
                                    CategoryGroup.CategoryMasterId = categoryId;
                                    CategoryGroup.IsActive = true;
                                    CategoryGroupsList.Add(CategoryGroup);
                                }
                            }

                            if (!string.IsNullOrEmpty(product.SubSubCategory2) && categoryId > 0)
                            {
                                string[] SubCategories = product.SubSubCategory2.Split(',');
                                foreach (var cat in SubCategories)
                                {
                                    var IsSubCategoryExists = SubCategoryData.Where(i => i.Title.ToLower() == cat.ToLower()).FirstOrDefault();
                                    if (IsSubCategoryExists != null)
                                    {
                                        CategorySubCategories IsCatAlreadyExists = CategorySubCategories.Where(i => i.CategoryId == categoryId && i.SubCategoryId == IsSubCategoryExists.Id).FirstOrDefault();

                                        if (IsCatAlreadyExists == null)
                                        {
                                            CategorySubCategories subcategory = new CategorySubCategories();
                                            subcategory.CategoryId = categoryId;
                                            subcategory.SubCategoryId = IsSubCategoryExists.Id;
                                            subcategory.IsActive = true;
                                            subcategory.TenantId = TenantId;
                                            CategorySubCategoriesList.Add(subcategory);
                                        }
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(product.GroupCategory3) && !string.IsNullOrEmpty(product.SubCategory3))
                        {
                            long groupId = GroupMasterData.Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory3.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                            long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory3.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

                            if (groupId > 0 && categoryId > 0)
                            {
                                CategoryGroups IsGroupAlreadyExists = CategoryGroupsList.Where(i => i.CategoryGroupId == groupId && i.CategoryMasterId == categoryId).FirstOrDefault();

                                if (IsGroupAlreadyExists == null)
                                {
                                    CategoryGroups CategoryGroup = new CategoryGroups();
                                    CategoryGroup.CategoryGroupId = groupId;
                                    CategoryGroup.CategoryMasterId = categoryId;
                                    CategoryGroup.IsActive = true;
                                    CategoryGroupsList.Add(CategoryGroup);
                                }
                            }

                            if (!string.IsNullOrEmpty(product.SubSubCategory3) && categoryId > 0)
                            {
                                string[] SubCategories = product.SubSubCategory3.Split(',');
                                foreach (var cat in SubCategories)
                                {
                                    var IsSubCategoryExists = SubCategoryData.Where(i => i.Title.ToLower() == cat.ToLower()).FirstOrDefault();
                                    if (IsSubCategoryExists != null)
                                    {
                                        CategorySubCategories IsCatAlreadyExists = CategorySubCategories.Where(i => i.CategoryId == categoryId && i.SubCategoryId == IsSubCategoryExists.Id).FirstOrDefault();

                                        if (IsCatAlreadyExists == null)
                                        {
                                            CategorySubCategories subcategory = new CategorySubCategories();
                                            subcategory.CategoryId = categoryId;
                                            subcategory.SubCategoryId = IsSubCategoryExists.Id;
                                            subcategory.IsActive = true;
                                            subcategory.TenantId = TenantId;
                                            CategorySubCategoriesList.Add(subcategory);
                                        }
                                    }
                                }
                            }
                        }


                        #endregion


                        #region Category collection assignment

                        //if (!string.IsNullOrEmpty(product.ProductCollections) && !string.IsNullOrEmpty(product.Productcategory))
                        //{

                        //    string[] Collections = product.ProductCollections.Split(',');
                        //    foreach (var collection in Collections)
                        //    {
                        //        CategoryCollections categoryCollection = new CategoryCollections();

                        //        long categoryId = CategoryMasterData.Where(i => i.CategoryTitle.ToLower().Trim() == product.Productcategory.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();
                        //        long CollectionId = CollectionsData.Where(i => i.CollectionName.ToLower().Trim() == collection.ToLower().Trim()).Select(i => i.Id).FirstOrDefault();

                        //        if (categoryId > 0 && CollectionId > 0)
                        //        {
                        //            CategoryCollections IsCollectionExists = collectionCategoryList.Where(i => i.CategoryId == categoryId && i.CollectionId == CollectionId).FirstOrDefault();
                        //            if (IsCollectionExists == null)
                        //            {
                        //                categoryCollection.CategoryId = categoryId;
                        //                categoryCollection.CollectionId = CollectionId;
                        //                categoryCollection.TenantId = TenantId;
                        //                categoryCollection.IsActive = true;
                        //                collectionCategoryList.Add(categoryCollection);
                        //            }

                        //        }
                        //    }
                        //}
                        #endregion

                        #endregion

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
                        //master.UnitOfMeasure = string.IsNullOrEmpty(product.UnitOfMeasure) ? 0 : Convert.ToInt32(product.UnitOfMeasure);
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
                            master.TurnAroundTimeId = TurnAroundTime.Where(i => i.NumberOfDays.ToLower() == product.TurnAroundTime.ToLower()).Select(i => i.Id).FirstOrDefault();
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

                        if (product.QuantityPriceVariantModel != null)
                        {
                            if (product.QuantityPriceVariantModel.Count > 0)
                            {
                                master.ProductHasPriceVariant = true;
                            }
                        }

                        #region ProductDetails

                        #region Product Material
                        if (!string.IsNullOrEmpty(product.ProductMaterial))
                        {

                            var ExistingData = _productAssignedMaterialsRepository.GetAllList(i => i.ProductId == master.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                AssignedMaterialsToBeDeleted.AddRange(ExistingData);
                                // await DeleteProductAssignedMaterialsAsync(ExistingData, TenantId);
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
                                    assignedMaterials.ProductId = master.Id;
                                    AssignedMaterials.Add(assignedMaterials);
                                }

                            }
                        }
                        #endregion

                        #region Product Brand
                        if (!string.IsNullOrEmpty(product.ProductBrands))
                        {

                            var ExistingData = _productAssignedBrandsRepository.GetAllList(i => i.ProductId == master.Id).ToList();
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
                                    AssignedBrand.ProductId = master.Id;
                                    AssignedBrands.Add(AssignedBrand);
                                }

                            }
                        }
                        #endregion

                        #region Product Tags
                        if (!string.IsNullOrEmpty(product.ProductTags))
                        {
                            var ExistingData = _productAssignedTagsRepository.GetAllList(i => i.ProductId == master.Id).ToList();
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
                                    AssignedTag.ProductId = master.Id;
                                    AssignedTags.Add(AssignedTag);
                                }

                            }
                        }
                        #endregion

                        #region Product Types
                        if (!string.IsNullOrEmpty(product.Caznerproducttypes))
                        {
                            var ExistingData = _productAssignedTypesRepository.GetAllList(i => i.ProductId == master.Id).ToList();
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
                                    AssignedType.ProductId = master.Id;
                                    AssignedTypes.Add(AssignedType);
                                }

                            }
                        }
                        #endregion

                        #region Product Vendors
                        if (!string.IsNullOrEmpty(product.ProductVendors))
                        {
                            var ExistingData = _productAssignedVendorsRepository.GetAllList(i => i.ProductId == master.Id).ToList();
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
                                    AssignedVendor.ProductId = master.Id;
                                    AssignedVendors.Add(AssignedVendor);
                                }

                            }
                        }
                        #endregion


                        #region Product Group Categories Master

                        if (!string.IsNullOrEmpty(product.GroupCategory1) || !string.IsNullOrEmpty(product.GroupCategory2) || !string.IsNullOrEmpty(product.GroupCategory3))
                        {
                            var ExistingData = _ProductAssignedCategoryMasterRepository.GetAllList(i => i.ProductId == master.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                AssignedCategoryToBeDeleted.AddRange(ExistingData);
                                // await DeleteProductAssignedMaterialsAsync(ExistingData, TenantId);
                            }
                        }

                        if (!string.IsNullOrEmpty(product.GroupCategory1))
                        {
                            ProductAssignedCategoryMaster AssignCategory = new ProductAssignedCategoryMaster();
                            var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory1.ToLower().Trim()).FirstOrDefault();

                            if (CategoryGroupId != null)
                            {
                                AssignCategory.CategoryId = CategoryGroupId.Id;
                                AssignCategory.ProductId = master.Id;
                                CategoryMasters.Add(AssignCategory);
                            }
                        }

                        if (!string.IsNullOrEmpty(product.GroupCategory2))
                        {

                            ProductAssignedCategoryMaster AssignCategory = new ProductAssignedCategoryMaster();
                            var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory2.ToLower().Trim()).FirstOrDefault();

                            if (CategoryGroupId != null)
                            {
                                AssignCategory.CategoryId = CategoryGroupId.Id;
                                AssignCategory.ProductId = master.Id;
                                CategoryMasters.Add(AssignCategory);
                            }
                        }
                        if (!string.IsNullOrEmpty(product.GroupCategory3))
                        {

                            ProductAssignedCategoryMaster AssignCategory = new ProductAssignedCategoryMaster();
                            var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory3.ToLower().Trim()).FirstOrDefault();

                            if (CategoryGroupId != null)
                            {
                                AssignCategory.CategoryId = CategoryGroupId.Id;
                                AssignCategory.ProductId = master.Id;
                                CategoryMasters.Add(AssignCategory);
                            }
                        }
                        #endregion


                        #region Product Categories

                        if (!string.IsNullOrEmpty(product.SubCategory1) || !string.IsNullOrEmpty(product.SubCategory2) || !string.IsNullOrEmpty(product.SubCategory3))
                        {
                            var ExistingData = _productAssignedCategoriesRepository.GetAllList(i => i.ProductId == master.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                AssignedSubCategoryToBeDeleted.AddRange(ExistingData);
                                // await DeleteProductAssignedMaterialsAsync(ExistingData, TenantId);
                            }
                        }

                        if (!string.IsNullOrEmpty(product.SubCategory1))
                        {

                            ProductAssignedSubCategories AssignedCategory = new ProductAssignedSubCategories();
                            var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory1.ToLower().Trim()).FirstOrDefault();
                            var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory1.ToLower().Trim()).FirstOrDefault();

                            if (CategoryMasterId != null)
                            {
                                AssignedCategory.SubCategoryId = CategoryMasterId.Id;
                                AssignedCategory.CategoryId = CategoryGroupId.Id;
                                AssignedCategory.ProductId = master.Id;
                                AssignedCategories.Add(AssignedCategory);
                            }
                        }
                        if (!string.IsNullOrEmpty(product.SubCategory2))
                        {

                            ProductAssignedSubCategories AssignedCategory = new ProductAssignedSubCategories();
                            var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory2.ToLower().Trim()).FirstOrDefault();
                            var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory2.ToLower().Trim()).FirstOrDefault();

                            if (CategoryMasterId != null && CategoryGroupId != null)
                            {
                                AssignedCategory.SubCategoryId = CategoryMasterId.Id;
                                AssignedCategory.CategoryId = CategoryGroupId.Id;
                                AssignedCategory.ProductId = master.Id;
                                AssignedCategories.Add(AssignedCategory);
                            }
                        }
                        if (!string.IsNullOrEmpty(product.SubCategory3))
                        {

                            ProductAssignedSubCategories AssignedCategory = new ProductAssignedSubCategories();
                            var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory3.ToLower().Trim()).FirstOrDefault();
                            var CategoryGroupId = GroupMasterData.ToList().Where(i => i.GroupTitle.ToLower().Trim() == product.GroupCategory3.ToLower().Trim()).FirstOrDefault();

                            if (CategoryMasterId != null && CategoryGroupId != null)
                            {
                                AssignedCategory.SubCategoryId = CategoryMasterId.Id;
                                AssignedCategory.CategoryId = CategoryGroupId.Id;
                                AssignedCategory.ProductId = master.Id;
                                AssignedCategories.Add(AssignedCategory);
                            }
                        }
                        #endregion

                        #region Product sub sub Categories


                        if (!string.IsNullOrEmpty(product.SubSubCategory1) || !string.IsNullOrEmpty(product.SubSubCategory2) || !string.IsNullOrEmpty(product.SubSubCategory3))
                        {
                            var ExistingData = _ProductAssignedSubSubCategories.GetAllList(i => i.ProductId == master.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                AssignedSubSubCategoryToBeDeleted.AddRange(ExistingData);
                                // await DeleteProductAssignedMaterialsAsync(ExistingData, TenantId);
                            }
                        }

                        if (!string.IsNullOrEmpty(product.SubSubCategory1))
                        {
                            var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory1.ToLower().Trim()).FirstOrDefault();
                            if (CategoryMasterId != null)
                            {
                                string[] SubSubCategories = product.SubSubCategory1.Split(',');
                                foreach (var subcategory in SubSubCategories)
                                {
                                    ProductAssignedSubSubCategories SubSubCategory = new ProductAssignedSubSubCategories();
                                    var SubSubCatId = SubCategoryData.ToList().Where(i => i.Title.ToLower().Trim() == subcategory.ToLower().Trim()).FirstOrDefault();

                                    if (CategoryMasterId != null)
                                    {
                                        SubSubCategory.SubCategoryId = CategoryMasterId.Id;
                                        SubSubCategory.SubSubCategoryId = SubSubCatId.Id;
                                        SubSubCategory.ProductId = master.Id;
                                        AssignedSubSubCategories.Add(SubSubCategory);
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(product.SubSubCategory2))
                        {
                            var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory2.ToLower().Trim()).FirstOrDefault();
                            if (CategoryMasterId != null)
                            {
                                string[] SubSubCategories = product.SubSubCategory2.Split(',');
                                foreach (var subcategory in SubSubCategories)
                                {
                                    ProductAssignedSubSubCategories SubSubCategory = new ProductAssignedSubSubCategories();
                                    var SubSubCatId = SubCategoryData.ToList().Where(i => i.Title.ToLower().Trim() == subcategory.ToLower().Trim()).FirstOrDefault();

                                    if (CategoryMasterId != null)
                                    {
                                        SubSubCategory.SubCategoryId = CategoryMasterId.Id;
                                        SubSubCategory.SubSubCategoryId = SubSubCatId.Id;
                                        SubSubCategory.ProductId = master.Id;
                                        AssignedSubSubCategories.Add(SubSubCategory);
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(product.SubSubCategory3))
                        {
                            var CategoryMasterId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.SubCategory3.ToLower().Trim()).FirstOrDefault();
                            if (CategoryMasterId != null)
                            {
                                string[] SubSubCategories = product.SubSubCategory3.Split(',');
                                foreach (var subcategory in SubSubCategories)
                                {
                                    ProductAssignedSubSubCategories SubSubCategory = new ProductAssignedSubSubCategories();
                                    var SubSubCatId = SubCategoryData.ToList().Where(i => i.Title.ToLower().Trim() == subcategory.ToLower().Trim()).FirstOrDefault();

                                    if (CategoryMasterId != null)
                                    {
                                        SubSubCategory.SubCategoryId = CategoryMasterId.Id;
                                        SubSubCategory.SubSubCategoryId = SubSubCatId.Id;
                                        SubSubCategory.ProductId = master.Id;
                                        AssignedSubSubCategories.Add(SubSubCategory);
                                    }
                                }
                            }
                        }
                        #endregion


                        #region Product Categories
                        //if (!string.IsNullOrEmpty(product.Productcategory))
                        //{
                        //    var ExistingData = _productAssignedCategoriesRepository.GetAllList(i => i.ProductId == master.Id).ToList();
                        //    if (ExistingData.Count > 0)
                        //    {
                        //        AssignedCategoriesToBeDeleted.AddRange(ExistingData);

                        //    }

                        //    ProductAssignedSubCategories AssignedCategory = new ProductAssignedSubCategories();
                        //    var CategoryId = CategoryMasterData.ToList().Where(i => i.CategoryTitle.ToLower().Trim() == product.Productcategory.ToLower().Trim()).FirstOrDefault();
                        //    if (CategoryId != null)
                        //    {
                        //        AssignedCategory.SubCategoryId = CategoryId.Id;
                        //        AssignedCategory.ProductId = master.Id;
                        //        AssignedCategories.Add(AssignedCategory);
                        //    }

                        //}
                        #endregion

                        #region Product Collections

                        if (!string.IsNullOrEmpty(product.ProductCollections))
                        {
                            var ExistingData = _productAssignedCollectionsRepository.GetAllList(i => i.ProductId == master.Id).ToList();
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
                                    AssignedCollection.ProductId = master.Id;
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
                            Newinventory.CartonWeight = !string.IsNullOrEmpty(product.CartonWeight) ? product.CartonWeight : "";
                            Newinventory.CartonCubicWeightKG = product.CartonCubicWeight;
                            Newinventory.ProductDiameter = product.ProductDiameter;
                            Newinventory.ProductDimensionNotes = product.ProductDimensionNotes;

                            if (!string.IsNullOrEmpty(product.UnitOfMeasure))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.UnitOfMeasure.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    Newinventory.ProductUnitMeasureId = MeasureId;
                                }
                            }
                            if (!string.IsNullOrEmpty(product.WeightUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.WeightUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    Newinventory.ProductWeightMeasureId = MeasureId;
                                }
                            }
                            if (!string.IsNullOrEmpty(product.CartonUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.CartonUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    Newinventory.CartonUnitOfMeasureId = MeasureId;
                                }

                            }
                            if (!string.IsNullOrEmpty(product.CartonWeightUOM))
                            {
                                long MeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.CartonWeightUOM.ToLower()).Select(i => i.Id).FirstOrDefault();
                                if (MeasureId > 0)
                                {
                                    Newinventory.CartonWeightMeasureId = MeasureId;
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
                                string ext = Path.GetExtension(product.MainProductImage).Replace(".","");
                                string type = "image/" + ext;
                                ProductImage.IsDefaultImage = true;
                                ProductImage.ImagePath = product.MainProductImage;
                                ProductImage.Ext = ext;
                                ProductImage.Type = type;
                                ProductImage.ImageName= ProductImage.Name + '.' + ext;
                                ProductImage.Name = ProductImage.Name +'.'+ ext;
                                ProductImage.Url= product.MainProductImage;
                                ProductImagesListToBeUpdated.Add(ProductImage);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(product.MainProductImage))
                                {
                                    string ext = Path.GetExtension(product.MainProductImage).Replace(".", "");
                                    string type = "image/" + ext;
                                    ProductImages imgDefault = new ProductImages();
                                    Guid ImageName = Guid.NewGuid();
                                    imgDefault.TenantId = TenantId;
                                    imgDefault.Ext = ext;
                                    imgDefault.Name = ImageName + "." + ext;
                                    imgDefault.Type = type;
                                    imgDefault.IsDefaultImage = true;
                                    imgDefault.ImagePath = product.MainProductImage;
                                    imgDefault.ImageName = ImageName.ToString() + "." + ext;
                                    imgDefault.Url = product.MainProductImage;
                                    imgDefault.ProductId = master.Id;
                                    images.Add(imgDefault);
                                }
                            }
                        }

                   

                        if (!string.IsNullOrEmpty(product.ProductImages))
                        {
                            var ExistingProductImages = allProductImages.Where(i => i.ProductId == master.Id && i.IsDefaultImage == false).ToList();
                            if (ExistingProductImages.Count > 0)
                            {
                                ProductImagesListToBeDeleted.AddRange(ExistingProductImages);

                            }

                            string[] ProductImages = product.ProductImages.Split(',');
                            ProductImages imgObj = new ProductImages();
                            foreach (var image in ProductImages)
                            {
                                if (!string.IsNullOrEmpty(image))
                                {
                                    string ext = Path.GetExtension(image).Replace(".", "");
                                    string type = "image/" + ext;
                                    imgObj = new ProductImages();
                                    Guid ImageName = Guid.NewGuid();
                                    imgObj.TenantId = TenantId;
                                    imgObj.Url = image;
                                    imgObj.Ext = ext;
                                    imgObj.IsDefaultImage = false;
                                    imgObj.Name = ImageName + "." + ext;
                                    imgObj.Type = type;
                                    imgObj.ImageName = ImageName + "." + ext;
                                    imgObj.ImagePath = image;
                                    imgObj.ProductId = master.Id;
                                    images.Add(imgObj);
                                }
                            }
                        }
                        if (images.Count > 0)
                        {
                            ProductImagesListToBeInserted.AddRange(images);
                        }
                        #endregion


                        #region product view images

                        if (!string.IsNullOrEmpty(product.ProductViews))
                        {

                            var ExistingProductViewImages = ProductViewImagesData.Where(i => i.ProductId == master.Id).ToList();
                            if (ExistingProductViewImages.Count > 0)
                            {
                                ProductViewImagesFinalListToBeDeleted.AddRange(ExistingProductViewImages);
                            }

                            string[] ProductImages = product.ProductViews.Split(',');
                            ProductViewImages productViewImages = new ProductViewImages();
                            foreach (var image in ProductImages)
                            {
                                if (!string.IsNullOrEmpty(image))
                                {

                                    string ext = Path.GetExtension(image).Replace(".", "");
                                    string type = "image/" + ext;
                                    productViewImages = new ProductViewImages();
                                    Guid ImageName = Guid.NewGuid();
                                    productViewImages.TenantId = TenantId;
                                    productViewImages.ImagePath = image;
                                    productViewImages.Ext = ext;
                                    productViewImages.Type = type;
                                    productViewImages.Name = ImageName + "." + ext;
                                    productViewImages.ImageName = ImageName + "." + ext;
                                    productViewImages.ProductId = master.Id;
                                    ProductViewImages.Add(productViewImages);
                                }
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

                            var ExistingAlternative = AlternativeProductsData.Where(i => i.ProductId == master.Id).ToList();
                            if (ExistingAlternative.Count > 0)
                            {
                                ProductAlternativesToBeDeleted.AddRange(ExistingAlternative);
                            }

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


                        if (!string.IsNullOrEmpty(product.RelatedProducts))
                        {

                            var ExistingRelative = RelativeProductsData.Where(i => i.ProductId == master.Id).ToList();
                            if (ExistingRelative.Count > 0)
                            {
                                RelativeProductsFinalListToBeDeleted.AddRange(ExistingRelative);
                            }

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
                        var lifeStyleTypeId = MediaType.Where(i => i.ProductMediaImageTypeName.ToLower().Trim() == "lifestyleimages").Select(i => i.Id).FirstOrDefault();
                        var OtherMediaTypeId = MediaType.Where(i => i.ProductMediaImageTypeName.ToLower().Trim() == "othermediatype").Select(i => i.Id).FirstOrDefault();


                        if (!string.IsNullOrEmpty(product.LineMediaArtImages))
                        {
                            var ExistinglineArtImages = allProductMediaImages.Where(i => i.ProductId == master.Id && i.ProductMediaImageTypeId == lineArtTypeId).ToList();
                            if (ExistinglineArtImages.Count > 0)
                            {
                                MediaImagesListToBeDeleted.AddRange(ExistinglineArtImages);

                            }
                        }

                        if (!string.IsNullOrEmpty(product.LifeStyleImages))
                        {
                            var ExistingLifeStyleImages = allProductMediaImages.Where(i => i.ProductId == master.Id && i.ProductMediaImageTypeId == lifeStyleTypeId).ToList();
                            if (ExistingLifeStyleImages.Count > 0)
                            {
                                MediaImagesListToBeDeleted.AddRange(ExistingLifeStyleImages);

                            }
                        }

                        if (!string.IsNullOrEmpty(product.OtherMediaImages))
                        {
                            var ExistingOtherMediaImages = allProductMediaImages.Where(i => i.ProductId == master.Id && i.ProductMediaImageTypeId == OtherMediaTypeId).ToList();
                            if (ExistingOtherMediaImages.Count > 0)
                            {
                                MediaImagesListToBeDeleted.AddRange(ExistingOtherMediaImages);

                            }
                        }
                        #endregion

                        if (!string.IsNullOrEmpty(product.LineMediaArtImages))
                        {
                            string[] ProductImages = product.LineMediaArtImages.Split(',');
                            ProductMediaImages imgLineArts = new ProductMediaImages();
                            foreach (var image in ProductImages)
                            {
                                if (!string.IsNullOrEmpty(image))
                                {
                                    string ext = Path.GetExtension(image);
                                    string type = "image/" + ext;
                                    imgLineArts = new ProductMediaImages();
                                    Guid ImageName = Guid.NewGuid();
                                    imgLineArts.TenantId = TenantId;
                                    imgLineArts.ImageUrl = image;
                                    imgLineArts.Ext = ext;
                                    imgLineArts.Type = type;
                                    imgLineArts.Url = image;
                                    imgLineArts.Name = ImageName + "." + ext;
                                    imgLineArts.ProductMediaImageTypeId = lineArtTypeId;
                                    imgLineArts.ImageName = ImageName + "." + ext;
                                    imgLineArts.ProductId = master.Id;
                                    MediaImages.Add(imgLineArts);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(product.LifeStyleImages))
                        {

                            string[] ProductImages = product.LifeStyleImages.Split(',');
                            ProductMediaImages lifeStyleImages = new ProductMediaImages();
                            foreach (var image in ProductImages)
                            {
                                if (!string.IsNullOrEmpty(image))
                                {
                                    string ext = Path.GetExtension(image);
                                    string type = "image/" + ext;
                                    lifeStyleImages = new ProductMediaImages();
                                    Guid ImageName = Guid.NewGuid();
                                    lifeStyleImages.TenantId = TenantId;
                                    lifeStyleImages.ImageUrl = image;
                                    lifeStyleImages.Ext = ext;
                                    lifeStyleImages.Type = type;
                                    lifeStyleImages.Name = ImageName + "." + ext;
                                    lifeStyleImages.Url = image;
                                    lifeStyleImages.ProductMediaImageTypeId = lifeStyleTypeId;
                                    lifeStyleImages.ImageName = ImageName + "." + ext;
                                    lifeStyleImages.ProductId = master.Id;
                                    MediaImages.Add(lifeStyleImages);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(product.OtherMediaImages))
                        {
                            string[] ProductImages = product.OtherMediaImages.Split(',');
                            ProductMediaImages otherMediaImages = new ProductMediaImages();
                            foreach (var image in ProductImages)
                            {
                                if (!string.IsNullOrEmpty(image))
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
                                    otherMediaImages.ProductMediaImageTypeId = OtherMediaTypeId;
                                    otherMediaImages.ImageName = ImageName + "." + ext;
                                    otherMediaImages.ProductId = master.Id;
                                    MediaImages.Add(otherMediaImages);
                                }
                            }
                        }
                        if (MediaImages.Count > 0)
                        {
                            MediaImagesListToBeInserted.AddRange(MediaImages);
                        }

                        #endregion

                        #region Product variant combinations

                        var PriceVariantsToBeDeleted = allProductPriceVariants.Where(i => i.ProductId == master.Id).ToList();
                        if (PriceVariantsToBeDeleted.Count > 0)
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
                                             ProductId = master.Id
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
                        long? BrandingMeasureId = null;
                        if (!string.IsNullOrEmpty(product.BrandingUOM))
                        {
                            BrandingMeasureId = SizeMasterData.Where(i => i.ProductSizeName.ToLower() == product.BrandingUOM.ToLower()).Select(i => i.Id).FirstOrDefault();

                        }

                        ProductBrandingLocData = (from item in BrandingPositionData
                                                  select new ProductBrandingPosition
                                                  {
                                                      ProductId = master.Id,
                                                      LayerTitle = item.Branding_Location_Title_,
                                                      PostionMaxHeight = !string.IsNullOrEmpty(item.Position_Max_Height_) ? Convert.ToDouble(item.Position_Max_Height_) : 0,
                                                      PostionMaxwidth = !string.IsNullOrEmpty(item.Position_Max_Width_) ? Convert.ToDouble(item.Position_Max_Width_) : 0,
                                                      ImageFileURL = item.Branding_Location_Image_,
                                                      ImageName = Guid.NewGuid().ToString(),
                                                      UnitOfMeasureId = BrandingMeasureId.HasValue ? BrandingMeasureId: null

                                                  }).ToList();
                        if (ProductBrandingLocData.Count > 0)
                        {
                            ProductBrandingPositionFinalList.AddRange(ProductBrandingLocData);
                        }
                        #endregion

                        #region Branding method assignments


                      
                        if (product.BrandingMethodsseparatedbyacommarelevanttothisproduct != null)
                        {
                            var BrandingMethodsToBeDeleted = allProductBrandingMethods.Where(i => i.ProductMasterId == master.Id).ToList();
                            if (BrandingMethodsToBeDeleted.Count > 0)
                            {
                                ProductBrandingMethodsToBeDeleted.AddRange(BrandingMethodsToBeDeleted);
                            }

                            string[] MethodIds = product.BrandingMethodsseparatedbyacommarelevanttothisproduct.Trim().Split(',');

                            foreach (var brandingmethodid in MethodIds)
                            {
                                if (brandingmethodid != "" && brandingmethodid != "#N/A")
                                {
                                    long Id = _brandingMethodMasterRepository.GetAllList(i => i.UniqueNumber == Convert.ToInt64(brandingmethodid)).Select(i => i.Id).FirstOrDefault();
                                    if (Id > 0)
                                    {
                                        ProductBrandingMethods Method = new ProductBrandingMethods();
                                        Method.ProductMasterId = master.Id;
                                        Method.BrandingMethodId = Id;
                                        Method.IsActive = true;
                                        ProductBrandingMethodsFinalList.Add(Method);
                                    }
                                }
                            }
                        }
                        #endregion

                    }
                    #endregion

                    if (productDataToUpdate.Count > 0)
                    {

                        //if (CategoryGroupsList.Count > 0)
                        //{
                        //    await CreateCategoryGroups(CategoryGroupsList.GroupBy(x => (x.CategoryGroupId, x.CategoryMasterId)).Select(i => i.First()).ToList(), TenantId);
                        //}


                        if (CategoryGroupsList.Count > 0)
                        {
                            await CreateCategoryGroups(CategoryGroupsList.GroupBy(x => (x.CategoryGroupId, x.CategoryMasterId)).Select(i => i.First()).ToList(), TenantId);
                        }

                        if (CategorySubCategoriesList.Count > 0)
                        {
                            await CreateCategorySubCategories(CategorySubCategoriesList.GroupBy(x => (x.CategoryId, x.SubCategoryId)).Select(i => i.First()).ToList(), TenantId);

                        }
                        if (CategoryMasters.Count > 0)
                        {
                            await CreateProductAssignedCategoryMaster(CategoryMasters.GroupBy(x => (x.CategoryId, x.ProductId)).Select(i => i.First()).ToList(), TenantId);

                        }
                        if (AssignedCategories.Count > 0)
                        {
                            await CreateProductAssignedCategoriesAsync(AssignedCategories.GroupBy(x => (x.CategoryId, x.SubCategoryId, x.ProductId)).Select(i => i.First()).ToList(), TenantId);

                        }

                        if (AssignedSubSubCategories.Count > 0)
                        {
                            await CreateProductAssignedSubSubCategories(AssignedSubSubCategories.GroupBy(x => (x.SubCategoryId, x.SubSubCategoryId, x.ProductId)).Select(i => i.First()).ToList(), TenantId);

                        }
                        if (collectionCategoryList.Count > 0)
                        {
                            await CreateCategoryCollection(collectionCategoryList.GroupBy(x => (x.CategoryId, x.CollectionId)).Select(i => i.First()).ToList(), TenantId);
                        }

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
                        //if (AssignedCategoriesToBeDeleted.Count > 0)
                        //{
                        //    await DeleteProductAssignedCategoriesAsync(AssignedCategoriesToBeDeleted, TenantId);
                        //}
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
                        if (AssignedCategoryToBeDeleted.Count > 0)
                        {
                            await DeleteAssignedCategoryMasters(AssignedCategoryToBeDeleted, TenantId);
                        }
                        if (AssignedSubCategoryToBeDeleted.Count > 0)
                        {
                            await DeleteAssignedSubCategories(AssignedSubCategoryToBeDeleted, TenantId);
                        }
                        if (AssignedSubSubCategoryToBeDeleted.Count > 0)
                        {
                            await DeleteAssignedSubSubCategories(AssignedSubSubCategoryToBeDeleted, TenantId);
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
                        //if (ProductDetailsListToBeInserted.Count > 0)
                        //{
                        //    await CreateProductDetailsAsync(ProductDetailsListToBeInserted, TenantId);
                        //}
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


        #region crud operations for DB 



        [UnitOfWork(IsolationLevel.ReadUncommitted)]
        private async Task CreateVariantsDataValuesAsync(List<ProductVariantOptionValues> productsVariant, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productVariantOptionValuesRepository.BulkInsertAsync(productsVariant);
                    //await _unitOfWorkManager.Current.SaveChangesAsync();
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
                    await _unitOfWorkManager.Current.SaveChangesAsync();
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
        //CreateSubCategoryMaster

        private async Task CreateSubCategoryMaster(List<SubCategoryMaster> SubCategoryMaster, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _SubCategoryMasterRepository.BulkInsertAsync(SubCategoryMaster);
                    await _unitOfWorkManager.Current.SaveChangesAsync();

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

        private async Task CreateCategorySubCategories(List<CategorySubCategories> categorySubData, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _categorySubCategoriesRepository.BulkInsertAsync(categorySubData);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private async Task CreateProductAssignedCategoryMaster(List<ProductAssignedCategoryMaster> categoryData, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _ProductAssignedCategoryMasterRepository.BulkInsertAsync(categoryData);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }


        private async Task CreateProductAssignedSubSubCategories(List<ProductAssignedSubSubCategories> categoryData, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _ProductAssignedSubSubCategories.BulkInsertAsync(categoryData);
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
                    await _unitOfWorkManager.Current.SaveChangesAsync();
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
                    await _unitOfWorkManager.Current.SaveChangesAsync();
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

        #region Product Details CRUD
        private async Task CreateProductDetailsAsync(List<ProductDetails> productsDetails, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    await _productDetailsRepository.BulkInsertAsync(productsDetails);
                    unitOfWork.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private async Task UpdateProductDetailsAsync(List<ProductDetails> productsDetails, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productDetailsRepository.BulkUpdate(productsDetails);
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
                    await _unitOfWorkManager.Current.SaveChangesAsync();
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
        
        private async Task DeleteAssignedCategoryMasters(List<ProductAssignedCategoryMaster> productAssignedCategories, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _ProductAssignedCategoryMasterRepository.BulkDelete(productAssignedCategories);
                    unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {

            }

        }

        private async Task DeleteAssignedSubCategories(List<ProductAssignedSubCategories> productAssignedCategories, int TenantId)
        {
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

        private async Task DeleteAssignedSubSubCategories(List<ProductAssignedSubSubCategories> productAssignedCategories, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _ProductAssignedSubSubCategories.BulkDelete(productAssignedCategories);
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

        private async Task DeleteVariantsWareHouses(List<ProductVariantWarehouse> ProductVariantWarehouse, int TenantId)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.Suppress))
                {
                    _productVariantWarehouseRepository.BulkDelete(ProductVariantWarehouse);
                    unitOfWork.Complete();

                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        #endregion

        public async Task DeleteOptionValuesList(List<long> variantList, int TenantId)
        {

            try
            {
                using (var connection = new SqlConnection(DBConnection))
                {
                    var data = variantList;
                    string fData = String.Join(",", data);
                    connection.Open();
                    SqlDataReader sqlDr = null;
                    SqlCommand cmd = new SqlCommand("usp_GetOptionToBeDeletedById", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter tvparam = cmd.Parameters.AddWithValue("@List", fData);
                    cmd.Parameters.AddWithValue("@TenantId", TenantId);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
