using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using CaznerAngularCoreApiDemo.Product.Dto;
using CaznerMarketplaceBackendApp.Artwork.Dto;
using CaznerMarketplaceBackendApp.ArtWork;
using CaznerMarketplaceBackendApp.Auditing.Dto;
using CaznerMarketplaceBackendApp.Authorization.Accounts.Dto;
using CaznerMarketplaceBackendApp.Authorization.Delegation;
using CaznerMarketplaceBackendApp.Authorization.Permissions.Dto;
using CaznerMarketplaceBackendApp.Authorization.Roles;
using CaznerMarketplaceBackendApp.Authorization.Roles.Dto;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Authorization.Users.Delegation.Dto;
using CaznerMarketplaceBackendApp.Authorization.Users.Dto;
using CaznerMarketplaceBackendApp.Authorization.Users.Importing.Dto;
using CaznerMarketplaceBackendApp.Authorization.Users.Profile.Dto;
using CaznerMarketplaceBackendApp.BannerLogo;
using CaznerMarketplaceBackendApp.BannerLogo.Dto;
using CaznerMarketplaceBackendApp.BrandingMethod;
using CaznerMarketplaceBackendApp.BrandingMethod.Dto;
using CaznerMarketplaceBackendApp.BrandingMethods;
using CaznerMarketplaceBackendApp.BulkColorCodes;
using CaznerMarketplaceBackendApp.BusinessSetting;
using CaznerMarketplaceBackendApp.BusinessSetting.Dto;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.CategoryCollection.Dto;
using CaznerMarketplaceBackendApp.Chat;
using CaznerMarketplaceBackendApp.Chat.Dto;
using CaznerMarketplaceBackendApp.ContactUs;
using CaznerMarketplaceBackendApp.ContactUs.Dto;
using CaznerMarketplaceBackendApp.Country;
using CaznerMarketplaceBackendApp.Country.Dto;
using CaznerMarketplaceBackendApp.Currency;
using CaznerMarketplaceBackendApp.Currency.Dto;
using CaznerMarketplaceBackendApp.Department;
using CaznerMarketplaceBackendApp.Department.Dto;
using CaznerMarketplaceBackendApp.DynamicEntityProperties.Dto;
using CaznerMarketplaceBackendApp.ECatalogue;
using CaznerMarketplaceBackendApp.ECatalogue.Dto;
using CaznerMarketplaceBackendApp.Editions;
using CaznerMarketplaceBackendApp.Editions.Dto;
using CaznerMarketplaceBackendApp.EquipmentsBranding;
using CaznerMarketplaceBackendApp.EquipmentsBranding.Dto;
using CaznerMarketplaceBackendApp.Friendships;
using CaznerMarketplaceBackendApp.Friendships.Cache;
using CaznerMarketplaceBackendApp.Friendships.Dto;
using CaznerMarketplaceBackendApp.HexColorCodes.Dto;
using CaznerMarketplaceBackendApp.Localization.Dto;
using CaznerMarketplaceBackendApp.MultiTenancy;
using CaznerMarketplaceBackendApp.MultiTenancy.Dto;
using CaznerMarketplaceBackendApp.MultiTenancy.HostDashboard.Dto;
using CaznerMarketplaceBackendApp.MultiTenancy.Payments;
using CaznerMarketplaceBackendApp.MultiTenancy.Payments.Dto;
using CaznerMarketplaceBackendApp.Notifications.Dto;
using CaznerMarketplaceBackendApp.Organizations.Dto;
using CaznerMarketplaceBackendApp.Position;
using CaznerMarketplaceBackendApp.Position.Dto;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.Product.Dto;
using CaznerMarketplaceBackendApp.Product.Masters;
using CaznerMarketplaceBackendApp.ProductBrand.Dto;
using CaznerMarketplaceBackendApp.ProductCollection.Dto;
using CaznerMarketplaceBackendApp.ProductMaterial.Dto;
using CaznerMarketplaceBackendApp.ProductSize.Dto;
using CaznerMarketplaceBackendApp.ProductTag.Dto;
using CaznerMarketplaceBackendApp.ProductType.Dto;
using CaznerMarketplaceBackendApp.Sessions.Dto;
using CaznerMarketplaceBackendApp.State.Dto;
using CaznerMarketplaceBackendApp.Store;
using CaznerMarketplaceBackendApp.Store.Dto;
using CaznerMarketplaceBackendApp.SubscriptionFeatures.Dto;
using CaznerMarketplaceBackendApp.SubscriptionPlan.Dto;
using CaznerMarketplaceBackendApp.SubscriptionPlanFeature.Dto;
using CaznerMarketplaceBackendApp.Tag.Dto;
using CaznerMarketplaceBackendApp.UniversalBranding.Dto;
using CaznerMarketplaceBackendApp.WareHouse;
using CaznerMarketplaceBackendApp.WebHooks.Dto;

