using Abp.IdentityServer4;
using Abp.Organizations;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CaznerMarketplaceBackendApp.Authorization.Delegation;
using CaznerMarketplaceBackendApp.Authorization.Roles;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Chat;
using CaznerMarketplaceBackendApp.Editions;
using CaznerMarketplaceBackendApp.Friendships;
using CaznerMarketplaceBackendApp.MultiTenancy;
using CaznerMarketplaceBackendApp.MultiTenancy.Accounting;
using CaznerMarketplaceBackendApp.MultiTenancy.Payments;
using CaznerMarketplaceBackendApp.Storage;
using CaznerMarketplaceBackendApp.Country;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.Product.Masters;
using CaznerMarketplaceBackendApp.FAQ;
using CaznerMarketplaceBackendApp.Position;
using CaznerMarketplaceBackendApp.WareHouse;
using CaznerMarketplaceBackendApp.Currency;
using CaznerMarketplaceBackendApp.Department;
using CaznerMarketplaceBackendApp.Store;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.ContactUs;
using CaznerMarketplaceBackendApp.BrandingMethods;
using CaznerMarketplaceBackendApp.BrandingMethod;
using CaznerMarketplaceBackendApp.BannerLogo;
using CaznerMarketplaceBackendApp.ArtWork;
using CaznerMarketplaceBackendApp.Security;
using CaznerMarketplaceBackendApp.SubCategory;
using CaznerMarketplaceBackendApp.BulkColorCodes;
using CaznerMarketplaceBackendApp.ECatalogue;

namespace CaznerMarketplaceBackendApp.EntityFrameworkCore
{
    public class CaznerMarketplaceBackendAppDbContext : AbpZeroDbContext<Tenant, Role, User, CaznerMarketplaceBackendAppDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<Countries> Countries { get; set; }

        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<PositionMaster> PositionMaster { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }

        public virtual DbSet<UserBusinessSettings> UserBusinessSettings { get; set; }
        public virtual DbSet<UserBankDetails> UserBankDetails { get; set; }

        public virtual DbSet<SubscriptionPlanMaster> SubscriptionPlanMaster { get; set; }
        public virtual DbSet<UserSubscriptionPlan> UserSubscriptionPlan { get; set; }

        public virtual DbSet<SubscriptionFeaturesMaster> SubscriptionFeaturesMaster { get; set; }
        public virtual DbSet<SubscriptionPlanFeatures> SubscriptionPlanFeatures { get; set; }

        //Masters
        public virtual DbSet<ProductBrandingPositionTitleMaster> ProductBrandingPositionTitleMaster { get; set; }
        public virtual DbSet<ProductBrandMaster> ProductBrandMaster { get; set; }
        public virtual DbSet<ProductCollectionMaster> ProductCollectionMaster { get; set; }
        public virtual DbSet<ProductColourMaster> ProductColourMaster { get; set; }
        public virtual DbSet<ProductMaterialMaster> ProductMaterialMaster { get; set; }
        public virtual DbSet<ProductMediaImageTypeMaster> ProductMediaImageTypeMaster { get; set; }
        public virtual DbSet<ProductSizeMaster> ProductSizeMaster { get; set; }
        public virtual DbSet<ProductTagMaster> ProductTagMaster { get; set; }
        public virtual DbSet<ProductTypeMaster> ProductTypeMaster { get; set; }
        //product tables
        public virtual DbSet<ProductDimensionsInventory> ProductDimensionsInventory { get; set; }
        public virtual DbSet<ProductAssignedBrands> ProductAssignedBrands { get; set; }
        public virtual DbSet<ProductAssignedCollections> ProductAssignedCollections { get; set; }
        public virtual DbSet<ProductAssignedMaterials> ProductAssignedMaterials { get; set; }
        public virtual DbSet<ProductAssignedTags> ProductAssignedTags { get; set; }
        public virtual DbSet<ProductAssignedTypes> ProductAssignedTypes { get; set; }
        public virtual DbSet<ProductAssignedVendors> ProductAssignedVendors { get; set; }

