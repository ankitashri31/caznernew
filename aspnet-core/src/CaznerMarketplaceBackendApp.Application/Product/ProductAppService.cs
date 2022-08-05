using Abp.Domain.Repositories;
using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.IO;
using Abp.Domain.Uow;
using CaznerMarketplaceBackendApp.Authorization.Users;
using Abp.Authorization.Users;
using CaznerMarketplaceBackendApp.Authorization.Roles;
using CaznerMarketplaceBackendApp.Tag.Dto;
using CaznerMarketplaceBackendApp.ProductBrand.Dto;
using CaznerMarketplaceBackendApp.ProductMaterial.Dto;
using CaznerMarketplaceBackendApp.ProductType.Dto;
using Microsoft.Extensions.Configuration;
using CaznerMarketplaceBackendApp.BrandingMethod.Dto;
using CaznerMarketplaceBackendApp.Product.Masters;
using Abp.Collections.Extensions;
using Abp.Authorization;
using CaznerMarketplaceBackendApp.WareHouse;
using CaznerMarketplaceBackendApp.WareHouse.Dto;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using CaznerAngularCoreApiDemo.Product.Dto;
using FimApp.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CaznerMarketplaceBackendApp.MultiTenancy;
using CaznerMarketplaceBackendApp.AzureBlobStorage;
using CaznerMarketplaceBackendApp.ProductSize.Dto;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using CaznerMarketplaceBackendApp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Abp.Data;
using CaznerMarketplaceBackendApp.Connections;
using Dapper;
using System.Text;
using CaznerMarketplaceBackendApp.BrandingMethods;
using CaznerMarketplaceBackendApp.SubCategory;
using CaznerMarketplaceBackendApp.SubCategory.Dto;
using FastMember;
using static CaznerMarketplaceBackendApp.AppConsts;

namespace CaznerMarketplaceBackendApp.Product
{
    [AbpAuthorize]
    public class ProductAppService : CaznerMarketplaceBackendAppAppServiceBase, IProductAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private IConfiguration _configuration;
        private readonly IRepository<ProductMaster, long> _repository;
        private readonly IRepository<ProductBrandingPosition, long> _productBrandingPositionRepository;
        private readonly IRepository<ProductBulkUploadVariations, long> _productBulkUploadVariationsRepository;
        private readonly IRepository<ProductStockLocation, long> _productStockLocationRepository;
        private readonly IRepository<ProductImages, long> _productImagesRepository;
        private readonly IRepository<CategoryGroupMaster, long> _categoryGroupRepository;
        private readonly IRepository<ProductMediaImages, long> _productMediaImagesRepository;
        private readonly IRepository<ProductVolumeDiscountVariant, long> _productVolumeDiscountVariantRepository;
        private readonly IRepository<ProductDimensionsInventory, long> _productDimensionsInventoryRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IRepository<ProductVariantsData, long> _productVariantDataRepository;
        private readonly IRepository<ProductVariantWarehouse, long> _productVariantWarehouseRepository;
        private readonly IRepository<ProductVariantOptionValues, long> _productVariantOptionValuesRepository;
        private readonly IRepository<ProductVariantdataImages, long> _productVariantdataImagesRepository;
        private readonly IRepository<BrandingMethodMaster, long> _repositoryBrandingMethod;
        private readonly IRepository<ProductOptionsMaster, long> _productOptionRepository;
        private readonly IRepository<ProductMethodAttributeValues, long> _productMethodAttributeValuesRepository;
        private readonly IRepository<WareHouseMaster, long> _wareHouseMasterRepository;
        private readonly IRepository<CategoryMaster, long> _categoryMasterRepository;
        private readonly IRepository<ProductBrandingPriceVariants, long> _productBrandingPriceVariantsRepository;
        private readonly IRepository<CategoryCollections, long> _categoryCollectionRepository;
        private readonly IRepository<CollectionMaster, long> _collectionMasterRepository;
        private readonly IRepository<CategoryGroups, long> _categoryGroupsRepository;
        private readonly IRepository<ProductBrandingMethods, long> _productBrandingMethodsRepository;
        private readonly IRepository<ProductAssignedBrands, long> _productAssignedBrandsRepository;
        private readonly IRepository<ProductAssignedCollections, long> _productAssignedCollectionsRepository;
        private readonly IRepository<ProductAssignedMaterials, long> _productAssignedMaterialsRepository;
        private readonly IRepository<ProductAssignedTags, long> _productAssignedTagsRepository;
        private readonly IRepository<ProductAssignedTypes, long> _productAssignedTypesRepository;
        private readonly IRepository<ProductAssignedVendors, long> _productAssignedVendorsRepository;
        private readonly IRepository<ProductAssignedSubCategories, long> _productAssignedSubCategoriesRepository;
        private readonly IRepository<ProductVariantQuantityPrices, long> _productVariantQuantityPricesRepository;
        private readonly IRepository<CompartmentVariantData, long> _productCompartmentDataRepository;
        private readonly IRepository<CompartmentOptionValues, long> _productCompartmentOptionValuesRepository;
        private readonly IRepository<ProductCompartmentBaseImages, long> _productBaseImageRepository;
        private readonly IRepository<ProductColourMaster, long> _productColorRepository;
        private readonly IRepository<BrandingMethodAssignedColors, long> _BrandingMethodAssignedColorsRepository;
        private readonly IRepository<ProductAssignedSubSubCategories, long> _repositoryAssignedSubSubCategories;
        private readonly IRepository<CategorySubCategories, long> _productCategorySubCategoryRepository;
        private readonly IRepository<SubCategoryMaster, long> _productSubCategoryRepository;
        private readonly IRepository<ProductAssignedCategoryMaster, long> _repositoryAssignedCategoryMaster;
        private readonly IRepository<ProductMediaImageTypeMaster, long> _repositoryProductMediaImageTypeMaster;
        string FolderName = string.Empty;
        string LogoFolderName = string.Empty;
        string CurrentWebsiteUrl = string.Empty;
        string AzureProductFolder = string.Empty;
        string AzureStorageUrl = string.Empty;
        private readonly IRepository<Tenant, int> _tenantManager;
        private IAzureBlobStorageService _azureBlobStorageService;
        private CaznerMarketplaceBackendAppDbContext _dbContext;
        private readonly IActiveTransactionProvider _transactionProvider;
        private readonly DbConnectionUtility _connectionUtility;
        private IDbConnection _db;
        public ProductAppService
            (IUnitOfWorkManager unitOfWorkManager, IConfiguration configuration,
            IRepository<ProductMaster, long> repository,
            IRepository<ProductBrandingPosition, long> productBrandingPositionRepository,
            IRepository<ProductBulkUploadVariations, long> productBulkUploadVariationsRepository,
            IRepository<ProductStockLocation, long> productStockLocationRepository,
            IRepository<ProductImages, long> productImagesRepository,
            IRepository<ProductMediaImages, long> productMediaImagesRepository,
            IRepository<ProductVolumeDiscountVariant, long> productVolumeDiscountVariantRepository,
            IRepository<ProductDimensionsInventory, long> productDimensionsInventoryRepository,
            IRepository<User, long> userRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<CategoryGroupMaster, long> categoryGroupRepository,
            IRepository<Role, int> roleRepository,
            IRepository<SubCategoryMaster, long> productSubCategoryRepository,
        IRepository<CategorySubCategories, long> productCategorySubCategoryRepository,
            IRepository<CompartmentOptionValues, long> compartmentOptionalRepository,
              IRepository<WareHouseMaster, long> wareHouseMasterRepository, IRepository<CategoryMaster, long> categoryMasterRepository,
            IRepository<ProductOptionsMaster, long> productOptionRepository, IRepository<ProductMethodAttributeValues, long> productMethodAttributeValuesRepository,
             IRepository<ProductVariantsData, long> productVariantDataRepository, IRepository<ProductVariantWarehouse, long> productVariantWarehouseRepository,
             IRepository<ProductVariantOptionValues, long> productVariantOptionValuesRepository, IRepository<ProductVariantdataImages, long> productVariantdataImagesRepository,
             IRepository<ProductBrandingPriceVariants, long> productBrandingPriceVariantsRepository,
             IRepository<CategoryCollections, long> categoryCollectionRepository, IRepository<CollectionMaster, long> collectionMasterRepository, IRepository<BrandingMethodMaster, long> repositoryBrandingMethod,
             IRepository<CategoryGroups, long> categoryGroupsRepository, IRepository<Tenant, int> tenantManager, IAzureBlobStorageService azureBlobStorageService,
             IRepository<ProductBrandingMethods, long> productBrandingMethodsRepository, IRepository<ProductAssignedBrands, long> productAssignedBrandsRepository, IRepository<ProductAssignedCollections, long> productAssignedCollectionsRepository,
             IRepository<ProductAssignedMaterials, long> productAssignedMaterialsRepository, IRepository<ProductAssignedTags, long> productAssignedTagsRepository, IRepository<ProductAssignedTypes, long> productAssignedTypesRepository,
             IRepository<ProductAssignedVendors, long> productAssignedVendorsRepository,
             IRepository<CompartmentVariantData, long> productCompartmentDataRepository,
             IRepository<CompartmentOptionValues, long> productCompartmentOptionValuesRepository,
             IRepository<ProductCompartmentBaseImages, long> productBaseImageRepository,
             IRepository<ProductAssignedCategoryMaster, long> repositoryAssignedCategoryMaster,
        IRepository<ProductAssignedSubCategories, long> productAssignedCategoriesRepository, IRepository<ProductVariantQuantityPrices, long> productVariantQuantityPricesRepository, CaznerMarketplaceBackendAppDbContext dbContext,
             IActiveTransactionProvider transactionProvider, IRepository<ProductAssignedSubSubCategories, long> productAssignedSubSubCategoriesRepository,

             DbConnectionUtility connectionUtility, IRepository<ProductColourMaster, long> productColorRepository, IRepository<BrandingMethodAssignedColors, long> BrandingMethodAssignedColorsRepository, IRepository<ProductMediaImageTypeMaster, long> repositoryProductMediaImageTypeMaster
            )
        {
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = configuration;
            _repository = repository;
            _productBrandingPositionRepository = productBrandingPositionRepository;
            _productBulkUploadVariationsRepository = productBulkUploadVariationsRepository;
            _productImagesRepository = productImagesRepository;
            _productMediaImagesRepository = productMediaImagesRepository;
            _productVolumeDiscountVariantRepository = productVolumeDiscountVariantRepository;
            _productDimensionsInventoryRepository = productDimensionsInventoryRepository;
            _productStockLocationRepository = productStockLocationRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _productOptionRepository = productOptionRepository;
            _productMethodAttributeValuesRepository = productMethodAttributeValuesRepository;
            _productVariantDataRepository = productVariantDataRepository;
            _productVariantdataImagesRepository = productVariantdataImagesRepository;
            _productVariantWarehouseRepository = productVariantWarehouseRepository;
            _productVariantOptionValuesRepository = productVariantOptionValuesRepository;
            _wareHouseMasterRepository = wareHouseMasterRepository;
            _categoryMasterRepository = categoryMasterRepository;
            _categoryGroupRepository = categoryGroupRepository;
            _repositoryBrandingMethod = repositoryBrandingMethod;
            _productBrandingPriceVariantsRepository = productBrandingPriceVariantsRepository;
            _categoryCollectionRepository = categoryCollectionRepository;
            LocalizationSourceName = CaznerMarketplaceBackendAppConsts.LocalizationSourceName;
            FolderName = _configuration["FileUpload:FolderName"];
            LogoFolderName = _configuration["FileUpload:FrontEndLogoFolderName"];
            CurrentWebsiteUrl = _configuration["FileUpload:WebsireDomainPath"];
            _collectionMasterRepository = collectionMasterRepository;
            _categoryGroupsRepository = categoryGroupsRepository;
            _tenantManager = tenantManager;
            _azureBlobStorageService = azureBlobStorageService;
            AzureProductFolder = _configuration["FileUpload:AzureProductFolder"];
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];