namespace CaznerMarketplaceBackendApp
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.PropertyName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();
            
            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */

            // Country
            configuration.CreateMap<Countries, CountryDto>();
            configuration.CreateMap<CountryDto, Countries>();
            configuration.CreateMap<Countries, CreateOrUpdateCountry>();
            configuration.CreateMap<CreateOrUpdateCountry, Countries>();

            // State
            configuration.CreateMap<States, StateDto>();
            configuration.CreateMap<StateDto, States>();
            configuration.CreateMap<States, CreateOrUpdateState>();
            configuration.CreateMap<CreateOrUpdateState, States>();

            // position
            configuration.CreateMap<PositionMaster, PositionDto>();
            configuration.CreateMap<PositionDto, PositionMaster>();
            configuration.CreateMap<PositionMaster, CreateOrUpdatePosition>();
            configuration.CreateMap<CreateOrUpdatePosition, PositionMaster>();


            // UserDetails
            configuration.CreateMap<UserDetails, UserDetailsDto>();
            configuration.CreateMap<UserDetailsDto, UserDetails>();

            // UserDetails
            configuration.CreateMap<CreateTenantRegistration, UserDetails>();
            configuration.CreateMap<UserDetails, CreateTenantRegistration>();

            configuration.CreateMap<CreateDistributorRegistration, UserDetails>();
            configuration.CreateMap<UserDetails, CreateDistributorRegistration>();


            //BusinessUserSetting           
            configuration.CreateMap<WareHouseMaster, WareHouseMasterDto>();
            configuration.CreateMap<WareHouseMasterDto, WareHouseMaster>();

            // SubscriptionFeatureMaster
            configuration.CreateMap<SubscriptionFeaturesMaster, SubscriptionFeatureMasterDto>();
            configuration.CreateMap<SubscriptionFeatureMasterDto, SubscriptionFeaturesMaster>();
            configuration.CreateMap<SubscriptionFeaturesMaster, CreateOrUpdateSubscriptionFeatures>();
            configuration.CreateMap<CreateOrUpdateSubscriptionFeatures, SubscriptionFeaturesMaster>();

            // SubscriptionPlanFeature
            configuration.CreateMap<SubscriptionPlanFeatures, SubscriptionPlanFeatureDto>();
            configuration.CreateMap<SubscriptionPlanFeatureDto, SubscriptionPlanFeatures>();
            configuration.CreateMap<SubscriptionPlanFeatures, CreateOrUpdateSubscriptionPlanFeature>();
            configuration.CreateMap<CreateOrUpdateSubscriptionPlanFeature, SubscriptionPlanFeatures>();

            // Product
            configuration.CreateMap<ProductMaster, ProductMasterDto>();
            configuration.CreateMap<ProductMasterDto, ProductMaster>();
            configuration.CreateMap<ProductMaster, CreateProductDto>();
            configuration.CreateMap<CreateProductDto, ProductMaster>();

            configuration.CreateMap<ProductDetails, ProductDetailDto>();
            configuration.CreateMap<ProductDetailDto, ProductDetails>();

            configuration.CreateMap<ProductDimensionsInventory, ProductDimensionsInventoryDto>();
            configuration.CreateMap<ProductDimensionsInventoryDto, ProductDimensionsInventory>();

            configuration.CreateMap<ProductStockLocation, ProductStockLocationDto>();
            configuration.CreateMap<ProductStockLocationDto, ProductStockLocation>();

            configuration.CreateMap<ProductVolumeDiscountVariant, ProductVolumeDiscountVariantDto>();
            configuration.CreateMap<ProductVolumeDiscountVariantDto, ProductVolumeDiscountVariant>();

            configuration.CreateMap<ProductBulkUploadVariations, ProductBulkUploadVariationsDto>();
            configuration.CreateMap<ProductBulkUploadVariationsDto, ProductBulkUploadVariations>();

            configuration.CreateMap<ProductBrandingPosition, ProductBrandingPositionDto>();
            configuration.CreateMap<ProductBrandingPositionDto, ProductBrandingPosition>();            


            // SubscriptionPlanFeature
            configuration.CreateMap<SubscriptionPlanMaster, SubscriptionPlanDto>();
            configuration.CreateMap<SubscriptionPlanDto, SubscriptionPlanMaster>();
            configuration.CreateMap<SubscriptionPlanMaster, CreateOrUpdateSubscriptionPlan>();
            configuration.CreateMap<CreateOrUpdateSubscriptionPlan, SubscriptionPlanMaster>();

            // Product Type
            configuration.CreateMap<ProductTypeMaster, ProductTypeDto>();
            configuration.CreateMap<ProductTypeDto, ProductTypeMaster>();
            configuration.CreateMap<ProductTypeMaster, CreateOrUpdateProductType>();
            configuration.CreateMap<CreateOrUpdateProductType, ProductTypeMaster>();

            // Product Brand
            configuration.CreateMap<ProductBrandMaster, ProductBrandDto>();
            configuration.CreateMap<ProductBrandDto, ProductTypeMaster>();
            configuration.CreateMap<ProductBrandMaster, CreateOrUpdateProductBrand>();
            configuration.CreateMap<CreateOrUpdateProductBrand, ProductBrandMaster>();

            // Product Material
            configuration.CreateMap<ProductMaterialMaster, ProductMaterialDto>();
            configuration.CreateMap<ProductMaterialDto, ProductMaterialMaster>();
            configuration.CreateMap<ProductMaterialMaster, CreateOrUpdateProductMaterial>();
            configuration.CreateMap<CreateOrUpdateProductMaterial, ProductMaterialMaster>();

            // Product Collection
            configuration.CreateMap<ProductCollectionMaster, ProductCollectionDto>();
            configuration.CreateMap<ProductCollectionDto, ProductCollectionMaster>();
            configuration.CreateMap<ProductCollectionMaster, CreateOrUpdateProductCollection>();
            configuration.CreateMap<CreateOrUpdateProductCollection, ProductCollectionMaster>();

            // Product Tag
            configuration.CreateMap<ProductTagMaster, ProductTagDto>();
            configuration.CreateMap<ProductTagDto, ProductTagMaster>();
            configuration.CreateMap<ProductTagMaster, CreateOrUpdateProductTag>();
            configuration.CreateMap<CreateOrUpdateProductTag, ProductTagMaster>();

            // BrandingMethod
            configuration.CreateMap<BrandingMethodMaster, BrandingMethodDto>();
            configuration.CreateMap<BrandingMethodDto, BrandingMethodMaster>();
            configuration.CreateMap<BrandingMethodMaster, CreateOrUpdateBrandingMethod>();
            configuration.CreateMap<CreateOrUpdateBrandingMethod, BrandingMethodMaster>();
            configuration.CreateMap<BrandingMethodMaster, CreateBrandingMethodDto>();
            configuration.CreateMap<CreateBrandingMethodDto, BrandingMethodMaster>();
            configuration.CreateMap<BrandingMethodModel, BrandingMethodMaster>();
            configuration.CreateMap<BrandingMethodMaster, BrandingMethodModel>();
            // Currency
            configuration.CreateMap<CurrencyMaster, CurrencyDto>();
            configuration.CreateMap<CurrencyDto, CurrencyMaster>();
            configuration.CreateMap<CurrencyMaster, CreateOrUpdateCurrency>();
            configuration.CreateMap<CreateOrUpdateCurrency, CurrencyMaster>();

            // Product Size Master
            configuration.CreateMap<ProductSizeMaster, ProductSizeDto>();
            configuration.CreateMap<ProductSizeDto, ProductSizeMaster>();
            configuration.CreateMap<ProductSizeMaster, CreateOrUpdateProductSize>();
            configuration.CreateMap<CreateOrUpdateProductSize, ProductSizeMaster>();

            // Department
            configuration.CreateMap<DepartmentMaster, DepartmentDto>();
            configuration.CreateMap<DepartmentDto, DepartmentMaster>();
            configuration.CreateMap<DepartmentMaster, CreateOrUpdateDepartment> ();
            configuration.CreateMap< CreateOrUpdateDepartment, DepartmentMaster>();

            // DepartmentUsers
            configuration.CreateMap<DepartmentUsers, DepartmentUsersDto>();
            configuration.CreateMap<DepartmentUsersDto, DepartmentUsers>();

            //storeTiming
            configuration.CreateMap<StoreOpeningTimings, StoreOpeningTimingsDto>();
            configuration.CreateMap<StoreOpeningTimingsDto, StoreOpeningTimings>();

            //contactus
            configuration.CreateMap<ContactUsMaster, ContactUsDto>();
            configuration.CreateMap<ContactUsDto, ContactUsMaster>();

            //CategoryMaster
            configuration.CreateMap<CategoryMaster, CategoryDto>();
            configuration.CreateMap<CategoryDto, CategoryMaster>();
            configuration.CreateMap<CategoryMaster, CreateCategoryDto>();
            configuration.CreateMap<CreateCategoryDto, CategoryMaster>();

            //CategoryCollection
            configuration.CreateMap<CategoryCollections, CategoryCollectionsDto>();
            configuration.CreateMap<CategoryCollectionsDto, CategoryCollections>();

            //CategoryGroups
            configuration.CreateMap<CategoryGroups, CategoryGroupDto>();
            configuration.CreateMap<CategoryGroupDto, CategoryGroups>();

            //CategoryCollectionMaster
            configuration.CreateMap<CategoryCollections, CategoryCollectionsDto>();
            configuration.CreateMap<CategoryCollectionsDto, CategoryCollections>();
            configuration.CreateMap<CategoryCollections, CreateCategoryCollectionDto>();
            configuration.CreateMap<CreateCategoryCollectionDto, CategoryCollections>();

            //UserShippingAddress
            configuration.CreateMap<UserShippingAddress, UserShippingAddressDto>();
            configuration.CreateMap<UserShippingAddressDto, UserShippingAddress>();

            //UserAdditionalShippingAddress
            configuration.CreateMap<UserAdditionalShippingAddress, UserAdditionalShippingAddressDto>();
            configuration.CreateMap<UserAdditionalShippingAddressDto, UserAdditionalShippingAddress>();

            //ProductBrandingPriceVariants
            configuration.CreateMap<ProductBrandingPriceVariants, ProductBrandingPriceVariantsDto>();
            configuration.CreateMap<ProductBrandingPriceVariantsDto, ProductBrandingPriceVariants>();

            //CategoryGroup
            configuration.CreateMap<CategoryGroupMaster, CategoryGroupDto>();
            configuration.CreateMap<CategoryGroupDto, CategoryGroupMaster>();

            //CollectionCalculations
            configuration.CreateMap<CollectionCalculations, CollectionCalculationsDto>();
            configuration.CreateMap<CollectionCalculationsDto, CollectionCalculations>();


            //BrandingAdditionalPrice
            configuration.CreateMap<BrandingMethodAdditionalPrice, BrandingMethodAdditionalPriceDto>();
            configuration.CreateMap<BrandingMethodAdditionalPriceDto, BrandingMethodAdditionalPrice>();

            //BrandingFontStyle and FontType
            configuration.CreateMap<BrandingSpecificFontStyleMaster, BrandingSpecificFontStyleMasterDto>();
            configuration.CreateMap<BrandingSpecificFontStyleMasterDto, BrandingSpecificFontStyleMaster>();
            configuration.CreateMap<BrandingSpecificFontTypeFaceMaster, BrandingSpecificFontTypeFaceMasterDto>();
            configuration.CreateMap<BrandingSpecificFontTypeFaceMasterDto, BrandingSpecificFontTypeFaceMaster>();
            
            //EquipmentBranding
            configuration.CreateMap<EquipmentBrandingMaster, EquipmentBrandingMasterDto>();
            configuration.CreateMap<EquipmentBrandingMasterDto, EquipmentBrandingMaster>();
            configuration.CreateMap<EquipmentBrandingMaster, CreateOrUpdateEquipmentBranding>();
            configuration.CreateMap<CreateOrUpdateEquipmentBranding, EquipmentBrandingMaster>();
            //UniversalBranding
            configuration.CreateMap<UniversalBrandingMaster, UniversalBrandingMasterDto>();
            configuration.CreateMap<UniversalBrandingMasterDto, UniversalBrandingMaster>();
            configuration.CreateMap<UniversalBrandingMaster, CreateOrUpdateUniversalBranding>();
            configuration.CreateMap<CreateOrUpdateUniversalBranding, UniversalBrandingMaster>();
            //BannerLogo
            configuration.CreateMap<BannerPageTypeMaster, BannerPageTypeMasterDto>();
            configuration.CreateMap<BannerPageTypeMasterDto, BannerPageTypeMaster>();
            configuration.CreateMap<BannerPageTypeMaster, CreateOrUpdateBannerLogo>();
            configuration.CreateMap<CreateOrUpdateBannerLogo, BannerPageTypeMaster>();

            //UserBannerLogoData
            configuration.CreateMap<UserBannerLogoData, UserLogoBannerDataDto>();
            configuration.CreateMap<UserLogoBannerDataDto, UserBannerLogoData>();

            //ImageTypeMaster
            configuration.CreateMap<ImageTypeMaster, ImageTypeMasterDto>();
            configuration.CreateMap<ImageTypeMasterDto, ImageTypeMaster>();
            configuration.CreateMap<ImageTypeMaster, CreateOrUpdateImageTypeMaster>();
            configuration.CreateMap<CreateOrUpdateImageTypeMaster, ImageTypeMaster>();

            configuration.CreateMap<ProductVariantQuantityPrices, ProductVariantQuantityPricesDto>();
            configuration.CreateMap<ProductVariantQuantityPricesDto, ProductVariantQuantityPrices>();

            configuration.CreateMap<ArtWorkMaster, ArtworkDto>();
            configuration.CreateMap<ArtworkDto, ArtWorkMaster>();

            configuration.CreateMap<HexColorCodesMaster, HexColorCodesRawData>();
            configuration.CreateMap<HexColorCodesRawData, HexColorCodesMaster>();

            configuration.CreateMap<HexColorCodesMaster, HexColorCodesMasterDto>();
            configuration.CreateMap<HexColorCodesMasterDto, HexColorCodesMaster>();
            configuration.CreateMap<HexColorCodesMaster, CreateOrUpdateHexColors>();
            configuration.CreateMap<CreateOrUpdateHexColors, HexColorCodesMaster>();

            configuration.CreateMap<ECatalogueMaster, ECatalogueDto>();
            configuration.CreateMap<ECatalogueDto, ECatalogueMaster>();
            configuration.CreateMap<ECatalogueMaster, CreateOrUpdateECatalogue>();
            configuration.CreateMap<CreateOrUpdateECatalogue, ECatalogueMaster>();
        }
    }
}