        // need to remove ProductDetails
        public virtual DbSet<ProductDetails> ProductDetails { get; set; }
        public virtual DbSet<ProductBranding> ProductBranding { get; set; }
        public virtual DbSet<ProductBrandingPosition> ProductBrandingPosition { get; set; }
        public virtual DbSet<ProductBulkUploadVariations> ProductBulkUploadVariations { get; set; }
        public virtual DbSet<ProductDesignHub> ProductDesignHub { get; set; }
        //public virtual DbSet<ProductDimension> ProductDimension { get; set; }
        public virtual DbSet<ProductImages> ProductImages { get; set; }
        //public virtual DbSet<ProductInventory> ProductInventory { get; set; }
        public virtual DbSet<ProductMaster> ProductMaster { get; set; }
        public virtual DbSet<ProductMediaImages> ProductMediaImages { get; set; }
        public virtual DbSet<ProductStockLocation> ProductStockLocation { get; set; }
        //public virtual DbSet<ProductPackageDimension> ProductPackageDimension { get; set; }
        public virtual DbSet<ProductPallet> ProductPallet { get; set; }
        public virtual DbSet<ProductVolumeDiscountVariant> ProductVolumeDiscountVariant { get; set; }
        public virtual DbSet<BrandingMethodMaster> BrandingMethodMaster { get; set; }
        public virtual DbSet<BrandingMethodAttributes> BrandingMethodAttributes { get; set; }
        public virtual DbSet<BrandingAttributeAssignment> BrandingAttributeAssignment { get; set; }
        public virtual DbSet<ProductMethodAttributeValues> ProductMethodAttributeValues { get; set; }
        public virtual DbSet<ProductOptionsMaster> ProductOptionsMaster { get; set; }
        public virtual DbSet<ProductVariantsData> ProductVariantsData { get; set; }
        public virtual DbSet<ProductVariantdataImages> ProductVariantdataImages { get; set; }
        public virtual DbSet<ProductVariantWarehouse> ProductVariantWarehouse { get; set; }
        public virtual DbSet<ProductVariantOptionValues> ProductVariantOptionValues { get; set; }
        //FAQ
        public virtual DbSet<QuestionsMaster> QuestionsMasters { get; set; }
        public virtual DbSet<AnswersMaster> AnswersMasters { get; set; }
        //Warehouse
        public virtual DbSet<WareHouseMaster> WareHouseMaster { get; set; }

       // public virtual DbSet<ProductBulkImportDataHistory> ProductBulkImportDataHistory { get; set; }
        //Currency
        public virtual DbSet<CurrencyMaster> CurrencyMaster { get; set; }
        public virtual DbSet<ContactUsMaster> ContactUsMaster { get; set; }

        //Department
        public virtual DbSet<DepartmentMaster> DepartmentMaster { get; set; }
        public virtual DbSet<DepartmentUsers> DepartmentUsers { get; set; }
        //Store
        public virtual DbSet<StoreMaster> StoreMaster { get; set; }
        public virtual DbSet<StoreOpeningTimings> StoreOpeningTimings { get; set; }

        public virtual DbSet<ProductBrandingPriceVariants> ProductBrandingPriceVariants { get; set; }

        public virtual DbSet<CategoryMaster> CategoryMaster { get; set; }

        public virtual DbSet<CategoryCollections> CategoryCollections { get; set; }

        public virtual DbSet<UserShippingAddress> UserShippingAddress { get; set; }

        public virtual DbSet<CategoryGroupMaster> CategoryGroupMaster { get; set; }

        public virtual DbSet<UserAdditionalShippingAddress> UserAdditionalShippingAddress { get; set; }

        public virtual DbSet<UserBannerLogoData> UserBannerLogoData { get; set; }
        public virtual DbSet<TurnAroundTime> TurnAroundTime { get; set; }

