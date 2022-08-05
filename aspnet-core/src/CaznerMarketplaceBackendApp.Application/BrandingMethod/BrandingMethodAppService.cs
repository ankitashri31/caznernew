using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using CaznerMarketplaceBackendApp.AzureBlobStorage;
using CaznerMarketplaceBackendApp.BrandingMethod.Dto;
using CaznerMarketplaceBackendApp.BrandingMethods;
using CaznerMarketplaceBackendApp.MultiTenancy;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.Product.Dto;
using CaznerMarketplaceBackendApp.Product.Masters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaznerMarketplaceBackendApp.Authorization.Roles;
using Abp.Authorization.Users;
using CaznerMarketplaceBackendApp.Authorization.Users;
using Abp.Domain.Uow;
using CaznerMarketplaceBackendApp.Tag.Dto;
using FimApp.EntityFrameworkCore;
using Abp.Threading;
using CaznerMarketplaceBackendApp.ProductSize.Dto;
using CaznerMarketplaceBackendApp.EquipmentsBranding.Dto;
using CaznerMarketplaceBackendApp.UniversalBranding.Dto;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using CaznerMarketplaceBackendApp.Connections;

namespace CaznerMarketplaceBackendApp.BrandingMethod
{
    public class BrandingMethodAppService :
        AsyncCrudAppService<BrandingMethodMaster, BrandingMethodDto, long, BrandingMethodResultRequestDto, CreateOrUpdateBrandingMethod, BrandingMethodDto>, IBrandingMethodAppService
    {
        private readonly IRepository<BrandingMethodMaster, long> _repository;
        private readonly IRepository<BrandingMethodAdditionalPrice, long> _repositoryBrandingAdditionalPrice;
        private readonly IRepository<UniversalBrandingMaster, long> _repositoryBrandingUniversal;
        private readonly IRepository<EquipmentBrandingMaster, long> _repositoryBrandingEquipment;
        private readonly IRepository<ProductBrandingPriceVariants, long> _productBrandingPriceVariantsRepository;
        private readonly IRepository<ProductTagMaster, long> _productTagMasterRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ProductColourMaster, long> _repositoryProductColour;
        private readonly IRepository<BrandingSpecificFontStyleMaster, long> _repositoryBrandingSpecificFontStyleMaster;
        private readonly IRepository<BrandingSpecificFontTypeFaceMaster, long> _repositoryBrandingSpecificFontTypeFaceMaster;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<ProductSizeMaster, long> _repositoryProductSizeMaster;
        private readonly IRepository<BrandingMethodAssignedColors, long> _brandingMethodAssignedColorsRepository;
        private readonly IRepository<BrandingMethodAssignedEquipments, long> _brandingMethodAssignedEquipmentsRepository;
        private readonly IRepository<BrandingMethodAssignedFontStyles, long> _brandingMethodAssignedFontStylesRepository;
        private readonly IRepository<BrandingMethodAssignedFontTypeFaces, long> _brandingMethodAssignedFontTypeFacesRepository;
        private readonly IRepository<BrandingMethodAssignedTags, long> _brandingMethodAssignedTagsRepository;
        private readonly IRepository<BrandingMethodAssignedUniversals, long> _brandingMethodAssignedUniversalsRepository;
        private readonly IRepository<BrandingMethodAssignedVendors, long> _brandingMethodAssignedVendorRepository;
        private readonly IRepository<BrandingAdditionalQtyPrices, long> _brandingAdditionalQtyPricesRepository;
        private readonly IRepository<BrandingAdditionalQuantities, long> _brandingAdditionalQuantitiesRepository;
        private IConfiguration _configuration;
        private readonly IRepository<Tenant, int> _tenantManager;
        string AzureProductFolder = string.Empty;
        string AzureStorageUrl = string.Empty;
        private IDbConnection _db;
        private readonly DbConnectionUtility _connectionUtility;
        public BrandingMethodAppService(IRepository<BrandingMethodMaster, long> repository, IConfiguration configuration, IRepository<Tenant, int> tenantManager,
        IRepository<BrandingMethodAdditionalPrice, long> repositoryBrandingAdditionalPrice, IRepository<UniversalBrandingMaster, long> repositoryBrandingUniversal, IRepository<EquipmentBrandingMaster, long> repositoryBrandingEquipment,
        IRepository<ProductBrandingPriceVariants, long> productBrandingPriceVariantsRepository, IRepository<ProductTagMaster, long> productTagMasterRepository, IRepository<Role, int> roleRepository, IRepository<UserRole, long> userRoleRepository, IRepository<ProductDetails, long> productDetailsRepository, IRepository<User, long> userRepository,
        IRepository<ProductColourMaster, long> repositoryProductColour, IRepository<BrandingSpecificFontStyleMaster, long> repositoryBrandingSpecificFontStyleMaster, IRepository<BrandingSpecificFontTypeFaceMaster, long> repositoryBrandingSpecificFontTypeFaceMaster, IUnitOfWorkManager unitOfWorkManager,
        IRepository<BrandingMethodAssignedColors, long> brandingMethodAssignedColorsRepository, IRepository<BrandingMethodAssignedEquipments, long> brandingMethodAssignedEquipmentsRepository, IRepository<BrandingMethodAssignedFontStyles, long> brandingMethodAssignedFontStylesRepository, IRepository<BrandingMethodAssignedFontTypeFaces, long> brandingMethodAssignedFontTypeFacesRepository,
        IRepository<BrandingMethodAssignedTags, long> brandingMethodAssignedTagsRepository, IRepository<BrandingMethodAssignedUniversals, long> brandingMethodAssignedUniversalsRepository, IRepository<BrandingMethodAssignedVendors, long> brandingMethodAssignedVendorRepository,
        IRepository<ProductSizeMaster, long> repositoryProductSizeMaster, IRepository<BrandingAdditionalQtyPrices, long> brandingAdditionalQtyPricesRepository, IRepository<BrandingAdditionalQuantities, long> brandingAdditionalQuantitiesRepository, DbConnectionUtility connectionUtility) : base(repository)
        {
            _repository = repository;
            _repositoryBrandingAdditionalPrice = repositoryBrandingAdditionalPrice;
            _repositoryBrandingUniversal = repositoryBrandingUniversal;
            _repositoryBrandingEquipment = repositoryBrandingEquipment;
            _productBrandingPriceVariantsRepository = productBrandingPriceVariantsRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _productTagMasterRepository = productTagMasterRepository;
            _repositoryProductColour = repositoryProductColour;
            _repositoryBrandingSpecificFontStyleMaster = repositoryBrandingSpecificFontStyleMaster;
            _repositoryBrandingSpecificFontTypeFaceMaster = repositoryBrandingSpecificFontTypeFaceMaster;
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = configuration;
            _tenantManager = tenantManager;
            _repositoryProductSizeMaster = repositoryProductSizeMaster;
            AzureProductFolder = _configuration["FileUpload:AzureProductFolder"];
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];
            _brandingMethodAssignedColorsRepository = brandingMethodAssignedColorsRepository;
            _brandingMethodAssignedEquipmentsRepository = brandingMethodAssignedEquipmentsRepository;
            _brandingMethodAssignedFontStylesRepository = brandingMethodAssignedFontStylesRepository;
            _brandingMethodAssignedFontTypeFacesRepository = brandingMethodAssignedFontTypeFacesRepository;
            _brandingMethodAssignedTagsRepository = brandingMethodAssignedTagsRepository;
            _brandingMethodAssignedUniversalsRepository = brandingMethodAssignedUniversalsRepository;
            _brandingMethodAssignedVendorRepository = brandingMethodAssignedVendorRepository;
            _brandingAdditionalQtyPricesRepository = brandingAdditionalQtyPricesRepository;
            _brandingAdditionalQuantitiesRepository = brandingAdditionalQuantitiesRepository;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);
            _connectionUtility = connectionUtility;
        }

        public async Task<List<BrandingMethodDto>> GetBrandingMethodData(BrandingMethodResultRequestDto input)
        {
            int TenantId = AbpSession.TenantId.Value;
            List<BrandingMethodDto> ResponseData = new List<BrandingMethodDto>();
            try
            {

                await _connectionUtility.EnsureConnectionOpenAsync();

                IEnumerable<BrandingMethodDto> ResponseDataList = await _db.QueryAsync<BrandingMethodDto>(@"SELECT Id,MethodName,ImageUrl,Ext, Height,ImageName,Size, Type,Url,Width,Notes,IsActive,UniqueNumber
                                                              FROM[dbo].[BrandingMethodMaster] where IsDeleted = 0 and TenantId = " + TenantId + " Order by MethodName", new
                {
                }, commandType: System.Data.CommandType.Text);
                ResponseData = ResponseDataList.OrderBy(i=>i.MethodName).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return ResponseData;
        }


        public async Task<CreateBrandingMethodDto> CreateBrandingMethod(CreateBrandingMethodDto createBrandingMethoddto)
        {
            string FolderName = _configuration["FileUpload:FolderName"];
            string CurrentWebsiteUrl = _configuration.GetValue<string>("FileUpload:WebsireDomainPath");
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            CreateBrandingMethodDto output = new CreateBrandingMethodDto();
            var IsNumberExists = _repository.GetAllList(i => i.UniqueNumber == createBrandingMethoddto.UniqueNumber).FirstOrDefault();
            if (IsNumberExists == null)
            {
                long BrandingMethodId = 0;
                try
                {

                    var BrandingMaster = new BrandingMethodMaster();

                   
                    BrandingMaster.MethodDescripition = createBrandingMethoddto.MethodDescripition;
                    BrandingMaster.MethodName = createBrandingMethoddto.MethodName;
                    BrandingMaster.MethodSKU = createBrandingMethoddto.MethodSKU;
                    BrandingMaster.IsMethodHasAdditionalPriceVariant = createBrandingMethoddto.IsMethodHasAdditionalPriceVariant;
                    BrandingMaster.IsMethodHasQuantityPriceVariant = createBrandingMethoddto.IsMethodHasQuantityPriceVariant;
                    BrandingMaster.IsSpecificFontStyle = createBrandingMethoddto.IsSpecificFontStyle;
                    BrandingMaster.IsSpecificFontTypeFace = createBrandingMethoddto.IsSpecificFontTypeFace;
                    BrandingMaster.IsChargeTaxOnThis = createBrandingMethoddto.IsChargeTaxOnThis;
                    BrandingMaster.Height = createBrandingMethoddto.Height;
                    BrandingMaster.ColorSelectionType = createBrandingMethoddto.ColorSelectionType;
                    BrandingMaster.NumberOfStiches = string.IsNullOrEmpty(createBrandingMethoddto.NumberOfStiches)? 0 : Convert.ToInt32(createBrandingMethoddto.NumberOfStiches);
                    BrandingMaster.Width = createBrandingMethoddto.Width;
                    BrandingMaster.MeasurementId = createBrandingMethoddto.MeasurementId;
                    BrandingMaster.IsActive = createBrandingMethoddto.IsActive;
                    BrandingMaster.Notes = createBrandingMethoddto.Notes;
                    BrandingMaster.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    BrandingMaster.Profit = createBrandingMethoddto.Profit == "" ? 0 : Convert.ToDecimal(createBrandingMethoddto.Profit);
                    BrandingMaster.UnitPrice = createBrandingMethoddto.UnitPrice == "" ? 0 : Convert.ToDecimal(createBrandingMethoddto.UnitPrice);
                    BrandingMaster.CostPerItem = createBrandingMethoddto.CostPerItem == "" ? 0 : Convert.ToDecimal(createBrandingMethoddto.CostPerItem);
                    BrandingMaster.UniqueNumber = createBrandingMethoddto.UniqueNumber;
                    #region branding method image
                    if (createBrandingMethoddto.ImageObj != null)
                    {
                        string ImageLocation = AzureStorageUrl + folderPath + createBrandingMethoddto.ImageObj.ImageName;
                        BrandingMaster.ImageName = createBrandingMethoddto.ImageObj.ImageName;
                        BrandingMaster.Ext = createBrandingMethoddto.ImageObj.Ext;
                        BrandingMaster.Name = createBrandingMethoddto.ImageObj.Name;
                        BrandingMaster.Size = createBrandingMethoddto.ImageObj.Size;
                        BrandingMaster.Url = ImageLocation;
                        BrandingMaster.Type = createBrandingMethoddto.ImageObj.Type;
                        BrandingMaster.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        BrandingMaster.ImageUrl = ImageLocation;
                        BrandingMaster.Url = ImageLocation;
                    }
                    #endregion

                    BrandingMethodId = await _repository.InsertAndGetIdAsync(BrandingMaster);
                    if (BrandingMethodId != 0)
                    {

                        #region BrandingPriceVariants

                        if (createBrandingMethoddto.ProductBrandingPriceVariants != null)
                        {
                            List<ProductBrandingPriceVariants> productBrandingVariantList = ObjectMapper.Map<List<ProductBrandingPriceVariants>>(createBrandingMethoddto.ProductBrandingPriceVariants.ToList());
                            foreach (var brandingVariantitem in productBrandingVariantList)
                            {
                                brandingVariantitem.BrandingMethodId = BrandingMethodId;
                                brandingVariantitem.IsActive = true;
                                brandingVariantitem.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                long ProductBrandingVariantId = await _productBrandingPriceVariantsRepository.InsertAndGetIdAsync(brandingVariantitem);
                            }
                        }
                        #endregion


                        #region BrandingMethod details dropdown bindings
                        #region TagsArray
                        if (createBrandingMethoddto.BrandingMethodDetails.BrandingTagArray != null)
                        {
                            List<BrandingMethodAssignedTags> BrandingTagsData = new List<BrandingMethodAssignedTags>();

                            foreach (var tags in createBrandingMethoddto.BrandingMethodDetails.BrandingTagArray)
                            {
                                BrandingMethodAssignedTags TagsEntity = new BrandingMethodAssignedTags();
                                TagsEntity.BrandingMethodId = BrandingMethodId;
                                TagsEntity.TagId = tags.Id;
                                TagsEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                TagsEntity.IsActive = true;
                                BrandingTagsData.Add(TagsEntity);
                            }
                            if (BrandingTagsData.Count > 0)
                            {
                                await _brandingMethodAssignedTagsRepository.BulkInsertAsync(BrandingTagsData);
                            }
                        }

                        #endregion
                        #region VendorsArray
                        if (createBrandingMethoddto.BrandingMethodDetails.BrandingVendorArray != null)
                        {
                            List<BrandingMethodAssignedVendors> BrandingVendorsData = new List<BrandingMethodAssignedVendors>();
                            foreach (var vendors in createBrandingMethoddto.BrandingMethodDetails.BrandingVendorArray)
                            {
                                BrandingMethodAssignedVendors VendorsEntity = new BrandingMethodAssignedVendors();
                                VendorsEntity.BrandingMethodId = BrandingMethodId;
                                VendorsEntity.VendorUserId = vendors.Id;
                                VendorsEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                VendorsEntity.IsActive = true;
                                BrandingVendorsData.Add(VendorsEntity);
                            }
                            if (BrandingVendorsData.Count > 0)
                            {
                                await _brandingMethodAssignedVendorRepository.BulkInsertAsync(BrandingVendorsData);
                            }
                        }

                        #endregion
                        #region ColorArray
                        if (createBrandingMethoddto.BrandingMethodDetails.BrandingColorArray != null)
                        {
                            List<BrandingMethodAssignedColors> BrandingColorData = new List<BrandingMethodAssignedColors>();
                            foreach (var Color in createBrandingMethoddto.BrandingMethodDetails.BrandingColorArray)
                            {
                                BrandingMethodAssignedColors ColorEntity = new BrandingMethodAssignedColors();
                                ColorEntity.BrandingMethodId = BrandingMethodId;
                                ColorEntity.ColorId = Color.Id;
                                ColorEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                ColorEntity.IsActive = true;
                                BrandingColorData.Add(ColorEntity);
                            }
                            if (BrandingColorData.Count > 0)
                            {
                                await _brandingMethodAssignedColorsRepository.BulkInsertAsync(BrandingColorData);
                            }
                        }

                        #endregion
                        #region EquipmentArray
                        if (createBrandingMethoddto.BrandingMethodDetails.BrandingEquipmentArray != null)
                        {
                            List<BrandingMethodAssignedEquipments> BrandingEquipmentData = new List<BrandingMethodAssignedEquipments>();
                            foreach (var Equipment in createBrandingMethoddto.BrandingMethodDetails.BrandingEquipmentArray)
                            {
                                BrandingMethodAssignedEquipments EquipmentEntity = new BrandingMethodAssignedEquipments();
                                EquipmentEntity.BrandingMethodId = BrandingMethodId;
                                EquipmentEntity.EquipmentBrandingId = Equipment.Id;
                                EquipmentEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                EquipmentEntity.IsActive = true;
                                BrandingEquipmentData.Add(EquipmentEntity);
                            }
                            if (BrandingEquipmentData.Count > 0)
                            {
                                await _brandingMethodAssignedEquipmentsRepository.BulkInsertAsync(BrandingEquipmentData);
                            }
                        }

                        #endregion
                        #region UniversalArray
                        if (createBrandingMethoddto.BrandingMethodDetails.UniversalBrandingArray != null)
                        {
                            List<BrandingMethodAssignedUniversals> UniversalData = new List<BrandingMethodAssignedUniversals>();
                            foreach (var Universal in createBrandingMethoddto.BrandingMethodDetails.UniversalBrandingArray)
                            {
                                BrandingMethodAssignedUniversals UniversalEntity = new BrandingMethodAssignedUniversals();
                                UniversalEntity.BrandingMethodId = BrandingMethodId;
                                UniversalEntity.UniversalId = Universal.Id;
                                UniversalEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                UniversalEntity.IsActive = true;
                                UniversalData.Add(UniversalEntity);
                            }
                            if (UniversalData.Count > 0)
                            {
                                await _brandingMethodAssignedUniversalsRepository.BulkInsertAsync(UniversalData);
                            }
                        }

                        #endregion
                        #region Font Style Array
                        if (createBrandingMethoddto.BrandingMethodDetails.SpecificFontStyleArray != null)
                        {
                            List<BrandingMethodAssignedFontStyles> FontStylesData = new List<BrandingMethodAssignedFontStyles>();
                            foreach (var FontStyle in createBrandingMethoddto.BrandingMethodDetails.SpecificFontStyleArray)
                            {
                                BrandingMethodAssignedFontStyles FontStylesEntity = new BrandingMethodAssignedFontStyles();
                                FontStylesEntity.BrandingMethodId = BrandingMethodId;
                                FontStylesEntity.BrandingSpecificFontStyleId = FontStyle.Id;
                                FontStylesEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                FontStylesEntity.IsActive = true;
                                FontStylesData.Add(FontStylesEntity);
                            }
                            if (FontStylesData.Count > 0)
                            {
                                await _brandingMethodAssignedFontStylesRepository.BulkInsertAsync(FontStylesData);
                            }
                        }

                        #endregion
                        #region FontTypeFace Array
                        if (createBrandingMethoddto.BrandingMethodDetails.SpecificFontTypeFaceArray != null)
                        {
                            List<BrandingMethodAssignedFontTypeFaces> BrandingMethodFontTypeFaceData = new List<BrandingMethodAssignedFontTypeFaces>();
                            foreach (var fontTypeFaces in createBrandingMethoddto.BrandingMethodDetails.SpecificFontTypeFaceArray)
                            {
                                BrandingMethodAssignedFontTypeFaces FontTypeFaceEntity = new BrandingMethodAssignedFontTypeFaces();
                                FontTypeFaceEntity.BrandingMethodId = BrandingMethodId;
                                FontTypeFaceEntity.BrandingSpecificFontTypeFaceId = fontTypeFaces.Id;
                                FontTypeFaceEntity.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                FontTypeFaceEntity.IsActive = true;
                                BrandingMethodFontTypeFaceData.Add(FontTypeFaceEntity);
                            }
                            if (BrandingMethodFontTypeFaceData.Count > 0)
                            {
                                await _brandingMethodAssignedFontTypeFacesRepository.BulkInsertAsync(BrandingMethodFontTypeFaceData);
                            }
                        }

                        #endregion

                        #endregion
                    }
                    output.Id = BrandingMethodId;
                    return output;
                }
                catch (Exception ex)
                {

                }
                return output;
            }
            else
            {
                throw new AbpAuthorizationException("Unique number already exists");
            }
        }

        public async Task<BrandingMethodMasterDataDto> GetBrandingMasterdata()
        {
            int TenantId = AbpSession.TenantId.Value;
            BrandingMethodMasterDataDto ResponseModel = new BrandingMethodMasterDataDto();

            await _connectionUtility.EnsureConnectionOpenAsync();

            IEnumerable<ProductTagDto> ProductTagData = await _db.QueryAsync<ProductTagDto>(@"SELECT Id,ProductTagName as 'Code',ProductTagName,IsActive
              FROM [dbo].[ProductTagMaster] where IsDeleted = 0 and TenantId = " + TenantId + "", new
            {

            }, commandType: System.Data.CommandType.Text);


            IEnumerable<UniversalBrandingMasterDto> UniversalBrandingData = await _db.QueryAsync<UniversalBrandingMasterDto>(@"SELECT Id,UniversalBrandingTitle as 'Code',UniversalBrandingTitle,IsActive
              FROM [dbo].[UniversalBrandingMaster] where IsDeleted = 0 and TenantId = " + TenantId + "", new
            {

            }, commandType: System.Data.CommandType.Text);



            IEnumerable<EquipmentBrandingMasterDto> EquipmentBrandingData = await _db.QueryAsync<EquipmentBrandingMasterDto>(@"SELECT Id,EquipmentTitle as 'Code',EquipmentTitle,IsActive
              FROM [dbo].[EquipmentBrandingMaster] where IsDeleted = 0 ", new
            {

            }, commandType: System.Data.CommandType.Text);


            IEnumerable<BrandingSpecificFontStyleMasterDto> BrandingSpecificFontStyleData = await _db.QueryAsync<BrandingSpecificFontStyleMasterDto>(@"SELECT Id,FontStyleTitle as 'Code',FontStyleTitle,IsActive
              FROM [dbo].[BrandingSpecificFontStyleMaster] where IsDeleted = 0 and TenantId = " + TenantId + "", new
            {

            }, commandType: System.Data.CommandType.Text);


            IEnumerable<BrandingSpecificFontTypeFaceMasterDto> BrandingSpecificFontTypeFaceData = await _db.QueryAsync<BrandingSpecificFontTypeFaceMasterDto>(@"SELECT Id,FontTypeFaceTitle as 'Code',FontTypeFaceTitle,IsActive
              FROM [dbo].[BrandingSpecificFontTypeFaceMaster] where IsDeleted = 0 ", new
            {

            }, commandType: System.Data.CommandType.Text);


            IEnumerable<ProductSizeDto> MeasurementSizeData = await _db.QueryAsync<ProductSizeDto>(@"SELECT Id,ProductSizeName as  'Code',ProductSizeName,IsActive
              FROM [dbo].[ProductSizeMaster] where IsDeleted = 0 and TenantId = " + TenantId + "  and (ProductSizeName) != 'kg' and (ProductSizeName) != 'gr'", new
            {

            }, commandType: System.Data.CommandType.Text);

            var ColorSelectionType = Enum.GetValues(typeof(AppConsts.ColorSelectionType)).Cast<AppConsts.ColorSelectionType>().Take(2).ToList();
            var enumValues = Enum.GetValues(typeof(AppConsts.ColorSelectionType)).Cast<AppConsts.ColorSelectionType>()
                                                         .Select(e => new KeyValuePair<int, string>((int)e, e.ToString()));


            var BrandingColorData = (from Type in enumValues
                                     select new ColorSelection
                                     {
                                         Id = Convert.ToInt32(Type.Key),
                                         SelectionTypeTitle = Type.Value,
                                         Code = Type.Value,
                                         BrandingColorData = (from BrandingColor in _repositoryProductColour.GetAll()
                                                              where Type.Value == "Color"
                                                              select new ProductColourMasterDto
                                                              {
                                                                  Id = BrandingColor.Id,
                                                                  Code = BrandingColor.ProductColourName,
                                                                  ProductColourName = BrandingColor.ProductColourName,
                                                                  IsActive = BrandingColor.IsActive
                                                              }).ToList()
                                     }).ToList();


            ResponseModel.UniversalBrandingData = UniversalBrandingData.ToList();
            ResponseModel.EquipmenBrandingData = EquipmentBrandingData.ToList();
            ResponseModel.ProductTagData = ProductTagData.ToList();
            ResponseModel.BrandingColorData = BrandingColorData;
            ResponseModel.SupplierListData = await GetSupplierList();
            ResponseModel.BrandingSpecificFontStyleData = BrandingSpecificFontStyleData.ToList();
            ResponseModel.BrandingSpecificFontTypeFaceData = BrandingSpecificFontTypeFaceData.ToList();
            ResponseModel.MeasurementSizeData = MeasurementSizeData.ToList();
            return ResponseModel;
        }

        #region Get Supplier User List
        private async Task<List<SupplierListDto>> GetSupplierList()
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

        #region Get branding method by id

        public async Task<BrandingMethodModel> GetBrandingMethodById(long Id)
        {
            BrandingMethodModel Response = new BrandingMethodModel();
            try
            {
                var MethodData = _repository.GetAllList(i => i.Id == Id).FirstOrDefault();
                Response = ObjectMapper.Map<BrandingMethodModel>(MethodData);
                int TenantId = AbpSession.TenantId.Value;
                Response.Profit = MethodData.Profit > 0 ? MethodData.Profit.ToString() : "";
                Response.UnitPrice = MethodData.UnitPrice > 0 ? MethodData.UnitPrice.ToString() : "";
                Response.CostPerItem = MethodData.CostPerItem > 0 ? MethodData.CostPerItem.ToString() : "";
                if (MethodData.MeasurementId.HasValue)
                {
                    Response.MeasurementTitle = _repositoryProductSizeMaster.GetAllList(i => i.Id == MethodData.MeasurementId).Select(i => i.ProductSizeName).FirstOrDefault();
                }
                await _connectionUtility.EnsureConnectionOpenAsync();
                string SelectionTitle = Enum.GetName(typeof(AppConsts.ColorSelectionType), MethodData.ColorSelectionType);
                IEnumerable<BrandingCombo> UniversalBrandingArrayLst = await _db.QueryAsync<BrandingCombo>(@"select Universal.Id as'Id', Universal.UniversalBrandingTitle as 'Name', Universal.UniversalBrandingTitle as 'Code',
                assign.Id as 'AssignmentId' from UniversalBrandingMaster Universal 
                join BrandingMethodAssignedUniversals assign on 
                Universal.Id = assign.UniversalId where assign.BrandingMethodId = " + Id + " and assign.IsDeleted = 0 and assign.TenantId = " + TenantId + "", new
                {

                }, commandType: System.Data.CommandType.Text);
                IEnumerable<BrandingCombo> BrandingColorArraylst = await _db.QueryAsync<BrandingCombo>(@"select Id = Color.Id,Name = Color.ProductColourName,Code = Color.ProductColourName,
                AssignmentId = assign.Id from ProductColourMaster Color 
                join  BrandingMethodAssignedColors assign 
                on Color.Id = assign.ColorId where assign.BrandingMethodId = " + Id + " and assign.IsDeleted = 0 and assign.TenantId = " + TenantId + "", new
                {

                }, commandType: System.Data.CommandType.Text);
                IEnumerable<BrandingCombo> BrandingEquipmentArrayLst = await _db.QueryAsync<BrandingCombo>(@"select Id = Equipment.Id,Name = Equipment.EquipmentTitle,Code = Equipment.EquipmentTitle,AssignmentId = assign.Id
                from EquipmentBrandingMaster  Equipment join BrandingMethodAssignedEquipments assign on 
                Equipment.Id = assign.EquipmentBrandingId where assign.BrandingMethodId = " + Id + " and assign.IsDeleted = 0 and assign.TenantId = " + TenantId + "", new
                {

                }, commandType: System.Data.CommandType.Text);
                IEnumerable<BrandingCombo> BrandingTagArrayLst = await _db.QueryAsync<BrandingCombo>(@"select Id = Tag.Id,Name = Tag.ProductTagName,Code = Tag.ProductTagName,AssignmentId = assign.Id
                from ProductTagMaster Tag join BrandingMethodAssignedTags assign on Tag.Id = assign.TagId where assign.BrandingMethodId = " + Id + " and assign.IsDeleted = 0 and assign.TenantId = " + TenantId + "", new
                {

                }, commandType: System.Data.CommandType.Text);
                IEnumerable<BrandingCombo> SpecificFontStyleArrayLst = await _db.QueryAsync<BrandingCombo>(@"select Id = Font.Id,Name = Font.FontStyleTitle,Code = Font.FontStyleTitle,AssignmentId = assign.Id
                from BrandingSpecificFontStyleMaster Font join BrandingMethodAssignedFontStyles assign on 
                Font.Id = assign.BrandingSpecificFontStyleId where assign.BrandingMethodId = " + Id + " and assign.IsDeleted = 0 and assign.TenantId = " + TenantId + "", new
                {

                }, commandType: System.Data.CommandType.Text);
                IEnumerable<BrandingCombo> SpecificFontTypeFaceArrayLst = await _db.QueryAsync<BrandingCombo>(@"select Id = FontFace.Id,Name = FontFace.FontTypeFaceTitle,Code = FontFace.FontTypeFaceTitle,AssignmentId = assign.Id
                from BrandingSpecificFontTypeFaceMaster FontFace join BrandingMethodAssignedFontTypeFaces assign 
                on FontFace.Id = assign.BrandingSpecificFontTypeFaceId where assign.BrandingMethodId = " + Id + " and assign.IsDeleted = 0 and assign.TenantId = " + TenantId + "", new
                {

                }, commandType: System.Data.CommandType.Text);
                IEnumerable<ProductBrandingPriceVariantsDto> ProductBrandingPriceVariantsList = await _db.QueryAsync<ProductBrandingPriceVariantsDto>(@"select Id = price.Id,BrandingUnitPrice = price.BrandingUnitPrice,Price = price.Price,Price1 = price.Price1,Price2 = price.Price2,
                Price3 = price.Price3,Quantity = price.Quantity,BrandingMethodId = price.BrandingMethodId
                from ProductBrandingPriceVariants price  where price.BrandingMethodId = " + Id + " and IsDeleted = 0 and TenantId = " + TenantId + "", new
                {

                }, commandType: System.Data.CommandType.Text);

                Response.BrandingMethodDetails =  new BrandingDetailComboDto
                                                  {
                                                      Id = Id,
                                                      UniversalBrandingArray = UniversalBrandingArrayLst.ToArray(),
                                                      BrandingColorArray = BrandingColorArraylst.ToArray(),
                                                      BrandingEquipmentArray = BrandingEquipmentArrayLst.ToArray(),
                                                      BrandingTagArray = BrandingTagArrayLst.ToArray(),
                                                      BrandingVendorArray = (from Vendor in _userRepository.GetAllList()
                                                                             join assign in _brandingMethodAssignedVendorRepository.GetAll() on Vendor.Id equals assign.VendorUserId
                                                                             where assign.BrandingMethodId == Id
                                                                             select new BrandingCombo
                                                                             {
                                                                                 Id = Vendor.Id,
                                                                                 Name = Vendor.FullName,
                                                                                 Code = Vendor.FullName,
                                                                                 AssignmentId = assign.Id
                                                                             }).ToArray(),
                                                      SpecificFontStyleArray = SpecificFontStyleArrayLst.ToArray(),
                                                      SpecificFontTypeFaceArray = SpecificFontTypeFaceArrayLst.ToArray(),
                                                      ColorSelectionType = (new ColorSelection
                                                      {
                                                          Id = MethodData.ColorSelectionType,
                                                          SelectionTypeTitle = SelectionTitle,
                                                          Code = SelectionTitle,
                                                          BrandingColorData = string.IsNullOrEmpty(SelectionTitle) ? new List<ProductColourMasterDto>() : (from BrandingColor in _repositoryProductColour.GetAll()
                                                                                                                                                           where SelectionTitle.ToLower() == "color"
                                                                                                                                                           select new ProductColourMasterDto
                                                                                                                                                           {
                                                                                                                                                               Id = BrandingColor.Id,
                                                                                                                                                               Code = BrandingColor.ProductColourName,
                                                                                                                                                               ProductColourName = BrandingColor.ProductColourName,
                                                                                                                                                               IsActive = BrandingColor.IsActive
                                                                                                                                                           }).ToList()

                                                      }),

                                                      BrandingMethodId = Id
                };

                Response.ImageObj = new ImageObj
                {
                    Ext = MethodData.Ext,
                    ImageName = MethodData.ImageName,
                    Name = MethodData.Name,
                    Size = MethodData.Size,
                    Type = MethodData.Type,
                    Url = MethodData.Url
                };

                Response.ProductBrandingPriceVariants = ProductBrandingPriceVariantsList.ToList();

                Response.BrandingMethodAdditionalPrice = (from qty in _brandingAdditionalQuantitiesRepository.GetAllList()
                                                          where qty.BrandingMethodId == Id
                                                          select new BrandingAdditionalModel
                                                          {
                                                              Quantity = qty.Quantity,
                                                              PricesObj = (from price in _brandingAdditionalQtyPricesRepository.GetAll()
                                                                           join quantity in _brandingAdditionalQuantitiesRepository.GetAll() on price.AdditionalQtyId equals quantity.Id
                                                                           where quantity.BrandingMethodId == Id
                                                                           select new BrandingAdditionalPriceDto
                                                                           {
                                                                               Id = price.Id,
                                                                               Price = price.Price,
                                                                               AssignmentPriceId = price.Id
                                                                           }).ToArray()
                                                          }).ToList();

            }
            catch (Exception ex)
            {

            }
            return Response;
        }

        #endregion
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

        public async Task UpdateBrandingMethodById(CreateBrandingMethodDto createBrandingMethoddto)
        {
            string FolderName = _configuration["FileUpload:FolderName"];
            string CurrentWebsiteUrl = _configuration.GetValue<string>("FileUpload:WebsireDomainPath");
            int TenantId = AbpSession.TenantId.Value;
            var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
            string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
            var BrandingMaster = _repository.GetAllList(x => x.Id == createBrandingMethoddto.Id).FirstOrDefault();
            try
            {
                if (BrandingMaster != null)
                {
                    BrandingMaster.Profit = createBrandingMethoddto.Profit == "" ? 0 : Convert.ToDecimal(createBrandingMethoddto.Profit);
                    BrandingMaster.UnitPrice = createBrandingMethoddto.UnitPrice == "" ? 0 : Convert.ToDecimal(createBrandingMethoddto.UnitPrice);
                    BrandingMaster.CostPerItem = createBrandingMethoddto.CostPerItem == "" ? 0 : Convert.ToDecimal(createBrandingMethoddto.CostPerItem);
                    BrandingMaster.IsActive = createBrandingMethoddto.IsActive;
                    BrandingMaster.MethodName = createBrandingMethoddto.MethodName;
                    BrandingMaster.MethodSKU = createBrandingMethoddto.MethodSKU;
                    BrandingMaster.MethodDescripition = createBrandingMethoddto.MethodDescripition;
                    BrandingMaster.IsChargeTaxOnThis = createBrandingMethoddto.IsChargeTaxOnThis;
                    BrandingMaster.IsMethodHasQuantityPriceVariant = createBrandingMethoddto.IsMethodHasQuantityPriceVariant;
                    BrandingMaster.IsMethodHasAdditionalPriceVariant = createBrandingMethoddto.IsMethodHasAdditionalPriceVariant;
                    BrandingMaster.Height = createBrandingMethoddto.Height;
                    BrandingMaster.Width = createBrandingMethoddto.Width;
                    BrandingMaster.Notes = createBrandingMethoddto.Notes;
                    BrandingMaster.ColorSelectionType = createBrandingMethoddto.ColorSelectionType;
                    BrandingMaster.NumberOfStiches = string.IsNullOrEmpty(createBrandingMethoddto.NumberOfStiches) ? 0 : Convert.ToInt32(createBrandingMethoddto.NumberOfStiches);
                    BrandingMaster.IsSpecificFontStyle = createBrandingMethoddto.IsSpecificFontStyle;
                    BrandingMaster.IsSpecificFontTypeFace = createBrandingMethoddto.IsSpecificFontTypeFace;
                    BrandingMaster.UniqueNumber = createBrandingMethoddto.UniqueNumber;
                    BrandingMaster.MeasurementId = createBrandingMethoddto.MeasurementId;
                    #region branding method image
                    if (createBrandingMethoddto.ImageObj != null)
                    {
                        string ImageLocation = AzureStorageUrl + folderPath + createBrandingMethoddto.ImageObj.ImageName;
                        BrandingMaster.ImageName = createBrandingMethoddto.ImageObj.ImageName;
                        BrandingMaster.Ext = createBrandingMethoddto.ImageObj.Ext;
                        BrandingMaster.Name = createBrandingMethoddto.ImageObj.Name;
                        BrandingMaster.Size = createBrandingMethoddto.ImageObj.Size;
                        BrandingMaster.Url = ImageLocation;
                        BrandingMaster.Type = createBrandingMethoddto.ImageObj.Type;
                        BrandingMaster.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        BrandingMaster.ImageUrl = ImageLocation;
                        BrandingMaster.Url = ImageLocation;
                    }
                    #endregion

                    await _repository.UpdateAsync(BrandingMaster);


                    #region BrandingPriceVariants

                    //ProductBrandingPriceVariants

                    if (createBrandingMethoddto.ProductBrandingPriceVariants != null)
                    {
                        var ExistingData = _productBrandingPriceVariantsRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id);
                        var OldData = createBrandingMethoddto.ProductBrandingPriceVariants.Where(i => i.Id > 0);
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteProductBrandingVariantAsync(DeletedData, TenantId);
                        }

                        foreach (var item in createBrandingMethoddto.ProductBrandingPriceVariants)
                        {
                            if (item.Id > 0)
                            {
                                var productBrandingVariant = _productBrandingPriceVariantsRepository.GetAllList(x => x.Id == item.Id).FirstOrDefault();
                                if (productBrandingVariant != null)
                                {
                                    productBrandingVariant.BrandingMethodId = item.BrandingMethodId;
                                    productBrandingVariant.Quantity = item.Quantity;
                                    productBrandingVariant.Price = item.Price;
                                    productBrandingVariant.Price1 = item.Price1;
                                    productBrandingVariant.Price2 = item.Price2;
                                    productBrandingVariant.Price3 = item.Price3;
                                    productBrandingVariant.IsActive = item.IsActive;
                                    await _productBrandingPriceVariantsRepository.UpdateAsync(productBrandingVariant);
                                }
                            }
                            else
                            {
                                ProductBrandingPriceVariants Model = new ProductBrandingPriceVariants();
                                Model.Quantity = item.Quantity;
                                Model.BrandingMethodId = item.BrandingMethodId;
                                Model.Price = item.Price;
                                Model.Price1 = item.Price1;
                                Model.Price2 = item.Price2;
                                Model.Price3 = item.Price3;
                                Model.IsActive = item.IsActive;
                                Model.BrandingMethodId = createBrandingMethoddto.Id;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                await _productBrandingPriceVariantsRepository.InsertAsync(Model);
                            }

                        }
                    }
                    else
                    {
                        var ExistingData = _productBrandingPriceVariantsRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeleteProductBrandingVariantAsync(ExistingData, TenantId);
                        }
                    }


                    #endregion
                    #region Branding Method Additional Price
                        

                  
                    #endregion

                    #region BrandingMethodDetails
                    #region New Update Code For Insertion 
                    #region Color Array Update
                    if (createBrandingMethoddto.BrandingMethodDetails.BrandingColorArray != null)
                    {
                        var ExistingData = _brandingMethodAssignedColorsRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id);
                        var OldData = createBrandingMethoddto.BrandingMethodDetails.BrandingColorArray.Where(i => i.AssignmentId > 0);
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedColorAsync(DeletedData, TenantId);
                        }

                        foreach (var item in createBrandingMethoddto.BrandingMethodDetails.BrandingColorArray)
                        {
                            if (item.AssignmentId > 0)
                            {
                                var AssignedColors = _brandingMethodAssignedColorsRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                if (AssignedColors != null)
                                {
                                    AssignedColors.ColorId = item.Id;
                                    await _brandingMethodAssignedColorsRepository.UpdateAsync(AssignedColors);
                                }
                            }
                            else
                            {
                                BrandingMethodAssignedColors Model = new BrandingMethodAssignedColors();
                                Model.ColorId = item.Id;
                                Model.BrandingMethodId = createBrandingMethoddto.Id;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                await _brandingMethodAssignedColorsRepository.InsertAsync(Model);
                            }

                        }
                    }
                    else
                    {
                        var ExistingData = _brandingMethodAssignedColorsRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedColorAsync(ExistingData, TenantId);
                        }
                    }
                    #endregion
                    #region Equipment Array Update
                    if (createBrandingMethoddto.BrandingMethodDetails.BrandingEquipmentArray != null)
                    {
                        var ExistingData = _brandingMethodAssignedEquipmentsRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id);
                        var OldData = createBrandingMethoddto.BrandingMethodDetails.BrandingEquipmentArray.Where(i => i.AssignmentId > 0);
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedEquipmentsAsync(DeletedData, TenantId);
                        }

                        foreach (var item in createBrandingMethoddto.BrandingMethodDetails.BrandingEquipmentArray)
                        {
                            if (item.AssignmentId > 0)
                            {
                                var AssignedEquipments = _brandingMethodAssignedEquipmentsRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                if (AssignedEquipments != null)
                                {
                                    AssignedEquipments.EquipmentBrandingId = item.Id;
                                    await _brandingMethodAssignedEquipmentsRepository.UpdateAsync(AssignedEquipments);
                                }
                            }
                            else
                            {
                                BrandingMethodAssignedEquipments Model = new BrandingMethodAssignedEquipments();
                                Model.EquipmentBrandingId = item.Id;
                                Model.BrandingMethodId = createBrandingMethoddto.Id;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                await _brandingMethodAssignedEquipmentsRepository.InsertAsync(Model);
                            }

                        }
                    }
                    else
                    {
                        var ExistingData = _brandingMethodAssignedEquipmentsRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedEquipmentsAsync(ExistingData, TenantId);
                        }
                    }
                    #endregion
                    #region FontStyles Array Update
                    if (createBrandingMethoddto.BrandingMethodDetails.SpecificFontStyleArray != null)
                    {
                        var ExistingData = _brandingMethodAssignedFontStylesRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id);
                        var OldData = createBrandingMethoddto.BrandingMethodDetails.SpecificFontStyleArray.Where(i => i.AssignmentId > 0);
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedFontStylesAsync(DeletedData, TenantId);
                        }

                        foreach (var item in createBrandingMethoddto.BrandingMethodDetails.SpecificFontStyleArray)
                        {
                            if (item.AssignmentId > 0)
                            {
                                var AssignedFontStyles = _brandingMethodAssignedFontStylesRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                if (AssignedFontStyles != null)
                                {
                                    AssignedFontStyles.BrandingSpecificFontStyleId = item.Id;
                                    await _brandingMethodAssignedFontStylesRepository.UpdateAsync(AssignedFontStyles);
                                }
                            }
                            else
                            {
                                BrandingMethodAssignedFontStyles Model = new BrandingMethodAssignedFontStyles();
                                Model.BrandingSpecificFontStyleId = item.Id;
                                Model.BrandingMethodId = createBrandingMethoddto.Id;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                await _brandingMethodAssignedFontStylesRepository.InsertAsync(Model);
                            }

                        }
                    }
                    else
                    {
                        var ExistingData = _brandingMethodAssignedFontStylesRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedFontStylesAsync(ExistingData, TenantId);
                        }
                    }
                    #endregion
                    #region FontFaces Array Update
                    if (createBrandingMethoddto.BrandingMethodDetails.SpecificFontTypeFaceArray != null)
                    {
                        var ExistingData = _brandingMethodAssignedFontTypeFacesRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id);
                        var OldData = createBrandingMethoddto.BrandingMethodDetails.SpecificFontTypeFaceArray.Where(i => i.AssignmentId > 0);
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedFontTypeFacesAsync(DeletedData, TenantId);
                        }

                        foreach (var item in createBrandingMethoddto.BrandingMethodDetails.SpecificFontTypeFaceArray)
                        {
                            if (item.AssignmentId > 0)
                            {
                                var AssignedFontTypeFaces = _brandingMethodAssignedFontTypeFacesRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                if (AssignedFontTypeFaces != null)
                                {
                                    AssignedFontTypeFaces.BrandingSpecificFontTypeFaceId = item.Id;
                                    await _brandingMethodAssignedFontTypeFacesRepository.UpdateAsync(AssignedFontTypeFaces);
                                }
                            }
                            else
                            {
                                BrandingMethodAssignedFontTypeFaces Model = new BrandingMethodAssignedFontTypeFaces();
                                Model.BrandingSpecificFontTypeFaceId = item.Id;
                                Model.BrandingMethodId = createBrandingMethoddto.Id;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                await _brandingMethodAssignedFontTypeFacesRepository.InsertAsync(Model);
                            }

                        }
                    }
                    else
                    {
                        var ExistingData = _brandingMethodAssignedFontTypeFacesRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedFontTypeFacesAsync(ExistingData, TenantId);
                        }
                    }
                    #endregion
                    #region Tags Array Update
                    if (createBrandingMethoddto.BrandingMethodDetails.BrandingTagArray != null)
                    {
                        var ExistingData = _brandingMethodAssignedTagsRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id);
                        var OldData = createBrandingMethoddto.BrandingMethodDetails.BrandingTagArray.Where(i => i.AssignmentId > 0);
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedTagsAsync(DeletedData, TenantId);
                        }

                        foreach (var item in createBrandingMethoddto.BrandingMethodDetails.BrandingTagArray)
                        {
                            if (item.AssignmentId > 0)
                            {
                                var AssignedTags = _brandingMethodAssignedTagsRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                if (AssignedTags != null)
                                {
                                    AssignedTags.TagId = item.Id;
                                    await _brandingMethodAssignedTagsRepository.UpdateAsync(AssignedTags);
                                }
                            }
                            else
                            {
                                BrandingMethodAssignedTags Model = new BrandingMethodAssignedTags();
                                Model.TagId = item.Id;
                                Model.BrandingMethodId = createBrandingMethoddto.Id;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                await _brandingMethodAssignedTagsRepository.InsertAsync(Model);
                            }

                        }
                    }
                    else
                    {
                        var ExistingData = _brandingMethodAssignedTagsRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedTagsAsync(ExistingData, TenantId);
                        }
                    }
                    #endregion
                    #region Univarsal Array Update
                    if (createBrandingMethoddto.BrandingMethodDetails.UniversalBrandingArray != null)
                    {
                        var ExistingData = _brandingMethodAssignedUniversalsRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id);
                        var OldData = createBrandingMethoddto.BrandingMethodDetails.UniversalBrandingArray.Where(i => i.AssignmentId > 0);
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedUniversalsAsync(DeletedData, TenantId);
                        }

                        foreach (var item in createBrandingMethoddto.BrandingMethodDetails.UniversalBrandingArray)
                        {
                            if (item.AssignmentId > 0)
                            {
                                var AssignedUniversal = _brandingMethodAssignedUniversalsRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                if (AssignedUniversal != null)
                                {
                                    AssignedUniversal.UniversalId = item.Id;
                                    await _brandingMethodAssignedUniversalsRepository.UpdateAsync(AssignedUniversal);
                                }
                            }
                            else
                            {
                                BrandingMethodAssignedUniversals Model = new BrandingMethodAssignedUniversals();
                                Model.UniversalId = item.Id;
                                Model.BrandingMethodId = createBrandingMethoddto.Id;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                await _brandingMethodAssignedUniversalsRepository.InsertAsync(Model);
                            }

                        }
                    }
                    else
                    {
                        var ExistingData = _brandingMethodAssignedUniversalsRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedUniversalsAsync(ExistingData, TenantId);
                        }
                    }
                    #endregion
                    #region Vendor Array Update
                    if (createBrandingMethoddto.BrandingMethodDetails.BrandingVendorArray != null)
                    {
                        var ExistingData = _brandingMethodAssignedVendorRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id);
                        var OldData = createBrandingMethoddto.BrandingMethodDetails.BrandingVendorArray.Where(i => i.AssignmentId > 0);
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.AssignmentId == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedVendorsAsync(DeletedData, TenantId);
                        }

                        foreach (var item in createBrandingMethoddto.BrandingMethodDetails.BrandingVendorArray)
                        {
                            if (item.AssignmentId > 0)
                            {
                                var AssignedVendor = _brandingMethodAssignedVendorRepository.GetAllList(x => x.Id == item.AssignmentId).FirstOrDefault();
                                if (AssignedVendor != null)
                                {
                                    AssignedVendor.VendorUserId = item.Id;
                                    await _brandingMethodAssignedVendorRepository.UpdateAsync(AssignedVendor);
                                }
                            }
                            else
                            {
                                BrandingMethodAssignedVendors Model = new BrandingMethodAssignedVendors();
                                Model.VendorUserId = item.Id;
                                Model.BrandingMethodId = createBrandingMethoddto.Id;
                                Model.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                await _brandingMethodAssignedVendorRepository.InsertAsync(Model);
                            }

                        }
                    }
                    else
                    {
                        var ExistingData = _brandingMethodAssignedVendorRepository.GetAllList(i => i.BrandingMethodId == createBrandingMethoddto.Id).ToList();
                        if (ExistingData.Count > 0)
                        {
                            await DeleteBrandingMethodAssignedVendorsAsync(ExistingData, TenantId);
                        }
                    }
                    #endregion
                    #endregion
                    #endregion
                }
            }
            catch (Exception ex)
            {

            }
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
        private async Task<bool> DeleteBrandingAdditionalPriceAsync(List<BrandingMethodAdditionalPrice> BrandingadditionalPrice, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _repositoryBrandingAdditionalPrice.BulkDelete(BrandingadditionalPrice);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        public async Task DeleteBrandingMethodById(long BrandingMethodId)
        {
            try
            {
                var BrandingMethodMasterData = _repository.GetAllList(i => i.Id == BrandingMethodId).FirstOrDefault();
                if (BrandingMethodMasterData != null)
                {
                   
                    #region Delete Assign Table
                    //Assigned Color table
                    var AssignedColorData = _brandingMethodAssignedColorsRepository.GetAllList(i => i.BrandingMethodId == BrandingMethodId).ToList();
                    if (AssignedColorData != null && (AssignedColorData.Count() > 0))
                    {
                        _brandingMethodAssignedColorsRepository.BulkDelete(AssignedColorData);

                    }
                    //Assigned Equipment table
                    var AssignedEquipmentData = _brandingMethodAssignedEquipmentsRepository.GetAllList(i => i.BrandingMethodId == BrandingMethodId).ToList();
                    if (AssignedEquipmentData != null && (AssignedEquipmentData.Count() > 0))
                    {
                        _brandingMethodAssignedEquipmentsRepository.BulkDelete(AssignedEquipmentData);

                    }
                    //Assigned FontStyles table
                    var AssignedFontStyleData = _brandingMethodAssignedFontStylesRepository.GetAllList(i => i.BrandingMethodId == BrandingMethodId).ToList();
                    if (AssignedFontStyleData != null && (AssignedFontStyleData.Count() > 0))
                    {
                        _brandingMethodAssignedFontStylesRepository.BulkDelete(AssignedFontStyleData);

                    }
                    //Assigned FontTypesFace table
                    var AssignedFontTypeFacesData = _brandingMethodAssignedFontTypeFacesRepository.GetAllList(i => i.BrandingMethodId == BrandingMethodId).ToList();
                    if (AssignedFontTypeFacesData != null && (AssignedFontTypeFacesData.Count() > 0))
                    {
                        _brandingMethodAssignedFontTypeFacesRepository.BulkDelete(AssignedFontTypeFacesData);

                    }
                    //Assigned Tags table
                    var AssignedTagsData = _brandingMethodAssignedTagsRepository.GetAllList(i => i.BrandingMethodId == BrandingMethodId).ToList();
                    if (AssignedTagsData != null && (AssignedTagsData.Count() > 0))
                    {
                        _brandingMethodAssignedTagsRepository.BulkDelete(AssignedTagsData);

                    }
                    //Assigned Universal table
                    var AssignedUniversalData = _brandingMethodAssignedUniversalsRepository.GetAllList(i => i.BrandingMethodId == BrandingMethodId).ToList();
                    if (AssignedUniversalData != null && (AssignedUniversalData.Count() > 0))
                    {
                        _brandingMethodAssignedUniversalsRepository.BulkDelete(AssignedUniversalData);

                    }
                    //Assigned Vendor table
                    var AssignedVendorData = _brandingMethodAssignedVendorRepository.GetAllList(i => i.BrandingMethodId == BrandingMethodId).ToList();
                    if (AssignedVendorData != null && (AssignedVendorData.Count() > 0))
                    {
                        _brandingMethodAssignedVendorRepository.BulkDelete(AssignedVendorData);

                    }
                    #endregion


                    //Branding Price Variants
                    var productBrandingPriceVariantsData = _productBrandingPriceVariantsRepository.GetAllList(i => i.BrandingMethodId == BrandingMethodId).ToList();
                    if (productBrandingPriceVariantsData != null && (productBrandingPriceVariantsData.Count() > 0))
                    {
                        _productBrandingPriceVariantsRepository.BulkDelete(productBrandingPriceVariantsData);
                    }

                    //Branding Additional Price 
                    var BrandingAdditionalPriceData = _repositoryBrandingAdditionalPrice.GetAllList(i => i.BrandingMethodId == BrandingMethodId).ToList();
                    if (BrandingAdditionalPriceData != null && (BrandingAdditionalPriceData.Count() > 0))
                    {
                        _repositoryBrandingAdditionalPrice.BulkDelete(BrandingAdditionalPriceData);
                    }

                    //Branding Method master

                    await _repository.DeleteAsync(BrandingMethodMasterData);
                    await _unitOfWorkManager.Current.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task<bool> DeleteBrandingMethodAssignedColorAsync(List<BrandingMethodAssignedColors> brandingMethodAssignedColors, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _brandingMethodAssignedColorsRepository.BulkDelete(brandingMethodAssignedColors);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteBrandingMethodAssignedEquipmentsAsync(List<BrandingMethodAssignedEquipments> brandingMethodAssignedEquipments, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _brandingMethodAssignedEquipmentsRepository.BulkDelete(brandingMethodAssignedEquipments);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteBrandingMethodAssignedFontStylesAsync(List<BrandingMethodAssignedFontStyles> brandingMethodAssignedFontStyles, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _brandingMethodAssignedFontStylesRepository.BulkDelete(brandingMethodAssignedFontStyles);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteBrandingMethodAssignedFontTypeFacesAsync(List<BrandingMethodAssignedFontTypeFaces> brandingMethodAssignedFontTypeFaces, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _brandingMethodAssignedFontTypeFacesRepository.BulkDelete(brandingMethodAssignedFontTypeFaces);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> DeleteBrandingMethodAssignedTagsAsync(List<BrandingMethodAssignedTags> brandingMethodAssignedTags, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _brandingMethodAssignedTagsRepository.BulkDelete(brandingMethodAssignedTags);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }
        private async Task<bool> DeleteBrandingMethodAssignedUniversalsAsync(List<BrandingMethodAssignedUniversals> brandingMethodAssignedUniversals, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _brandingMethodAssignedUniversalsRepository.BulkDelete(brandingMethodAssignedUniversals);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return IsDeleted;
        }

        private async Task<bool> DeleteBrandingMethodAssignedVendorsAsync(List<BrandingMethodAssignedVendors> brandingMethodAssignedVendors, int TenantId)
        {
            bool IsDeleted = false;
            try
            {
                using (_unitOfWorkManager.Current.SetTenantId(TenantId))
                {
                    _brandingMethodAssignedVendorRepository.BulkDelete(brandingMethodAssignedVendors);
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