            _productBrandingMethodsRepository = productBrandingMethodsRepository;
            _productAssignedBrandsRepository = productAssignedBrandsRepository;
            _productAssignedCollectionsRepository = productAssignedCollectionsRepository;
            _productAssignedMaterialsRepository = productAssignedMaterialsRepository;
            _productAssignedTagsRepository = productAssignedTagsRepository;
            _productAssignedTypesRepository = productAssignedTypesRepository;
            _productAssignedVendorsRepository = productAssignedVendorsRepository;
            _productAssignedSubCategoriesRepository = productAssignedCategoriesRepository;
            _productVariantQuantityPricesRepository = productVariantQuantityPricesRepository;
            _productCompartmentDataRepository = productCompartmentDataRepository;
            _productCompartmentOptionValuesRepository = productCompartmentOptionValuesRepository;
            _productBaseImageRepository = productBaseImageRepository;
            _transactionProvider = transactionProvider;
            _dbContext = dbContext;
            _connectionUtility = connectionUtility;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);
            _productColorRepository = productColorRepository;
            _BrandingMethodAssignedColorsRepository = BrandingMethodAssignedColorsRepository;
            _repositoryAssignedSubSubCategories = productAssignedSubSubCategoriesRepository;
            _productCategorySubCategoryRepository = productCategorySubCategoryRepository;
            _productSubCategoryRepository = productSubCategoryRepository;
            _repositoryAssignedCategoryMaster = repositoryAssignedCategoryMaster;
            _repositoryProductMediaImageTypeMaster = repositoryProductMediaImageTypeMaster;
        }
        public async Task<ProductMasterDto> CreateProduct(CreateProductDto createProductdto)
        {
            string FolderName = _configuration["FileUpload:FolderName"];
            string CurrentWebsiteUrl = _configuration.GetValue<string>("FileUpload:WebsireDomainPath");
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            ProductMasterDto output = new ProductMasterDto();
            long resultUniqueId = 0;
            try
            {
                var productMaster = ObjectMapper.Map<ProductMaster>(createProductdto);
                productMaster.TenantId = Convert.ToInt32(AbpSession.TenantId);

                var IsTitleExists = _repository.GetAllList(i => i.ProductTitle.ToLower().Trim() == createProductdto.ProductTitle.ToLower().Trim()).FirstOrDefault();
                if (IsTitleExists != null)
                {
                    output.ErrorMessage = "Product title already exists, please enter unique title!";
                    output.IsValidate = true;
                    return output;
                }

                var IsSKUExists = _repository.GetAllList(i => i.ProductSKU == createProductdto.ProductSKU).FirstOrDefault();
                if (IsSKUExists != null)
                {
                    output.ErrorMessage = "Product SKU already exists, please enter unique SKU!";
                    output.IsValidate = true;
                    return output;
                }

                if (createProductdto.IsProductCompartmentType)
                {
                    var IsCompartmentTitleExists = _repository.GetAllList(i => i.CompartmentBuilderTitle.ToLower().Trim() == createProductdto.CompartmentBuilderTitle.ToLower().Trim()).FirstOrDefault();

                    if (IsCompartmentTitleExists != null)
                    {
                        output.ErrorMessage = "Compartment builder title already exists, please enter unique title!";
                        output.IsValidate = true;
                        return output;
                    }
                }

                if (createProductdto.ProductPrice != null)
                {
                    productMaster.Profit = createProductdto.ProductPrice.Profit == "" ? 0 : Convert.ToDecimal(createProductdto.ProductPrice.Profit);
                    productMaster.UnitPrice = createProductdto.ProductPrice.UnitPrice == "" ? 0 : Convert.ToDecimal(createProductdto.ProductPrice.UnitPrice);
                    productMaster.CostPerItem = createProductdto.ProductPrice.CostPerItem == "" ? 0 : Convert.ToDecimal(createProductdto.ProductPrice.CostPerItem);
                    productMaster.MarginIncreaseOnSalePrice = createProductdto.ProductPrice.MarginIncreaseOnSalePrice == "" ? 0 : Convert.ToDecimal(createProductdto.ProductPrice.MarginIncreaseOnSalePrice);
                    productMaster.SalePrice = createProductdto.ProductPrice.SalePrice == "" ? 0 : Convert.ToDecimal(createProductdto.ProductPrice.SalePrice);
                    productMaster.OnSale = createProductdto.ProductPrice.OnSale;
                    productMaster.UnitOfMeasure = createProductdto.ProductPrice.UnitOfMeasure == "" ? 0 : Convert.ToInt32(createProductdto.ProductPrice.UnitOfMeasure);
                    productMaster.MinimumOrderQuantity = createProductdto.ProductPrice.MinimumOrderQuantity == "" ? 0 : Convert.ToInt32(createProductdto.ProductPrice.MinimumOrderQuantity);
                    productMaster.DepositRequired = createProductdto.ProductPrice.DepositRequired == "" ? 0 : Convert.ToDecimal(createProductdto.ProductPrice.DepositRequired);
                    productMaster.ChargeTaxOnThis = createProductdto.ProductPrice.ChargeTaxOnThis;
                    productMaster.ProductHasPriceVariant = createProductdto.ProductPrice.ProductHasPriceVariant;
                    productMaster.IsProductHasMultipleOptions = createProductdto.IsProductHasMultipleOptions;
                    productMaster.DiscountPercentage = createProductdto.ProductPrice.DiscountPercentage == "" ? 0 : Convert.ToDouble(createProductdto.ProductPrice.DiscountPercentage);
                    productMaster.IsActive = true;
                    productMaster.IsIndentOrder = createProductdto.IsIndentOrder;
                    productMaster.Features = createProductdto.Features;
                    productMaster.DiscountPercentageDraft = createProductdto.ProductPrice.DiscountPercentageDraft == "" ? 0 : Convert.ToDouble(createProductdto.ProductPrice.DiscountPercentageDraft);
                    productMaster.IsProductHasCompartmentBuilder = createProductdto.IsProductHasCompartmentBuilder;
                    productMaster.IsProductIsCompartmentType = createProductdto.IsProductCompartmentType;
                    productMaster.CompartmentBuilderTitle = createProductdto.CompartmentBuilderTitle; ;
                    if (createProductdto.IsHasSubProducts == true)
                    {
                        productMaster.IsHasSubProducts = true;
                        productMaster.NumberOfSubProducts = createProductdto.NumberOfSubProducts;
                    }
                    else
                    {
                        productMaster.IsHasSubProducts = false;
                        productMaster.NumberOfSubProducts = 0;
                    }
                }
                resultUniqueId = await _repository.InsertAndGetIdAsync(productMaster);
                if (resultUniqueId != 0)
                {
                    #region ProductImages
                    //for DefaultImage save
                    if (createProductdto.ProductDefaultImage != null)
                    {
                        ProductImages Model = new ProductImages();
                        string ImageLocation = AzureStorageUrl + folderPath + createProductdto.ProductDefaultImage.FileName;
                        Model.ImageName = createProductdto.ProductDefaultImage.FileName;
                        Model.ProductId = resultUniqueId;
                        Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        Model.ImagePath = ImageLocation;
                        Model.Url = ImageLocation;
                        Model.IsProductSubmissionDone = true;
                        Model.IsDefaultImage = true;
                        Model.Ext = createProductdto.ProductDefaultImage.Ext;
                        Model.Size = createProductdto.ProductDefaultImage.Size;
                        Model.Type = createProductdto.ProductDefaultImage.Type;
                        Model.Name = createProductdto.ProductDefaultImage.Name;
                        long Id = await _productImagesRepository.InsertAndGetIdAsync(Model);
                    }
                    if (createProductdto.ProductImagesNames != null)
                    {
                        foreach (var imagename in createProductdto.ProductImagesNames)
                        {
                            if (imagename.FileName != null && imagename.FileName != "")
                            {
                                ProductImages Model = new ProductImages();
                                string ImageLocation = AzureStorageUrl + folderPath + imagename.FileName;
                                Model.ImageName = imagename.FileName;
                                Model.ProductId = resultUniqueId;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                Model.ImagePath = ImageLocation;
                                Model.Url = ImageLocation;
                                Model.IsProductSubmissionDone = true;
                                Model.Ext = imagename.Ext;
                                Model.Size = imagename.Size;
                                Model.Type = imagename.Type;
                                Model.Name = imagename.Name;
                                long Id = await _productImagesRepository.InsertAndGetIdAsync(Model);
                            }
                        }
                    }
                    #endregion

                    #region ProductVolumeVariations
                    if (createProductdto.ProductVolumeDiscountVariant != null && productMaster.ProductHasPriceVariant == true)
                    {
                        List<ProductVolumeDiscountVariant> productVolumeDiscountVariantList = ObjectMapper.Map<List<ProductVolumeDiscountVariant>>(createProductdto.ProductVolumeDiscountVariant.ToList());
                        foreach (var volumeDiscountVariantitem in productVolumeDiscountVariantList)
                        {
                            volumeDiscountVariantitem.ProductId = resultUniqueId;
                            volumeDiscountVariantitem.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            long ProductVolumeDiscountVariantId = await _productVolumeDiscountVariantRepository.InsertAndGetIdAsync(volumeDiscountVariantitem);
                        }
                    }

                    #endregion

                    #region ProductDetails

                    #region separate product details insertion

                    #region BrandArray
                    List<ProductAssignedBrands> ProductBrandData = new List<ProductAssignedBrands>();
                    foreach (var brand in createProductdto.ProductDetail.ProductBrandArray)
                    {
                        ProductAssignedBrands BrandEntity = new ProductAssignedBrands();
                        BrandEntity.ProductId = resultUniqueId;
                        BrandEntity.ProductBrandId = brand;
                        BrandEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        BrandEntity.IsActive = true;
                        ProductBrandData.Add(BrandEntity);
                    }
                    if (ProductBrandData.Count > 0)
                    {
                        await _productAssignedBrandsRepository.BulkInsertAsync(ProductBrandData);
                    }

                    #endregion
                    #region CollectionArray
                    List<ProductAssignedCollections> ProductCollectionData = new List<ProductAssignedCollections>();
                    foreach (var collection in createProductdto.ProductDetail.ProductCollectionArray)
                    {
                        ProductAssignedCollections CollectionEntity = new ProductAssignedCollections();
                        CollectionEntity.ProductId = resultUniqueId;
                        CollectionEntity.CollectionId = collection;
                        CollectionEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        CollectionEntity.IsActive = true;
                        ProductCollectionData.Add(CollectionEntity);
                    }
                    if (ProductCollectionData.Count > 0)
                    {
                        await _productAssignedCollectionsRepository.BulkInsertAsync(ProductCollectionData);
                    }
                    #endregion

                    #region MaterialArray
                    List<ProductAssignedMaterials> ProductMaterialData = new List<ProductAssignedMaterials>();
                    foreach (var materials in createProductdto.ProductDetail.ProductMaterialArray)
                    {
                        ProductAssignedMaterials MaterialEntity = new ProductAssignedMaterials();
                        MaterialEntity.ProductId = resultUniqueId;
                        MaterialEntity.MaterialId = materials;
                        MaterialEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        MaterialEntity.IsActive = true;
                        ProductMaterialData.Add(MaterialEntity);
                    }
                    if (ProductMaterialData.Count > 0)
                    {
                        await _productAssignedMaterialsRepository.BulkInsertAsync(ProductMaterialData);
                    }

                    #endregion
                    #region TagsArray
                    List<ProductAssignedTags> ProductTagsData = new List<ProductAssignedTags>();
                    foreach (var tags in createProductdto.ProductDetail.ProductTagArray)
                    {
                        ProductAssignedTags TagsEntity = new ProductAssignedTags();
                        TagsEntity.ProductId = resultUniqueId;
                        TagsEntity.TagId = tags;
                        TagsEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        TagsEntity.IsActive = true;
                        ProductTagsData.Add(TagsEntity);
                    }
                    if (ProductTagsData.Count > 0)
                    {
                        await _productAssignedTagsRepository.BulkInsertAsync(ProductTagsData);
                    }

                    #endregion


                    #endregion
                    #region TypesArray
                    List<ProductAssignedTypes> ProductTypesData = new List<ProductAssignedTypes>();
                    foreach (var types in createProductdto.ProductDetail.ProductTypeArray)
                    {
                        ProductAssignedTypes TypesEntity = new ProductAssignedTypes();
                        TypesEntity.ProductId = resultUniqueId;
                        TypesEntity.TypeId = types;
                        TypesEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        TypesEntity.IsActive = true;
                        ProductTypesData.Add(TypesEntity);
                    }
                    if (ProductTypesData.Count > 0)
                    {
                        await _productAssignedTypesRepository.BulkInsertAsync(ProductTypesData);
                    }

                    #endregion
                    #region VendorsArray
                    List<ProductAssignedVendors> ProductVendorsData = new List<ProductAssignedVendors>();
                    foreach (var vendors in createProductdto.ProductDetail.ProductVendorArray)
                    {
                        ProductAssignedVendors VendorsEntity = new ProductAssignedVendors();
                        VendorsEntity.ProductId = resultUniqueId;
                        VendorsEntity.VendorUserId = vendors;
                        VendorsEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        VendorsEntity.IsActive = true;
                        ProductVendorsData.Add(VendorsEntity);
                    }
                    if (ProductVendorsData.Count > 0)
                    {
                        await _productAssignedVendorsRepository.BulkInsertAsync(ProductVendorsData);
                    }

                    #endregion
                    #region CategoriesArray
                    List<ProductAssignedCategoryMaster> ProductCategoriesData = new List<ProductAssignedCategoryMaster>();
                    List<ProductAssignedSubCategories> ProductSubCategoriesData = new List<ProductAssignedSubCategories>();
                    List<ProductAssignedSubSubCategories> ProductSubSubCategoriesData = new List<ProductAssignedSubSubCategories>();
                    foreach (var categories in createProductdto.ProductDetail.ProductCategories)
                    {
                        ProductAssignedCategoryMaster master = new ProductAssignedCategoryMaster();
                        master.CategoryId = categories.CategoryId;
                        master.ProductId = resultUniqueId;
                        master.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        master.IsActive = true;
                        if (categories.ProductSubCategories.Count > 0)
                        {
                            foreach (var subcategoryData in categories.ProductSubCategories)
                            {
                                ProductAssignedSubCategories subcategory = new ProductAssignedSubCategories();
                                subcategory.SubCategoryId = subcategoryData.SubCategoryId;
                                subcategory.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                subcategory.ProductId = resultUniqueId;
                                subcategory.IsActive = true;
                                subcategory.CategoryId = categories.CategoryId;

                                if (subcategoryData.ProductSubSubCategories.Count > 0)
                                {
                                    foreach (var subSubcategoryData in subcategoryData.ProductSubSubCategories)
                                    {
                                        ProductAssignedSubSubCategories subSubCAtegoryData = new ProductAssignedSubSubCategories();
                                        subSubCAtegoryData.SubSubCategoryId = subSubcategoryData.SubSubCategoryId;
                                        subSubCAtegoryData.SubCategoryId = subcategoryData.SubCategoryId;
                                        subSubCAtegoryData.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                        subSubCAtegoryData.ProductId = resultUniqueId;
                                        subSubCAtegoryData.IsActive = true;
                                        ProductSubSubCategoriesData.Add(subSubCAtegoryData);
                                    }
                                }
                                ProductSubCategoriesData.Add(subcategory);
                            }
                            ProductCategoriesData.Add(master);
                        }

                    }
                    if (ProductSubSubCategoriesData.Count > 0)
                    {

                        await _repositoryAssignedSubSubCategories.BulkInsertAsync(ProductSubSubCategoriesData);
                    }
                    if (ProductSubCategoriesData.Count > 0)
                    {
                        await _productAssignedSubCategoriesRepository.BulkInsertAsync(ProductSubCategoriesData);
                    }
                    if (ProductCategoriesData.Count > 0)
                    {
                        await _repositoryAssignedCategoryMaster.BulkInsertAsync(ProductCategoriesData);
                    }



                    #endregion

                    #endregion

                    #region ProductBrandingPosition      
                    if (createProductdto.ProductBrandingPosition != null)
                    {
                        int i = 0;
                        foreach (var ProductBrandingPositionitem in createProductdto.ProductBrandingPosition)
                        {

                            ProductBrandingPosition productBrandData = new ProductBrandingPosition();
                            i = i + 1;


                            productBrandData.ProductId = resultUniqueId;
                            productBrandData.IsProductSubmissionDone = true;
                            productBrandData.LayerNumber = i.ToString();
                            productBrandData.LayerTitle = ProductBrandingPositionitem.LayerTitle;
                            productBrandData.PostionMaxHeight = Convert.ToDouble(ProductBrandingPositionitem.PositionMaxHeight);
                            productBrandData.PostionMaxwidth = Convert.ToDouble(ProductBrandingPositionitem.PositionMaxwidth);
                            productBrandData.BrandingLocationNote = ProductBrandingPositionitem.BrandingLocationNote;

                            productBrandData.UnitOfMeasureId = createProductdto.BrandingUnitOfMeasureId;

                            if (ProductBrandingPositionitem.ImageObj != null)
                            {

                                string ImageLocation = AzureStorageUrl + folderPath + ProductBrandingPositionitem.ImageObj.FileName;
                                productBrandData.ImageName = ProductBrandingPositionitem.ImageObj.FileName;
                                productBrandData.ImageFileURL = ImageLocation;
                                productBrandData.Ext = ProductBrandingPositionitem.ImageObj.Ext;
                                productBrandData.Name = ProductBrandingPositionitem.ImageObj.FileName;
                                productBrandData.Url = ImageLocation;
                                productBrandData.Size = ProductBrandingPositionitem.ImageObj.Size;
                                productBrandData.Type = ProductBrandingPositionitem.ImageObj.Type;
                            }
                            long Id = await _productBrandingPositionRepository.InsertAndGetIdAsync(productBrandData);

                        }
                    }
                    #endregion

                    #region ProductBrandingMethods
                    if (createProductdto.BrandingMethodData != null)
                    {

                        foreach (var brandingmethod in createProductdto.BrandingMethodData)
                        {

                            ProductBrandingMethods Method = new ProductBrandingMethods();
                            Method.ProductMasterId = resultUniqueId;
                            Method.BrandingMethodId = brandingmethod.Id;
                            Method.MethodCustomizedColor = brandingmethod.SelectedColor;
                            Method.IsActive = true;
                            await _productBrandingMethodsRepository.InsertAsync(Method);
                        }
                    }
                    #endregion

                    #region ProductBulkUploadVariations
                    try
                    {
                        if (createProductdto.ProductBulkUploadVariations != null && productMaster.IsProductHasMultipleOptions == true)
                        {

                            foreach (var bulkUploadVariationsitem in createProductdto.ProductBulkUploadVariations)
                            {
                                if (bulkUploadVariationsitem.productOptionId > 0)
                                {
                                    ProductBulkUploadVariations BulkuploadModel = new ProductBulkUploadVariations();
                                    BulkuploadModel.ProductId = resultUniqueId;
                                    BulkuploadModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    //Note: This "Id" is option Id //
                                    BulkuploadModel.productOptionId = bulkUploadVariationsitem.productOptionId;
                                    if (bulkUploadVariationsitem.ProductOptionValue != null && bulkUploadVariationsitem.ProductOptionValue.Count > 0)
                                    {
                                        int i = 0;
                                        string Value = string.Empty;
                                        foreach (var optionValues in bulkUploadVariationsitem.ProductOptionValue)
                                        {
                                            if (i == 0)
                                            {
                                                Value = "'" + string.Join("','", optionValues.Value) + "'";
                                            }
                                            else
                                            {
                                                Value = Value + ",'" + string.Join("','", optionValues.Value) + "'";
                                            }
                                            i = i + 1;
                                        }
                                        if (!string.IsNullOrEmpty(Value))
                                        {
                                            BulkuploadModel.ProductOptionValue = Value;
                                        }
                                        try
                                        {
                                            long productBulkUploadVariationsId = await _productBulkUploadVariationsRepository.InsertAndGetIdAsync(BulkuploadModel);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    #region ProductVariantData

                    if (createProductdto.ProductVariantData != null)
                    {
                        ProductVariantsData Model = new ProductVariantsData();
                        foreach (var productVariantsDataitem in createProductdto.ProductVariantData)
                        {
                            Model = new ProductVariantsData();
                            Model.ProductId = resultUniqueId;
                            Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            if (productVariantsDataitem.VariantDetails != null)
                            {
                                string[] VariantIds = productVariantsDataitem.VariantDetails.Split('/');
                                StringBuilder sb = new StringBuilder();
                                int finalIndex = VariantIds.Count() - 1;
                                for (int i = 0; i < VariantIds.Length; i++)
                                {
                                    if (i == finalIndex)
                                    {

                                        sb.Append(Convert.ToInt32(VariantIds[i]) % (10));
                                    }
                                    else
                                    {
                                        sb.Append(Convert.ToInt32(VariantIds[i]) % (10) + "/");
                                    }

                                }

                                string varientDetails = sb.ToString();
                                Model.VariantMasterIds = varientDetails;
                            }
                            else
                            {
                                Model.VariantMasterIds = productVariantsDataitem.VariantDetails;


                            }

                            Model.Variant = productVariantsDataitem.Variant;
                            Model.QuantityStockUnit = productVariantsDataitem.VariantQuantity;

                            Model.Price = productVariantsDataitem.VariantPrice == "" ? 0 : Convert.ToDecimal(productVariantsDataitem.VariantPrice);
                            Model.ComparePrice = productVariantsDataitem.ComparePrice == "" ? 0 : Convert.ToDecimal(productVariantsDataitem.ComparePrice);
                            Model.CostPerItem = productVariantsDataitem.CostPerItem == "" ? 0 : Convert.ToDecimal(productVariantsDataitem.CostPerItem);
                            Model.Margin = productVariantsDataitem.Margin;
                            Model.ProfitCurrencySymbol = productVariantsDataitem.ProfitCurrencySymbol == "" ? 0 : Convert.ToInt32(productVariantsDataitem.ProfitCurrencySymbol);
                            Model.Profit = productVariantsDataitem.Profit == "" ? 0 : Convert.ToDecimal(productVariantsDataitem.Profit);
                            Model.IsChargeTaxVariant = productVariantsDataitem.IsChargeTaxVariant;
                            Model.Shape = productVariantsDataitem.Shape;

                            Model.SKU = productVariantsDataitem.VariantSKU;
                            Model.BarCode = productVariantsDataitem.VariantBarCode;
                            Model.IsTrackQuantity = productVariantsDataitem.IsTrackQuantity;
                            Model.IsActive = productVariantsDataitem.IsActive;
                            Model.NextShipment = productVariantsDataitem.NextShipment;
                            Model.IncomingQuantity = productVariantsDataitem.IncomingQuantity;
                            long productVariantsDataId = await _productVariantDataRepository.InsertAndGetIdAsync(Model);
                            Model.IsMultiColorVariant = productVariantsDataitem.IsMultiColorVariant;
                            if (productVariantsDataitem.VariantWarehouse != null)
                            {
                                ProductVariantWarehouse warehouse = new ProductVariantWarehouse();
                                warehouse.WarehouseId = productVariantsDataitem.VariantWarehouse.WarehouseId;
                                warehouse.LocationA = productVariantsDataitem.VariantWarehouse.LocationA;
                                warehouse.LocationB = productVariantsDataitem.VariantWarehouse.LocationB;
                                warehouse.LocationC = productVariantsDataitem.VariantWarehouse.LocationC;
                                warehouse.ProductVariantId = productVariantsDataId;
                                warehouse.QuantityThisLocation = string.IsNullOrEmpty(productVariantsDataitem.VariantWarehouse.QuantityThisLocation) ? 0 : Convert.ToDouble(productVariantsDataitem.VariantWarehouse.QuantityThisLocation);
                                await _productVariantWarehouseRepository.InsertAsync(warehouse);
                            }

                            if (productVariantsDataitem.VariantDetails != null)
                            {
                                string[] VariantIds = productVariantsDataitem.VariantDetails.Split('/');
                                string[] VariantValues = productVariantsDataitem.Variant.Split('/');

                                for (int i = 0; i < VariantIds.Length; i++)
                                {
                                    ProductVariantOptionValues ModelVariantOptionValues = new ProductVariantOptionValues();
                                    ModelVariantOptionValues.ProductVariantId = productVariantsDataId;
                                    ModelVariantOptionValues.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    // ModelVariantOptionValues.VariantOptionId = Convert.ToInt32(VariantIds[i]);
                                    //change requested for appending compartment builder title with variant combo
                                    ModelVariantOptionValues.VariantOptionId = Convert.ToInt32(VariantIds[i]) % (10);
                                    ModelVariantOptionValues.VariantOptionValue = VariantValues[i].ToString();
                                    long ModelVariantOptionValuesId = await _productVariantOptionValuesRepository.InsertAndGetIdAsync(ModelVariantOptionValues);
                                }
                            }

                            if (!string.IsNullOrEmpty(productVariantsDataitem.ImageName))
                            {

                                string ImageLocation = AzureStorageUrl + folderPath + productVariantsDataitem.ImageName;
                                ProductVariantdataImages DataImages = new ProductVariantdataImages();
                                DataImages.ProductVariantId = productVariantsDataId;
                                DataImages.ImageURL = ImageLocation;
                                DataImages.ImageFileName = productVariantsDataitem.ImageName;
                                DataImages.ProductId = resultUniqueId;
                                DataImages.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                try
                                {
                                    long Id = await _productVariantdataImagesRepository.InsertAndGetIdAsync(DataImages);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    #endregion
                    #region Product Compartment Builder

                    if (productMaster.IsProductHasCompartmentBuilder == true)
                    {
                        try
                        {
                            if (createProductdto.CompartmentVariantData != null)
                            {
                                foreach (var compartemntData in createProductdto.CompartmentVariantData)
                                {
                                    if (compartemntData.VarientList != null)
                                    {

                                        foreach (var data in compartemntData.VarientList)
                                        {
                                            CompartmentVariantData variant = new CompartmentVariantData();
                                            var varientList = _productVariantDataRepository.GetAllList(i => i.Id == data.ProductVarientId).FirstOrDefault();
                                            variant.CompartmentSubTitle = compartemntData.CompartmentSubTitle;
                                            variant.CompartmentTitle = compartemntData.CompartmentTitle;
                                            variant.ProductId = resultUniqueId;
                                            variant.TenantId = TenantId;
                                            variant.Compartment = varientList.Variant;
                                            variant.SKU = varientList.SKU;
                                            variant.Price = varientList.Price;
                                            variant.ProductVarientId = varientList.Id;
                                            if (data.ImageObj != null)
                                            {
                                                string ImageLocation = AzureStorageUrl + folderPath + data.ImageObj.FileName;
                                                variant.Name = data.ImageObj.FileName;
                                                variant.Url = ImageLocation;
                                                variant.ImagePath = ImageLocation;
                                                variant.ImageFileName = data.ImageObj.FileName;
                                                variant.Ext = data.ImageObj.Ext;
                                                variant.Size = data.ImageObj.Size;
                                                variant.Type = data.ImageObj.Type;
                                            }
                                            var productcompartmentDataId = await _productCompartmentDataRepository.InsertAndGetIdAsync(variant);
                                        }

                                    }


                                }

                            }
                        }

                        catch (Exception ex)
                        {

                        }

                        if (createProductdto.CompartmentBaseImage != null)
                        {
                            try
                            {
                                ProductCompartmentBaseImages Model = new ProductCompartmentBaseImages();
                                string ImageLocation = AzureStorageUrl + folderPath + createProductdto.CompartmentBaseImage.FileName;
                                Model.ProductId = resultUniqueId;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                Model.Ext = createProductdto.CompartmentBaseImage.Ext;
                                Model.Size = createProductdto.CompartmentBaseImage.Size;
                                Model.Type = createProductdto.CompartmentBaseImage.Type;
                                Model.Url = ImageLocation;
                                Model.Name = createProductdto.CompartmentBaseImage.FileName;
                                Model.ImageFileName = createProductdto.CompartmentBaseImage.FileName;
                                Model.ImagePath = ImageLocation;
                                long Id = await _productBaseImageRepository.InsertAndGetIdAsync(Model);

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }


                    #endregion

                    #region ProductDimensionsInventory
                    ProductDimensionsInventory productDimensionsInventory = new ProductDimensionsInventory();
                    productDimensionsInventory.ProductId = resultUniqueId;
                    productDimensionsInventory.ProductHeight = createProductdto.ProductDimensionsInventory.ProductHeight;
                    productDimensionsInventory.ProductWidth = createProductdto.ProductDimensionsInventory.ProductWidth;
                    productDimensionsInventory.ProductLength = createProductdto.ProductDimensionsInventory.ProductLength;
                    productDimensionsInventory.UnitWeight = createProductdto.ProductDimensionsInventory.UnitWeight;
                    productDimensionsInventory.ProductPackaging = createProductdto.ProductDimensionsInventory.ProductPackaging;
                    productDimensionsInventory.CartonWeight = createProductdto.ProductDimensionsInventory.CartonWeight;
                    productDimensionsInventory.CartonQuantity = createProductdto.ProductDimensionsInventory.CartonQuantity;
                    productDimensionsInventory.ProductDimensionNotes = createProductdto.ProductDimensionsInventory.ProductDimensionNotes;
                    productDimensionsInventory.ProductDiameter = createProductdto.ProductDimensionsInventory.ProductDiameter;
                    productDimensionsInventory.ProductUnitMeasureId = createProductdto.ProductDimensionsInventory.ProductUnitMeasureId;
                    productDimensionsInventory.ProductWeightMeasureId = createProductdto.ProductDimensionsInventory.ProductWeightMeasureId;
                    if (string.IsNullOrEmpty(createProductdto.ProductDimensionsInventory.UnitPerProduct))
                    {
                        productDimensionsInventory.UnitPerProduct = 0;
                    }
                    else
                    {
                        productDimensionsInventory.UnitPerProduct = Convert.ToDouble(createProductdto.ProductDimensionsInventory.UnitPerProduct);
                    }

                    productDimensionsInventory.CartonHeight = createProductdto.ProductDimensionsInventory.CartonHeight;
                    productDimensionsInventory.CartonWidth = createProductdto.ProductDimensionsInventory.CartonWidth;
                    productDimensionsInventory.CartonLength = createProductdto.ProductDimensionsInventory.CartonLength;
                    productDimensionsInventory.CartonUnitOfMeasureId = createProductdto.ProductDimensionsInventory.CartonUnitOfMeasureId;
                    productDimensionsInventory.CartonWeightMeasureId = createProductdto.ProductDimensionsInventory.CartonWeightMeasureId;
                    productDimensionsInventory.CartonCubicWeightKG = createProductdto.ProductDimensionsInventory.CartonCubicWeightKG;
                    productDimensionsInventory.CartonWeight = createProductdto.ProductDimensionsInventory.CartonWeight;
                    if (string.IsNullOrEmpty(createProductdto.ProductDimensionsInventory.UnitPerCarton))
                    {
                        productDimensionsInventory.UnitPerCarton = 0;
                    }
                    else
                    {
                        productDimensionsInventory.UnitPerCarton = Convert.ToDouble(createProductdto.ProductDimensionsInventory.UnitPerCarton);
                    }
                    productDimensionsInventory.CartonPackaging = createProductdto.ProductDimensionsInventory.CartonPackaging;
                    productDimensionsInventory.CartonNote = createProductdto.ProductDimensionsInventory.CartonNote;
                    productDimensionsInventory.PalletWeight = createProductdto.ProductDimensionsInventory.PalletWeight;
                    if (string.IsNullOrEmpty(createProductdto.ProductDimensionsInventory.CartonPerPallet))
                    {
                        productDimensionsInventory.CartonPerPallet = 0;
                    }
                    else
                    {
                        productDimensionsInventory.CartonPerPallet = Convert.ToDouble(createProductdto.ProductDimensionsInventory.CartonPerPallet);
                    }
                    if (string.IsNullOrEmpty(createProductdto.ProductDimensionsInventory.UnitPerPallet))
                    {
                        productDimensionsInventory.UnitPerPallet = 0;
                    }
                    else
                    {
                        productDimensionsInventory.UnitPerPallet = Convert.ToDouble(createProductdto.ProductDimensionsInventory.UnitPerPallet);
                    }
                    productDimensionsInventory.PalletNote = createProductdto.ProductDimensionsInventory.PalletNote;
                    productDimensionsInventory.Barcode = createProductdto.ProductDimensionsInventory.Barcode;
                    productDimensionsInventory.StockkeepingUnit = createProductdto.ProductDimensionsInventory.StockkeepingUnit == "" ? 0 : Convert.ToInt32(createProductdto.ProductDimensionsInventory.StockkeepingUnit);
                    productDimensionsInventory.TotalNumberAvailable = createProductdto.ProductDimensionsInventory.TotalNumberAvailable == "" ? 0 : Convert.ToInt64(createProductdto.ProductDimensionsInventory.TotalNumberAvailable);
                    productDimensionsInventory.AlertRestockNumber = createProductdto.ProductDimensionsInventory.AlertRestockNumber == "" ? 0 : Convert.ToInt32(createProductdto.ProductDimensionsInventory.AlertRestockNumber);
                    productDimensionsInventory.IsTrackQuantity = createProductdto.ProductDimensionsInventory.IsTrackQuantity;
                    productDimensionsInventory.IsStopSellingStockZero = createProductdto.ProductDimensionsInventory.IsStopSellingStockZero;
                    productDimensionsInventory.IsActive = true;
                    productDimensionsInventory.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    long ProductandPackageDimensionId = await _productDimensionsInventoryRepository.InsertAndGetIdAsync(productDimensionsInventory);
                    #endregion

                    #region Warehouse ProductStockLocation
                    if (createProductdto.ProductStockLocation != null)
                    {
                        List<ProductStockLocation> productStockLocation = ObjectMapper.Map<List<ProductStockLocation>>(createProductdto.ProductStockLocation);
                        foreach (var productStockLocationitem in productStockLocation)
                        {
                            productStockLocationitem.ProductId = resultUniqueId;
                            productStockLocationitem.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            long productStockLocationId = await _productStockLocationRepository.InsertAndGetIdAsync(productStockLocationitem);
                        }
                    }
                    #endregion

                    #region Product Media Images
                    if (createProductdto.ProductMediaImages != null)
                    {
                        foreach (var image in createProductdto.ProductMediaImages)
                        {
                            ProductMediaImages Model = new ProductMediaImages();
                            if (image.FileName != null && image.FileName != "")
                            {

                                string ImageLocation = AzureStorageUrl + folderPath + image.FileName;
                                Model.ImageName = image.FileName;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                Model.ProductMediaImageTypeId = image.MediaType;
                                Model.IsProductSubmissionDone = true;
                                Model.ImageUrl = ImageLocation;
                                Model.ProductId = resultUniqueId;
                                Model.Ext = image.Ext;
                                Model.Size = image.Size;
                                Model.Type = image.Type;
                                Model.Name = image.Name;
                                long Id = await _productMediaImagesRepository.InsertAndGetIdAsync(Model);
                            }
                        }
                    }
                    #endregion
                }
                output.Id = resultUniqueId;
            }
            catch (Exception ex)
            {

            }
            return output;
        }

        public async Task<ProductImageDto> UploadProductImage(ProductImageRequest FileObj)
        {
            string PhysicalFilePath = string.Empty;
            string ImagePath = string.Empty;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";

            ProductImageDto Response = new ProductImageDto();
            Guid ImageName = Guid.NewGuid();
            try
            {
                string FolderName = _configuration["FileUpload:ProductImageFiles"];
                PhysicalFilePath = _configuration["FileUpload:PhysicalFileLocation"];
                var test = _configuration.GetValue<string>("FileUpload:PhysicalFileLocation");
                string CurrentWebsiteUrl = _configuration.GetValue<string>("FileUpload:WebsireDomainPath");
                string FilePath = await _azureBlobStorageService.SaveBlobImage(FileObj.ImageData, folderPath, ImageName.ToString() + "." + FileObj.Extension, "image/" + FileObj.Extension);
                ProductImages Model = new ProductImages();
                Model.ImageFileData = FileObj.ImageData;
                Model.ImageName = ImageName.ToString() + "." + FileObj.Extension;
                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                Model.IsProductSubmissionDone = false;
                Model.ImagePath = FilePath;
                Model.ImageExtension = FileObj.Extension;
                long Id = await _productImagesRepository.InsertAndGetIdAsync(Model);
                Response.Id = Id;
                Response.ImageName = ImageName.ToString() + "." + FileObj.Extension;
            }
            catch (Exception ex)
            {
            }
            return Response;
        }


        public async Task<string> UploadFileOnLocation(IFormFile FileObj)
        {
            string CurrentWebsiteUrl = string.Empty;
            bool IsInvalidType = false;
            string FilePath = string.Empty;
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + LogoFolderName + "/"}";

            try
            {
                var files = FileObj;
                byte[] ImageArray;

                Guid ImageName = Guid.NewGuid();
                IsInvalidType = false;

                using (var memoryStream = new MemoryStream())
                {
                    await files.CopyToAsync(memoryStream);
                    ImageArray = memoryStream.ToArray();
                }
                var ImageExt = files.ContentType.Split('/');

                FilePath = await _azureBlobStorageService.SaveBlobImage(ImageArray, folderPath, ImageName.ToString() + "." + ImageExt[1], files.ContentType);

                string ImageLocation = FilePath;
            }
            catch (Exception ex)
            {

            }
            return FilePath;
        }


        #region Get Supplier User List
        public async Task<List<SupplierListDto>> GetSupplierList()
        {
            List<SupplierListDto> UserList = new List<SupplierListDto>();
            Role Role = new Role();
            try
            {
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    Role = _roleRepository.GetAllList(i => i.Name.ToLower() == "supplier").FirstOrDefault();
                }

                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    var AllUsers = _userRepository.GetAll();
                    var UserRoles = _userRoleRepository.GetAllList(i => i.RoleId == Role.Id);
                    UserList = (from user in AllUsers
                                join userrole in UserRoles on user.Id equals userrole.UserId
                                select new SupplierListDto
                                {
                                    Id = user.Id,
                                    Name = user.Name + " " + user.Surname
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return UserList;
        }
        #endregion


        public async Task<ProductMasterDataDto> GetProductMasterdata()
        {
            int TenantId = AbpSession.TenantId.Value;
            ProductMasterDataDto ResponseModel = new ProductMasterDataDto();

            await _connectionUtility.EnsureConnectionOpenAsync();

            IEnumerable<ProductTagDto> ProductTagData = await _db.QueryAsync<ProductTagDto>(@"SELECT Id,ProductTagName,IsActive,ProductTagName as 'Code'
              FROM [dbo].[ProductTagMaster] where IsDeleted = 0 and TenantId = " + TenantId + " order by ProductTagName asc", new
            {

            }, commandType: System.Data.CommandType.Text);



            IEnumerable<ProductBrandDto> ProductBrandData = await _db.QueryAsync<ProductBrandDto>(@"SELECT Id,ProductBrandName,IsActive,ProductBrandName as 'Code'
              FROM [dbo].[ProductBrandMaster] where IsDeleted = 0 and TenantId = " + TenantId + " order by ProductBrandName asc", new
            {

            }, commandType: System.Data.CommandType.Text);



            IEnumerable<ProductTypeDto> ProductTypeData = await _db.QueryAsync<ProductTypeDto>(@"SELECT Id,ProductTypeName,IsActive,ProductTypeName as 'Code'
              FROM [dbo].[ProductTypeMaster] where IsDeleted = 0 order by ProductTypeName asc", new
            {

            }, commandType: System.Data.CommandType.Text);



            IEnumerable<ProductMaterialDto> ProductMaterialData = await _db.QueryAsync<ProductMaterialDto>(@"SELECT Id,ProductMaterialName,IsActive,ProductMaterialName as 'Code'
              FROM [dbo].[ProductMaterialMaster] where IsDeleted = 0 and TenantId = " + TenantId + " order by ProductMaterialName asc", new
            {

            }, commandType: System.Data.CommandType.Text);


            IEnumerable<CategoryCollectionsDto> ProductCollectionData = await _db.QueryAsync<CategoryCollectionsDto>(@"SELECT Id,CollectionName,IsActive,CollectionName as 'Code'
              FROM [dbo].[CollectionMaster] where IsDeleted = 0 and TenantId = " + TenantId + " order by CollectionName asc", new
            {

            }, commandType: System.Data.CommandType.Text);

            IEnumerable<SubCategoryMasterDto> SubcategoryMasterData = await _db.QueryAsync<SubCategoryMasterDto>(@"SELECT Id,Title,IsActive,Title as 'Code'
              FROM [dbo].[SubCategoryMaster] where IsDeleted = 0 and TenantId = " + TenantId + " order by Title asc", new
            {

            }, commandType: System.Data.CommandType.Text);

            var OptionData = _productOptionRepository.GetAllList(i => i.OptionName.ToLower().Trim() == "compartment").FirstOrDefault();

            IEnumerable<ProductOptionDto> ProductOptiondata = await _db.QueryAsync<ProductOptionDto>(@"SELECT Id,OptionName,IsActive,OptionName as 'Code'
              FROM [dbo].[ProductOptionsMaster] where IsDeleted = 0 and Id != " + OptionData.Id + " order by OptionName asc", new
            {

            }, commandType: System.Data.CommandType.Text);


            IEnumerable<WareHouseDto> WareHouseData = await _db.QueryAsync<WareHouseDto>(@"SELECT Id,WarehouseTitle,IsActive,WarehouseTitle as 'Code'
              FROM [dbo].[WareHouseMaster] where IsDeleted = 0 and TenantId = " + TenantId + " order by WarehouseTitle asc", new
            {

            }, commandType: System.Data.CommandType.Text);



            IEnumerable<TurnAroundTimeDto> TurnaroundTime = await _db.QueryAsync<TurnAroundTimeDto>(@"SELECT Id,Time,IsActive,Time as 'Code'
              FROM [dbo].[TurnAroundTime] where IsDeleted = 0 order by Id asc", new
            {

            }, commandType: System.Data.CommandType.Text);

            IEnumerable<ProductSizeDto> SizeData = await _db.QueryAsync<ProductSizeDto>(@"SELECT Id,ProductSizeName,IsActive,ProductSizeName as 'Code'
              FROM [dbo].[ProductSizeMaster] where IsDeleted = 0 and TenantId = " + TenantId + " and (ProductSizeName) != 'kg' and (ProductSizeName) != 'gr' order by ProductSizeName asc", new
            {

            }, commandType: System.Data.CommandType.Text);



            IEnumerable<ProductSizeDto> WeightData = await _db.QueryAsync<ProductSizeDto>(@" SELECT Id,ProductSizeName,IsActive,ProductSizeName as 'Code'
            FROM [dbo].[ProductSizeMaster] where IsDeleted = 0 and TenantId = " + TenantId + " and (ProductSizeName = 'kg' or ProductSizeName = 'gr')order by ProductSizeName asc", new
            {

            }, commandType: System.Data.CommandType.Text);

            IEnumerable<ProductOptionDto> CompartmentOption = await _db.QueryAsync<ProductOptionDto>(@"SELECT Id,OptionName,IsActive,OptionName as 'Code'
              FROM [dbo].[ProductOptionsMaster] where IsDeleted = 0 and (OptionName = 'Color' or OptionName = 'Compartment') order by OptionName asc", new
            {

            }, commandType: System.Data.CommandType.Text);
            try
            {
                var CategoryData = (from data in _categoryGroupRepository.GetAllList()
                                    select new CategoryIdDto
                                    {
                                        Id = data.Id,
                                        CategoryTitle = data.GroupTitle,
                                        CategoryList = (from coll in _categoryGroupsRepository.GetAll()
                                                        join subCat in _categoryMasterRepository.GetAll() on coll.CategoryMasterId equals subCat.Id
                                                        where coll.CategoryGroupId == data.Id
                                                        select new SubCategoryIdDto
                                                        {
                                                            SubCategoryId = subCat.Id,
                                                            SubCategoryTitle = subCat.CategoryTitle,
                                                            subSubCategory = (from cat in _productCategorySubCategoryRepository.GetAll()
                                                                              join subSubCat in _productSubCategoryRepository.GetAll() on cat.SubCategoryId equals subSubCat.Id
                                                                              where cat.CategoryId == subCat.Id
                                                                              select new SubSubCategoryIdDto
                                                                              {
                                                                                  SubSubCategoryId = subSubCat.Id,
                                                                                  SubSubCategoryTitle = subSubCat.Title
                                                                              }).OrderBy(i => i.SubSubCategoryTitle).ToList(),
                                                        }).OrderBy(i => i.SubCategoryTitle).ToList(),
                                    }).OrderBy(i => i.CategoryTitle).ToList();
                ResponseModel.CategoryList = CategoryData.ToList();
            }
            catch (Exception ex) { }


            ResponseModel.UnitSizeList = SizeData.ToList();
            ResponseModel.WeightMeasureList = WeightData.ToList();
            ResponseModel.ProductTagData = ProductTagData.ToList();
            ResponseModel.ProductBrandData = ProductBrandData.ToList();
            ResponseModel.ProductTypeData = ProductTypeData.ToList();
            ResponseModel.ProductMaterialData = ProductMaterialData.ToList();
            ResponseModel.ProductCollectionData = ProductCollectionData.ToList();
            ResponseModel.ProductOptionsData = ProductOptiondata.ToList();
            ResponseModel.SupplierListData = await GetSupplierList();
            ResponseModel.BrandingmethodData = await GetBrandingMethodData();
            ResponseModel.WareHouseData = WareHouseData.ToList();
            ResponseModel.TurnAroundTimeData = TurnaroundTime.ToList();
           
            ResponseModel.SubCategoryMasterData = SubcategoryMasterData.ToList();
            return ResponseModel;
        }


        #region Save file in www root location of solution
        private string SaveFileInWWWRootLocation(byte[] ImgStr, string ImgName, string WebrootPath)
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

        public async Task<List<BrandingMethodDto>> GetBrandingMethodData()
        {
            List<BrandingMethodDto> ResponseData = new List<BrandingMethodDto>();
            try
            {

                ResponseData = (from brandingMethod in _repositoryBrandingMethod.GetAll()
                                where brandingMethod.IsActive == true
                                // join assign in _BrandingMethodAssignedColorsRepository.GetAll() on brandingMethod.Id equals assign.BrandingMethodId
                                select new BrandingMethodDto
                                {
                                    Id = brandingMethod.Id,
                                    MethodName = brandingMethod.MethodName,
                                    ImageUrl = brandingMethod.ImageUrl,
                                    Ext = brandingMethod.Ext,
                                    UniqueNumber = brandingMethod.UniqueNumber,
                                    Height = brandingMethod.Height,
                                    ImageName = brandingMethod.ImageName,
                                    Size = brandingMethod.Size,
                                    Type = brandingMethod.Type,
                                    Url = brandingMethod.Url,
                                    Width = brandingMethod.Width,
                                    Notes = brandingMethod.Notes,
                                    IsActive = brandingMethod.IsActive,
                                    SelectedColor = (from assign in _BrandingMethodAssignedColorsRepository.GetAll()
                                                     join color in _productColorRepository.GetAll() on assign.ColorId equals color.Id
                                                     where assign.BrandingMethodId == brandingMethod.Id
                                                     select color.ProductColourName).FirstOrDefault(),
                                    //assign.ColorId > 0? _productColorRepository.GetAll().Where(i=>i.Id == assign.ColorId).Select(i=>i.ProductColourName).FirstOrDefault() : "",
                                    SelectedColorDraft = (from assign in _BrandingMethodAssignedColorsRepository.GetAll()
                                                          join color in _productColorRepository.GetAll() on assign.ColorId equals color.Id
                                                          where assign.BrandingMethodId == brandingMethod.Id
                                                          select color.ProductColourName).FirstOrDefault(),
                                    //assign.ColorId > 0 ? _productColorRepository.GetAll().Where(i => i.Id == assign.ColorId).Select(i => i.ProductColourName).FirstOrDefault() : "",
                                }).OrderBy(i => i.MethodName).ToList();
            }
            catch (Exception ex)
            {

            }
            return ResponseData;
        }


        #region Get Product List   

        private DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {

            var command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            // command.Transaction = GetActiveTransaction();

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }

        private async Task EnsureConnectionOpenAsync()
        {
            var connection = _dbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
        }

        private DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
                {
                {"ContextType", typeof(CaznerMarketplaceBackendAppDbContext) },
                {"MultiTenancySide", null }
                });
        }



        public async Task<ProductCustomlistDto> GetProductListData(ProductMasterResultRequestDto Input)
        {

            int TotalCount = 0;
            bool IsLoadMore = false;
            ProductCustomlistDto response = new ProductCustomlistDto();
            List<ProductListViewDto> ProductData = new List<ProductListViewDto>();
            try
            {

                await EnsureConnectionOpenAsync();

                IEnumerable<ProductListViewDto> ProductDataa = await _db.QueryAsync<ProductListViewDto>("usp_GetProductListBySearch", new
                {
                    SearchText = Input.SearchText,
                    TenantId = AbpSession.TenantId.Value,
                    Type = Input.TypeId
                }, commandType: System.Data.CommandType.StoredProcedure);

                ProductData = ProductDataa.ToList();

                List<CategoryCollections> CollectionList = new List<CategoryCollections>();

                if (Input.CategoryId.HasValue)
                {
                    ////var CategoryData = (from data in _categoryMasterRepository.GetAll()
                    ////                    join grp in _categoryGroupsRepository.GetAll() on data.Id equals grp.CategoryMasterId
                    ////                    where grp.CategoryGroupId == Input.CategoryId.Value
                    ////                    select data).ToList();

                    ProductData = (from master in ProductData
                                   join assign in _repositoryAssignedCategoryMaster.GetAll() on master.Id equals assign.ProductId
                                   where assign.CategoryId == Input.CategoryId.Value
                                   select master).ToList();


                }
                if (Input.SubSubCategoryIds != null)
                {
                    ProductData = (from master in ProductData
                                   join assign in _productAssignedSubCategoriesRepository.GetAll() on master.Id equals assign.ProductId
                                   join category in Input.SubSubCategoryIds on assign.SubCategoryId equals category
                                   select master).ToList();

                }

                TotalCount = ProductData.Count();

                ProductData = ProductData.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).ToList();
                ProductData = await ApplyDtoSortingList(ProductData, Input);
                ProductData = ProductData.Skip(Input.SkipCount).Take(Input.MaxResultCount).ToList();

                if (Input.SkipCount + Input.MaxResultCount < TotalCount)
                {
                    IsLoadMore = true;
                }
                response.SkipCount = Input.SkipCount;
                response.items = ProductData;
                response.TotalCount = TotalCount;
                response.IsLoadMore = IsLoadMore;
            }
            catch (Exception ex)
            {
            }
            return response;
        }

        #endregion

        #region Get products by collection id

        public async Task<ProductCustomlistDto> GetProductsByCollectionId(ProductMasterResultRequestDto Input)
        {

            int TotalCount = 0;
            bool IsLoadMore = false;
            ProductCustomlistDto response = new ProductCustomlistDto();
            List<ProductListViewDto> ProductData = new List<ProductListViewDto>();
            try
            {

                await EnsureConnectionOpenAsync();

                IEnumerable<ProductListViewDto> ProductDataa = await _db.QueryAsync<ProductListViewDto>("usp_GetProductListBySearch", new
                {
                    SearchText = Input.SearchText,
                    TenantId = AbpSession.TenantId.Value,
                    Type = Input.TypeId
                }, commandType: System.Data.CommandType.StoredProcedure);

                ProductData = ProductDataa.ToList();

                List<CategoryCollections> CollectionList = new List<CategoryCollections>();

                if (Input.CollectionId.HasValue)
                {
                    ProductData = (from master in ProductData
                                   join assign in _productAssignedCollectionsRepository.GetAll() on master.Id equals assign.ProductId
                                   where assign.CollectionId == Input.CollectionId.Value
                                   select master).ToList();

                }

                TotalCount = ProductData.Count();

                ProductData = ProductData.GroupBy(g => g.Id).Select(x => x.FirstOrDefault()).ToList();
                ProductData = await ApplyDtoSortingList(ProductData, Input);
                ProductData = ProductData.Skip(Input.SkipCount).Take(Input.MaxResultCount).ToList();

                if (Input.SkipCount + Input.MaxResultCount < TotalCount)
                {
                    IsLoadMore = true;
                }
                response.SkipCount = Input.SkipCount;
                response.items = ProductData;
                response.TotalCount = TotalCount;
                response.IsLoadMore = IsLoadMore;
            }
            catch (Exception ex)
            {
            }
            return response;
        }

        #endregion

        #region sorting 
        private async Task<IQueryable<ProductListViewDto>> ApplyDtoSorting(IQueryable<ProductListViewDto> query, ProductMasterResultRequestDto input)
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
                            query = query.OrderBy(x => x.ProductTitle);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle);
                        }
                        break;
                    case 3://Name
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.ProductTitle);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ProductTitle);
                        }
                        break;

                    case 4://price
                        if (input.FilterBy == (int)FilterByEnum.ascending)
                        {
                            query = query.OrderBy(x => x.UnitPrice);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.UnitPrice);
                        }
                        break;
                }
            }
            return query;
        }
        private async Task<List<ProductListViewDto>> ApplyDtoSortingList(List<ProductListViewDto> query, ProductMasterResultRequestDto input)
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

                    case 4://price
                        if (input.FilterBy == (int)FilterByEnum.ascending)
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


        #endregion
        #region Get Product Data by Id    
        public async Task<GetProductDto> GetProductDataById(ProductDataRequestDto Input)
        {
            long[] ValuesArray;
            string[] categorySelectedTitle = new string[0] { };
            string[] subcategorySelectedTitle = new string[0] { };
            string[] subsubCategorySelectedTitle = new string[0] { };
            GetProductDto response = new GetProductDto();
            try
            {
                var ProductData = new GetProductDto();
                List<CompartmentVariantCustomDataDto> CompartmentVariantList = new List<CompartmentVariantCustomDataDto>();
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    await _connectionUtility.EnsureConnectionOpenAsync();

                    var product = await _db.QueryFirstOrDefaultAsync<ProductCustomDataDto>("usp_GetProductDataById", new
                    {
                        ProductId = Input.ProductId,
                        TenantId = AbpSession.TenantId.Value
                    }, commandType: System.Data.CommandType.StoredProcedure);

                    if (product != null)
                    {


                        IEnumerable<ProductImageType> ProductImages = await _db.QueryAsync<ProductImageType>(@"select images.ImageName as FileName, images.ImagePath,images.IsDefaultImage, images.Ext,images.[Name], images.[Name] as FileName,(case when images.[Size] IS NULL Then 0 else images.Size end) as Size,
	            images.[Type],images.Id, images.ImagePath as Url , masters.ProductSKU  from ProductImages as images
                inner join ProductMaster as masters on masters.Id = images.ProductId where images.ProductId = " + Input.ProductId + " and images.TenantId = " + AbpSession.TenantId.Value + " and images.IsDeleted=0", new
                        {
                            ProductId = Input.ProductId
                        }, commandType: System.Data.CommandType.Text);

                        IEnumerable<ProductBrandingPositionDto> productBrandingPositionDtos = await _db.QueryAsync<ProductBrandingPositionDto>("usp_GetProductBrandingPositionByProductId", new
                        {
                            ProductId = Input.ProductId
                        }, commandType: System.Data.CommandType.StoredProcedure);

                        IEnumerable<ProductStockLocationDto> productStockLocationDtos = await _db.QueryAsync<ProductStockLocationDto>("usp_GetProductLocationByProductId", new
                        {
                            ProductId = Input.ProductId
                        }, commandType: System.Data.CommandType.StoredProcedure);

                        IEnumerable<BrandingMethodDto> brandingMethodList = await _db.QueryAsync<BrandingMethodDto>("usp_GetProductBrandingMethodDataByProductId", new
                        {
                            ProductId = Input.ProductId
                        }, commandType: System.Data.CommandType.StoredProcedure);



                        IEnumerable<ProductDetailCombo> ProductTypeData = await _db.QueryAsync<ProductDetailCombo>(@"select types.Id,types.ProductTypeName,types.ProductTypeName as Code, assign.Id as AssignmentId  from ProductTypeMaster as types
						join ProductAssignedTypes as assign on types.Id = assign.TypeId where assign.ProductId = " + Input.ProductId + " and assign.IsDeleted = 0 and assign.TenantId =" + AbpSession.TenantId.Value + "", new
                        {
                            ProductId = Input.ProductId
                        }, commandType: System.Data.CommandType.Text);

                        IEnumerable<ProductDetailCombo> ProductBrandData = await _db.QueryAsync<ProductDetailCombo>(@"select types.Id,types.ProductBrandName,types.ProductBrandName as Code, assign.Id as AssignmentId  from ProductBrandMaster as types
						join ProductAssignedBrands as assign on types.Id = assign.ProductBrandId where assign.ProductId = " + Input.ProductId + " and assign.IsDeleted = 0 and assign.TenantId =" + AbpSession.TenantId.Value + "", new
                        {
                            ProductId = Input.ProductId
                        }, commandType: System.Data.CommandType.Text);


                        IEnumerable<ProductDetailCombo> ProductCollectionData = await _db.QueryAsync<ProductDetailCombo>(@"select types.Id,types.CollectionName,types.CollectionName as Code, assign.Id as AssignmentId  
                        from CollectionMaster as types join ProductAssignedCollections as assign on types.Id = assign.CollectionId where assign.ProductId = " + Input.ProductId + " and assign.IsDeleted = 0 and assign.TenantId =" + AbpSession.TenantId.Value + "", new
                        {
                            ProductId = Input.ProductId
                        }, commandType: System.Data.CommandType.Text);
                        IEnumerable<ProductDetailCombo> ProductMaterialData = await _db.QueryAsync<ProductDetailCombo>(@"select types.Id,types.ProductMaterialName,types.ProductMaterialName as Code, assign.Id as AssignmentId  
						from ProductMaterialMaster as types join ProductAssignedMaterials as assign on types.Id = assign.MaterialId where assign.ProductId = " + Input.ProductId + " and assign.IsDeleted = 0 and assign.TenantId =" + AbpSession.TenantId.Value + "", new
                        {
                            ProductId = Input.ProductId
                        }, commandType: System.Data.CommandType.Text);

                        IEnumerable<ProductDetailCombo> ProductVendorsData = await _db.QueryAsync<ProductDetailCombo>(@"select types.Id,types.Name+' '+types.Surname,types.Name+' '+types.Surname as Code, assign.Id as AssignmentId  
						from AbpUsers as types join ProductAssignedVendors as assign on types.Id = assign.VendorUserId where assign.ProductId = " + Input.ProductId + " and assign.IsDeleted = 0 and assign.TenantId =" + AbpSession.TenantId.Value + "", new
                        {
                            ProductId = Input.ProductId
                        }, commandType: System.Data.CommandType.Text);

                        IEnumerable<ProductDetailCombo> ProductTagsData = await _db.QueryAsync<ProductDetailCombo>(@"select types.Id,types.ProductTagName, types.ProductTagName as Code, assign.Id as AssignmentId  
						from ProductTagMaster as types join ProductAssignedTags as assign on types.Id = assign.TagId where assign.ProductId = " + Input.ProductId + " and assign.IsDeleted = 0 and assign.TenantId =" + AbpSession.TenantId.Value + "", new
                        {
                            ProductId = Input.ProductId
                        }, commandType: System.Data.CommandType.Text);



                        //                  IEnumerable<ProductDetailCombo> ProductCategoriesData = await _db.QueryAsync<ProductDetailCombo>(@"select types.Id,types.CategoryTitle, types.CategoryTitle as Code, assign.Id as AssignmentId  
                        //from CategoryMaster as types join ProductAssignedCategories as assign on types.Id = assign.CategoryId where assign.ProductId = " + Input.ProductId + " and assign.IsDeleted = 0 and assign.TenantId =" + AbpSession.TenantId.Value + "", new
                        //                  {
                        //                      ProductId = Input.ProductId
                        //                  }, commandType: System.Data.CommandType.Text);


                        //IEnumerable<ProductDetailCombo> CollectionListByCategoryId = Enumerable.Empty<ProductDetailCombo>();
                        //if (ProductCategoriesData.ToList().Count > 0)
                        //{
                        //    long CategoryId = ProductCategoriesData.ToList().Select(i => i.Id).FirstOrDefault();
                        //    CollectionListByCategoryId = await _db.QueryAsync<ProductDetailCombo>(@"select CollectionMaster.Id,CollectionMaster.CollectionName, CollectionMaster.CollectionName as Code from CollectionMaster join CategoryCollections on CollectionMaster.Id = CategoryCollections.CollectionId where CategoryId = " + CategoryId + " and CollectionMaster.IsDeleted=0 and CollectionMaster.TenantId=" + AbpSession.TenantId.Value + "", new
                        //    {

                        //    }, commandType: System.Data.CommandType.Text);
                        //}
                        //IEnumerable<ProductDetailCombo> SubCategoryListByCategoryId = Enumerable.Empty<ProductDetailCombo>();
                        //if (ProductCategoriesData.ToList().Count > 0)
                        //{
                        //    long CategoryId = ProductCategoriesData.ToList().Select(i => i.Id).FirstOrDefault();
                        //    SubCategoryListByCategoryId = await _db.QueryAsync<ProductDetailCombo>(@"select SubCategoryMaster.Id,SubCategoryMaster.Title, SubCategoryMaster.Title as Code from SubCategoryMaster join CategorySubCategories on SubCategoryMaster.Id = CategorySubCategories.SubCategoryId where CategoryId = " + CategoryId + " and SubCategoryMaster.IsDeleted=0 and SubCategoryMaster.TenantId=" + AbpSession.TenantId.Value + "", new
                        //    {

                        //    }, commandType: System.Data.CommandType.Text);
                        //}


                        IEnumerable<ProductVolumeDiscountVariantDto> VolumeVariant = await _db.QueryAsync<ProductVolumeDiscountVariantDto>(@"select variant.Id, variant.ProductId, variant.QuantityFrom, variant.Price,variant.IsActive from ProductVolumeDiscountVariant as variant
                        where variant.ProductId = " + Input.ProductId + " and variant.IsDeleted = 0 and variant.TenantId =" + AbpSession.TenantId.Value + "", new
                        {
                            ProductId = Input.ProductId
                        }, commandType: System.Data.CommandType.Text);
                        IEnumerable<CompartmentVariantCustomDataDto> CompartmentVariantData = await _db.QueryAsync<CompartmentVariantCustomDataDto>("usp_GetCompartmentData", new
                        {
                            productId = Input.ProductId,
                            tenantId = AbpSession.TenantId.Value
                        }, commandType: System.Data.CommandType.StoredProcedure);

                        IEnumerable<ProductCategory> Categories = await _db.QueryAsync<ProductCategory>("usp_GetCategoriesByProductId", new
                        {
                            tenantId = AbpSession.TenantId.Value,
                            productId = Input.ProductId
                        }, commandType: System.Data.CommandType.StoredProcedure);


                        var SubSubEmptyArray = new SubSubCategoryIdDto[] { };
                        var SubEmptyArray =  new SubCategoryIdDto[] { };

                        var CategoryListDistinct = Categories.GroupBy(g => g.AssignmentCategoryId).Select(x => x.FirstOrDefault()).Distinct().ToList();
                        CompartmentVariantList = CompartmentVariantData.GroupBy(g => g.CompartmentTitle).Select(x => x.FirstOrDefault()).Distinct().ToList();
                        ProductData = new GetProductDto()
                        {
                            Id = product.Id,
                            ProductSKU = product.ProductSKU,
                            ProductTitle = product.ProductTitle,
                            ProductDescripition = product.ProductDescripition,
                            ShortDescripition = product.ShortDescripition,
                            ProductNotes = product.ProductNotes,
                            IsActive = product.IsActive,
                            IsProductHasMultipleOptions = product.IsProductHasMultipleOptions,
                            IsProductHasCompartmentBuilder = product.IsProductHasCompartmentBuilder,
                            IsPhysicalProduct = product.IsPhysicalProduct,
                            IsPublished = product.IsPublished,
                            IsIndentOrder = product.IsIndentOrder,
                            TurnAroundTime = product.TurnAroundTime,
                            Features = product.Features,
                            BrandingUnitOfMeasure = product.BrandingUnitOfMeasure,
                            TurnAroundTimeId = product.TurnAroundTimeId,
                            IsHasSubProducts = product.IsHasSubProducts,
                            NumberOfSubProducts = product.NumberOfSubProducts,
                            CompartmentBuilderTitle = product.CompartmentBuilderTitle,
                            IsProductCompartmentType = product.IsProductCompartmentType,
                            CompartmentDescription = product.CompartmentDescription,
                            CompartmentTile = product.CompartmentTitle,
                            IsHasPriceVariants = VolumeVariant.ToList().Count > 0 ? true : false,
                            CompartmentBaseImage = !string.IsNullOrEmpty(product.CompartmentBaseFileName) ? (new ProductImageType
                            {
                                Url = product.CompartmentBaseImageUrl,
                                Type = product.CompartmentBaseImageType,
                                Size = product.CompartmentBaseImageSize,
                                Name = product.CompartmentBaseImageName,
                                Ext = product.CompartmentBaseImageExt,
                                FileName = product.CompartmentBaseFileName,
                                ImagePath = product.CompartmentBaseImagePath
                            }) : null,
                            BrandingUnitOfMeasureId = Convert.ToString(product.BrandingUnitOfMeasureId) == "" ? 0 : product.BrandingUnitOfMeasureId,
                            ProductPrice = (new ProductPriceDto
                            {
                                Id = product.Id,
                                Profit = Convert.ToString(product.Profit),
                                UnitPrice = Convert.ToString(product.UnitPrice),
                                CostPerItem = Convert.ToString(product.CostPerItem),
                                MarginIncreaseOnSalePrice = Convert.ToString(product.MarginIncreaseOnSalePrice),
                                SalePrice = Convert.ToString(product.SalePrice),
                                OnSale = product.OnSale,
                                UnitOfMeasure = Convert.ToString(product.UnitOfMeasure),
                                MinimumOrderQuantity = Convert.ToString(product.MinimumOrderQuantity),
                                DepositRequired = Convert.ToString(product.DepositRequired),
                                ChargeTaxOnThis = product.ChargeTaxOnThis,
                                ProductHasPriceVariant = product.ProductHasPriceVariant,
                                DiscountPercentage = Convert.ToString(product.DiscountPercentage),
                                DiscountPercentageDraft = Convert.ToString(product.DiscountPercentageDraft)
                            }),

                            //ProductVolumeDiscountVariant
                            ProductVolumeDiscountVariant = VolumeVariant.Where(i => i.Price > 0 && i.QuantityFrom > 0).OrderBy(i => i.QuantityFrom).ToList(),

                            //ProductStockLocation
                            ProductStockLocation = productStockLocationDtos.ToList(),

                            ProductDimensionsInventory = new ProductDimensionsInventoryDto
                            {

                                Id = product.InventoryId,
                                PalletWeight = product.PalletWeight,
                                CartonPerPallet = Convert.ToString(product.CartonPerPallet),
                                UnitPerPallet = Convert.ToString(product.UnitPerPallet),
                                PalletNote = product.PalletNote,
                                IsActive = true,
                                CartonHeight = product.CartonHeight,
                                CartonWeight = product.CartonWeight,
                                CartonWidth = product.CartonWidth,
                                CartonLength = product.CartonLength,
                                CartonPackaging = product.CartonPackaging,
                                CartonNote = product.CartonNote,
                                CartonCubicWeightKG = product.CartonCubicWeightKG,
                                CartonUnitOfMeasureId = product.CartonUnitOfMeasureId.ToString() == "" ? 0 : product.CartonUnitOfMeasureId,
                                CartonWeightMeasureId = product.CartonWeightMeasureId.ToString() == "" ? 0 : product.CartonWeightMeasureId,
                                CartonUnitOfMeasureTitle = product.CartonUnitOfMeasureTitle,
                                CartonWeightMeasureTitle = product.CartonWeightMeasureTitle,
                                ProductId = Input.ProductId,
                                ProductHeight = product.ProductHeight,
                                ProductWidth = product.ProductWidth,
                                ProductLength = product.ProductLength,
                                UnitWeight = product.UnitWeight,
                                ProductPackaging = product.ProductPackaging,
                                UnitPerProduct = Convert.ToString(product.UnitPerProduct),
                                ProductDiameter = product.ProductDiameter,
                                ProductDimensionNotes = product.ProductDimensionNotes,
                                ProductUnitMeasureId = product.ProductUnitMeasureId.ToString() == "" ? 0 : product.ProductUnitMeasureId,
                                ProductWeightMeasureId = product.ProductWeightMeasureId.ToString() == "" ? 0 : product.ProductWeightMeasureId,
                                ProductUnitMeasureTitle = product.ProductUnitMeasureTitle,
                                ProductWeightMeasureTitle = product.ProductWeightMeasureTitle,
                                Barcode = product.Barcode,
                                TotalNumberAvailable = product.TotalNumberAvailable.ToString(),
                                AlertRestockNumber = product.AlertRestockNumber.ToString()

                            },

                            BrandingMethodData = brandingMethodList.ToList(),

                            //ProductDetail
                            ProductDetail = new ProductDetailComboDto
                            {

                                ProductTypeArray = ProductTypeData.ToArray(),
                                ProductBrandArray = ProductBrandData.ToArray(),
                                ProductCollectionArray = ProductCollectionData.ToArray(),
                                ProductMaterialArray = ProductMaterialData.ToArray(),
                                ProductVendorArray = ProductVendorsData.ToArray(),
                                ProductTagArray = ProductTagsData.ToArray(),
                                //ProductCategoriesArray = ProductCategoriesData.ToList().FirstOrDefault(),

                                ProductCategoriesData = (from data in CategoryListDistinct
                                                         select new CategoryIdDto
                                                         {
                                                             Id = data.CategoryId,
                                                             ProductId = data.ProductId,
                                                             CategoryTitle = data.CategoryName,
                                                             categorySelectedTitle = categorySelectedTitle,
                                                             AssignmentId = data.AssignmentCategoryId,
                                                             CategoryList =  (from coll in _categoryGroupsRepository.GetAll()
                                                                             join subCat in _categoryMasterRepository.GetAll() on coll.CategoryMasterId equals subCat.Id
                                                                             where coll.CategoryGroupId == data.CategoryId
                                                                             select new SubCategoryIdDto
                                                                             {
                                                                                 SubCategoryId = subCat.Id,
                                                                                 SubCategoryTitle = subCat.CategoryTitle,
                                                                                 subSubCategory =  (from cat in _productCategorySubCategoryRepository.GetAll()
                                                                                                   join subSubCat in _productSubCategoryRepository.GetAll() on cat.SubCategoryId equals subSubCat.Id
                                                                                                   where cat.CategoryId == subCat.Id
                                                                                                   select new SubSubCategoryIdDto
                                                                                                   {
                                                                                                       SubSubCategoryId = subSubCat.Id,
                                                                                                       SubSubCategoryTitle = subSubCat.Title
                                                                                                   }).ToList(), //Array.Empty<SubSubCategoryIdDto>(),
                                                                             }).ToList(), //Array.Empty<SubCategoryIdDto>(),
                                                             subCategorySelectedTitle = (from item in Categories
                                                                                         where item.CategoryId == data.CategoryId && item.SubCategoryId >0
                                                                                         select new SubCategoryIdDto
                                                                                         {
                                                                                             SubCategoryId = item.SubCategoryId,
                                                                                             SubCategoryTitle = item.SubCategoryName,
                                                                                             AssignmentId = item.AssignnmentSubCategoryId,
                                                                                             SubsubCategorySelectedTitle = (from subCategorydata in Categories
                                                                                                                            where subCategorydata.SubCategoryId == item.SubCategoryId && subCategorydata.SubSubCategoryId>0
                                                                                                                            select new SubSubCategoryIdDto
                                                                                                                            {
                                                                                                                                SubSubCategoryId = subCategorydata.SubSubCategoryId,
                                                                                                                                SubSubCategoryTitle = subCategorydata.SubSubCategoryName,
                                                                                                                                AssignmentId = subCategorydata.AssignnmentSubSubCategoryId

                                                                                                                            }).OrderBy(i => i.SubSubCategoryId).GroupBy(g => g.SubSubCategoryId).Select(x => x.FirstOrDefault()).ToList(),

                                                                                             subSubCategory =  (from cat in _productCategorySubCategoryRepository.GetAll()
                                                                                                               join subSubCat in _productSubCategoryRepository.GetAll() on cat.SubCategoryId equals subSubCat.Id
                                                                                                               where cat.CategoryId == item.SubCategoryId
                                                                                                               select new SubSubCategoryIdDto
                                                                                                               {
                                                                                                                   SubSubCategoryId = subSubCat.Id,
                                                                                                                   SubSubCategoryTitle = subSubCat.Title
                                                                                                               }).ToList(),//Array.Empty<SubSubCategoryIdDto>(),
                                                                                         }).OrderBy(i => i.SubCategoryTitle).GroupBy(g => g.SubCategoryId).Select(x => x.FirstOrDefault()).ToList(),
                                                         }).ToList()

                            },
                            //ProductBrandingPosition
                            ProductBrandingPosition = productBrandingPositionDtos.ToList(),

                            //ProductDefaultImage
                            ProductDefaultImage = ProductImages.Where(i => i.IsDefaultImage == true).FirstOrDefault(),

                            ProductImagesNames = ProductImages.Where(i => i.IsDefaultImage == false).ToList(),

                            CompartmentVariantData = (from compartmentList in CompartmentVariantList
                                                      select new CompartmentVariantDataDto
                                                      {
                                                          Id = compartmentList.Id,
                                                          ProductId = compartmentList.ProductId,
                                                          CompartmentTitle = compartmentList.CompartmentTitle,
                                                          CompartmentSubTitle = compartmentList.CompartmentSubTitle,
                                                          VarientList = (from item in CompartmentVariantData.ToList()
                                                                         where item.CompartmentTitle.ToLower().Trim() == compartmentList.CompartmentTitle.ToLower().Trim()
                                                                         select new VarientList
                                                                         {
                                                                             Id = item.ProductVarientId.Value,
                                                                             CompartmentId = item.Id,
                                                                             Price = item.Price,
                                                                             SKU = item.SKU,
                                                                             ProductVarientId = item.ProductVarientId.Value,
                                                                             Color = item.Color,
                                                                             Variant = item.Variant,
                                                                             ImageObj = new ProductImageType
                                                                             {
                                                                                 Ext = item.Ext,
                                                                                 Url = item.Url,
                                                                                 FileName = item.ImageFileName,
                                                                                 ImagePath = item.ImagePath,
                                                                                 Size = item.Size,
                                                                                 Name = item.Name,
                                                                                 Type = item.Type
                                                                             }
                                                                         }).ToList()


                                                      }).ToList()
                        };

                    }

                    response = ProductData;
                }
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public async Task<List<CompartmentVariantData>> GetCompartmentVariantById(long VarientId, long ProductId)
        {
            int TenantId = AbpSession.TenantId.Value;
            List<CompartmentVariantData> Response = new List<CompartmentVariantData>();
            try
            {
                await _connectionUtility.EnsureConnectionOpenAsync();
                IEnumerable<CompartmentVariantData> CompartmentVariantData = await _db.QueryAsync<CompartmentVariantData>(@"Select Id, CompartmentTitle, CompartmentSubTitle,Ext,(case when images.[Size] IS NULL Then 0 else images.Size end) as Size, Type,Name,Url,ImageFileName,
                                                                                                                                             Price from CompartmentVariantData                                                                                                                                                                    
                                                                                                     Where IsDeleted = 0  and ProductId = " + ProductId + " and ProductVarientId = " + VarientId + " and TenantId = " + AbpSession.TenantId.Value + "", new
                {
                    ProductVarientId = VarientId,
                    ProductId = ProductId,
                    TenantId = AbpSession.TenantId.Value

                }, commandType: System.Data.CommandType.Text);

                Response = CompartmentVariantData.ToList();

            }
            catch (Exception ex)
            {

            }
            return Response;
        }


        public async Task DeleteCompartmentById(long Id)
        {
            var compartmentData = _productCompartmentDataRepository.GetAllList(i => i.Id == Id).FirstOrDefault();
            if (compartmentData != null)
            {
                var OptionValues = _productCompartmentOptionValuesRepository.GetAllList(i => i.CompartmentVariantId == compartmentData.Id).ToList();
                if (OptionValues.Count > 0)
                {
                    _productCompartmentOptionValuesRepository.BulkDelete(OptionValues);
                }
                await _productCompartmentDataRepository.DeleteAsync(compartmentData);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }


        public async Task<GetProductVariantMediaDto> GetProductVariantMediaDataById(ProductDataRequestDto Input)
        {

            var Imagesparameter = new SqlParameter[] { };
            var ProductData = new GetProductVariantMediaDto();


            var OtherMediaImageId = _repositoryProductMediaImageTypeMaster.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "othermediatype").FirstOrDefault();
            var LifeStyleImagesId = _repositoryProductMediaImageTypeMaster.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "lifestyleimages").FirstOrDefault();
            var LineArtId = _repositoryProductMediaImageTypeMaster.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "lineart").FirstOrDefault();


            try
            {
                await _connectionUtility.EnsureConnectionOpenAsync();

                IEnumerable<ProductImageType> ProductMediaImages = await _db.QueryAsync<ProductImageType>(@"select images.ImageName as FileName,images.ProductMediaImageTypeId as MediaType, images.ImageUrl as ImagePath,images.IsDefaultImage, images.Ext, images.[Name],images.Size,
	                        images.[Type],images.Id, images.[Url]  from ProductMediaImages as images
	                        where images.ProductId = " + Input.ProductId + " and images.TenantId = " + AbpSession.TenantId.Value + " and images.IsDeleted=0", new
                {
                    ProductId = Input.ProductId
                }, commandType: System.Data.CommandType.Text);

                IEnumerable<ProductVariantValueDto> ProductVariantData = await _db.QueryAsync<ProductVariantValueDto>("usp_GetFrontendVariantDataByProdId", new
                {
                    ProductId = Input.ProductId,
                    TenantId = AbpSession.TenantId.Value
                }, commandType: System.Data.CommandType.StoredProcedure);


                IEnumerable<ProductBrandingPositionDto> ProductBrandingPosition = await _db.QueryAsync<ProductBrandingPositionDto>(@"select method.Id, method.LayerTitle, method.PostionMaxwidth as PositionMaxwidth, method.PostionMaxHeight as PositionMaxHeight, method.BrandingLocationNote,
method.ImageName, method.ImageFileURL, method.UnitOfMeasureId, method.Ext, method.Size, method.Type, method.Name, method.Url,
(select top 1 ProductSizeName from ProductSizeMaster as masters where masters.Id = method.UnitOfMeasureId) as UnitOfMeasure
from ProductBrandingPosition as method where method.ProductId = " + Input.ProductId + " and method.TenantId = " + AbpSession.TenantId.Value + " and method.IsDeleted=0", new
                {
                    ProductId = Input.ProductId
                }, commandType: System.Data.CommandType.Text);


                ProductData.ProductBrandingPosition = (from position in ProductBrandingPosition
                                                       select new ProductBrandingPositionDto
                                                       {
                                                           Id = position.Id,
                                                           LayerTitle = position.LayerTitle,
                                                           PositionMaxwidth = position.PositionMaxwidth,
                                                           PositionMaxHeight = position.PositionMaxHeight,
                                                           BrandingLocationNote = position.BrandingLocationNote,
                                                           UnitOfMeasureId = position.UnitOfMeasureId,
                                                           UnitOfMeasure = position.UnitOfMeasure,
                                                           ImageFileURL = position.ImageFileURL,
                                                           ImageName = position.ImageName,
                                                           ImageObj = new ProductImageType
                                                           {
                                                               FileName = position.ImageName,
                                                               Url = position.Url,
                                                               Ext = position.Ext,
                                                               Name = position.Name,
                                                               Type = position.Type,
                                                               Size = position.Size
                                                           }
                                                       }).ToList();


                //ProductBrandingPosition.ToList();


                ProductData.LineArtMediaImages = ProductMediaImages.Where(x => x.MediaType == LineArtId.Id).ToList();
                ProductData.LifeStyleMediaImages = ProductMediaImages.Where(x => x.MediaType == LifeStyleImagesId.Id).ToList();
                ProductData.OtherMediaImages = ProductMediaImages.Where(x => x.MediaType == OtherMediaImageId.Id).ToList();
                ProductData.ProductVariantValues = ProductVariantData.ToList();
            }
            catch (Exception ex)
            {

            }

            return ProductData;

        }



        #endregion

        private static async Task<string> GetSizeVariantValue(long SizeId, List<ProductVariantOptionValues> ValuesList)
        {
            string SizeValue = string.Empty;
            var IsSizeExists = ValuesList.Where(i => i.VariantOptionId == SizeId).FirstOrDefault();
            if (IsSizeExists != null)
            {
                SizeValue = IsSizeExists.VariantOptionValue;
            }
            return SizeValue;
        }

        private static async Task<string> GetColorVariantValue(long ColorId, List<ProductVariantOptionValues> ValuesList)
        {
            string ColorValue = string.Empty;
            var IsSizeExists = ValuesList.Where(i => i.VariantOptionId == ColorId).FirstOrDefault();
            if (IsSizeExists != null)
            {
                ColorValue = IsSizeExists.VariantOptionValue;
            }
            return ColorValue;
        }

        public async Task<ProductVariantValueDto> GetProductVariantById(long Id)
        {
            int TenantId = AbpSession.TenantId.Value;
            ProductVariantValueDto Response = new ProductVariantValueDto();
            try
            {
                var VariantsData = _productVariantDataRepository.GetAllList(i => i.Id == Id).FirstOrDefault();

                if (VariantsData != null)
                {

                    await _connectionUtility.EnsureConnectionOpenAsync();

                    IEnumerable<ProductVariantWarehouseDto> ProductVariantWarehouse = await _db.QueryAsync<ProductVariantWarehouseDto>(@"select warehouse.Id,(SELECT top 1  WarehouseTitle FROM WareHouseMaster where  id = WarehouseId ) as WarehouseTitle,
                     warehouse.WarehouseId, warehouse.LocationA, warehouse.LocationB,warehouse.LocationC,warehouse.QuantityThisLocation, warehouse.StockAlertQuantity from ProductVariantWarehouse warehouse where IsDeleted = 0 and ProductVariantId = " + Id + "", new
                    {

                    }, commandType: System.Data.CommandType.Text);


                    IEnumerable<ProductVariantImagesDto> ProductVariantImages = await _db.QueryAsync<ProductVariantImagesDto>(@"SELECT Id,ProductVariantId,ImageFileName as FileName,ImageURL,Ext,Name,Size,Type, ImageURL as Url
                                                                                      FROM [dbo].[ProductVariantdataImages] where IsDeleted = 0 and ProductVariantId = " + Id + "", new
                    {

                    }, commandType: System.Data.CommandType.Text);

                    //ProductVolumeDiscountVariant

                    IEnumerable<ProductVariantQuantityPricesDto> ProductQuantityPrices = await _db.QueryAsync<ProductVariantQuantityPricesDto>(@"SELECT Id,ProductVariantId,Price,QuantityFrom,IsActive
                                                                                      FROM [dbo].[ProductVariantQuantityPrices] where IsDeleted = 0 and ProductVariantId = " + Id + "", new
                    {

                    }, commandType: System.Data.CommandType.Text);


                    string[] splitVals = VariantsData.VariantMasterIds.Split('/');
                    string[] splitVariant = VariantsData.Variant.Split('/');
                    VariantModel CheckToBeExists = new VariantModel();

                    if (!string.IsNullOrEmpty(VariantsData.Variant))
                    {
                        int? Index;

                        var IsSizeExists = splitVals.Where(i => i == "1").FirstOrDefault();
                        if (IsSizeExists != null)
                        {
                            int pos = Array.IndexOf(splitVals, "1");
                            if (pos > -1)
                            {
                                Index = pos;
                                Response.Size = splitVariant[Index.Value];

                            }
                        }
                        int IsMaterialExists = Array.IndexOf(splitVals, "2");
                        if (IsMaterialExists > -1)
                        {
                            Index = IsMaterialExists;
                            Response.Material = splitVariant[Index.Value];

                        }

                        int position = Array.IndexOf(splitVals, "3");
                        if (position > -1)
                        {
                            Index = position;
                            Response.Color = splitVariant[Index.Value];

                        }
                        int IsStyleExists = Array.IndexOf(splitVals, "4");
                        if (IsStyleExists > -1)
                        {
                            Index = IsStyleExists;
                            Response.Style = splitVariant[Index.Value];

                        }
                    }
                    Response.ProductId = VariantsData.ProductId;
                    Response.VariantSKU = VariantsData.SKU;
                    Response.Shape = VariantsData.Shape;
                    Response.VariantBarCode = VariantsData.BarCode;
                    Response.VariantPrice = VariantsData.Price.ToString();
                    Response.VariantQuantity = VariantsData.QuantityStockUnit;
                    Response.IsTrackQuantity = VariantsData.IsTrackQuantity;
                    Response.QuantityStockUnit = VariantsData.QuantityStockUnit;
                    Response.Variant = VariantsData.Variant;
                    Response.Margin = VariantsData.Margin;
                    Response.NextShipment = VariantsData.NextShipment;
                    Response.IncomingQuantity = VariantsData.IncomingQuantity;
                    Response.IsVariantHasMixAndMatch = VariantsData.IsVariantHasMixAndMatch;

                    Response.VariantPriceModel = new VariantPriceModel
                    {

                        ComparePrice = VariantsData.ComparePrice.ToString(),
                        Profit = VariantsData.Profit,
                        CostPerItem = VariantsData.CostPerItem.ToString(),
                        OnSale = VariantsData.OnSale,
                        SaleUnitPrice = VariantsData.SaleUnitPrice,
                        SalePrice = VariantsData.SalePrice,
                        DiscountPercentage = VariantsData.DiscountPercentage,
                        DiscountPercentageDraft = VariantsData.DiscountPercentageDraft
                    };

                    Response.ProductVariantWarehouse = ProductVariantWarehouse.ToList();
                    Response.ProductVariantImages = ProductVariantImages.ToList();
                    Response.ProductQuantityPrices = ProductQuantityPrices.ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return Response;
        }


        public async Task CreateProductVariant(ProductVariantValueDto Model)
        {
            try
            {
                long productVariantsDataId = 0;
                if (Model != null)
                {

                    long SizeId = _productOptionRepository.GetAllList(i => i.OptionName.ToLower() == "size").Select(i => i.Id).FirstOrDefault();
                    long ColorId = _productOptionRepository.GetAllList(i => i.OptionName.ToLower() == "color").Select(i => i.Id).FirstOrDefault();
                    long MaterialId = _productOptionRepository.GetAllList(i => i.OptionName.ToLower() == "material").Select(i => i.Id).FirstOrDefault();
                    long StyleId = _productOptionRepository.GetAllList(i => i.OptionName.ToLower() == "style").Select(i => i.Id).FirstOrDefault();
                    long CompartmentId = _productOptionRepository.GetAllList(i => i.OptionName.ToLower() == "compartment").Select(i => i.Id).FirstOrDefault();


                    var IsVariantExists = (from variants in _productVariantDataRepository.GetAll()
                                           join option in _productVariantOptionValuesRepository.GetAll() on variants.Id equals option.ProductVariantId
                                           where variants.ProductId == Model.ProductId
                                           select option).ToList();


                    string Variant = string.Empty;
                    string VariantMasterIds = string.Empty;
                    bool IsSizeExists = false;
                    bool IsMaterialExists = false;
                    bool IsColorExists = false;
                    bool IsStyleExists = false;
                    bool IsCompartmentExists = false;

                    var Compartment = _repository.GetAllList(i => i.Id == Model.ProductId).FirstOrDefault();
                    if (!string.IsNullOrEmpty(Compartment.CompartmentBuilderTitle) && Compartment.IsProductHasCompartmentBuilder == true)
                    {
                        Variant = Compartment.CompartmentBuilderTitle;
                        VariantMasterIds = CompartmentId.ToString();
                        IsCompartmentExists = true;
                    }
                    if (!string.IsNullOrEmpty(Model.Color))
                    {
                        //Variant = Model.Color;
                        //VariantMasterIds = ColorId.ToString();
                        Variant = string.IsNullOrEmpty(Variant) ? Model.Color : Variant + "/" + Model.Color;
                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? ColorId.ToString() : VariantMasterIds + "/" + ColorId;
                        IsColorExists = true;
                    }
                    if (!string.IsNullOrEmpty(Model.Size))
                    {
                        Variant = string.IsNullOrEmpty(Variant) ? Model.Size : Variant + "/" + Model.Size;
                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? SizeId.ToString() : VariantMasterIds + "/" + SizeId;
                        IsSizeExists = true;
                    }
                    if (!string.IsNullOrEmpty(Model.Material))
                    {
                        Variant = string.IsNullOrEmpty(Variant) ? Model.Material : Variant + "/" + Model.Material;
                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? MaterialId.ToString() : VariantMasterIds + "/" + MaterialId;
                        IsMaterialExists = true;
                    }
                    if (!string.IsNullOrEmpty(Model.Style))
                    {
                        Variant = string.IsNullOrEmpty(Variant) ? Model.Style : Variant + "/" + Model.Style;
                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? StyleId.ToString() : VariantMasterIds + "/" + StyleId;
                        IsStyleExists = true;
                    }

                    ProductVariantsData variant = new ProductVariantsData();
                    variant.ProductId = Model.ProductId;
                    variant.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    variant.VariantMasterIds = VariantMasterIds;
                    variant.Variant = Variant;
                    variant.QuantityStockUnit = Model.QuantityStockUnit;
                    variant.Price = Model.VariantPrice == "" ? 0 : Convert.ToDecimal(Model.VariantPrice);

                    variant.Margin = Model.Margin;
                    variant.ProfitCurrencySymbol = Model.ProfitCurrencySymbol == "" ? 0 : Convert.ToInt32(Model.ProfitCurrencySymbol);
                    variant.IsChargeTaxVariant = Model.IsChargeTaxVariant;
                    variant.Shape = Model.Shape;

                    variant.SKU = Model.VariantSKU;
                    variant.BarCode = Model.VariantBarCode;
                    variant.IsTrackQuantity = Model.IsTrackQuantity;
                    variant.IsActive = Model.IsActive;
                    variant.NextShipment = Model.NextShipment;
                    variant.IncomingQuantity = Model.IncomingQuantity;
                    variant.IsMultiColorVariant = Model.IsMultiColorVariant;

                    if (Model.VariantPriceModel != null)
                    {
                        variant.ComparePrice = Model.VariantPriceModel.ComparePrice == "" ? 0 : Convert.ToDecimal(Model.VariantPriceModel.ComparePrice);
                        variant.CostPerItem = Model.VariantPriceModel.CostPerItem == "" ? 0 : Convert.ToDecimal(Model.VariantPriceModel.CostPerItem);
                        variant.Profit = Model.VariantPriceModel.Profit;
                        variant.SaleUnitPrice = Convert.ToDecimal(Model.VariantPriceModel.SaleUnitPrice); 
                        variant.SalePrice = Convert.ToDecimal(Model.VariantPriceModel.SalePrice);
                        variant.OnSale = Model.VariantPriceModel.OnSale;
                        variant.DiscountPercentage = Model.VariantPriceModel.DiscountPercentage;
                        variant.DiscountPercentageDraft = Model.VariantPriceModel.DiscountPercentageDraft;
                    }
                    productVariantsDataId = await _productVariantDataRepository.InsertAndGetIdAsync(variant);

                    if (!string.IsNullOrEmpty(VariantMasterIds))
                    {
                        string[] VariantIds = VariantMasterIds.Split('/');
                        string[] VariantValues = Variant.Split('/');

                        for (int i = 0; i < VariantIds.Length; i++)
                        {
                            ProductVariantOptionValues ModelVariantOptionValues = new ProductVariantOptionValues();
                            ModelVariantOptionValues.ProductVariantId = productVariantsDataId;
                            ModelVariantOptionValues.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            //changes requested by kamlesh 
                            //ModelVariantOptionValues.VariantOptionId = Convert.ToInt32(VariantIds[i]);
                            ModelVariantOptionValues.VariantOptionId = Convert.ToInt32(VariantIds[i]) % (10);
                            //product.IsHasSubProducts == true ? product.NumberOfSubProducts : 0,

                            ModelVariantOptionValues.VariantOptionValue = VariantValues[i].ToString();
                            long ModelVariantOptionValuesId = await _productVariantOptionValuesRepository.InsertAndGetIdAsync(ModelVariantOptionValues);
                        }
                    }

                    if (Model.ProductVariantWarehouse.Count > 0)
                    {
                        foreach (var ProductStockLocationitem in Model.ProductVariantWarehouse)
                        {
                            ProductVariantWarehouse productStockLocation = new ProductVariantWarehouse();

                            if (productStockLocation != null)
                            {
                                ProductVariantWarehouse productStockLocationModel = new ProductVariantWarehouse();
                                productStockLocationModel.ProductVariantId = productVariantsDataId;
                                productStockLocationModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                productStockLocationModel.WarehouseId = ProductStockLocationitem.WarehouseId;
                                productStockLocationModel.LocationA = ProductStockLocationitem.LocationA;
                                productStockLocationModel.LocationB = ProductStockLocationitem.LocationB;
                                productStockLocationModel.LocationC = ProductStockLocationitem.LocationC;
                                productStockLocationModel.StockAlertQuantity = string.IsNullOrEmpty(ProductStockLocationitem.StockAlertQuantity) ? 0 : Convert.ToInt32(ProductStockLocationitem.StockAlertQuantity);
                                productStockLocationModel.QuantityThisLocation = string.IsNullOrEmpty(ProductStockLocationitem.QuantityThisLocation) ? 0 : Convert.ToInt32(ProductStockLocationitem.QuantityThisLocation);
                                productStockLocationModel.IsActive = true;
                                long productStockLocationId = await _productVariantWarehouseRepository.InsertAndGetIdAsync(productStockLocationModel);
                            }
                        }
                    }

                    if (Model.ProductVariantImages != null)
                    {
                        int TenantId = AbpSession.TenantId.Value;
                        var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
                        string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";

                        foreach (var imagename in Model.ProductVariantImages)
                        {
                            if (imagename.Name != null && imagename.Name != "")
                            {

                                ProductVariantdataImages image = new ProductVariantdataImages();
                                string ImageLocation = AzureStorageUrl + folderPath + imagename.Name;
                                image.ImageFileName = imagename.Name;
                                image.ProductId = Model.ProductId;
                                image.ProductVariantId = productVariantsDataId;
                                image.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                image.ImageURL = ImageLocation;
                                image.Ext = imagename.Ext;
                                image.Size = imagename.Size;
                                image.Type = imagename.Type;
                                image.Name = imagename.Name;
                                long Id = await _productVariantdataImagesRepository.InsertAndGetIdAsync(image);
                            }
                        }
                    }

                    if (Model.ProductQuantityPrices != null)
                    {
                        #region ProductVolumeVariations

                        List<ProductVariantQuantityPrices> productPriceQtyList = ObjectMapper.Map<List<ProductVariantQuantityPrices>>(Model.ProductQuantityPrices);
                        foreach (var volumeDiscountVariantitem in productPriceQtyList)
                        {
                            volumeDiscountVariantitem.ProductVariantId = productVariantsDataId;
                            volumeDiscountVariantitem.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            long ProductVolumeDiscountVariantId = await _productVariantQuantityPricesRepository.InsertAndGetIdAsync(volumeDiscountVariantitem);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task CreateCompartmentData(CompartmentVariantDataDto Model)
        {
            try
            {
                int TenantId = AbpSession.TenantId.Value;
                var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
                string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
                if (Model != null)
                {

                    foreach (var data in Model.VarientList)
                    {
                        CompartmentVariantData variant = new CompartmentVariantData();
                        variant.CompartmentTitle = Model.CompartmentTitle;
                        variant.CompartmentSubTitle = Model.CompartmentSubTitle;
                        variant.ProductId = Model.ProductId;
                        variant.ProductVarientId = data.Id;
                        variant.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        var varientList = _productVariantDataRepository.GetAllList(i => i.Id == data.Id).FirstOrDefault();
                        variant.Compartment = varientList.Variant;
                        variant.SKU = varientList.SKU;
                        variant.Price = varientList.Price;
                        if (data.ImageObj != null)
                        {
                            string ImageLocation = AzureStorageUrl + folderPath + data.ImageObj.FileName;
                            variant.Name = data.ImageObj.FileName;
                            variant.Url = ImageLocation;
                            variant.ImageFileName = data.ImageObj.FileName;
                            variant.Ext = data.ImageObj.Ext;
                            variant.Size = data.ImageObj.Size;
                            variant.Type = data.ImageObj.Type;
                        }
                        var productcompartmentDataId = await _productCompartmentDataRepository.InsertAndGetIdAsync(variant);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task UpdateProductVariantById(ProductVariantValueDto Model)
        {
            try
            {
                var VariantsData = _productVariantDataRepository.GetAllList(i => i.Id == Model.Id).FirstOrDefault();
                if (VariantsData != null)
                {
                    VariantsData.Price = string.IsNullOrEmpty(Model.VariantPrice) ? 0 : Convert.ToDecimal(Model.VariantPrice);
                    VariantsData.QuantityStockUnit = Model.QuantityStockUnit;
                    VariantsData.IsTrackQuantity = Model.IsTrackQuantity;
                    VariantsData.SKU = Model.VariantSKU;
                    VariantsData.BarCode = Model.VariantBarCode;
                    VariantsData.Margin = Model.Margin;

                    VariantsData.Shape = Model.Shape;
                    VariantsData.IsVariantHasMixAndMatch = Model.IsVariantHasMixAndMatch;
                    VariantsData.IsChargeTaxVariant = Model.IsChargeTaxVariant;
                    VariantsData.NextShipment = Model.NextShipment;
                    VariantsData.IncomingQuantity = Model.IncomingQuantity;

                    if (Model.VariantPriceModel != null)
                    {
                        VariantsData.ComparePrice = string.IsNullOrEmpty(Model.VariantPriceModel.ComparePrice) ? 0 : Convert.ToDecimal(Model.VariantPriceModel.ComparePrice);
                        VariantsData.CostPerItem = string.IsNullOrEmpty(Model.VariantPriceModel.CostPerItem) ? 0 : Convert.ToDecimal(Model.VariantPriceModel.CostPerItem);
                        VariantsData.Profit = Model.VariantPriceModel.Profit;
                        VariantsData.SaleUnitPrice = Convert.ToDecimal(Model.VariantPriceModel.SaleUnitPrice);
                        
                        VariantsData.SalePrice = Convert.ToDecimal(Model.VariantPriceModel.SalePrice);
                        
                        VariantsData.OnSale = Model.VariantPriceModel.OnSale;
                        VariantsData.DiscountPercentage = Model.VariantPriceModel.DiscountPercentage;
                        VariantsData.DiscountPercentageDraft = Model.VariantPriceModel.DiscountPercentageDraft;
                    }

                    //----------------------------------------------Update variant combination
                    var MasterOptions = _productOptionRepository.GetAll();
                    var ColorId = MasterOptions.Where(i => i.OptionName.ToLower() == "color").Select(i => i.Id).FirstOrDefault();
                    var SizeId = MasterOptions.Where(i => i.OptionName.ToLower() == "size").Select(i => i.Id).FirstOrDefault();
                    var MaterialId = MasterOptions.Where(i => i.OptionName.ToLower() == "material").Select(i => i.Id).FirstOrDefault();
                    var StyleId = MasterOptions.Where(i => i.OptionName.ToLower() == "style").Select(i => i.Id).FirstOrDefault();
                    long CompartmentId = _productOptionRepository.GetAllList(i => i.OptionName.ToLower() == "compartment").Select(i => i.Id).FirstOrDefault();

                    string Variant = string.Empty;
                    string VariantMasterIds = string.Empty;

                    var Compartment = _repository.GetAllList(i => i.Id == Model.ProductId).FirstOrDefault();
                    if (Compartment != null)
                    {
                        if (!string.IsNullOrEmpty(Compartment.CompartmentBuilderTitle) && Compartment.IsProductHasCompartmentBuilder == true)
                        {
                            Variant = Compartment.CompartmentBuilderTitle;
                            VariantMasterIds = CompartmentId.ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(Model.Color))
                    {
                        //Variant = Model.Color;
                        //VariantMasterIds = ColorId.ToString();
                        Variant = string.IsNullOrEmpty(Variant) ? Model.Color : Variant + "/" + Model.Color;
                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? ColorId.ToString() : VariantMasterIds + "/" + ColorId;
                    }
                    if (!string.IsNullOrEmpty(Model.Size))
                    {
                        Variant = string.IsNullOrEmpty(Variant) ? Model.Size : Variant + "/" + Model.Size;
                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? SizeId.ToString() : VariantMasterIds + "/" + SizeId;
                    }
                    if (!string.IsNullOrEmpty(Model.Material))
                    {
                        Variant = string.IsNullOrEmpty(Variant) ? Model.Material : Variant + "/" + Model.Material;
                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? MaterialId.ToString() : VariantMasterIds + "/" + MaterialId;
                    }
                    if (!string.IsNullOrEmpty(Model.Style))
                    {
                        Variant = string.IsNullOrEmpty(Variant) ? Model.Style : Variant + "/" + Model.Style;
                        VariantMasterIds = string.IsNullOrEmpty(VariantMasterIds) ? StyleId.ToString() : VariantMasterIds + "/" + StyleId;
                    }

                    if (!string.IsNullOrEmpty(VariantMasterIds) && !string.IsNullOrEmpty(Variant))
                    {
                        VariantsData.VariantMasterIds = VariantMasterIds;
                        VariantsData.Variant = Variant;
                    }

                    await _productVariantDataRepository.UpdateAsync(VariantsData);

                    //----------------update variant option values
                    if (!string.IsNullOrEmpty(VariantMasterIds) && !string.IsNullOrEmpty(Variant))
                    {
                        string[] VariantIds = VariantMasterIds.Split('/');
                        string[] VariantValues = Variant.Split('/');

                        for (int i = 0; i < VariantIds.Length; i++)
                        {
                            // area for ProductVariantOptionValues
                            if (VariantIds[i] != "" && VariantValues[i] != "")
                            {
                                long VariantId = Convert.ToInt64(VariantIds[i]);
                                var isExists = _productVariantOptionValuesRepository.GetAllList(i => i.ProductVariantId == Model.Id && i.VariantOptionId == VariantId).FirstOrDefault();

                                if (isExists == null)
                                {
                                    ProductVariantOptionValues ModelVariantOptionValues = new ProductVariantOptionValues();
                                    ModelVariantOptionValues.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    ModelVariantOptionValues.ProductVariantId = Model.Id;
                                    //changes reqested by kamlesh sir
                                    //ModelVariantOptionValues.VariantOptionId = Convert.ToInt64(VariantIds[i]);
                                    ModelVariantOptionValues.VariantOptionId = Convert.ToInt64(VariantIds[i]) % (10);
                                    ModelVariantOptionValues.VariantOptionValue = VariantValues[i].ToString();
                                    await _productVariantOptionValuesRepository.InsertAsync(ModelVariantOptionValues);
                                }
                                else
                                {
                                    isExists.VariantOptionValue = VariantValues[i].ToString();
                                    await _productVariantOptionValuesRepository.UpdateAsync(isExists);
                                }
                            }
                            //------------------------------------------
                        }

                    }
                    //----------------update variant option values ends

                    if (Model.ProductVariantWarehouse.Count > 0)
                    {
                        var ExistingData = _productVariantWarehouseRepository.GetAllList(i => i.ProductVariantId == VariantsData.Id);
                        var OldData = Model.ProductVariantWarehouse.Where(i => i.Id > 0);
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteProductVariantStockDataAsync(DeletedData, AbpSession.TenantId.Value);
                        }

                        foreach (var ProductStockLocationitem in Model.ProductVariantWarehouse)
                        {
                            if (ProductStockLocationitem.Id > 0)
                            {
                                var productStockLocation = _productVariantWarehouseRepository.GetAllList(x => x.Id == ProductStockLocationitem.Id).FirstOrDefault();
                                if (productStockLocation != null)
                                {
                                    productStockLocation.WarehouseId = ProductStockLocationitem.WarehouseId;
                                    productStockLocation.LocationA = ProductStockLocationitem.LocationA;
                                    productStockLocation.LocationB = ProductStockLocationitem.LocationB;
                                    productStockLocation.LocationC = ProductStockLocationitem.LocationC;
                                    productStockLocation.StockAlertQuantity = string.IsNullOrEmpty(ProductStockLocationitem.StockAlertQuantity) ? 0 : Convert.ToInt32(ProductStockLocationitem.StockAlertQuantity);
                                    productStockLocation.QuantityThisLocation = string.IsNullOrEmpty(ProductStockLocationitem.QuantityThisLocation) ? 0 : Convert.ToInt32(ProductStockLocationitem.QuantityThisLocation);
                                    productStockLocation.IsActive = true;
                                    await _productVariantWarehouseRepository.UpdateAsync(productStockLocation);
                                }
                            }
                            else
                            {
                                ProductVariantWarehouse productStockLocationModel = new ProductVariantWarehouse();
                                productStockLocationModel.ProductVariantId = VariantsData.Id;
                                productStockLocationModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                productStockLocationModel.WarehouseId = ProductStockLocationitem.WarehouseId;
                                productStockLocationModel.LocationA = ProductStockLocationitem.LocationA;
                                productStockLocationModel.LocationB = ProductStockLocationitem.LocationB;
                                productStockLocationModel.LocationC = ProductStockLocationitem.LocationC;
                                productStockLocationModel.StockAlertQuantity = string.IsNullOrEmpty(ProductStockLocationitem.StockAlertQuantity) ? 0 : Convert.ToInt32(ProductStockLocationitem.StockAlertQuantity);
                                productStockLocationModel.QuantityThisLocation = string.IsNullOrEmpty(ProductStockLocationitem.QuantityThisLocation) ? 0 : Convert.ToInt32(ProductStockLocationitem.QuantityThisLocation);
                                productStockLocationModel.IsActive = true;
                                long productStockLocationId = await _productVariantWarehouseRepository.InsertAndGetIdAsync(productStockLocationModel);

                            }
                        }
                    }
                    else
                    {
                        var ExistingData = _productVariantWarehouseRepository.GetAllList(i => i.ProductVariantId == VariantsData.Id).ToList();

                        if (ExistingData.Count > 0)
                        {
                            await DeleteProductVariantStockDataAsync(ExistingData, AbpSession.TenantId.Value);
                        }
                    }

                    if (Model.ProductVariantImages.Count > 0 && Model.ProductVariantImages != null)
                    {

                        var ExistingData = _productVariantdataImagesRepository.GetAllList(i => i.ProductVariantId == VariantsData.Id).FirstOrDefault();
                        int TenantId = AbpSession.TenantId.Value;
                        var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
                        string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
                        if (ExistingData != null)
                        {
                            if(ExistingData.ImageFileName != Model.ProductVariantImages.Select(i => i.Name).FirstOrDefault())
                            {
                                //update
                                string ImageLocation = AzureStorageUrl + folderPath + Model.ProductVariantImages.Select(i => i.Name).FirstOrDefault();
                                ExistingData.ImageFileName = Model.ProductVariantImages.Select(i => i.Name).FirstOrDefault();
                                ExistingData.ProductId = VariantsData.ProductId;
                                ExistingData.ProductVariantId = VariantsData.Id;
                                ExistingData.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                ExistingData.ImageURL = ImageLocation;
                                ExistingData.Ext = Model.ProductVariantImages.Select(i => i.Ext).FirstOrDefault();
                                ExistingData.Size = Model.ProductVariantImages.Select(i => i.Size).FirstOrDefault();
                                ExistingData.Type = Model.ProductVariantImages.Select(i => i.Type).FirstOrDefault();
                                ExistingData.Name = Model.ProductVariantImages.Select(i => i.Name).FirstOrDefault();
                                await _productVariantdataImagesRepository.UpdateAsync(ExistingData);
                            }
                        }
                        else
                        {   
                           // insert
                            if (Model.ProductVariantImages.Select(i=>i.Name).FirstOrDefault() != null && Model.ProductVariantImages.Select(i => i.Name).FirstOrDefault() != "")
                            {
                                ProductVariantdataImages image = new ProductVariantdataImages();
                                string ImageLocation = AzureStorageUrl + folderPath + Model.ProductVariantImages.Select(i => i.Name).FirstOrDefault();
                                image.ImageFileName = Model.ProductVariantImages.Select(i => i.Name).FirstOrDefault();
                                image.ProductId = VariantsData.ProductId;
                                image.ProductVariantId = VariantsData.Id;
                                image.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                image.ImageURL = ImageLocation;
                                image.Ext = Model.ProductVariantImages.Select(i => i.Ext).FirstOrDefault();
                                image.Size = Model.ProductVariantImages.Select(i => i.Size).FirstOrDefault();
                                image.Type = Model.ProductVariantImages.Select(i => i.Type).FirstOrDefault();
                                image.Name = Model.ProductVariantImages.Select(i => i.Name).FirstOrDefault();
                                long Id = await _productVariantdataImagesRepository.InsertAndGetIdAsync(image);
                            }
                        }
                        #region commented code for multiple image upload
                        //int TenantId = AbpSession.TenantId.Value;
                        //var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
                        //string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
                        //var ExistingData = _productVariantdataImagesRepository.GetAllList(i => i.ProductVariantId == VariantsData.Id);
                        //var OldData = Model.ProductVariantImages.Where(i => i.Id > 0);
                        //var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();

                        //if (DeletedData.Count > 0)
                        //{
                        //    await DeleteVariantProductImagesAsync(DeletedData, AbpSession.TenantId.Value);
                        //}

                        //foreach (var imagename in Model.ProductVariantImages)
                        //{
                        //    if (imagename.Name != null && imagename.Name != "")
                        //    {

                        //        ProductVariantdataImages image = new ProductVariantdataImages();
                        //        string ImageLocation = AzureStorageUrl + folderPath + imagename.Name;
                        //        image.ImageFileName = imagename.Name;
                        //        image.ProductId = VariantsData.ProductId;
                        //        image.ProductVariantId = VariantsData.Id;
                        //        image.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        //        image.ImageURL = ImageLocation;
                        //        image.Ext = imagename.Ext;
                        //        image.Size = imagename.Size;
                        //        image.Type = imagename.Type;
                        //        image.Name = imagename.Name;
                        //        long Id = await _productVariantdataImagesRepository.InsertAndGetIdAsync(image);
                        //    }
                        //}
                        #endregion
                    }
                    //else
                    //{
                    //    var ExistingData = _productVariantdataImagesRepository.GetAllList(i => i.ProductVariantId == VariantsData.Id).ToList();

                    //    if (ExistingData.Count > 0)
                    //    {
                    //        await DeleteVariantProductImagesAsync(ExistingData, AbpSession.TenantId.Value);
                    //    }
                    //}

                    //ProductVolumeDiscountVariant               
                    if (Model.ProductQuantityPrices != null)
                    {
                        var ExistingData = _productVariantQuantityPricesRepository.GetAllList(i => i.ProductVariantId == Model.Id);
                        var OldData = Model.ProductQuantityPrices.Where(i => i.Id > 0);
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeletePriceVariantsAsync(DeletedData, AbpSession.TenantId.Value);
                        }

                        foreach (var item in Model.ProductQuantityPrices)
                        {
                            if (item.Id > 0)
                            {
                                var productVolumeDiscountVariant = _productVariantQuantityPricesRepository.GetAllList(x => x.Id == item.Id).FirstOrDefault();
                                if (productVolumeDiscountVariant != null)
                                {
                                    productVolumeDiscountVariant.QuantityFrom = item.QuantityFrom;
                                    productVolumeDiscountVariant.Price = item.Price;
                                    productVolumeDiscountVariant.IsActive = item.IsActive;
                                    await _productVariantQuantityPricesRepository.UpdateAsync(productVolumeDiscountVariant);
                                }
                            }
                            else
                            {
                                ProductVariantQuantityPrices PriceModel = new ProductVariantQuantityPrices();
                                PriceModel.QuantityFrom = item.QuantityFrom;
                                PriceModel.Price = item.Price;
                                PriceModel.IsActive = item.IsActive;
                                PriceModel.ProductVariantId = PriceModel.Id;
                                PriceModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                var productVolumeDiscountVariantId = await _productVariantQuantityPricesRepository.InsertAndGetIdAsync(PriceModel);
                            }
                        }
                    }
                    else
                    {
                        var ExistingData = _productVariantQuantityPricesRepository.GetAllList(i => i.ProductVariantId == Model.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeletePriceVariantsAsync(ExistingData, AbpSession.TenantId.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        private static async Task<long[]> ConvertToLongArray(string Value)
        {
            long[] Response = new long[] { };
            try
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    string SeparatedValues = Value.Replace("'", "");
                    SeparatedValues = SeparatedValues.Trim();
                    Response = SeparatedValues.Split(',').Select(long.Parse).ToArray();
                }
            }
            catch (Exception ex)
            {

            }
            return Response;

        }

        public async Task<ProductMasterDto> UpdateProductById(GetProductDto Input)
        {
            int TenantId = AbpSession.TenantId.Value;
            string FolderName = _configuration["FileUpload:FolderName"];
            string CurrentWebsiteUrl = _configuration.GetValue<string>("FileUpload:WebsireDomainPath");
            ProductMasterDto output = new ProductMasterDto();
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";

            try
            {
                var productMaster = _repository.GetAllList(x => x.Id == Input.Id).FirstOrDefault();
                if (productMaster != null)
                {
                    //var IsTitleExists = _repository.GetAllList(i => i.ProductTitle.ToLower().Trim() == Input.ProductTitle.ToLower().Trim() && i.Id != Input.Id && i.ProductSKU != ).FirstOrDefault();
                    //if (IsTitleExists != null)
                    //{
                    //    output.ErrorMessage = "Product title already exists, please enter unique title!";
                    //    output.IsValidate = true;
                    //    return output;
                    //}


                    var OtherMediaImageId = _repositoryProductMediaImageTypeMaster.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "othermediatype").FirstOrDefault();
                    var LifeStyleImagesId = _repositoryProductMediaImageTypeMaster.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "lifestyleimages").FirstOrDefault();
                    var LineArtId = _repositoryProductMediaImageTypeMaster.GetAllList(i => i.ProductMediaImageTypeName.ToLower().Trim() == "lineart").FirstOrDefault();



                    if (Input.IsProductCompartmentType)
                    {
                        var IsCompartmentTitleExists = _repository.GetAllList(i => i.CompartmentBuilderTitle.ToLower().Trim() == Input.CompartmentBuilderTitle.ToLower().Trim() && i.Id != Input.Id).FirstOrDefault();

                        if (IsCompartmentTitleExists != null)
                        {
                            output.ErrorMessage = "Compartment builder title already exists, please enter unique title!";
                            output.IsValidate = true;
                            return output;
                        }
                    }

                    productMaster.ProductSKU = Input.ProductSKU;
                    productMaster.ProductTitle = Input.ProductTitle;
                    productMaster.ShortDescripition = Input.ShortDescripition;
                    productMaster.ProductDescripition = Input.ProductDescripition;
                    productMaster.ProductNotes = Input.ProductNotes;
                    productMaster.IsActive = Input.IsActive;
                    productMaster.IsProductHasMultipleOptions = Input.IsProductHasMultipleOptions;
                    productMaster.IsPhysicalProduct = Input.IsPhysicalProduct;
                    productMaster.IsIndentOrder = Input.IsIndentOrder;
                    productMaster.IsPublished = Input.IsPublished;
                    productMaster.TurnAroundTimeId = Input.TurnAroundTimeId;
                    if (Input.IsHasSubProducts == true)
                    {
                        productMaster.IsHasSubProducts = true;
                        productMaster.NumberOfSubProducts = Input.NumberOfSubProducts;
                    }
                    else
                    {
                        productMaster.IsHasSubProducts = false;
                        productMaster.NumberOfSubProducts = 0;
                    }

                    productMaster.Profit = !string.IsNullOrEmpty(Input.ProductPrice.Profit) ? Convert.ToDecimal(Input.ProductPrice.Profit) : 0;
                    productMaster.UnitPrice = !string.IsNullOrEmpty(Input.ProductPrice.UnitPrice) ? Convert.ToDecimal(Input.ProductPrice.UnitPrice) : 0;
                    productMaster.CostPerItem = !string.IsNullOrEmpty(Input.ProductPrice.CostPerItem) ? Convert.ToDecimal(Input.ProductPrice.CostPerItem) : 0;
                    productMaster.MarginIncreaseOnSalePrice = !string.IsNullOrEmpty(Input.ProductPrice.MarginIncreaseOnSalePrice) ? Convert.ToDecimal(Input.ProductPrice.MarginIncreaseOnSalePrice) : 0;
                    productMaster.SalePrice = !string.IsNullOrEmpty(Input.ProductPrice.SalePrice) ? Convert.ToDecimal(Input.ProductPrice.SalePrice) : 0;
                    productMaster.OnSale = Input.ProductPrice.OnSale;
                    productMaster.UnitOfMeasure = !string.IsNullOrEmpty(Input.ProductPrice.UnitOfMeasure) ? Convert.ToInt32(Input.ProductPrice.UnitOfMeasure) : 0;
                    productMaster.MinimumOrderQuantity = !string.IsNullOrEmpty(Input.ProductPrice.MinimumOrderQuantity) ? Convert.ToInt32(Input.ProductPrice.MinimumOrderQuantity) : 0;
                    productMaster.DepositRequired = !string.IsNullOrEmpty(Input.ProductPrice.DepositRequired) ? Convert.ToDecimal(Input.ProductPrice.DepositRequired) : 0;
                    productMaster.ChargeTaxOnThis = Input.ProductPrice.ChargeTaxOnThis;
                    productMaster.ProductHasPriceVariant = Input.ProductPrice.ProductHasPriceVariant;
                    productMaster.DiscountPercentage = !string.IsNullOrEmpty(Input.ProductPrice.DiscountPercentage) ? Convert.ToDouble(Input.ProductPrice.DiscountPercentage) : 0;
                    productMaster.Features = Input.Features;
                    productMaster.DiscountPercentageDraft = !string.IsNullOrEmpty(Input.ProductPrice.DiscountPercentageDraft) ? Convert.ToDouble(Input.ProductPrice.DiscountPercentageDraft) : 0;
                    productMaster.IsProductHasCompartmentBuilder = Input.IsProductHasCompartmentBuilder;
                    productMaster.CompartmentBuilderTitle = Input.CompartmentBuilderTitle;
                    productMaster.IsProductIsCompartmentType = Input.IsProductCompartmentType;
                    await _repository.UpdateAsync(productMaster);

                    // ProductDetail
                    if (Input.ProductDetail != null)
                    {
                        #region New Update Code For Insertion 
                        #region Brand Array Update
                        if (Input.ProductDetail.ProductBrandArray != null)
                        {
                            var ExistingData = _productAssignedBrandsRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            var OldData = Input.ProductDetail.ProductBrandArray.Where(i => i.AssignmentId > 0).ToList();
                            var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                            if (DeletedData.Count > 0)
                            {
                                await DeleteProductAssignedBrandsAsync(DeletedData, TenantId);
                            }

                            foreach (var item in Input.ProductDetail.ProductBrandArray)
                            {
                                if (item.AssignmentId > 0)
                                {
                                    var productAssignedBrands = _productAssignedBrandsRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                    if (productAssignedBrands != null)
                                    {
                                        productAssignedBrands.ProductBrandId = item.Id;
                                        await _productAssignedBrandsRepository.UpdateAsync(productAssignedBrands);
                                    }
                                }
                                else
                                {
                                    ProductAssignedBrands Model = new ProductAssignedBrands();
                                    Model.ProductBrandId = item.Id;
                                    Model.ProductId = Input.Id;
                                    Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    await _productAssignedBrandsRepository.InsertAsync(Model);
                                }

                            }
                        }
                        else
                        {
                            var ExistingData = _productAssignedBrandsRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                await DeleteProductAssignedBrandsAsync(ExistingData, TenantId);
                            }
                        }
                        #endregion

                        #region Collection Array Update
                        if (Input.ProductDetail.ProductCollectionArray != null)
                        {
                            var ExistingData = _productAssignedCollectionsRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            var OldData = Input.ProductDetail.ProductCollectionArray.Where(i => i.AssignmentId > 0).ToList();
                            var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                            if (DeletedData.Count > 0)
                            {
                                await DeleteProductAssignedCollectionsAsync(DeletedData, TenantId);
                            }

                            foreach (var item in Input.ProductDetail.ProductCollectionArray)
                            {
                                if (item.AssignmentId > 0)
                                {
                                    var productAssignedCollections = _productAssignedCollectionsRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                    if (productAssignedCollections != null)
                                    {
                                        productAssignedCollections.CollectionId = item.Id;
                                        await _productAssignedCollectionsRepository.UpdateAsync(productAssignedCollections);
                                    }
                                }
                                else
                                {
                                    ProductAssignedCollections Model = new ProductAssignedCollections();
                                    Model.CollectionId = item.Id;
                                    Model.ProductId = Input.Id;
                                    Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    await _productAssignedCollectionsRepository.InsertAsync(Model);
                                }

                            }
                        }
                        else
                        {
                            var ExistingData = _productAssignedCollectionsRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                await DeleteProductAssignedCollectionsAsync(ExistingData, TenantId);
                            }
                        }
                        #endregion

                        if (Input.ProductDetail.ProductCategories != null)
                        {

                            var ExistingData = _repositoryAssignedCategoryMaster.GetAllList(i => i.ProductId == Input.Id).ToList();
                            var OldData = Input.ProductDetail.ProductCategories.Where(i => i.AssignmentId > 0).ToList();
                            var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();
                            if (DeletedData.Count > 0)
                            {
                                // delete Category master assignment
                                await DeleteProductAssignedCategoryAsync(DeletedData, TenantId);

                                List<ProductAssignedSubCategories> SubCategories = new List<ProductAssignedSubCategories>();

                                // delete all related sub categories with master categories.
                                foreach(var data in DeletedData)
                                {
                                    var SubCategoriesData = _productAssignedSubCategoriesRepository.GetAllList(i => i.ProductId == Input.Id && i.CategoryId == data.CategoryId).ToList();
                                    if (SubCategoriesData.Count > 0)
                                    {
                                        await DeleteProductAssignedSubCategoryAsync(SubCategoriesData, TenantId);

                                    }
                                    SubCategories.AddRange(SubCategoriesData); 
                                }
                                // delete all related sub sub categories with sub categories.
                                if (SubCategories.Count > 0)
                                {
                                    foreach (var data in SubCategories)
                                    {
                                        var SubSubCategoriesData = _repositoryAssignedSubSubCategories.GetAllList(i => i.ProductId == Input.Id && i.SubCategoryId == data.SubCategoryId).ToList();
                                        if (SubSubCategoriesData.Count > 0)
                                        {
                                            await DeleteProductAssignedSubSubCategoryAsync(SubSubCategoriesData, TenantId);

                                        }
                                    }
                                }
                            }
                            foreach (var item in Input.ProductDetail.ProductCategories)
                            {

                                if (item.AssignmentId > 0)
                                {
                                    var productAssignedCategories = _repositoryAssignedCategoryMaster.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                    if (productAssignedCategories != null)

                                    {
                                        productAssignedCategories.CategoryId = item.CategoryId;
                                        await _repositoryAssignedCategoryMaster.UpdateAsync(productAssignedCategories);
                                    }


                                    var lst = Input.ProductDetail.ProductCategories.SelectMany(e => e.ProductSubCategories).ToList();
                                    var ExistingSubCategoryData = _productAssignedSubCategoriesRepository.GetAllList(i => i.ProductId == Input.Id && i.CategoryId == item.CategoryId).ToList();
                                    var OldSubCategoryData = lst.Where(i => i.AssignmentId > 0).ToList();
                                    var DeletedSubCategoriesData = ExistingSubCategoryData.Where(p => !OldSubCategoryData.Any(p2 => p2.AssignmentId == p.Id)).ToList();
                                    if (DeletedSubCategoriesData.Count > 0)
                                    {
                                        await DeleteProductAssignedSubCategoryAsync(DeletedSubCategoriesData, TenantId);

                                        // delete all related sub sub categories with sub categories.
                                        if (DeletedSubCategoriesData.Count > 0)
                                        {
                                            foreach (var data in DeletedSubCategoriesData)
                                            {
                                                var SubSubCategoriesData = _repositoryAssignedSubSubCategories.GetAllList(i => i.ProductId == Input.Id && i.SubCategoryId == data.SubCategoryId).ToList();
                                                if (SubSubCategoriesData.Count > 0)
                                                {
                                                    await DeleteProductAssignedSubSubCategoryAsync(SubSubCategoriesData, TenantId);

                                                }
                                            }
                                        }
                                    }
                                    foreach (var subdata in item.ProductSubCategories)
                                    {
                                        if (subdata.AssignmentId > 0)
                                        {
                                            var productAssignedSubCategories = _productAssignedSubCategoriesRepository.GetAllList(x => x.Id == subdata.AssignmentId).FirstOrDefault();
                                            if (productAssignedSubCategories != null)
                                            {
                                                productAssignedSubCategories.SubCategoryId = subdata.SubCategoryId;
                                                await _productAssignedSubCategoriesRepository.UpdateAsync(productAssignedSubCategories);

                                            }
                                            var lstSub = item.ProductSubCategories.SelectMany(e => e.ProductSubSubCategories).ToList();
                                            var ExistingSubSubData = _repositoryAssignedSubSubCategories.GetAllList(i => i.ProductId == Input.Id && i.SubCategoryId == subdata.SubCategoryId).ToList();
                                            var OldSubSubData = lstSub.Where(i => i.AssignmentId > 0).ToList();
                                            var DeletedSubSubData = ExistingSubSubData.Where(p => !OldSubSubData.Any(p2 => p2.AssignmentId == p.Id)).ToList();
                                            if (DeletedSubSubData.Count > 0)
                                            {
                                                await DeleteProductAssignedSubSubCategoryAsync(DeletedSubSubData, TenantId);
                                            }
                                            foreach (var subsubData in subdata.ProductSubSubCategories)
                                            {
                                                if (subsubData.AssignmentId > 0)
                                                {
                                                    var productAssignedSubSubCategories = _repositoryAssignedSubSubCategories.GetAllList(x => x.Id == subsubData.AssignmentId).FirstOrDefault();
                                                    if (productAssignedSubSubCategories != null)
                                                    {
                                                        productAssignedSubSubCategories.SubSubCategoryId = subsubData.SubSubCategoryId;
                                                        await _repositoryAssignedSubSubCategories.UpdateAsync(productAssignedSubSubCategories);
                                                    }
                                                }
                                                else
                                                {
                                                    if (subsubData.SubSubCategoryId > 0)
                                                    {
                                                        ProductAssignedSubSubCategories SubSubDataModel = new ProductAssignedSubSubCategories();
                                                        SubSubDataModel.SubCategoryId = subdata.SubCategoryId;
                                                        SubSubDataModel.ProductId = Input.Id;
                                                        SubSubDataModel.SubSubCategoryId = subsubData.SubSubCategoryId;
                                                        SubSubDataModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                                        await _repositoryAssignedSubSubCategories.InsertAsync(SubSubDataModel);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (subdata.SubCategoryId > 0)
                                            {
                                                ProductAssignedSubCategories SubDataModel = new ProductAssignedSubCategories();
                                                SubDataModel.CategoryId = item.CategoryId;
                                                SubDataModel.ProductId = Input.Id;
                                                SubDataModel.SubCategoryId = subdata.SubCategoryId;
                                                SubDataModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                                await _productAssignedSubCategoriesRepository.InsertAsync(SubDataModel);
                                                foreach (var subdt in subdata.ProductSubSubCategories)
                                                {
                                                    if (subdt.SubSubCategoryId > 0)
                                                    {

                                                        ProductAssignedSubSubCategories SubSubDataModel = new ProductAssignedSubSubCategories();
                                                        SubSubDataModel.SubCategoryId = subdata.SubCategoryId;
                                                        SubSubDataModel.ProductId = Input.Id;
                                                        SubSubDataModel.SubSubCategoryId = subdt.SubSubCategoryId;
                                                        SubSubDataModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                                        await _repositoryAssignedSubSubCategories.InsertAsync(SubSubDataModel);


                                                    }
                                                }
                                                
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ProductAssignedCategoryMaster Model = new ProductAssignedCategoryMaster();
                                    Model.CategoryId = item.CategoryId;
                                    Model.ProductId = Input.Id;
                                    Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    await _repositoryAssignedCategoryMaster.InsertAsync(Model);
                                    if (item.ProductSubCategories.Count > 0)
                                    {
                                        foreach (var itemSub in item.ProductSubCategories)
                                        {
                                            ProductAssignedSubCategories SubModel = new ProductAssignedSubCategories();
                                            SubModel.CategoryId = item.CategoryId;
                                            SubModel.ProductId = Input.Id;
                                            SubModel.SubCategoryId = itemSub.SubCategoryId;
                                            SubModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                            await _productAssignedSubCategoriesRepository.InsertAsync(SubModel);
                                            if (itemSub.ProductSubSubCategories.Count > 0)
                                            {
                                                foreach (var itemSubSub in itemSub.ProductSubSubCategories)
                                                {
                                                    if (itemSubSub.SubSubCategoryId > 0)
                                                    {
                                                        ProductAssignedSubSubCategories SubSubModel = new ProductAssignedSubSubCategories();
                                                        SubSubModel.SubCategoryId = itemSub.SubCategoryId;
                                                        SubSubModel.ProductId = Input.Id;
                                                        SubSubModel.SubSubCategoryId = itemSubSub.SubSubCategoryId;
                                                        SubSubModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                                        await _repositoryAssignedSubSubCategories.InsertAsync(SubSubModel);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            var ExistingData = _repositoryAssignedCategoryMaster.GetAllList(i => i.ProductId == Input.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                await DeleteProductAssignedCategoryAsync(ExistingData, TenantId);
                            }
                        }




                        //#region SubCategory Array Update
                        //if (Input.ProductDetail.ProductSubCategoryArray != null)
                        //{
                        //    var ExistingData = _repositoryAssignedSubSubCategories.GetAllList(i => i.ProductId == Input.Id).ToList();
                        //    var OldData = Input.ProductDetail.ProductSubCategoryArray.Where(i => i.AssignmentId > 0).ToList();
                        //    var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                        //    if (DeletedData.Count > 0)
                        //    {
                        //        await DeleteProductAssignedSubcategoryAsync(DeletedData, TenantId);
                        //    }

                        //    foreach (var item in Input.ProductDetail.ProductSubCategoryArray)
                        //    {
                        //        if (item.AssignmentId > 0)
                        //        {
                        //            var productAssignedSubCategory = _repositoryAssignedSubSubCategories.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                        //            if (productAssignedSubCategory != null)
                        //            {
                        //                productAssignedSubCategory.SubSubCategoryId = item.Id;
                        //                await _repositoryAssignedSubSubCategories.UpdateAsync(productAssignedSubCategory);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            ProductAssignedSubSubCategories Model = new ProductAssignedSubSubCategories();
                        //            Model.SubSubCategoryId = item.Id;
                        //            Model.ProductId = Input.Id;
                        //            Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        //            await _repositoryAssignedSubSubCategories.InsertAsync(Model);
                        //        }

                        //    }
                        //}
                        //else
                        //{
                        //    var ExistingData = _repositoryAssignedSubSubCategories.GetAllList(i => i.ProductId == Input.Id).ToList();
                        //    if (ExistingData.Count > 0)
                        //    {
                        //        await DeleteProductAssignedSubcategoryAsync(ExistingData, TenantId);
                        //    }
                        //}
                        //#endregion

                        #region Materials Array Update
                        if (Input.ProductDetail.ProductMaterialArray != null)
                        {
                            var ExistingData = _productAssignedMaterialsRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            var OldData = Input.ProductDetail.ProductMaterialArray.Where(i => i.AssignmentId > 0).ToList();
                            var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                            if (DeletedData.Count > 0)
                            {
                                await DeleteProductAssignedMaterialsAsync(DeletedData, TenantId);
                            }

                            foreach (var item in Input.ProductDetail.ProductMaterialArray)
                            {
                                if (item.AssignmentId > 0)
                                {
                                    var productAssignedMaterials = _productAssignedMaterialsRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                    if (productAssignedMaterials != null)
                                    {
                                        productAssignedMaterials.MaterialId = item.Id;
                                        await _productAssignedMaterialsRepository.UpdateAsync(productAssignedMaterials);
                                    }
                                }
                                else
                                {
                                    ProductAssignedMaterials Model = new ProductAssignedMaterials();
                                    Model.MaterialId = item.Id;
                                    Model.ProductId = Input.Id;
                                    Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    await _productAssignedMaterialsRepository.InsertAsync(Model);
                                }

                            }
                        }
                        else
                        {
                            var ExistingData = _productAssignedMaterialsRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                await DeleteProductAssignedMaterialsAsync(ExistingData, TenantId);
                            }
                        }
                        #endregion

                        #region Tags Array Update
                        if (Input.ProductDetail.ProductTagArray != null)
                        {
                            var ExistingData = _productAssignedTagsRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            var OldData = Input.ProductDetail.ProductTagArray.Where(i => i.AssignmentId > 0).ToList();
                            var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                            if (DeletedData.Count > 0)
                            {
                                await DeleteProductAssignedTagsAsync(DeletedData, TenantId);
                            }

                            foreach (var item in Input.ProductDetail.ProductTagArray)
                            {
                                if (item.AssignmentId > 0)
                                {
                                    var productAssignedTags = _productAssignedTagsRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                    if (productAssignedTags != null)
                                    {
                                        productAssignedTags.TagId = item.Id;
                                        await _productAssignedTagsRepository.UpdateAsync(productAssignedTags);
                                    }
                                }
                                else
                                {
                                    ProductAssignedTags Model = new ProductAssignedTags();
                                    Model.TagId = item.Id;
                                    Model.ProductId = Input.Id;
                                    Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    await _productAssignedTagsRepository.InsertAsync(Model);
                                }

                            }
                        }
                        else
                        {
                            var ExistingData = _productAssignedTagsRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                await DeleteProductAssignedTagsAsync(ExistingData, TenantId);
                            }
                        }
                        #endregion

                        #region Types Array Update
                        if (Input.ProductDetail.ProductTypeArray != null)
                        {
                            var ExistingData = _productAssignedTypesRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            var OldData = Input.ProductDetail.ProductTypeArray.Where(i => i.AssignmentId > 0).ToList();
                            var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                            if (DeletedData.Count > 0)
                            {
                                await DeleteProductAssignedTypesAsync(DeletedData, TenantId);
                            }

                            foreach (var item in Input.ProductDetail.ProductTypeArray)
                            {
                                if (item.AssignmentId > 0)
                                {
                                    var productAssignedTypes = _productAssignedTypesRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                    if (productAssignedTypes != null)
                                    {
                                        productAssignedTypes.TypeId = item.Id;
                                        await _productAssignedTypesRepository.UpdateAsync(productAssignedTypes);
                                    }
                                }
                                else
                                {
                                    ProductAssignedTypes Model = new ProductAssignedTypes();
                                    Model.TypeId = item.Id;
                                    Model.ProductId = Input.Id;
                                    Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    await _productAssignedTypesRepository.InsertAsync(Model);
                                }

                            }
                        }
                        else
                        {
                            var ExistingData = _productAssignedTypesRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                await DeleteProductAssignedTypesAsync(ExistingData, TenantId);
                            }
                        }
                        #endregion

                        #region Vendors Array Update
                        if (Input.ProductDetail.ProductVendorArray != null)
                        {
                            var ExistingData = _productAssignedVendorsRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            var OldData = Input.ProductDetail.ProductVendorArray.Where(i => i.AssignmentId > 0).ToList();
                            var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                            if (DeletedData.Count > 0)
                            {
                                await DeleteProductAssignedVendorsAsync(DeletedData, TenantId);
                            }

                            foreach (var item in Input.ProductDetail.ProductVendorArray)
                            {
                                if (item.AssignmentId > 0)
                                {
                                    var productAssignedVendors = _productAssignedVendorsRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                    if (productAssignedVendors != null)
                                    {
                                        productAssignedVendors.VendorUserId = item.Id;
                                        await _productAssignedVendorsRepository.UpdateAsync(productAssignedVendors);
                                    }
                                }
                                else
                                {
                                    ProductAssignedVendors Model = new ProductAssignedVendors();
                                    Model.VendorUserId = item.Id;
                                    Model.ProductId = Input.Id;
                                    Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    await _productAssignedVendorsRepository.InsertAsync(Model);
                                }

                            }
                        }
                        else
                        {
                            var ExistingData = _productAssignedVendorsRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                await DeleteProductAssignedVendorsAsync(ExistingData, TenantId);
                            }
                        }
                        #endregion


                        #endregion

                    }


                    #region Product Compartment Builder

                    if (productMaster.IsProductHasCompartmentBuilder == true)
                    {

                        if (Input.CompartmentVariantData != null)
                        {
                            foreach (var compartemntData in Input.CompartmentVariantData)
                            {
                                var ExistingData = _productCompartmentDataRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                                var OldData = compartemntData.VarientList.Where(i => i.CompartmentId > 0).ToList();
                                var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.CompartmentId == p.Id)).ToList();

                                if (DeletedData.Count > 0)
                                {
                                    await DeleteCompartmentVariantData(DeletedData, TenantId);
                                }

                                if (compartemntData.VarientList != null)
                                {

                                    foreach (var data in compartemntData.VarientList)
                                    {
                                        if (data.CompartmentId > 0)
                                        {
                                            var CompartmentData = _productCompartmentDataRepository.GetAllList(i => i.Id == data.CompartmentId).FirstOrDefault();
                                            if (CompartmentData != null)
                                            {
                                                CompartmentData.CompartmentTitle = compartemntData.CompartmentTitle;
                                                CompartmentData.CompartmentSubTitle = compartemntData.CompartmentSubTitle;
                                                CompartmentData.ProductVarientId = data.Id;
                                                CompartmentData.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                                var varientList = _productVariantDataRepository.GetAllList(i => i.Id == data.ProductVarientId).FirstOrDefault();
                                                CompartmentData.Compartment = varientList.Variant;
                                                CompartmentData.SKU = varientList.SKU;
                                                CompartmentData.Price = varientList.Price;
                                                if (data.ImageObj != null)
                                                {
                                                    string ImageLocation = AzureStorageUrl + folderPath + data.ImageObj.FileName;
                                                    CompartmentData.Name = data.ImageObj.FileName;
                                                    CompartmentData.Url = ImageLocation;
                                                    CompartmentData.ImagePath = ImageLocation;
                                                    CompartmentData.ImageFileName = data.ImageObj.FileName;
                                                    CompartmentData.Ext = data.ImageObj.Ext;
                                                    CompartmentData.Size = data.ImageObj.Size;
                                                    CompartmentData.Type = data.ImageObj.Type;
                                                }
                                                var productcompartmentDataId = await _productCompartmentDataRepository.UpdateAsync(CompartmentData);
                                            }

                                        }
                                        else
                                        {
                                            CompartmentVariantData variant = new CompartmentVariantData();
                                            variant.CompartmentTitle = compartemntData.CompartmentTitle;
                                            variant.CompartmentSubTitle = compartemntData.CompartmentSubTitle;
                                            variant.ProductId = Input.Id;
                                            variant.ProductVarientId = data.Id;
                                            variant.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                            var varientList = _productVariantDataRepository.GetAllList(i => i.Id == data.ProductVarientId).FirstOrDefault();
                                            variant.Compartment = varientList.Variant;
                                            variant.SKU = varientList.SKU;
                                            variant.Price = varientList.Price;
                                            if (data.ImageObj != null)
                                            {
                                                string ImageLocation = AzureStorageUrl + folderPath + data.ImageObj.FileName;
                                                variant.Name = data.ImageObj.FileName;
                                                variant.Url = ImageLocation;
                                                variant.ImagePath = ImageLocation;
                                                variant.ImageFileName = data.ImageObj.FileName;
                                                variant.Ext = data.ImageObj.Ext;
                                                variant.Size = data.ImageObj.Size;
                                                variant.Type = data.ImageObj.Type;
                                            }
                                            var productcompartmentDataId = await _productCompartmentDataRepository.InsertAsync(variant);


                                        }


                                    }
                                }
                            }
                        }
                        else
                        {

                            var ExistingData = _productCompartmentDataRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                            if (ExistingData.Count > 0)
                            {
                                await DeleteCompartmentVariantData(ExistingData, TenantId);
                            }
                        }
                    }

                    #endregion



                    //ProductDimensionsInventoryDto
                    if (Input.ProductDimensionsInventory != null)
                    {
                        var productDimensionsInventory = _productDimensionsInventoryRepository.GetAllList(x => x.ProductId == Input.Id).FirstOrDefault();
                        if (productDimensionsInventory != null)
                        {
                            productDimensionsInventory.ProductHeight = Input.ProductDimensionsInventory.ProductHeight;
                            productDimensionsInventory.ProductWidth = Input.ProductDimensionsInventory.ProductWidth;
                            productDimensionsInventory.ProductLength = Input.ProductDimensionsInventory.ProductLength;

                            productDimensionsInventory.ProductDimensionNotes = Input.ProductDimensionsInventory.ProductDimensionNotes;
                            productDimensionsInventory.ProductDiameter = Input.ProductDimensionsInventory.ProductDiameter;
                            productDimensionsInventory.ProductUnitMeasureId = Input.ProductDimensionsInventory.ProductUnitMeasureId;
                            productDimensionsInventory.ProductWeightMeasureId = Input.ProductDimensionsInventory.ProductWeightMeasureId;


                            productDimensionsInventory.UnitWeight = Input.ProductDimensionsInventory.UnitWeight;
                            productDimensionsInventory.ProductPackaging = Input.ProductDimensionsInventory.ProductPackaging;
                            productDimensionsInventory.UnitPerProduct = Input.ProductDimensionsInventory.UnitPerProduct == "" ? 0 : Convert.ToDouble(Input.ProductDimensionsInventory.UnitPerProduct);
                            productDimensionsInventory.CartonWeight = Input.ProductDimensionsInventory.CartonWeight;
                            productDimensionsInventory.CartonQuantity = Input.ProductDimensionsInventory.CartonQuantity;

                            productDimensionsInventory.CartonHeight = Input.ProductDimensionsInventory.CartonHeight;
                            productDimensionsInventory.CartonWidth = Input.ProductDimensionsInventory.CartonWidth;
                            productDimensionsInventory.CartonLength = Input.ProductDimensionsInventory.CartonLength;
                            productDimensionsInventory.UnitPerCarton = Input.ProductDimensionsInventory.UnitPerCarton == "" ? 0 : Convert.ToDouble(Input.ProductDimensionsInventory.UnitPerCarton);
                            productDimensionsInventory.CartonPackaging = Input.ProductDimensionsInventory.CartonPackaging;
                            productDimensionsInventory.CartonNote = Input.ProductDimensionsInventory.CartonNote;

                            productDimensionsInventory.CartonUnitOfMeasureId = Input.ProductDimensionsInventory.CartonUnitOfMeasureId;
                            productDimensionsInventory.CartonWeightMeasureId = Input.ProductDimensionsInventory.CartonWeightMeasureId;
                            productDimensionsInventory.CartonCubicWeightKG = Input.ProductDimensionsInventory.CartonCubicWeightKG;
                            productDimensionsInventory.CartonWeight = Input.ProductDimensionsInventory.CartonWeight;

                            productDimensionsInventory.PalletWeight = Input.ProductDimensionsInventory.PalletWeight;
                            productDimensionsInventory.CartonPerPallet = Input.ProductDimensionsInventory.CartonPerPallet == "" ? 0 : Convert.ToDouble(Input.ProductDimensionsInventory.CartonPerPallet);
                            productDimensionsInventory.UnitPerPallet = Input.ProductDimensionsInventory.UnitPerPallet == "" ? 0 : Convert.ToDouble(Input.ProductDimensionsInventory.UnitPerPallet);
                            productDimensionsInventory.PalletNote = Input.ProductDimensionsInventory.PalletNote;

                            productDimensionsInventory.StockkeepingUnit = Input.ProductDimensionsInventory.StockkeepingUnit == "" ? 0 : Convert.ToInt32(Input.ProductDimensionsInventory.StockkeepingUnit);
                            productDimensionsInventory.Barcode = Input.ProductDimensionsInventory.Barcode;
                            productDimensionsInventory.TotalNumberAvailable = Input.ProductDimensionsInventory.TotalNumberAvailable == "" ? 0 : Convert.ToInt32(Input.ProductDimensionsInventory.TotalNumberAvailable);
                            productDimensionsInventory.AlertRestockNumber = Input.ProductDimensionsInventory.AlertRestockNumber == "" ? 0 : Convert.ToInt32(Input.ProductDimensionsInventory.AlertRestockNumber);
                            productDimensionsInventory.IsTrackQuantity = Input.ProductDimensionsInventory.IsTrackQuantity;
                            productDimensionsInventory.IsStopSellingStockZero = Input.ProductDimensionsInventory.IsStopSellingStockZero;
                            productDimensionsInventory.IsActive = Input.ProductDimensionsInventory.IsActive;
                            await _productDimensionsInventoryRepository.UpdateAsync(productDimensionsInventory);
                        }
                        else
                        {
                            ProductDimensionsInventory productDimensionsModel = new ProductDimensionsInventory();
                            // add new inventory
                            productDimensionsModel.ProductId = Input.Id;
                            productDimensionsModel.TenantId = TenantId;
                            productDimensionsModel.ProductHeight = Input.ProductDimensionsInventory.ProductHeight;
                            productDimensionsModel.ProductWidth = Input.ProductDimensionsInventory.ProductWidth;
                            productDimensionsModel.ProductLength = Input.ProductDimensionsInventory.ProductLength;

                            productDimensionsModel.ProductDimensionNotes = Input.ProductDimensionsInventory.ProductDimensionNotes;
                            productDimensionsModel.ProductDiameter = Input.ProductDimensionsInventory.ProductDiameter;
                            productDimensionsModel.ProductUnitMeasureId = Input.ProductDimensionsInventory.ProductUnitMeasureId;
                            productDimensionsModel.ProductWeightMeasureId = Input.ProductDimensionsInventory.ProductWeightMeasureId;


                            productDimensionsModel.UnitWeight = Input.ProductDimensionsInventory.UnitWeight;
                            productDimensionsModel.ProductPackaging = Input.ProductDimensionsInventory.ProductPackaging;
                            productDimensionsModel.UnitPerProduct = Input.ProductDimensionsInventory.UnitPerProduct == "" ? 0 : Convert.ToDouble(Input.ProductDimensionsInventory.UnitPerProduct);
                            productDimensionsModel.CartonWeight = Input.ProductDimensionsInventory.CartonWeight;
                            productDimensionsModel.CartonQuantity = Input.ProductDimensionsInventory.CartonQuantity;

                            productDimensionsModel.CartonUnitOfMeasureId = Input.ProductDimensionsInventory.CartonUnitOfMeasureId;
                            productDimensionsModel.CartonWeightMeasureId = Input.ProductDimensionsInventory.CartonWeightMeasureId;
                            productDimensionsModel.CartonCubicWeightKG = Input.ProductDimensionsInventory.CartonCubicWeightKG;
                            productDimensionsModel.CartonWeight = Input.ProductDimensionsInventory.CartonWeight;

                            productDimensionsModel.CartonHeight = Input.ProductDimensionsInventory.CartonHeight;
                            productDimensionsModel.CartonWidth = Input.ProductDimensionsInventory.CartonWidth;
                            productDimensionsModel.CartonLength = Input.ProductDimensionsInventory.CartonLength;
                            productDimensionsModel.UnitPerCarton = Input.ProductDimensionsInventory.UnitPerCarton == "" ? 0 : Convert.ToDouble(Input.ProductDimensionsInventory.UnitPerCarton);
                            productDimensionsModel.CartonPackaging = Input.ProductDimensionsInventory.CartonPackaging;
                            productDimensionsModel.CartonNote = Input.ProductDimensionsInventory.CartonNote;

                            productDimensionsModel.PalletWeight = Input.ProductDimensionsInventory.PalletWeight;
                            productDimensionsModel.CartonPerPallet = Input.ProductDimensionsInventory.CartonPerPallet == "" ? 0 : Convert.ToDouble(Input.ProductDimensionsInventory.CartonPerPallet);
                            productDimensionsModel.UnitPerPallet = Input.ProductDimensionsInventory.UnitPerPallet == "" ? 0 : Convert.ToDouble(Input.ProductDimensionsInventory.UnitPerPallet);
                            productDimensionsModel.PalletNote = Input.ProductDimensionsInventory.PalletNote;

                            productDimensionsModel.StockkeepingUnit = Input.ProductDimensionsInventory.StockkeepingUnit == "" ? 0 : Convert.ToInt32(Input.ProductDimensionsInventory.StockkeepingUnit);
                            productDimensionsModel.Barcode = Input.ProductDimensionsInventory.Barcode;
                            productDimensionsModel.TotalNumberAvailable = Input.ProductDimensionsInventory.TotalNumberAvailable == "" ? 0 : Convert.ToInt32(Input.ProductDimensionsInventory.TotalNumberAvailable);
                            productDimensionsModel.AlertRestockNumber = Input.ProductDimensionsInventory.AlertRestockNumber == "" ? 0 : Convert.ToInt32(Input.ProductDimensionsInventory.AlertRestockNumber);
                            productDimensionsModel.IsTrackQuantity = Input.ProductDimensionsInventory.IsTrackQuantity;
                            productDimensionsModel.IsStopSellingStockZero = Input.ProductDimensionsInventory.IsStopSellingStockZero;
                            productDimensionsModel.IsActive = Input.ProductDimensionsInventory.IsActive;
                            await _productDimensionsInventoryRepository.InsertAsync(productDimensionsModel);
                        }
                    }

                    //ProductVolumeDiscountVariant               
                    if (Input.ProductVolumeDiscountVariant != null)
                    {
                        var ExistingData = _productVolumeDiscountVariantRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                        var OldData = Input.ProductVolumeDiscountVariant.Where(i => i.Id > 0).ToList();
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteProductVolumeDiscountAsync(DeletedData, TenantId);
                        }

                        foreach (var item in Input.ProductVolumeDiscountVariant)
                        {
                            if (item.Id > 0)
                            {
                                var productVolumeDiscountVariant = _productVolumeDiscountVariantRepository.GetAllList(x => x.Id == item.Id).FirstOrDefault();
                                if (productVolumeDiscountVariant != null)
                                {
                                    productVolumeDiscountVariant.QuantityFrom = item.QuantityFrom;
                                    productVolumeDiscountVariant.Price = item.Price;
                                    productVolumeDiscountVariant.IsActive = item.IsActive;
                                    await _productVolumeDiscountVariantRepository.UpdateAsync(productVolumeDiscountVariant);
                                }
                            }
                            else
                            {
                                ProductVolumeDiscountVariant Model = new ProductVolumeDiscountVariant();
                                Model.QuantityFrom = item.QuantityFrom;
                                Model.Price = item.Price;
                                Model.IsActive = item.IsActive;
                                Model.ProductId = Input.Id;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                var productVolumeDiscountVariantId = await _productVolumeDiscountVariantRepository.InsertAndGetIdAsync(Model);
                            }

                        }
                    }
                    else
                    {
                        var ExistingData = _productVolumeDiscountVariantRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeleteProductVolumeDiscountAsync(ExistingData, TenantId);
                        }
                    }

                    #region variant combination data

                    #region ProductVariantData

                    if (Input.ProductVariantData != null)
                    {
                        ProductVariantsData Model = new ProductVariantsData();
                        foreach (var productVariantsDataitem in Input.ProductVariantData)
                        {
                            Model = new ProductVariantsData();
                            Model.ProductId = Input.Id;
                            Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            if (productVariantsDataitem.VariantDetails != null)
                            {
                                string[] VariantIds = productVariantsDataitem.VariantDetails.Split('/');
                                StringBuilder sb = new StringBuilder();
                                int finalIndex = VariantIds.Count() - 1;
                                for (int i = 0; i < VariantIds.Length; i++)
                                {
                                    if (i == finalIndex)
                                    {

                                        sb.Append(Convert.ToInt32(VariantIds[i]) % (10));
                                    }
                                    else
                                    {
                                        sb.Append(Convert.ToInt32(VariantIds[i]) % (10) + "/");
                                    }

                                }

                                string varientDetails = sb.ToString();
                                Model.VariantMasterIds = varientDetails;
                            }
                            else
                            {
                                Model.VariantMasterIds = productVariantsDataitem.VariantDetails;


                            }
                            Model.Variant = productVariantsDataitem.Variant;
                            Model.QuantityStockUnit = productVariantsDataitem.VariantQuantity;

                            Model.Price = productVariantsDataitem.VariantPrice == "" ? 0 : Convert.ToDecimal(productVariantsDataitem.VariantPrice);
                            Model.ComparePrice = productVariantsDataitem.ComparePrice == "" ? 0 : Convert.ToDecimal(productVariantsDataitem.ComparePrice);
                            Model.CostPerItem = productVariantsDataitem.CostPerItem == "" ? 0 : Convert.ToDecimal(productVariantsDataitem.CostPerItem);
                            Model.Margin = productVariantsDataitem.Margin;
                            Model.ProfitCurrencySymbol = productVariantsDataitem.ProfitCurrencySymbol == "" ? 0 : Convert.ToInt32(productVariantsDataitem.ProfitCurrencySymbol);
                            Model.Profit = productVariantsDataitem.Profit == "" ? 0 : Convert.ToDecimal(productVariantsDataitem.Profit);
                            Model.IsChargeTaxVariant = productVariantsDataitem.IsChargeTaxVariant;
                            Model.Shape = productVariantsDataitem.Shape;

                            Model.SKU = productVariantsDataitem.VariantSKU;
                            Model.BarCode = productVariantsDataitem.VariantBarCode;
                            Model.IsTrackQuantity = productVariantsDataitem.IsTrackQuantity;
                            Model.IsActive = productVariantsDataitem.IsActive;
                            Model.NextShipment = productVariantsDataitem.NextShipment;
                            Model.IncomingQuantity = productVariantsDataitem.IncomingQuantity;
                            long productVariantsDataId = await _productVariantDataRepository.InsertAndGetIdAsync(Model);

                            if (productVariantsDataitem.VariantWarehouse != null)
                            {
                                ProductVariantWarehouse warehouse = new ProductVariantWarehouse();
                                warehouse.WarehouseId = productVariantsDataitem.VariantWarehouse.WarehouseId;
                                warehouse.LocationA = productVariantsDataitem.VariantWarehouse.LocationA;
                                warehouse.LocationB = productVariantsDataitem.VariantWarehouse.LocationB;
                                warehouse.LocationC = productVariantsDataitem.VariantWarehouse.LocationC;
                                warehouse.ProductVariantId = productVariantsDataId;
                                warehouse.QuantityThisLocation = string.IsNullOrEmpty(productVariantsDataitem.VariantWarehouse.QuantityThisLocation) ? 0 : Convert.ToDouble(productVariantsDataitem.VariantWarehouse.QuantityThisLocation);
                                await _productVariantWarehouseRepository.InsertAsync(warehouse);
                            }

                            if (productVariantsDataitem.VariantDetails != null)
                            {
                                string[] VariantIds = productVariantsDataitem.VariantDetails.Split('/');
                                string[] VariantValues = productVariantsDataitem.Variant.Split('/');

                                for (int i = 0; i < VariantIds.Length; i++)
                                {
                                    ProductVariantOptionValues ModelVariantOptionValues = new ProductVariantOptionValues();
                                    ModelVariantOptionValues.ProductVariantId = productVariantsDataId;
                                    ModelVariantOptionValues.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    //changes requested by kamlesh sir
                                    // ModelVariantOptionValues.VariantOptionId = Convert.ToInt32(VariantIds[i]);
                                    ModelVariantOptionValues.VariantOptionId = Convert.ToInt32(VariantIds[i]) % (10);
                                    ModelVariantOptionValues.VariantOptionValue = VariantValues[i].ToString();
                                    long ModelVariantOptionValuesId = await _productVariantOptionValuesRepository.InsertAndGetIdAsync(ModelVariantOptionValues);
                                }
                            }

                            if (!string.IsNullOrEmpty(productVariantsDataitem.ImageName))
                            {
                                //string ImageLocationOld = CurrentWebsiteUrl + "/" + FolderName + "/" + productVariantsDataitem.ImageName;
                                string ImageLocation = AzureStorageUrl + folderPath + productVariantsDataitem.ImageName;
                                ProductVariantdataImages DataImages = new ProductVariantdataImages();
                                DataImages.ProductVariantId = productVariantsDataId;
                                DataImages.ImageURL = ImageLocation;
                                DataImages.ImageFileName = productVariantsDataitem.ImageName;
                                DataImages.ProductId = Input.Id;
                                DataImages.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                try
                                {
                                    long Id = await _productVariantdataImagesRepository.InsertAndGetIdAsync(DataImages);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    #endregion

                    #endregion

                    if (Input.CompartmentBaseImage != null)
                    {
                        try
                        {

                            ProductCompartmentBaseImages Model = new ProductCompartmentBaseImages();
                            string ImageLocation = AzureStorageUrl + folderPath + Input.CompartmentBaseImage.FileName;
                            Model.ProductId = Input.Id;
                            Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            Model.Ext = Input.CompartmentBaseImage.Ext;
                            Model.Size = Input.CompartmentBaseImage.Size;
                            Model.Type = Input.CompartmentBaseImage.Type;
                            Model.Name = Input.CompartmentBaseImage.FileName;
                            Model.ImageFileName = Input.CompartmentBaseImage.FileName;
                            Model.Url = ImageLocation;
                            Model.ImagePath = ImageLocation;
                            await _productBaseImageRepository.UpdateAsync(Model);

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    //  #endregion

                    //default Image
                    if (Input.ProductDefaultImage != null)
                    {
                        var ProductDefaultImage = _productImagesRepository.GetAllList(x => x.ProductId == Input.Id && x.IsDefaultImage == true).OrderByDescending(i => i.Id).FirstOrDefault();
                        if (ProductDefaultImage != null)
                        {
                            if (ProductDefaultImage.ImageName != Input.ProductDefaultImage.FileName)
                            {
                                string ImageLocation = AzureStorageUrl + folderPath + Input.ProductDefaultImage.FileName;
                                ProductDefaultImage.ImageName = Input.ProductDefaultImage.FileName;
                                ProductDefaultImage.ImagePath = ImageLocation;
                                ProductDefaultImage.Url = ImageLocation;
                                ProductDefaultImage.Url = ImageLocation;
                                ProductDefaultImage.Ext = Input.ProductDefaultImage.Ext;
                                ProductDefaultImage.Size = Input.ProductDefaultImage.Size;
                                ProductDefaultImage.Type = Input.ProductDefaultImage.Type;
                                ProductDefaultImage.Name = Input.ProductDefaultImage.Name;
                                await _productImagesRepository.UpdateAsync(ProductDefaultImage);
                            }
                        }
                    }

                    //product Images 
                    if (Input.ProductImagesNames != null)
                    {
                        var ExistingData = _productImagesRepository.GetAllList(i => i.ProductId == Input.Id && i.IsDefaultImage == false).ToList();
                        var OldData = Input.ProductImagesNames.Where(i => i.Id > 0).ToList();
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteProductImagesAsync(DeletedData, TenantId);
                        }

                        foreach (var imagename in Input.ProductImagesNames.Where(i => i.Id == 0))
                        {
                            if (imagename.FileName != null && imagename.FileName != "")
                            {

                                ProductImages Model = new ProductImages();
                                string ImageLocation = AzureStorageUrl + folderPath + imagename.FileName;
                                Model.ImageName = imagename.FileName;
                                Model.ProductId = Input.Id;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                Model.ImagePath = ImageLocation;
                                Model.IsProductSubmissionDone = true;
                                Model.Ext = imagename.Ext;
                                Model.Size = imagename.Size;
                                Model.Type = imagename.Type;
                                Model.Name = imagename.Name;
                                long Id = await _productImagesRepository.InsertAndGetIdAsync(Model);
                            }
                        }
                    }
                    else
                    {
                        var ExistingData = _productImagesRepository.GetAllList(i => i.ProductId == Input.Id && i.IsDefaultImage == false).ToList();

                        if (ExistingData.Count > 0)
                        {
                            await DeleteProductImagesAsync(ExistingData, TenantId);
                        }
                    }

                    #region media images changes
                    if (Input.LineArtMediaImages != null)
                    {
                        var ExistingData = _productMediaImagesRepository.GetAllList(i => i.ProductId == Input.Id && i.ProductMediaImageTypeId == LineArtId.Id).ToList();
                        var OldData = Input.LineArtMediaImages.Where(i => i.Id > 0).ToList();
                        var DeletedMediaData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();
                        if (DeletedMediaData.Count > 0)
                        {
                            await DeleteProductMediaImagesAsync(DeletedMediaData, TenantId);
                        }

                        foreach (var mediatype in Input.LineArtMediaImages.Where(i => i.Id == 0))
                        {
                            if (mediatype != null)
                            {
                                if (mediatype.FileName != null && mediatype.FileName != "")
                                {
                                    string ImageLocation = AzureStorageUrl + folderPath + mediatype.FileName;
                                    ProductMediaImages Model = new ProductMediaImages();
                                    Model.ImageName = mediatype.FileName;
                                    Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    Model.ProductMediaImageTypeId = mediatype.MediaType;
                                    Model.IsProductSubmissionDone = true;
                                    Model.ImageUrl = ImageLocation;
                                    Model.Url = ImageLocation;
                                    Model.ProductId = Input.Id;
                                    Model.Type = mediatype.Type;
                                    Model.Size = mediatype.Size;
                                    Model.Ext = mediatype.Ext;
                                    Model.Name = mediatype.Name;
                                    long Id = await _productMediaImagesRepository.InsertAndGetIdAsync(Model);
                                }
                            }
                        }
                    }
                    else
                    {
                        var ExistingData = _productMediaImagesRepository.GetAllList(i => i.ProductId == Input.Id && i.ProductMediaImageTypeId == LineArtId.Id).ToList();

                        if (ExistingData.Count > 0)
                        {
                            await DeleteProductMediaImagesAsync(ExistingData, TenantId);
                        }
                    }

                    if (Input.LifeStyleMediaImages.Count > 0 && Input.LifeStyleMediaImages != null)
                    {
                        var ExistingData = _productMediaImagesRepository.GetAllList(i => i.ProductId == Input.Id && i.ProductMediaImageTypeId == LifeStyleImagesId.Id).ToList();
                        var OldData = Input.LifeStyleMediaImages.Where(i => i.Id > 0).ToList();
                        var DeletedMediaData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();
                        if (DeletedMediaData.Count > 0)
                        {
                            await DeleteProductMediaImagesAsync(DeletedMediaData, TenantId);
                        }

                        foreach (var mediatype in Input.LifeStyleMediaImages.Where(i => i.Id == 0))
                        {
                            if (mediatype != null)
                            {
                                if (mediatype.FileName != null && mediatype.FileName != "")
                                {
                                    string ImageLocation = AzureStorageUrl + folderPath + mediatype.FileName;

                                    ProductMediaImages Model = new ProductMediaImages();
                                    Model.ImageName = mediatype.FileName;
                                    Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    Model.ProductMediaImageTypeId = mediatype.MediaType;
                                    Model.IsProductSubmissionDone = true;
                                    Model.ImageUrl = ImageLocation;
                                    Model.Url = ImageLocation;
                                    Model.ProductId = Input.Id;
                                    Model.Type = mediatype.Type;
                                    Model.Size = mediatype.Size;
                                    Model.Ext = mediatype.Ext;
                                    Model.Name = mediatype.Name;
                                    long Id = await _productMediaImagesRepository.InsertAndGetIdAsync(Model);
                                }
                            }
                        }
                    }
                    else
                    {
                        var ExistingData = _productMediaImagesRepository.GetAllList(i => i.ProductId == Input.Id && i.ProductMediaImageTypeId == LifeStyleImagesId.Id).ToList();

                        if (ExistingData.Count > 0)
                        {
                            await DeleteProductMediaImagesAsync(ExistingData, TenantId);
                        }
                    }

                    if (Input.OtherMediaImages.Count > 0 && Input.OtherMediaImages != null)
                    {

                        var ExistingData = _productMediaImagesRepository.GetAllList(i => i.ProductId == Input.Id && i.ProductMediaImageTypeId == OtherMediaImageId.Id).ToList();
                        var OldData = Input.OtherMediaImages.Where(i => i.Id > 0).ToList();
                        var DeletedMediaData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();
                        if (DeletedMediaData.Count > 0)
                        {
                            await DeleteProductMediaImagesAsync(DeletedMediaData, TenantId);
                        }

                        foreach (var mediatype in Input.OtherMediaImages.Where(i => i.Id == 0))
                        {
                            if (mediatype != null)
                            {
                                if (mediatype.FileName != null && mediatype.FileName != "")
                                {
                                    string ImageLocation = AzureStorageUrl + folderPath + mediatype.FileName;

                                    ProductMediaImages Model = new ProductMediaImages();
                                    Model.ImageName = mediatype.FileName;
                                    Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    Model.ProductMediaImageTypeId = mediatype.MediaType;
                                    Model.IsProductSubmissionDone = true;
                                    Model.ImageUrl = ImageLocation;
                                    Model.Url = ImageLocation;
                                    Model.ProductId = Input.Id;
                                    Model.Type = mediatype.Type;
                                    Model.Size = mediatype.Size;
                                    Model.Ext = mediatype.Ext;
                                    Model.Name = mediatype.Name;
                                    long Id = await _productMediaImagesRepository.InsertAndGetIdAsync(Model);
                                }
                            }
                        }
                    }
                    else
                    {
                        var ExistingData = _productMediaImagesRepository.GetAllList(i => i.ProductId == Input.Id && i.ProductMediaImageTypeId == OtherMediaImageId.Id).ToList();

                        if (ExistingData.Count > 0)
                        {
                            await DeleteProductMediaImagesAsync(ExistingData, TenantId);
                        }
                    }
                    #endregion

                    //ProductBrandingPosition
                    if (Input.ProductBrandingPosition != null)
                    {

                        var ExistingData = _productBrandingPositionRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                        var OldData = Input.ProductBrandingPosition.Where(i => i.Id > 0).ToList();
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteProductBrandingPositionAsync(DeletedData, TenantId);
                        }

                        foreach (var ProductBrandingPositionitem in Input.ProductBrandingPosition)
                        {
                            string ImageLocation = AzureStorageUrl + folderPath + ProductBrandingPositionitem.ImageName;

                            if (ProductBrandingPositionitem.Id > 0)
                            {
                                var ProductBrandingPosition = _productBrandingPositionRepository.GetAllList(x => x.Id == ProductBrandingPositionitem.Id).FirstOrDefault();
                                ProductBrandingPosition.ImageName = ProductBrandingPositionitem.ImageName;
                                ProductBrandingPosition.ImageFileURL = ImageLocation;
                                ProductBrandingPosition.LayerTitle = ProductBrandingPositionitem.LayerTitle;
                                ProductBrandingPosition.PostionMaxHeight = Convert.ToDouble(ProductBrandingPositionitem.PositionMaxHeight);
                                ProductBrandingPosition.PostionMaxwidth = Convert.ToDouble(ProductBrandingPositionitem.PositionMaxwidth);
                                ProductBrandingPosition.BrandingLocationNote = ProductBrandingPositionitem.BrandingLocationNote;
                                ProductBrandingPosition.UnitOfMeasureId = Input.BrandingUnitOfMeasureId;

                                if (ProductBrandingPositionitem.ImageObj != null)
                                {
                                    if (ProductBrandingPosition.ImageName != ProductBrandingPositionitem.ImageObj.FileName)
                                    {
                                        string ImageLoc = AzureStorageUrl + folderPath + ProductBrandingPositionitem.ImageObj.FileName;
                                        ProductBrandingPosition.ImageName = ProductBrandingPositionitem.ImageObj.FileName;
                                        ProductBrandingPosition.ImageFileURL = ImageLoc;
                                        ProductBrandingPosition.Ext = ProductBrandingPositionitem.ImageObj.Ext;
                                        ProductBrandingPosition.Name = ProductBrandingPositionitem.ImageObj.FileName;
                                        ProductBrandingPosition.Url = ImageLoc;
                                        ProductBrandingPosition.Size = ProductBrandingPositionitem.ImageObj.Size;
                                        ProductBrandingPosition.Type = ProductBrandingPositionitem.ImageObj.Type;
                                    }
                                }
                                await _productBrandingPositionRepository.UpdateAsync(ProductBrandingPosition);
                            }
                            else
                            {
                                ProductBrandingPosition productBrandData = new ProductBrandingPosition();
                                productBrandData.ProductId = Input.Id;
                                productBrandData.LayerTitle = ProductBrandingPositionitem.LayerTitle;
                                productBrandData.PostionMaxHeight = Convert.ToDouble(ProductBrandingPositionitem.PositionMaxHeight);
                                productBrandData.PostionMaxwidth = Convert.ToDouble(ProductBrandingPositionitem.PositionMaxwidth);
                                productBrandData.BrandingLocationNote = ProductBrandingPositionitem.BrandingLocationNote;
                                productBrandData.UnitOfMeasureId = Input.BrandingUnitOfMeasureId;

                                if (ProductBrandingPositionitem.ImageObj != null)
                                {

                                    string ImageLoc = AzureStorageUrl + folderPath + ProductBrandingPositionitem.ImageObj.FileName;
                                    productBrandData.ImageName = ProductBrandingPositionitem.ImageObj.FileName;
                                    productBrandData.ImageFileURL = ImageLoc;
                                    productBrandData.Ext = ProductBrandingPositionitem.ImageObj.Ext;
                                    productBrandData.Name = ProductBrandingPositionitem.ImageObj.FileName;
                                    productBrandData.Url = ImageLoc;
                                    productBrandData.Size = ProductBrandingPositionitem.ImageObj.Size;
                                    productBrandData.Type = ProductBrandingPositionitem.ImageObj.Type;
                                }

                                long Id = await _productBrandingPositionRepository.InsertAndGetIdAsync(productBrandData);
                            }
                        }
                    }
                    else
                    {
                        var ExistingData = _productBrandingPositionRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeleteProductBrandingPositionAsync(ExistingData, TenantId);
                        }
                    }

                    // ProductStockLocation
                    if (Input.ProductStockLocation != null)
                    {
                        var ExistingData = _productStockLocationRepository.GetAllList(i => i.ProductId == Input.Id).ToList();
                        var OldData = Input.ProductStockLocation.Where(i => i.Id > 0).ToList();
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteProductStockDataAsync(DeletedData, TenantId);
                        }

                        foreach (var ProductStockLocationitem in Input.ProductStockLocation)
                        {
                            if (ProductStockLocationitem.Id > 0)
                            {
                                var productStockLocation = _productStockLocationRepository.GetAllList(x => x.Id == ProductStockLocationitem.Id).FirstOrDefault();
                                if (productStockLocation != null)
                                {
                                    productStockLocation.WareHouseId = ProductStockLocationitem.WareHouseId;
                                    productStockLocation.LocationA = ProductStockLocationitem.LocationA;
                                    productStockLocation.LocationB = ProductStockLocationitem.LocationB;
                                    productStockLocation.LocationC = ProductStockLocationitem.LocationC;
                                    productStockLocation.StockAlertQty = ProductStockLocationitem.StockAlertQty;
                                    productStockLocation.QuantityAtLocation = ProductStockLocationitem.QuantityAtLocation;
                                    productStockLocation.IsActive = ProductStockLocationitem.IsActive;
                                    await _productStockLocationRepository.UpdateAsync(productStockLocation);
                                }
                            }
                            else
                            {
                                ProductStockLocation productStockLocationModel = new ProductStockLocation();
                                productStockLocationModel.ProductId = Input.Id;
                                productStockLocationModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                productStockLocationModel.WareHouseId = ProductStockLocationitem.WareHouseId;
                                productStockLocationModel.LocationA = ProductStockLocationitem.LocationA;
                                productStockLocationModel.LocationB = ProductStockLocationitem.LocationB;
                                productStockLocationModel.LocationC = ProductStockLocationitem.LocationC;
                                productStockLocationModel.StockAlertQty = ProductStockLocationitem.StockAlertQty;
                                productStockLocationModel.QuantityAtLocation = ProductStockLocationitem.QuantityAtLocation;
                                productStockLocationModel.IsActive = ProductStockLocationitem.IsActive;
                                long productStockLocationId = await _productStockLocationRepository.InsertAndGetIdAsync(productStockLocationModel);

                            }
                        }
                    }
                    else
                    {
                        var ExistingData = _productStockLocationRepository.GetAllList(i => i.ProductId == Input.Id).ToList();

                        if (ExistingData.Count > 0)
                        {
                            await DeleteProductStockDataAsync(ExistingData, TenantId);
                        }
                    }

                    //BrandingMethodData

                    if (Input.BrandingMethodData != null)
                    {
                        var ExistingData = _productBrandingMethodsRepository.GetAllList(i => i.ProductMasterId == Input.Id).ToList();
                        var OldData = Input.BrandingMethodData.Where(i => i.AssignmentId > 0).ToList();
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteProductMethods(DeletedData, TenantId);
                        }
                        foreach (var brandingMethodDataItem in Input.BrandingMethodData)
                        {
                            var IsExists = ExistingData.Where(i => i.Id == brandingMethodDataItem.AssignmentId).FirstOrDefault();
                            if (IsExists != null)
                            {
                                IsExists.BrandingMethodId = brandingMethodDataItem.Id;
                                IsExists.ProductMasterId = Input.Id;
                                IsExists.MethodCustomizedColor = brandingMethodDataItem.SelectedColor;
                                await _productBrandingMethodsRepository.UpdateAsync(IsExists);
                            }
                            else
                            {
                                ProductBrandingMethods Model = new ProductBrandingMethods();
                                Model.BrandingMethodId = brandingMethodDataItem.Id;
                                Model.ProductMasterId = Input.Id;
                                Model.MethodCustomizedColor = brandingMethodDataItem.SelectedColor;
                                long id = await _productBrandingMethodsRepository.InsertAndGetIdAsync(Model);
                            }
                        }
                    }
                    else
                    {
                        var ExistingData = _productBrandingMethodsRepository.GetAllList(i => i.ProductMasterId == Input.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeleteProductMethods(ExistingData, TenantId);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return output;
        }

        #region old delete productby id api
        //public async Task<ProductMasterDto> DeleteProductById(ProductDataRequestDto Input)
        //{
        //    ProductMasterDto output = new ProductMasterDto();
        //    try
        //    {                
        //        var ProductMasterData = _repository.GetAllList(i => i.Id == Input.ProductId).FirstOrDefault();
        //        if (ProductMasterData != null)
        //        {
        //            #region Assigned Tables delete

        //            // product Brand Array
        //            var productAssignedBrandsData = _productAssignedBrandsRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productAssignedBrandsData != null && (productAssignedBrandsData.Count() > 0))
        //            {
        //                _productAssignedBrandsRepository.BulkDelete(productAssignedBrandsData);

        //            }
        //            // product Caterories Array
        //            var productAssignedCategoriesData = _productAssignedSubCategoriesRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productAssignedCategoriesData != null && (productAssignedCategoriesData.Count() > 0))
        //            {
        //                _productAssignedSubCategoriesRepository.BulkDelete(productAssignedCategoriesData);

        //            }
        //            // product Collections Array
        //            var productAssignedCollectionsData = _productAssignedCollectionsRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productAssignedCollectionsData != null && (productAssignedCollectionsData.Count() > 0))
        //            {
        //                _productAssignedCollectionsRepository.BulkDelete(productAssignedCollectionsData);

        //            }
        //            // product Materials Array
        //            var productAssignedMaterialsData = _productAssignedMaterialsRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productAssignedMaterialsData != null && (productAssignedMaterialsData.Count() > 0))
        //            {
        //                _productAssignedMaterialsRepository.BulkDelete(productAssignedMaterialsData);

        //            }
        //            // product Tags Array
        //            var productAssignedTagsData = _productAssignedTagsRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productAssignedTagsData != null && (productAssignedTagsData.Count() > 0))
        //            {
        //                _productAssignedTagsRepository.BulkDelete(productAssignedTagsData);

        //            }
        //            // product Types Array
        //            var productAssignedTypesData = _productAssignedTypesRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productAssignedTypesData != null && (productAssignedTypesData.Count() > 0))
        //            {
        //                _productAssignedTypesRepository.BulkDelete(productAssignedTypesData);

        //            }
        //            // product Vendors Array
        //            var productAssignedVendorsData = _productAssignedVendorsRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productAssignedVendorsData != null && (productAssignedVendorsData.Count() > 0))
        //            {
        //                _productAssignedVendorsRepository.BulkDelete(productAssignedVendorsData);

        //            }
        //            #endregion
        //            //productDimensionsInventory
        //            var productDimensionsInventoryData = _productDimensionsInventoryRepository.GetAllList(i => i.ProductId == Input.ProductId).FirstOrDefault();
        //            if (productDimensionsInventoryData != null)
        //            {

        //                await _productDimensionsInventoryRepository.DeleteAsync(productDimensionsInventoryData);                       
        //            }

        //            // productVolumeDiscountVariantRepository
        //            var productVolumeDiscountVariantData = _productVolumeDiscountVariantRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productVolumeDiscountVariantData != null && (productVolumeDiscountVariantData.Count() > 0))
        //            {
        //                _productVolumeDiscountVariantRepository.BulkDelete(productVolumeDiscountVariantData);

        //            }

        //            //ProductStockLocation
        //            var productStockLocationData = _productStockLocationRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productStockLocationData != null && (productStockLocationData.Count() > 0))
        //            {
        //                _productStockLocationRepository.BulkDelete(productStockLocationData);

        //            }

        //            //ProductBrandingPosition
        //            var productBrandingPositionData = _productBrandingPositionRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productBrandingPositionData != null && (productBrandingPositionData.Count() > 0))
        //            {
        //                _productBrandingPositionRepository.BulkDelete(productBrandingPositionData);

        //            }
        //            //

        //            //productImages
        //            var productImagesData = _productImagesRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productImagesData != null && (productImagesData.Count() > 0))
        //            {
        //                _productImagesRepository.BulkDelete(productImagesData);

        //            }

        //            //productMediaImages
        //            var productMediaImagesData = _productMediaImagesRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productMediaImagesData != null && (productMediaImagesData.Count() > 0))
        //            {
        //                _productImagesRepository.BulkDelete(productImagesData);

        //            }

        //            //productBulkUploadVariations
        //            var productBulkUploadVariationsData = _productBulkUploadVariationsRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productBulkUploadVariationsData != null && (productBulkUploadVariationsData.Count() > 0))
        //            {
        //                _productBulkUploadVariationsRepository.BulkDelete(productBulkUploadVariationsData);

        //            }
        //            //productBrandingMethods
        //            var productMethodData = _productBrandingMethodsRepository.GetAllList(i => i.ProductMasterId == Input.ProductId).ToList();
        //            if (productMethodData != null && (productMethodData.Count() > 0))
        //            {
        //                _productBrandingMethodsRepository.BulkDelete(productMethodData);

        //            }

        //            // ProductVariantOptionValues
        //            var productVariantOptionValuesData = (from value in _productVariantDataRepository.GetAllList()
        //                                                  join items in _productVariantOptionValuesRepository.GetAllList() on value.Id equals items.ProductVariantId
        //                                                  where value.ProductId == Input.ProductId
        //                                                  select items).ToList();
        //            if ((productVariantOptionValuesData.Count() > 0) && productVariantOptionValuesData != null)
        //            {
        //                _productVariantOptionValuesRepository.BulkDelete(productVariantOptionValuesData);
        //            }                

        //            //ProductVariantdataImages
        //            var productVariantdataImagesData = (from value in _productVariantDataRepository.GetAllList()
        //                                                  join items in _productVariantdataImagesRepository.GetAllList() on value.Id equals items.ProductVariantId
        //                                                  where value.ProductId == Input.ProductId
        //                                                  select items).ToList();
        //            if ((productVariantdataImagesData.Count() > 0) && productVariantdataImagesData != null)
        //            {
        //                _productVariantdataImagesRepository.BulkDelete(productVariantdataImagesData);
        //            }

        //            //ProductVariantWarehouse
        //            var productVariantWarehouseData = (from value in _productVariantDataRepository.GetAllList()
        //                                                join items in _productVariantWarehouseRepository.GetAllList() on value.Id equals items.ProductVariantId
        //                                                where value.ProductId == Input.ProductId
        //                                                select items).ToList();
        //            if ((productVariantWarehouseData.Count() > 0) && productVariantWarehouseData != null)
        //            {
        //                _productVariantWarehouseRepository.BulkDelete(productVariantWarehouseData);
        //            }

        //            //ProductVariantData
        //            var productVariantData = _productVariantDataRepository.GetAllList(i => i.ProductId == Input.ProductId).ToList();
        //            if (productVariantData != null && productVariantData.Count > 0)
        //            {
        //                _productVariantDataRepository.BulkDelete(productVariantData);                       

        //            }

        //            //productmaster
        //            var productMasterData = _repository.GetAllList(i => i.Id == Input.ProductId).FirstOrDefault();
        //            if (productMasterData != null)
        //            {
        //                await _repository.DeleteAsync(productMasterData);
        //                await _unitOfWorkManager.Current.SaveChangesAsync();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return output;
        //}
        #endregion

        public async Task<ProductMasterDto> DeleteProductById(ProductDataRequestDto Input)
        {
            ProductMasterDto output = new ProductMasterDto();
            try
            {
                var ProductMasterData = _repository.GetAllList(i => i.Id == Input.ProductId).FirstOrDefault();
                if (ProductMasterData != null)
                {
                    await _connectionUtility.EnsureConnectionOpenAsync();

                    var product = await _db.QueryFirstOrDefaultAsync<ProductMasterDto>("usp_DeleteProductById", new
                    {
                        productId = Input.ProductId,
                        tanentId = AbpSession.TenantId.Value
                    }, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return output;
        }

        public async Task<bool> updateSubCategory(List<AllSubCategoryIdDto> categories, long productId, int tenantId, long categoryId)
        {
            bool IsUpdated = false;
            try
            {

                if (categories != null)
                {
                    var ExistingData = _productAssignedSubCategoriesRepository.GetAllList(i => i.ProductId == productId).ToList();
                    var OldData = categories.Where(i => i.AssignmentId > 0).ToList();
                    var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();
                    if (DeletedData.Count > 0)
                    {
                        await DeleteProductAssignedSubCategoryAsync(DeletedData, tenantId);
                    }
                    foreach (var item in categories)
                    {
                        if (item.AssignmentId > 0)
                        {
                            var getsubsubData = updateSubSubCategory(item.ProductSubSubCategories, productId, tenantId, item.SubCategoryId);

                            var productAssignedSubCategories = _productAssignedSubCategoriesRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                            if (productAssignedSubCategories != null)
                            {
                                productAssignedSubCategories.SubCategoryId = item.SubCategoryId;
                                await _productAssignedSubCategoriesRepository.UpdateAsync(productAssignedSubCategories);
                            }
                        }
                        else
                        {
                            ProductAssignedSubCategories Model = new ProductAssignedSubCategories();
                            Model.CategoryId = categoryId;
                            Model.ProductId = productId;
                            Model.SubCategoryId = item.SubCategoryId;
                            Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            await _productAssignedSubCategoriesRepository.InsertAsync(Model);
                        }

                    }
                }
                else
                {
                    var ExistingData = _productAssignedSubCategoriesRepository.GetAllList(i => i.ProductId == productId).ToList();
                    if (ExistingData.Count > 0)
                    {
                        await DeleteProductAssignedSubCategoryAsync(ExistingData, tenantId);
                    }
                }


            }
            catch (Exception ex)
            {

            }
            return IsUpdated;
        }

        public async Task<bool> updateSubSubCategory(List<AllSubSubCategoryIdDto> categories, long productId, int tenantId, long categoryId)
        {
            bool IsUpdated = false;
            try
            {

                if (categories != null)
                {
                    var ExistingData = _repositoryAssignedSubSubCategories.GetAllList(i => i.ProductId == productId).ToList();
                    var OldData = categories.Where(i => i.AssignmentId > 0).ToList();
                    var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();
                    if (DeletedData.Count > 0)
                    {
                        await DeleteProductAssignedSubSubCategoryAsync(DeletedData, tenantId);
                    }
                    foreach (var item in categories)
                    {
                        if (item.AssignmentId > 0)
                        {

                            var productAssignedSubSubCategories = _repositoryAssignedSubSubCategories.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                            if (productAssignedSubSubCategories != null)
                            {
                                productAssignedSubSubCategories.SubSubCategoryId = item.SubSubCategoryId;
                                await _repositoryAssignedSubSubCategories.UpdateAsync(productAssignedSubSubCategories);
                            }
                        }
                        else
                        {
                            ProductAssignedSubSubCategories Model = new ProductAssignedSubSubCategories();
                            Model.SubCategoryId = categoryId;
                            Model.ProductId = productId;
                            Model.SubSubCategoryId = item.SubSubCategoryId;
                            Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            await _repositoryAssignedSubSubCategories.InsertAsync(Model);
                        }

                    }
                }
                else
                {
                    var ExistingData = _repositoryAssignedSubSubCategories.GetAllList(i => i.ProductId == productId).ToList();
                    if (ExistingData.Count > 0)
                    {
                        await DeleteProductAssignedSubSubCategoryAsync(ExistingData, tenantId);
                    }
                }


            }
            catch (Exception ex)
            {

            }
            return IsUpdated;
        }

        private async Task<bool> DeleteVariantProductImagesAsync(List<ProductVariantdataImages> ProductImages, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productVariantdataImagesRepository.BulkDelete(ProductImages);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        public async Task DeleteVariantById(long Id)
        {
            var VariantData = _productVariantDataRepository.GetAllList(i => i.Id == Id).FirstOrDefault();
            if (VariantData != null)
            {
                var OptionValues = _productVariantOptionValuesRepository.GetAllList(i => i.ProductVariantId == VariantData.Id).ToList();
                if (OptionValues.Count > 0)
                {
                    _productVariantOptionValuesRepository.BulkDelete(OptionValues);
                }
                var WareHouses = _productVariantWarehouseRepository.GetAllList(i => i.ProductVariantId == VariantData.Id).ToList();
                if (WareHouses.Count > 0)
                {
                    _productVariantWarehouseRepository.BulkDelete(WareHouses);
                }
                var Images = _productVariantdataImagesRepository.GetAllList(i => i.ProductVariantId == VariantData.Id).ToList();
                if (Images.Count > 0)
                {
                    _productVariantdataImagesRepository.BulkDelete(Images);
                }

                await _productVariantDataRepository.DeleteAsync(VariantData);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        private async Task<bool> DeleteProductImagesAsync(List<ProductImages> ProductImages, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productImagesRepository.BulkDelete(ProductImages);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteProductMediaImagesAsync(List<ProductMediaImages> ProductMediaImages, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productMediaImagesRepository.BulkDelete(ProductMediaImages);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> DeleteProductBrandingPositionAsync(List<ProductBrandingPosition> BrandingPosition, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productBrandingPositionRepository.BulkDelete(BrandingPosition);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteProductStockDataAsync(List<ProductStockLocation> ProductStockData, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productStockLocationRepository.BulkDelete(ProductStockData);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteCompartmentVariantData(List<CompartmentVariantData> CompartmentVariantData, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productCompartmentDataRepository.BulkDelete(CompartmentVariantData);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteProductMethodAttrVals(List<ProductMethodAttributeValues> ProductAttrData, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productMethodAttributeValuesRepository.BulkDelete(ProductAttrData);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> DeleteProductMethods(List<ProductBrandingMethods> ProductMethods, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productBrandingMethodsRepository.BulkDelete(ProductMethods);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> DeleteProductVariantStockDataAsync(List<ProductVariantWarehouse> ProductStockData, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productVariantWarehouseRepository.BulkDelete(ProductStockData);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> DeleteProductVolumeDiscountAsync(List<ProductVolumeDiscountVariant> VariantDiscountVariant, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productVolumeDiscountVariantRepository.BulkDelete(VariantDiscountVariant);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> DeletePriceVariantsAsync(List<ProductVariantQuantityPrices> VariantDiscountVariant, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productVariantQuantityPricesRepository.BulkDelete(VariantDiscountVariant);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> DeleteProductBrandingVariantAsync(List<ProductBrandingPriceVariants> VariantBrandingVariant, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productBrandingPriceVariantsRepository.BulkDelete(VariantBrandingVariant);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> DeleteProductAssignedBrandsAsync(List<ProductAssignedBrands> productAssignedBrands, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productAssignedBrandsRepository.BulkDelete(productAssignedBrands);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> DeleteProductAssignedCollectionsAsync(List<ProductAssignedCollections> productAssignedCollections, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productAssignedCollectionsRepository.BulkDelete(productAssignedCollections);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        public async Task<bool> DeleteProductAssignedCategoryAsync(List<ProductAssignedCategoryMaster> productAssignedCategories, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryAssignedCategoryMaster.BulkDelete(productAssignedCategories);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        public async Task<bool> DeleteProductAssignedSubCategoryAsync(List<ProductAssignedSubCategories> productAssignedSubCategories, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productAssignedSubCategoriesRepository.BulkDelete(productAssignedSubCategories);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        public async Task<bool> DeleteProductAssignedSubSubCategoryAsync(List<ProductAssignedSubSubCategories> productAssignedSubSubCategories, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryAssignedSubSubCategories.BulkDelete(productAssignedSubSubCategories);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteProductAssignedMaterialsAsync(List<ProductAssignedMaterials> productAssignedMaterials, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productAssignedMaterialsRepository.BulkDelete(productAssignedMaterials);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> DeleteProductAssignedTagsAsync(List<ProductAssignedTags> productAssignedTags, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productAssignedTagsRepository.BulkDelete(productAssignedTags);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteProductAssignedTypesAsync(List<ProductAssignedTypes> productAssignedTypes, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productAssignedTypesRepository.BulkDelete(productAssignedTypes);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteProductAssignedVendorsAsync(List<ProductAssignedVendors> productAssignedVendors, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _productAssignedVendorsRepository.BulkDelete(productAssignedVendors);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        public async Task UpdateProductsPricesInBulk(List<UpdatePriceModel> Data)
        {

            try
            {

                var data = Data.Select(e => e).ToList();
                DataTable table = new DataTable();
                using (var reader = ObjectReader.Create(data))
                {
                    table.Load(reader);
                }

                await _connectionUtility.EnsureConnectionOpenAsync();

                var dts = await _db.QueryAsync<string>("usp_UpdateDataInProdTable", new
                {
                    TVP = table
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }


        }


        public async Task<List<VarientList>> GetProductVariantByTitle(string title)
        {
            List<VarientList> Response = new List<VarientList>();
            try
            {
                await _connectionUtility.EnsureConnectionOpenAsync();
                string FolderName = _configuration["FileUpload:FolderName"];
                string CurrentWebsiteUrl = _configuration.GetValue<string>("FileUpload:WebsireDomainPath");
                int TenantId = AbpSession.TenantId.Value;
                var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
                string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
                IEnumerable<GetVariantListDto> ResponseData = await _db.QueryAsync<GetVariantListDto>("usp_GetProductVariantByTitle", new
                {
                    title = title,
                    TenantId = AbpSession.TenantId.Value
                }, commandType: System.Data.CommandType.StoredProcedure);

                Response = (from varient in ResponseData
                            select new VarientList
                            {
                                Id = varient.Id,
                                Variant = varient.Variant,
                                Color = varient.Color,
                                SKU = varient.VariantSKU,
                                ProductVarientId = varient.Id,
                                Price = Convert.ToDecimal(varient.VariantPrice),
                                ImageObj = new ProductImageType
                                {
                                    Ext = varient.Ext,
                                    Size = varient.Size,
                                    FileName = varient.ImageFileName,
                                    Url = varient.ImageURL,
                                    Type = varient.Type,
                                    Name = varient.Name,
                                    ImagePath = varient.ImageURL
                                },
                            }).ToList();
            }
            catch (Exception ex)
            {

            }
            return Response;
        }
    }
}