        public virtual DbSet<CollectionMaster> CollectionMaster { get; set; }
        public virtual DbSet<CollectionCalculations> CollectionCalculations { get; set; }
        public virtual DbSet<CategoryGroups> CategoryGroups { get; set; }

        public virtual DbSet<CalculationTypeTags> CalculationTypeTags { get; set; }
        public virtual DbSet<CalculationTypes> CalculationTypes { get; set; }
        public virtual DbSet<CalculationTypeAttributes> CalculationTypeAttributes { get; set; }

        // need to remove BrandingMethodDetails
       // public virtual DbSet<BrandingMethodDetails> BrandingMethodDetails { get; set; }
        public virtual DbSet<EquipmentBrandingMaster> EquipmentBrandingMaster { get; set; }
        public virtual DbSet<UniversalBrandingMaster> UniversalBrandingMaster { get; set; }
        public virtual DbSet<BrandingMethodAdditionalPrice> BrandingMethodAdditionalPrice { get; set; }
        public virtual DbSet<BrandingSpecificFontStyleMaster> BrandingSpecificFontStyleMaster { get; set; }
        public virtual DbSet<BrandingSpecificFontTypeFaceMaster> BrandingSpecificFontTypeFaceMaster { get; set; }
        public virtual DbSet<ProductBrandingMethods> ProductBrandingMethods { get; set; }
        public virtual DbSet<ProductAssignedSubCategories> ProductAssignedSubCategories { get; set; }
        public virtual DbSet<BannerPageTypeMaster> BannerPageTypeMaster { get; set; }
        public virtual DbSet<ImageTypeMaster> ImageTypeMaster { get; set; }   
        public virtual DbSet<BrandingMethodAssignedColors> BrandingMethodAssignedColors { get; set; }
        public virtual DbSet<BrandingMethodAssignedEquipments> BrandingMethodAssignedEquipments { get; set; }
        public virtual DbSet<BrandingMethodAssignedFontStyles> BrandingMethodAssignedFontStyles { get; set; }
        public virtual DbSet<BrandingMethodAssignedFontTypeFaces> BrandingMethodAssignedFontTypeFaces { get; set; }
        public virtual DbSet<BrandingMethodAssignedTags> BrandingMethodAssignedTags { get; set; }
        public virtual DbSet<BrandingMethodAssignedUniversals> BrandingMethodAssignedUniversals { get; set; }
        public virtual DbSet<BrandingMethodAssignedVendors> BrandingMethodAssignedVendors { get; set; }
        public virtual DbSet<AlternativeProducts> AlternativeProducts { get; set; }
        public virtual DbSet<RelativeProducts> RelativeProducts { get; set; }
        public virtual DbSet<ProductViewImages> ProductViewImages { get; set; }

        public virtual DbSet<BrandingAdditionalQuantities> BrandingAdditionalQuantities { get; set; }
        public virtual DbSet<BrandingAdditionalQtyPrices> BrandingAdditionalQtyPrices { get; set; }

        public virtual DbSet<ProductVariantQuantityPrices> ProductVariantQuantityPrices { get; set; }

        public virtual DbSet<ProductBulkImportRawData> ProductBulkImportRawData { get; set; }

        public virtual DbSet<ArtWorkMaster> ArtWorkMaster { get; set; }
        public virtual DbSet<ArtworkImages> ArtworkImages { get; set; }
        public virtual DbSet<ArtworkMockupImages> ArtworkMockupImages { get; set; }

        public virtual DbSet<CompartmentVariantData> CompartmentVariantData { get; set; }
        public virtual DbSet<CompartmentOptionValues> CompartmentOptionValues { get; set; }
        public virtual DbSet<ProductCompartmentBaseImages> ProductCompartmentBaseImages { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<SMTPSettingsMaster> SMTPSettingsMaster { get; set; }
        public virtual DbSet<SubCategoryMaster> SubCategoryMaster { get; set; }
        public virtual DbSet<CategorySubCategories> CategorySubCategories { get; set; }
        public virtual DbSet<ProductAssignedSubSubCategories> ProductAssignedSubSubCategories { get; set; }
        public virtual DbSet<HexColorCodesMaster> HexColorCodesMaster { get; set; }
        public virtual DbSet<HexColorCodesRawData> HexColorCodesRawData { get; set; }
        public virtual DbSet<ProductAssignedCategoryMaster> ProductAssignedCategoryMaster { get; set; }
        public virtual DbSet<ECatalogueMaster> ECatalogueMaster { get; set; }

        public virtual DbSet<StatesTempRawData> StatesTempRawData { get; set; }
        public virtual DbSet<CollectionHomePage> CollectionHomePage { get; set; }
        public virtual DbSet<CategoryHomePage> CategoryHomePage { get; set; }
        public CaznerMarketplaceBackendAppDbContext(DbContextOptions<CaznerMarketplaceBackendAppDbContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(36000);
        }                                        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });
           
            //modelBuilder.Entity<ProductBulkImportRawData>(b =>
            //{
            //    b.HasNoKey();
            //});

            //--------------------------------------------------------------//
            //--------------adding query idnexes in tables

            //modelBuilder.Entity<ProductMaster>(b =>
            //{
            //    b.HasIndex(e => new { e.TenantId,e.TurnAroundTimeId }).IsUnique();
            //});

            //modelBuilder.Entity<ProductImages>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId, e.TenantId }).IsUnique();
            //});

            //modelBuilder.Entity<ProductMediaImages>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductVariantsData>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductVariantWarehouse>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductVariantId, e.TenantId, e.WarehouseId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductVariantdataImages>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductVariantId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductVariantOptionValues>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductVariantId, e.TenantId }).IsUnique();
            //});

            //modelBuilder.Entity<ProductAssignedBrands>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId, e.TenantId, e.ProductBrandId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductAssignedCategories>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId, e.TenantId, e.CategoryId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductAssignedCollections>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId,e.CollectionId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductAssignedTags>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId,e.TagId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductAssignedMaterials>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId,e.MaterialId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductAssignedTypes>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId,e.TypeId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductAssignedVendors>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId,e.VendorUserId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductBrandingMethods>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductMasterId, e.TenantId , e.BrandingMethodId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductBulkUploadVariations>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId, e.TenantId, e.productOptionId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductDimensionsInventory>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId, e.TenantId}).IsUnique();
            //});
            //modelBuilder.Entity<ProductStockLocation>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId, e.TenantId, e.WareHouseId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductVolumeDiscountVariant>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductVariantQuantityPrices>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductVariantId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductBrandingPosition>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<ProductViewImages>(b =>
            //{
            //    b.HasIndex(e => new { e.ProductId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<CategoryGroups>(b =>
            //{
            //    b.HasIndex(e => new { e.CategoryMasterId,e.CategoryGroupId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<CategoryCollections>(b =>
            //{
            //    b.HasIndex(e => new { e.CategoryId, e.CollectionId, e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<CategoryGroupMaster>(b =>
            //{
            //    b.HasIndex(e => new { e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<CategoryMaster>(b =>
            //{
            //    b.HasIndex(e => new { e.TenantId }).IsUnique();
            //});
            //modelBuilder.Entity<CollectionMaster>(b =>
            //{
            //    b.HasIndex(e => new { e.TenantId }).IsUnique();
            //});
            ////modelBuilder.Entity<UserBannerLogoData>(b =>
            ////{
            ////    b.HasIndex(e => new { e.TenantId }).IsUnique();
            ////});
            ////modelBuilder.Entity<WareHouseMaster>(b =>
            ////{
            ////    b.HasIndex(e => new { e.TenantId }).IsUnique();
            ////});
            //modelBuilder.Entity<UserAdditionalShippingAddress>(b =>
            //{
            //    b.HasIndex(e => new { e.TenantId }).IsUnique();
            //}); 

            //--------------------------------------------------------------//
            //--------------adding query idnexes in tables ends-----------------

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
