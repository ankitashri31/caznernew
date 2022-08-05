using Abp.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.BusinessSetting;
using CaznerMarketplaceBackendApp.Department;
using CaznerMarketplaceBackendApp.Store;
using CaznerMarketplaceBackendApp.WareHouse;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using CaznerMarketplaceBackendApp.BusinessSetting.Dto;
using CaznerMarketplaceBackendApp.Authorization.Users.Dto;
using FimApp.EntityFrameworkCore;
using Abp.Domain.Uow;
using Abp.UI;
using CaznerMarketplaceBackendApp.Country;
using Abp.Linq.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using CaznerMarketplaceBackendApp.Connections;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using CaznerMarketplaceBackendApp.State.Dto;
using CaznerMarketplaceBackendApp.Country.Dto;
using CaznerMarketplaceBackendApp.Store.Dto;
using CaznerMarketplaceBackendApp.Product.Dto;
using CaznerMarketplaceBackendApp.MultiTenancy;

namespace CaznerMarketplaceBackendApp.BusinessSettings
{
    [AbpAuthorize]
    public class BusinessSettingsAppService : CaznerMarketplaceBackendAppAppServiceBase, IBusinessSettingsAppService
    {
        private readonly IRepository<UserBusinessSettings, long> _userBusinessSettingsRepository;
        private readonly IRepository<UserBankDetails, long> _userBankDetailsRepository;
        private readonly IRepository<WareHouseMaster, long> _wareHouseMasterRepository;
        private readonly IRepository<DepartmentUsers, long> _departmentUsersRepository;
        private readonly IRepository<DepartmentMaster, long> _departmentMasterRepository;
        private readonly IRepository<StoreMaster, long> _storeMasterRepository;
        private readonly IRepository<StoreOpeningTimings, long> _storeOpeningTimingsRepository;
        private readonly IRepository<UserShippingAddress, long> _userShippingAddressRepository;
        private readonly IRepository<UserAdditionalShippingAddress, long> _userAdditionalShippingRepository;
        private readonly IRepository<UserDetails, long> _userDetailsRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Countries, long> _countriesRepository;
        private readonly IRepository<States, long> _statesRepository;
        private IConfiguration _configuration;
        private readonly DbConnectionUtility _connectionUtility;
        private IDbConnection _db;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        string AzureStorageUrl = string.Empty;
        string AzureProductFolder = string.Empty;
        private readonly IRepository<Tenant, int> _tenantManager;
        public BusinessSettingsAppService
          (IRepository<UserBusinessSettings, long> userBusinessSettingsRepository,
          IRepository<UserBankDetails, long> userBankDetailsRepository,
          IRepository<WareHouseMaster, long> wareHouseMasterRepository,
          IRepository<DepartmentUsers, long> departmentUsersRepository,
          IRepository<StoreMaster, long> storeMasterRepository,
          IRepository<StoreOpeningTimings, long> storeOpeningTimingsRepository,
          IRepository<UserShippingAddress, long> userShippingAddressRepository,
          IRepository<UserDetails, long> userDetailsRepository,
          IRepository<DepartmentMaster, long> departmentMasterRepository,
          IConfiguration configuration, IRepository<User, long> userRepository, IRepository<UserAdditionalShippingAddress, long> userAdditionalShippingRepository, IUnitOfWorkManager unitOfWorkManager
            , IRepository<Countries, long> countriesRepository, IRepository<States, long> statesRepository, DbConnectionUtility connectionUtility, IRepository<Tenant, int> tenantManager)
        {
            _userBusinessSettingsRepository = userBusinessSettingsRepository;
            _userBankDetailsRepository = userBankDetailsRepository;
            _wareHouseMasterRepository = wareHouseMasterRepository;
            _departmentUsersRepository = departmentUsersRepository;
            _departmentMasterRepository = departmentMasterRepository;
            _storeMasterRepository = storeMasterRepository;
            _storeOpeningTimingsRepository = storeOpeningTimingsRepository;
            _userShippingAddressRepository = userShippingAddressRepository;
            _userDetailsRepository = userDetailsRepository;
            _configuration = configuration;
            _userRepository = userRepository;
            _userAdditionalShippingRepository = userAdditionalShippingRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _countriesRepository = countriesRepository;
            _statesRepository = statesRepository;
            _connectionUtility = connectionUtility;
            _tenantManager = tenantManager;
            _db = new SqlConnection(_configuration["ConnectionStrings:Default"]);
            AzureProductFolder = _configuration["FileUpload:AzureProductFolder"];
            AzureStorageUrl = _configuration["FileUpload:AzureStoragePath"];
        }


        public async Task<UserBusinessSettingsDto> CreateUserSettings(UserBusinessSettingsDto userBusinessSettingsDto)
        {
            UserBusinessSettingsDto model = new UserBusinessSettingsDto();

            try
            {
                var settingExists = _userBusinessSettingsRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value).FirstOrDefault();
                int TenantId = AbpSession.TenantId.Value;
                var Tenant = await _tenantManager.FirstOrDefaultAsync(TenantId);
                string folderPath = $"{ Tenant.TenancyName + "/" + AzureProductFolder}";
                if (settingExists == null)
                {
                    UserBusinessSettings userBusinessSettingsModel = new UserBusinessSettings();
                    userBusinessSettingsModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    userBusinessSettingsModel.UserId = Convert.ToInt32(AbpSession.UserId);
                    if (userBusinessSettingsDto.CurrencyMasterId == 0)
                    {
                        userBusinessSettingsModel.CurrencyMasterId = null;
                    }
                    else
                    {
                        userBusinessSettingsModel.CurrencyMasterId = userBusinessSettingsDto.CurrencyMasterId;
                    }
                    userBusinessSettingsModel.MeasurementId = userBusinessSettingsDto.MeasurementId;
                    userBusinessSettingsModel.IsChargeMinimumOrderFee = userBusinessSettingsDto.IsChargeMinimumOrderFee == "" ? 0 : Convert.ToDouble(userBusinessSettingsDto.IsChargeMinimumOrderFee);
                    userBusinessSettingsModel.IsChargeTaxGstorVst = userBusinessSettingsDto.IsChargeTaxGstorVst;
                    userBusinessSettingsModel.IsChargeTaxOnShipping = userBusinessSettingsDto.IsChargeTaxOnShipping;
                    userBusinessSettingsModel.IsShipingRatesIncludingTax = userBusinessSettingsDto.IsShipingRatesIncludingTax;
                    userBusinessSettingsModel.IsShowPriceWithoutAccount = userBusinessSettingsDto.IsShowPriceWithoutAccount;
                    userBusinessSettingsModel.MOQFee = userBusinessSettingsDto.MOQFee;
                    userBusinessSettingsModel.MOQFeeSKU = userBusinessSettingsDto.MOQFeeSKU;
                    userBusinessSettingsModel.NumberOfDaysQuoteIsValid = userBusinessSettingsDto.NumberOfDaysQuoteIsValid;
                    userBusinessSettingsModel.QuoteTerms = userBusinessSettingsDto.QuoteTerms;
                    userBusinessSettingsModel.TaxPercentOnAddProduct = userBusinessSettingsDto.TaxPercentOnAddProduct;
                    userBusinessSettingsModel.TaxCode = userBusinessSettingsDto.TaxCode;
                    userBusinessSettingsModel.ProductPricePrefix = userBusinessSettingsDto.ProductPricePrefix;
                    userBusinessSettingsModel.IsShowPriceTax = userBusinessSettingsDto.IsShowPriceTax;
                    userBusinessSettingsModel.ValidNoOfDays = userBusinessSettingsDto.ValidNoOfDays == "" ? 0 : Convert.ToInt32(userBusinessSettingsDto.ValidNoOfDays);
                    userBusinessSettingsModel.IsActive = userBusinessSettingsDto.IsActive;
                    userBusinessSettingsModel.BusinessContactNumber = userBusinessSettingsDto.businessPhoneNumber;
                    userBusinessSettingsModel.IsHideContactDetails = userBusinessSettingsDto.IsHideContactDetails;
                    // connect domain
                    userBusinessSettingsModel.ConnectDomain = userBusinessSettingsDto.ConnectDomain;
                    userBusinessSettingsModel.TermsAndCondition = userBusinessSettingsDto.TermsAndCondition;

                    // Business Logo 

                    if (userBusinessSettingsDto.BusinessSettingsLogo != null)
                    {
                        string FolderName = _configuration["FileUpload:FolderName"];
                        string CurrentWebsiteUrl = _configuration.GetValue<string>("FileUpload:WebsireDomainPath");
                        string ImageLocation = AzureStorageUrl + folderPath + userBusinessSettingsDto.BusinessSettingsLogo.FileName;
                        //Path.Combine(CurrentWebsiteUrl, FolderName, userBusinessSettingsDto.BusinessSettingsLogo.FileName.ToString());
                        userBusinessSettingsModel.BusinessLogoUrl = ImageLocation;
                        userBusinessSettingsModel.LogoExt = userBusinessSettingsDto.BusinessSettingsLogo.Ext;
                        userBusinessSettingsModel.LogoUrl = ImageLocation;
                        userBusinessSettingsModel.LogoName = userBusinessSettingsDto.BusinessSettingsLogo.FileName;
                        userBusinessSettingsModel.LogoSize = userBusinessSettingsDto.BusinessSettingsLogo.Size;
                        userBusinessSettingsModel.LogoType = userBusinessSettingsDto.BusinessSettingsLogo.Type;

                    }
                    userBusinessSettingsModel.BusinessName = userBusinessSettingsDto.BusinessName;
                    userBusinessSettingsModel.CompanyNumber = userBusinessSettingsDto.CompanyNumber;
                    userBusinessSettingsModel.BusinessWebsite = userBusinessSettingsDto.BusinessWebsite;
                    userBusinessSettingsModel.UserPhoneNumber = userBusinessSettingsDto.UserPhoneNumber;
                    userBusinessSettingsModel.BusinessEmail = userBusinessSettingsDto.BusinessEmail;
                    userBusinessSettingsModel.FirstName = userBusinessSettingsDto.FirstName;
                    userBusinessSettingsModel.LastName = userBusinessSettingsDto.LastName;
                    userBusinessSettingsModel.PositionId = userBusinessSettingsDto.PositionId;
                    userBusinessSettingsModel.EmailAddress = userBusinessSettingsDto.EmailAddress;
                    userBusinessSettingsModel.UserPhoneNumber = userBusinessSettingsDto.UserPhoneNumber;
                    userBusinessSettingsModel.UserMobileNumber = userBusinessSettingsDto.UserMobileNumber;
                    userBusinessSettingsModel.BSB = userBusinessSettingsDto.BSB;
                    userBusinessSettingsModel.AccountNumber = userBusinessSettingsDto.AccountNumber;
                    //shipping details
                    if (userBusinessSettingsDto.UserShippingAddress != null)
                    {
                        if (userBusinessSettingsDto.UserShippingAddress.CountryId.Value > 0)
                        {
                            UserShippingAddress userShippingAddressModel = new UserShippingAddress();

                            userShippingAddressModel = new UserShippingAddress();
                            userShippingAddressModel.StreetAddress = userBusinessSettingsDto.UserShippingAddress.StreetAddress;
                            userShippingAddressModel.CountryId = userBusinessSettingsDto.UserShippingAddress.CountryId;
                            userShippingAddressModel.StateId = userBusinessSettingsDto.UserShippingAddress.StateId;
                            userShippingAddressModel.AddressType = userBusinessSettingsDto.UserShippingAddress.AddressType;
                            userShippingAddressModel.PostCode = userBusinessSettingsDto.UserShippingAddress.PostCode;
                            userShippingAddressModel.City = userBusinessSettingsDto.UserShippingAddress.City;
                            userShippingAddressModel.MobileNumber = userBusinessSettingsDto.UserShippingAddress.MobileNumber;
                            userShippingAddressModel.AddressType = userBusinessSettingsDto.UserShippingAddress.AddressType;
                            userShippingAddressModel.UserId = Convert.ToInt32(AbpSession.UserId);
                            await _userShippingAddressRepository.InsertAsync(userShippingAddressModel);
                        }
                    }

                    //billing details
                    if (userBusinessSettingsDto.UserBillingAddress != null)
                    {
                        if (userBusinessSettingsDto.UserBillingAddress.CountryId.Value > 0)
                        {
                            UserShippingAddress userShippingAddressModel = new UserShippingAddress();

                            userShippingAddressModel = new UserShippingAddress();
                            userShippingAddressModel.StreetAddress = userBusinessSettingsDto.UserBillingAddress.StreetAddress;
                            userShippingAddressModel.CountryId = userBusinessSettingsDto.UserBillingAddress.CountryId;
                            userShippingAddressModel.City = userBusinessSettingsDto.UserBillingAddress.City;
                            userShippingAddressModel.StateId = userBusinessSettingsDto.UserBillingAddress.StateId;
                            userShippingAddressModel.AddressType = userBusinessSettingsDto.UserBillingAddress.AddressType;
                            userShippingAddressModel.PostCode = userBusinessSettingsDto.UserBillingAddress.PostCode;
                            userShippingAddressModel.MobileNumber = userBusinessSettingsDto.UserBillingAddress.MobileNumber;
                            userShippingAddressModel.AddressType = userBusinessSettingsDto.UserBillingAddress.AddressType;
                            userShippingAddressModel.UserId = Convert.ToInt32(AbpSession.UserId);
                            await _userShippingAddressRepository.InsertAsync(userShippingAddressModel);
                        }
                    }
                    if (userBusinessSettingsDto.BusinessSettingsFavicon != null)
                    {
                        string FolderName = _configuration["FileUpload:FolderName"];
                        string CurrentWebsiteUrl = _configuration.GetValue<string>("FileUpload:WebsireDomainPath");
                        string ImageLocation = AzureStorageUrl + folderPath + userBusinessSettingsDto.BusinessSettingsFavicon.FileName;
                        //Path.Combine(CurrentWebsiteUrl, FolderName, userBusinessSettingsDto.BusinessSettingsFavicon.FileName.ToString());
                        userBusinessSettingsModel.FaviconImageName = userBusinessSettingsDto.BusinessSettingsFavicon.FileName;
                        userBusinessSettingsModel.FaviconImageURL = ImageLocation;
                        userBusinessSettingsModel.FaviconExt = userBusinessSettingsDto.BusinessSettingsFavicon.Ext;
                        userBusinessSettingsModel.FaviconSize = userBusinessSettingsDto.BusinessSettingsFavicon.Size;
                        userBusinessSettingsModel.FaviconType = userBusinessSettingsDto.BusinessSettingsFavicon.Type;


                    }


                    // social media
                    userBusinessSettingsModel.InstagramLink = userBusinessSettingsDto.InstagramLink;
                    userBusinessSettingsModel.FacebookLink = userBusinessSettingsDto.FacebookLink;
                    userBusinessSettingsModel.TwitterLink = userBusinessSettingsDto.TwitterLink;
                    userBusinessSettingsModel.PinterestLink = userBusinessSettingsDto.PinterestLink;
                    var resultUniqueId = await _userBusinessSettingsRepository.InsertAndGetIdAsync(userBusinessSettingsModel);

                    //store openingsHours
                    StoreMaster storeMasterModel = new StoreMaster();
                    if (userBusinessSettingsDto.StoreMaster != null)
                    {
                        foreach (var StoreMasteritem in userBusinessSettingsDto.StoreMaster)
                        {
                            storeMasterModel = new StoreMaster();
                            storeMasterModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            storeMasterModel.StoreLocation = StoreMasteritem.StoreLocation;
                            storeMasterModel.StoreName = StoreMasteritem.StoreName;
                            storeMasterModel.IsActive = StoreMasteritem.IsActive;
                            long StoreMasterId = await _storeMasterRepository.InsertAndGetIdAsync(storeMasterModel);
                            //openingsHours
                            var storeOpeningTimingsList = ObjectMapper.Map<List<StoreOpeningTimings>>(StoreMasteritem.StoreOpeningTimings);
                            foreach (var storeOpeningTimingsitem in storeOpeningTimingsList)
                            {
                                storeOpeningTimingsitem.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                storeOpeningTimingsitem.UserId = Convert.ToInt32(AbpSession.UserId);
                                storeOpeningTimingsitem.StoreMasterId = StoreMasterId;
                                long storeOpeningTimingsId = await _storeOpeningTimingsRepository.InsertAndGetIdAsync(storeOpeningTimingsitem);

                            }
                        }

                    }

                    //bank Details
                    UserBankDetails userBankDetailModel = new UserBankDetails();
                    userBankDetailModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    userBankDetailModel.UserId = Convert.ToInt32(AbpSession.UserId);//userBusinessSettingsDto.UserId;
                    userBankDetailModel.SettingsId = resultUniqueId;
                    userBankDetailModel.SwiftCode = userBusinessSettingsDto.SwiftCode;
                    userBankDetailModel.BankName = userBusinessSettingsDto.BankName;
                    userBankDetailModel.AccountName = userBusinessSettingsDto.AccountName;
                    userBankDetailModel.BranchAddress = userBusinessSettingsDto.BranchAddress;
                    userBankDetailModel.IsStripeAccount = userBusinessSettingsDto.IsStripeAccount;
                    userBankDetailModel.IsPayPalAccount = userBusinessSettingsDto.IsPayPalAccount;
                    var userBankDetailId = await _userBankDetailsRepository.InsertAndGetIdAsync(userBankDetailModel);

                    //warehouse
                    if (userBusinessSettingsDto.WareHouseMaster != null)
                    {
                        WareHouseMaster wareHouseModel = new WareHouseMaster();
                        foreach (var Warehouseitem in userBusinessSettingsDto.WareHouseMaster)
                        {
                            wareHouseModel = new WareHouseMaster();
                            wareHouseModel.SettingsId = resultUniqueId;
                            wareHouseModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            wareHouseModel.WarehouseTitle = Warehouseitem.WarehouseTitle;
                            wareHouseModel.StreetAddress = Warehouseitem.StreetAddress;
                            wareHouseModel.City = Warehouseitem.City;
                            wareHouseModel.PostCode = Warehouseitem.PostCode;
                            wareHouseModel.TimezoneId = Warehouseitem.TimezoneId;
                            wareHouseModel.IsActive = Warehouseitem.IsActive;
                            wareHouseModel.CountryId = Warehouseitem.CountryId.Id;
                            wareHouseModel.StateId = Warehouseitem.StateId.Id;
                            long WarehouseId = await _wareHouseMasterRepository.InsertAndGetIdAsync(wareHouseModel);
                        }
                    }

                    //DepartmentUser
                    try
                    {
                        if (userBusinessSettingsDto.DepartmentUser != null)
                        {
                            foreach (var Department in userBusinessSettingsDto.DepartmentUser)
                            {
                                var departmetMaster = _departmentMasterRepository.GetAllList(x => x.Id == Department.Id).FirstOrDefault();
                                if (departmetMaster != null)
                                {
                                    departmetMaster.DepartmentName = Department.DepartmentName;
                                    departmetMaster.IsActive = Department.IsActive;
                                    await _departmentMasterRepository.UpdateAsync(departmetMaster);
                                }
                                else
                                {
                                    DepartmentMaster departmentData = new DepartmentMaster();
                                    departmentData.DepartmentName = Department.DepartmentName;
                                    departmentData.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    departmentData.IsActive = Department.IsActive;

                                    long depatmentMaster = await _departmentMasterRepository.InsertAndGetIdAsync(departmentData);

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    { }

                    model.Id = resultUniqueId;
                }

                else
                {

                    settingExists.TenantId = Convert.ToInt32(AbpSession.TenantId);
                    settingExists.UserId = Convert.ToInt32(AbpSession.UserId);
                    if (userBusinessSettingsDto.CurrencyMasterId == 0)
                    {
                        settingExists.CurrencyMasterId = null;
                    }
                    else
                    {
                        settingExists.CurrencyMasterId = userBusinessSettingsDto.CurrencyMasterId;
                    }

                    settingExists.MeasurementId = userBusinessSettingsDto.MeasurementId;
                    settingExists.IsChargeMinimumOrderFee = userBusinessSettingsDto.IsChargeMinimumOrderFee == "" ? 0 : Convert.ToDouble(userBusinessSettingsDto.IsChargeMinimumOrderFee);
                    settingExists.IsChargeTaxGstorVst = userBusinessSettingsDto.IsChargeTaxGstorVst;
                    settingExists.IsChargeTaxOnShipping = userBusinessSettingsDto.IsChargeTaxOnShipping;
                    settingExists.IsShipingRatesIncludingTax = userBusinessSettingsDto.IsShipingRatesIncludingTax;
                    settingExists.IsShowPriceWithoutAccount = userBusinessSettingsDto.IsShowPriceWithoutAccount;
                    settingExists.MOQFee = userBusinessSettingsDto.MOQFee;
                    settingExists.MOQFeeSKU = userBusinessSettingsDto.MOQFeeSKU;
                    settingExists.NumberOfDaysQuoteIsValid = userBusinessSettingsDto.NumberOfDaysQuoteIsValid;
                    settingExists.QuoteTerms = userBusinessSettingsDto.QuoteTerms;
                    settingExists.TaxPercentOnAddProduct = userBusinessSettingsDto.TaxPercentOnAddProduct;
                    settingExists.TaxCode = userBusinessSettingsDto.TaxCode;
                    settingExists.ProductPricePrefix = userBusinessSettingsDto.ProductPricePrefix;
                    settingExists.IsShowPriceTax = userBusinessSettingsDto.IsShowPriceTax;
                    settingExists.ValidNoOfDays = userBusinessSettingsDto.ValidNoOfDays == "" ? 0 : Convert.ToInt32(userBusinessSettingsDto.ValidNoOfDays);
                    settingExists.IsActive = userBusinessSettingsDto.IsActive;
                    settingExists.IsHideContactDetails = userBusinessSettingsDto.IsHideContactDetails;
                    // connect domain
                    settingExists.ConnectDomain = userBusinessSettingsDto.ConnectDomain;
                    settingExists.BusinessName = userBusinessSettingsDto.BusinessName;
                    settingExists.CompanyNumber = userBusinessSettingsDto.CompanyNumber;
                    settingExists.BusinessWebsite = userBusinessSettingsDto.BusinessWebsite;
                    settingExists.UserPhoneNumber = userBusinessSettingsDto.UserPhoneNumber;
                    settingExists.BusinessEmail = userBusinessSettingsDto.BusinessEmail;
                    settingExists.FirstName = userBusinessSettingsDto.FirstName;
                    settingExists.LastName = userBusinessSettingsDto.LastName;
                    settingExists.PositionId = userBusinessSettingsDto.PositionId;
                    settingExists.EmailAddress = userBusinessSettingsDto.EmailAddress;
                    settingExists.UserPhoneNumber = userBusinessSettingsDto.UserPhoneNumber;
                    settingExists.UserMobileNumber = userBusinessSettingsDto.UserMobileNumber;
                    settingExists.TermsAndCondition = userBusinessSettingsDto.TermsAndCondition;
                    settingExists.BSB = userBusinessSettingsDto.BSB;
                    settingExists.AccountNumber = userBusinessSettingsDto.AccountNumber;
                    // BusinessImageLogo

                    if (userBusinessSettingsDto.BusinessSettingsLogo != null)
                    {
                        string FolderName = _configuration["FileUpload:FolderName"];
                        string CurrentWebsiteUrl = _configuration.GetValue<string>("FileUpload:WebsireDomainPath");
                        string ImageLocation = AzureStorageUrl + folderPath + userBusinessSettingsDto.BusinessSettingsLogo.FileName;
                        //Path.Combine(CurrentWebsiteUrl, FolderName, userBusinessSettingsDto.BusinessSettingsLogo.FileName.ToString());
                        settingExists.BusinessLogoUrl = ImageLocation;
                        settingExists.LogoUrl = ImageLocation;
                        settingExists.LogoExt = userBusinessSettingsDto.BusinessSettingsLogo.Ext;
                        settingExists.LogoName = userBusinessSettingsDto.BusinessSettingsLogo.FileName;
                        settingExists.LogoSize = userBusinessSettingsDto.BusinessSettingsLogo.Size;
                        settingExists.LogoType = userBusinessSettingsDto.BusinessSettingsLogo.Type;
                        //settingExists.LogoUrl = userBusinessSettingsDto.BusinessImageLogo.BusinessLogoUrl;
                    }
                    // userBusinessFeviconImages                

                    if (userBusinessSettingsDto.BusinessSettingsFavicon != null)
                    {
                        string FolderName = _configuration["FileUpload:FolderName"];
                        string CurrentWebsiteUrl = _configuration.GetValue<string>("FileUpload:WebsireDomainPath");
                        string ImageLocation = AzureStorageUrl + folderPath + userBusinessSettingsDto.BusinessSettingsFavicon.FileName;
                        //Path.Combine(CurrentWebsiteUrl, FolderName, userBusinessSettingsDto.BusinessSettingsFavicon.FileName);
                        settingExists.FaviconImageName = userBusinessSettingsDto.BusinessSettingsFavicon.FileName;
                        settingExists.FaviconImageURL = ImageLocation;
                        settingExists.FaviconExt = userBusinessSettingsDto.BusinessSettingsFavicon.Ext;
                        settingExists.FaviconSize = userBusinessSettingsDto.BusinessSettingsFavicon.Size;
                        settingExists.FaviconType = userBusinessSettingsDto.BusinessSettingsFavicon.Type;
                        // settingExists.LogoUrl = userBusinessSettingsDto.userBusinessFeviconImages.FaviconImageURL;
                    }

                    // social media
                    settingExists.InstagramLink = userBusinessSettingsDto.InstagramLink;
                    settingExists.FacebookLink = userBusinessSettingsDto.FacebookLink;
                    settingExists.TwitterLink = userBusinessSettingsDto.TwitterLink;
                    settingExists.PinterestLink = userBusinessSettingsDto.PinterestLink;
                    var resultUniqueId = await _userBusinessSettingsRepository.UpdateAsync(settingExists);

                    //bank details
                    var bankInfo = _userBankDetailsRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value).FirstOrDefault();
                    if (bankInfo != null)
                    {
                        // UserBankDetails userBankDetailModel = new UserBankDetails();
                        bankInfo.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        bankInfo.UserId = Convert.ToInt32(AbpSession.UserId);
                        bankInfo.SwiftCode = userBusinessSettingsDto.SwiftCode;
                        bankInfo.BankName = userBusinessSettingsDto.BankName;
                        bankInfo.AccountName = userBusinessSettingsDto.AccountName;
                        bankInfo.BranchAddress = userBusinessSettingsDto.BranchAddress;
                        bankInfo.IsStripeAccount = userBusinessSettingsDto.IsStripeAccount;
                        bankInfo.IsPayPalAccount = userBusinessSettingsDto.IsPayPalAccount;
                        await _userBankDetailsRepository.UpdateAsync(bankInfo);
                    }
                    else
                    {
                        UserBankDetails userBankDetailModel = new UserBankDetails();
                        userBankDetailModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        userBankDetailModel.UserId = Convert.ToInt32(AbpSession.UserId);//userBusinessSettingsDto.UserId;
                        userBankDetailModel.SettingsId = settingExists.Id;
                        userBankDetailModel.SwiftCode = userBusinessSettingsDto.SwiftCode;
                        userBankDetailModel.BankName = userBusinessSettingsDto.BankName;
                        userBankDetailModel.AccountName = userBusinessSettingsDto.AccountName;
                        userBankDetailModel.BranchAddress = userBusinessSettingsDto.BranchAddress;
                        userBankDetailModel.IsStripeAccount = userBusinessSettingsDto.IsStripeAccount;
                        userBankDetailModel.IsPayPalAccount = userBusinessSettingsDto.IsPayPalAccount;
                        var userBankDetailId = await _userBankDetailsRepository.InsertAndGetIdAsync(userBankDetailModel);

                    }

                    //shipping details
                    if (userBusinessSettingsDto.UserShippingAddress != null)
                    {
                        var UserShipping = _userShippingAddressRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value && x.UserId == AbpSession.UserId.Value && x.AddressType == userBusinessSettingsDto.UserShippingAddress.AddressType).FirstOrDefault();
                        if (UserShipping != null)
                        {
                           
                                    UserShipping.StreetAddress = userBusinessSettingsDto.UserShippingAddress.StreetAddress;
                                    if(userBusinessSettingsDto.UserShippingAddress.CountryId.Value > 0)
                                    {
                                        UserShipping.CountryId = userBusinessSettingsDto.UserShippingAddress.CountryId;
                                    }
                                    if(userBusinessSettingsDto.UserShippingAddress.StateId.Value > 0)
                                    {
                                        UserShipping.StateId = userBusinessSettingsDto.UserShippingAddress.StateId;
                                    }
                                    UserShipping.MobileNumber = userBusinessSettingsDto.UserShippingAddress.MobileNumber;
                                    UserShipping.City = userBusinessSettingsDto.UserShippingAddress.City;
                                    UserShipping.PostCode = userBusinessSettingsDto.UserShippingAddress.PostCode;
                                    UserShipping.AddressType = userBusinessSettingsDto.UserShippingAddress.AddressType;
                                    await _userShippingAddressRepository.UpdateAsync(UserShipping);
                           }
                           else
                           {
                                    if (userBusinessSettingsDto.UserShippingAddress.CountryId.Value > 0)
                                    {
                                        UserShippingAddress userShippingAddressModel = new UserShippingAddress();
                                        userShippingAddressModel.StreetAddress = userBusinessSettingsDto.UserShippingAddress.StreetAddress;
                                        userShippingAddressModel.CountryId = userBusinessSettingsDto.UserShippingAddress.CountryId;
                                        userShippingAddressModel.StateId = userBusinessSettingsDto.UserShippingAddress.StateId;
                                        userShippingAddressModel.PostCode = userBusinessSettingsDto.UserShippingAddress.PostCode;
                                        userShippingAddressModel.MobileNumber = userBusinessSettingsDto.UserShippingAddress.MobileNumber;
                                        userShippingAddressModel.AddressType = userBusinessSettingsDto.UserShippingAddress.AddressType;
                                        userShippingAddressModel.City = userBusinessSettingsDto.UserShippingAddress.City;
                                        userShippingAddressModel.UserId = AbpSession.UserId.Value;
                                        await _userShippingAddressRepository.InsertAsync(userShippingAddressModel);
                                    }
                            }
                    }
                    else
                    {
                        var UserShipping = _userShippingAddressRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value && x.UserId == AbpSession.UserId.Value && x.AddressType == 1).ToList();
                        if (UserShipping.Count > 0)
                        {
                            _userShippingAddressRepository.BulkDelete(UserShipping);
                        }
                    }

                    //billing details
                    if (userBusinessSettingsDto.UserBillingAddress != null)
                    {
                        var UserShipping = _userShippingAddressRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value && x.UserId == AbpSession.UserId.Value && x.AddressType == userBusinessSettingsDto.UserBillingAddress.AddressType).FirstOrDefault();
                        if (UserShipping != null)
                        {
                            if (userBusinessSettingsDto.UserBillingAddress.CountryId.Value > 0)
                            {
                                UserShipping.StreetAddress = userBusinessSettingsDto.UserBillingAddress.StreetAddress;
                                if (userBusinessSettingsDto.UserBillingAddress.CountryId.Value > 0)
                                {
                                    UserShipping.CountryId = userBusinessSettingsDto.UserBillingAddress.CountryId;
                                }
                                if (userBusinessSettingsDto.UserBillingAddress.StateId.Value > 0)
                                {
                                    UserShipping.StateId = userBusinessSettingsDto.UserBillingAddress.StateId;
                                }
                                UserShipping.City = userBusinessSettingsDto.UserBillingAddress.City;
                                UserShipping.PostCode = userBusinessSettingsDto.UserBillingAddress.PostCode;
                                UserShipping.MobileNumber = userBusinessSettingsDto.UserBillingAddress.MobileNumber;
                                UserShipping.AddressType = userBusinessSettingsDto.UserBillingAddress.AddressType;
                                await _userShippingAddressRepository.UpdateAsync(UserShipping);
                            }
                        }
                        else
                        {
                            if (userBusinessSettingsDto.UserBillingAddress.CountryId.Value > 0)
                            {
                                UserShippingAddress userShippingAddressModel = new UserShippingAddress();
                                userShippingAddressModel.StreetAddress = userBusinessSettingsDto.UserBillingAddress.StreetAddress;
                                userShippingAddressModel.CountryId = userBusinessSettingsDto.UserBillingAddress.CountryId;
                                userShippingAddressModel.StateId = userBusinessSettingsDto.UserBillingAddress.StateId;
                                userShippingAddressModel.PostCode = userBusinessSettingsDto.UserBillingAddress.PostCode;
                                userShippingAddressModel.MobileNumber = userBusinessSettingsDto.UserBillingAddress.MobileNumber;
                                userShippingAddressModel.AddressType = userBusinessSettingsDto.UserBillingAddress.AddressType;
                                userShippingAddressModel.City = userBusinessSettingsDto.UserBillingAddress.City;
                                userShippingAddressModel.UserId = AbpSession.UserId.Value;
                                await _userShippingAddressRepository.InsertAsync(userShippingAddressModel);
                            }
                        }
                    }
                    else
                    {
                        var UserShipping = _userShippingAddressRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value && x.UserId == AbpSession.UserId.Value && x.AddressType == 2).ToList();
                        if (UserShipping.Count > 0)
                        {
                            _userShippingAddressRepository.BulkDelete(UserShipping);
                        }
                    }

                    if (userBusinessSettingsDto.WareHouseMaster != null)
                    {

                        var ExistingData = _wareHouseMasterRepository.GetAllList(i => i.SettingsId == settingExists.Id).ToList();
                        var OldData = userBusinessSettingsDto.WareHouseMaster.Where(i => i.Id > 0).ToList();
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                             _wareHouseMasterRepository.BulkDelete(DeletedData);
                        }

                        foreach (var warehouseMaster in userBusinessSettingsDto.WareHouseMaster)
                        {
                            var UserwareHouse = _wareHouseMasterRepository.GetAllList(x => x.Id == warehouseMaster.Id).FirstOrDefault();
                            if (UserwareHouse != null)
                            {
                                UserwareHouse.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                UserwareHouse.WarehouseTitle = warehouseMaster.WarehouseTitle;
                                UserwareHouse.StreetAddress = warehouseMaster.StreetAddress;
                                UserwareHouse.City = warehouseMaster.City;
                                if (warehouseMaster.StateId.Id > 0)
                                {
                                    UserwareHouse.StateId = warehouseMaster.StateId.Id;
                                }
                                UserwareHouse.PostCode = warehouseMaster.PostCode;
                                if (warehouseMaster.CountryId.Id > 0)
                                {
                                    UserwareHouse.CountryId = warehouseMaster.CountryId.Id;
                                }
                                UserwareHouse.TimezoneId = warehouseMaster.TimezoneId;
                                UserwareHouse.IsActive = warehouseMaster.IsActive;
                                await _wareHouseMasterRepository.UpdateAsync(UserwareHouse);
                            }
                            else
                            {
                                   WareHouseMaster wareHouseModel = new WareHouseMaster();
                             
                                    wareHouseModel = new WareHouseMaster();
                                    wareHouseModel.SettingsId = userBusinessSettingsDto.Id;
                                    wareHouseModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    wareHouseModel.WarehouseTitle = warehouseMaster.WarehouseTitle;
                                    wareHouseModel.StreetAddress = warehouseMaster.StreetAddress;
                                    wareHouseModel.City = warehouseMaster.City;
                                    wareHouseModel.PostCode = warehouseMaster.PostCode;
                                    wareHouseModel.TimezoneId = warehouseMaster.TimezoneId;
                                    wareHouseModel.IsActive = warehouseMaster.IsActive;
                                if (warehouseMaster.CountryId.Id > 0)
                                {
                                    wareHouseModel.CountryId = warehouseMaster.CountryId.Id;
                                }
                                if (warehouseMaster.StateId.Id > 0)
                                {
                                    wareHouseModel.StateId = warehouseMaster.StateId.Id;
                                }
                                    wareHouseModel.SettingsId = settingExists.Id;
                                    long WarehouseId = await _wareHouseMasterRepository.InsertAndGetIdAsync(wareHouseModel);
                            }
                        }
                    }
                    else
                    {
                        var WareHouses = _wareHouseMasterRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value).ToList();
                        if (WareHouses != null)
                        {
                            _wareHouseMasterRepository.BulkDelete(WareHouses);
                        }
                    }

                    //DepartmentUser
                    if (userBusinessSettingsDto.DepartmentUser != null)
                    {
                        var ExistingData = _departmentMasterRepository.GetAllList(i => i.TenantId == AbpSession.TenantId.Value).ToList();
                        var OldData = userBusinessSettingsDto.DepartmentUser.Where(i => i.Id > 0).ToList();
                        var DeletedData = ExistingData.Where(p => !OldData.Any(p2 => p2.Id == p.Id)).ToList();

                        if (DeletedData.Count > 0)
                        {
                            _departmentMasterRepository.BulkDelete(DeletedData);
                        }

                        foreach (var dept in userBusinessSettingsDto.DepartmentUser)
                        {
                            var departmetMaster = _departmentMasterRepository.GetAllList(x => x.Id == dept.Id).FirstOrDefault();
                            if (departmetMaster != null)
                            {

                                departmetMaster.DepartmentName = dept.DepartmentName;
                                await _departmentMasterRepository.UpdateAsync(departmetMaster);
                            }
                            else
                            {
                                    DepartmentMaster department = new DepartmentMaster();
                                    department.DepartmentName = dept.DepartmentName;
                                    department.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    department.IsActive =true;
                                    long depatmentMaster = await _departmentMasterRepository.InsertAndGetIdAsync(department);
                            }
                        }
                    }
                    else
                    {
                        var DeptUsers = _departmentMasterRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value).ToList();
                        if (DeptUsers.Count > 0)
                        {
                            _departmentMasterRepository.BulkDelete(DeptUsers);
                        }
                    }


                    // StoreMaster
                    if (userBusinessSettingsDto.StoreMaster != null)
                    {

                        foreach (var storeMaster in userBusinessSettingsDto.StoreMaster)
                        {
                            var UserStoreMaster = _storeMasterRepository.GetAllList(x => x.Id == storeMaster.Id).FirstOrDefault();
                            if (UserStoreMaster != null)
                            {
                                UserStoreMaster.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                UserStoreMaster.StoreLocation = storeMaster.StoreLocation;
                                UserStoreMaster.StoreName = storeMaster.StoreName;
                                UserStoreMaster.IsActive = storeMaster.IsActive;
                                var storeOpeningTimingsList = ObjectMapper.Map<List<StoreOpeningTimings>>(storeMaster.StoreOpeningTimings);
                                foreach (var storeOpeningTimingsitem in storeOpeningTimingsList)
                                {
                                    var TimingExists = _storeOpeningTimingsRepository.GetAllList(x => x.Id == storeOpeningTimingsitem.Id).FirstOrDefault();
                                    if (TimingExists != null)
                                    {
                                        storeOpeningTimingsitem.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                        storeOpeningTimingsitem.UserId = Convert.ToInt32(AbpSession.UserId);
                                        await _storeOpeningTimingsRepository.UpdateAsync(storeOpeningTimingsitem);
                                    }
                                    else
                                    {
                                        storeOpeningTimingsitem.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                        storeOpeningTimingsitem.UserId = Convert.ToInt32(AbpSession.UserId);
                                        storeOpeningTimingsitem.StoreMasterId = UserStoreMaster.Id;
                                        await _storeOpeningTimingsRepository.InsertAsync(storeOpeningTimingsitem);
                                    }
                                }
                                await _storeMasterRepository.UpdateAsync(UserStoreMaster);
                            }
                            else
                            {
                                StoreMaster storeMasterModel = new StoreMaster();
                                storeMasterModel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                storeMasterModel.StoreLocation = storeMaster.StoreLocation;
                                storeMasterModel.StoreName = storeMaster.StoreName;
                                storeMasterModel.IsActive = storeMaster.IsActive;
                                long StoreMasterId = await _storeMasterRepository.InsertAndGetIdAsync(storeMasterModel);
                                //openingsHours
                                var storeOpeningTimingsList = ObjectMapper.Map<List<StoreOpeningTimings>>(storeMaster.StoreOpeningTimings);
                                foreach (var storeOpeningTimingsitem in storeOpeningTimingsList)
                                {
                                    storeOpeningTimingsitem.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                    storeOpeningTimingsitem.UserId = Convert.ToInt32(AbpSession.UserId);
                                    storeOpeningTimingsitem.StoreMasterId = StoreMasterId;
                                    await _storeOpeningTimingsRepository.InsertAsync(storeOpeningTimingsitem);

                                }
                            }
                          }
                        }
                    else
                    {
                        var Stores = _storeMasterRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value).ToList();
                        if (Stores.Count > 0)
                        {
                            _storeMasterRepository.BulkDelete(Stores);

                            var Timings = _storeOpeningTimingsRepository.GetAllList(x => x.TenantId == AbpSession.TenantId.Value).ToList();
                            if(Timings.Count > 0)
                            {
                                _storeOpeningTimingsRepository.BulkDelete(Timings);
                            }
                        }
                    }
                      
                  }
               
            }
            catch (System.Exception ex)
            {

                //throw;
            }
            return model;
        }

        public async Task CreateOrUpdateShippingAddress(UserShippingAddressDto userShippingAddressDto)
        {
            bool IsErrorExists = false;
            try
            {
                if (userShippingAddressDto.ShippingAddressId.HasValue)
                {
                    // case for additional child shipping address
                    await CreateAdditionalShippingAddress(userShippingAddressDto);
                }
                else
                {
                    // case for primary/main shipping address
                    if (userShippingAddressDto.Id == 0)
                    {
                        var EmailExists = _userShippingAddressRepository.GetAllList(i => i.BusinessEmail.ToLower() == userShippingAddressDto.BusinessEmail.ToLower()).FirstOrDefault();
                        if (EmailExists == null)
                        {
                            UserShippingAddress userShippingAddressmodel = new UserShippingAddress();

                            userShippingAddressmodel.FirstName = userShippingAddressDto.FirstName;
                            userShippingAddressmodel.LastName = userShippingAddressDto.LastName;
                            userShippingAddressmodel.MobileNumber = userShippingAddressDto.MobileNumber;
                            userShippingAddressmodel.BusinessEmail = userShippingAddressDto.BusinessEmail;
                            userShippingAddressmodel.CompanyName = userShippingAddressDto.CompanyName;
                            userShippingAddressmodel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            userShippingAddressmodel.UserId = Convert.ToInt32(AbpSession.UserId);
                            userShippingAddressmodel.StreetAddress = userShippingAddressDto.StreetAddress;
                            userShippingAddressmodel.City = userShippingAddressDto.City;
                            userShippingAddressmodel.StateId = userShippingAddressDto.StateId;
                            userShippingAddressmodel.CountryId = userShippingAddressDto.CountryId;
                            userShippingAddressmodel.IsPrimaryAddress = userShippingAddressDto.IsPrimaryAddress;
                            userShippingAddressmodel.PostCode = userShippingAddressDto.PostCode;
                       
                            #region update previous primary address status
                            if (userShippingAddressDto.IsPrimaryAddress == true)
                            {
                                var Address = _userShippingAddressRepository.GetAllList(i => i.UserId == AbpSession.UserId.Value && i.IsPrimaryAddress == true);
                                if (Address != null)
                                {
                                    foreach (var add in Address)
                                    {
                                        add.IsPrimaryAddress = false;
                                        await _userShippingAddressRepository.UpdateAsync(add);
                                    }
                                }
                            }
                            #endregion

                            var resultUniqueId = await _userShippingAddressRepository.InsertAndGetIdAsync(userShippingAddressmodel);
                        }
                        else
                        {
                            IsErrorExists = true;
                        }
                    }
                    else
                    {
                        var userShippingAddressmodel = _userShippingAddressRepository.Get(userShippingAddressDto.Id);

                        if (userShippingAddressmodel != null)
                        {
                            var EmailExists = _userShippingAddressRepository.GetAllList(i => i.BusinessEmail.ToLower() == userShippingAddressDto.BusinessEmail.ToLower() && i.Id != userShippingAddressDto.Id).FirstOrDefault();
                            if (EmailExists == null)
                            {


                                userShippingAddressmodel.FirstName = userShippingAddressDto.FirstName;
                                userShippingAddressmodel.LastName = userShippingAddressDto.LastName;
                                userShippingAddressmodel.MobileNumber = userShippingAddressDto.MobileNumber;
                                userShippingAddressmodel.BusinessEmail = userShippingAddressDto.BusinessEmail;
                                userShippingAddressmodel.CompanyName = userShippingAddressDto.CompanyName;
                                userShippingAddressmodel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                                userShippingAddressmodel.IsPrimaryAddress = userShippingAddressDto.IsPrimaryAddress;
                                userShippingAddressmodel.UserId = Convert.ToInt32(AbpSession.UserId);
                                userShippingAddressmodel.StreetAddress = userShippingAddressDto.StreetAddress;
                                userShippingAddressmodel.City = userShippingAddressDto.City;
                                userShippingAddressmodel.StateId = userShippingAddressDto.StateId;
                                userShippingAddressmodel.CountryId = userShippingAddressDto.CountryId;
                                userShippingAddressmodel.PostCode = userShippingAddressDto.PostCode;

                                #region update previous primary address status
                                if (userShippingAddressDto.IsPrimaryAddress == true)
                                {
                                    var Address = _userShippingAddressRepository.GetAllList(i => i.UserId == AbpSession.UserId.Value && i.IsPrimaryAddress == true && i.Id != userShippingAddressDto.Id);
                                    if (Address != null)
                                    {
                                        foreach (var add in Address)
                                        {
                                            add.IsPrimaryAddress = false;
                                            await _userShippingAddressRepository.UpdateAsync(add);
                                        }
                                    }
                                }
                                #endregion

                                await _userShippingAddressRepository.UpdateAsync(userShippingAddressmodel);
                            }
                            else
                            {
                                IsErrorExists = true;
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
            }
            if (IsErrorExists == true)
            {
                throw new UserFriendlyException("Email address already exists in the system");
            }
        }
        private async Task CreateAdditionalShippingAddress(UserShippingAddressDto userShippingAddressDto)
        {
            bool IsErrorExists = false;
            try
            {
                if (userShippingAddressDto.Id == 0)
                {
                    var EmailExists = _userAdditionalShippingRepository.GetAllList(i => i.BusinessEmail.ToLower() == userShippingAddressDto.BusinessEmail.ToLower() && i.ShippingAddressId == userShippingAddressDto.ShippingAddressId).FirstOrDefault();
                    if (EmailExists == null)
                    {
                        UserAdditionalShippingAddress userShippingAddressmodel = new UserAdditionalShippingAddress();

                        userShippingAddressmodel.ShippingAddressId = userShippingAddressDto.ShippingAddressId;
                        userShippingAddressmodel.FirstName = userShippingAddressDto.FirstName;
                        userShippingAddressmodel.LastName = userShippingAddressDto.LastName;
                        userShippingAddressmodel.MobileNumber = userShippingAddressDto.MobileNumber;
                        userShippingAddressmodel.BusinessEmail = userShippingAddressDto.BusinessEmail;
                        userShippingAddressmodel.CompanyName = userShippingAddressDto.CompanyName;
                        userShippingAddressmodel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                        userShippingAddressmodel.UserId = Convert.ToInt32(AbpSession.UserId);
                        userShippingAddressmodel.StreetAddress = userShippingAddressDto.StreetAddress;
                        userShippingAddressmodel.City = userShippingAddressDto.City;
                        userShippingAddressmodel.StateId = userShippingAddressDto.StateId;
                        userShippingAddressmodel.CountryId = userShippingAddressDto.CountryId;
                        userShippingAddressmodel.IsPrimaryAddress = userShippingAddressDto.IsPrimaryAddress;
                        userShippingAddressmodel.PostCode = userShippingAddressDto.PostCode;

                        #region update previous primary address status
                        if (userShippingAddressDto.IsPrimaryAddress == true)
                        {
                            var Address = _userAdditionalShippingRepository.GetAllList(i => i.UserId == AbpSession.UserId.Value && i.IsPrimaryAddress == true);
                            if (Address != null)
                            {
                                foreach (var add in Address)
                                {
                                    add.IsPrimaryAddress = false;
                                    await _userAdditionalShippingRepository.UpdateAsync(add);
                                }
                            }
                        }
                        #endregion

                        var resultUniqueId = await _userAdditionalShippingRepository.InsertAndGetIdAsync(userShippingAddressmodel);
                    }
                    else
                    {
                        IsErrorExists = true;
                    }
                }
                else
                {
                    var userShippingAddressmodel = _userAdditionalShippingRepository.GetAllList(i => i.Id == userShippingAddressDto.Id).FirstOrDefault();

                    if (userShippingAddressmodel != null)
                    {
                        var EmailExists = _userAdditionalShippingRepository.GetAllList(i => i.BusinessEmail.ToLower() == userShippingAddressDto.BusinessEmail.ToLower() && i.ShippingAddressId == userShippingAddressDto.ShippingAddressId && i.Id != userShippingAddressDto.Id).FirstOrDefault();
                        if (EmailExists == null)
                        {

                            userShippingAddressmodel.FirstName = userShippingAddressDto.FirstName;
                            userShippingAddressmodel.LastName = userShippingAddressDto.LastName;
                            userShippingAddressmodel.MobileNumber = userShippingAddressDto.MobileNumber;
                            userShippingAddressmodel.BusinessEmail = userShippingAddressDto.BusinessEmail;
                            userShippingAddressmodel.CompanyName = userShippingAddressDto.CompanyName;
                            userShippingAddressmodel.TenantId = Convert.ToInt32(AbpSession.TenantId);
                            userShippingAddressmodel.IsPrimaryAddress = userShippingAddressDto.IsPrimaryAddress;
                            userShippingAddressmodel.UserId = Convert.ToInt32(AbpSession.UserId);
                            userShippingAddressmodel.StreetAddress = userShippingAddressDto.StreetAddress;
                            userShippingAddressmodel.City = userShippingAddressDto.City;
                            userShippingAddressmodel.StateId = userShippingAddressDto.StateId;
                            userShippingAddressmodel.CountryId = userShippingAddressDto.CountryId;
                            userShippingAddressmodel.PostCode = userShippingAddressDto.PostCode;

                            #region update previous primary address status
                            if (userShippingAddressDto.IsPrimaryAddress == true)
                            {
                                var Address = _userAdditionalShippingRepository.GetAllList(i => i.UserId == AbpSession.UserId.Value && i.IsPrimaryAddress == true && i.Id != userShippingAddressDto.Id);
                                if (Address != null)
                                {
                                    foreach (var add in Address)
                                    {
                                        add.IsPrimaryAddress = false;
                                        await _userAdditionalShippingRepository.UpdateAsync(add);
                                    }
                                }
                            }
                            #endregion

                            await _userAdditionalShippingRepository.UpdateAsync(userShippingAddressmodel);
                        }
                        else
                        {
                            IsErrorExists = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (IsErrorExists == true)
            {
                throw new UserFriendlyException("Email address already exists in the system");
            }
        }


        public async Task<UserShippingAddressDto> GetUserPrimaryAddress()
        {
            long UserId = AbpSession.UserId.Value;
            UserShippingAddressDto Response = new UserShippingAddressDto();
            try
            {
                var User = _userRepository.GetAllList(i => i.Id == UserId);
                var UserDetails = _userShippingAddressRepository.GetAllList(i => i.UserId == UserId && i.IsPrimaryAddress == true);


                Response = (from user in User
                            join detail in UserDetails on user.Id equals detail.UserId
                            select new UserShippingAddressDto
                            {
                                Id = detail.Id,
                                FirstName = detail.FirstName,
                                LastName = detail.LastName,
                                StateId = detail.StateId,
                                CountryId = detail.CountryId,
                                City = detail.City,
                                StreetAddress = detail.StreetAddress,
                                PostCode = detail.PostCode,
                                BusinessEmail = detail.BusinessEmail,
                                CompanyName = detail.CompanyName,
                                MobileNumber = detail.MobileNumber,
                                StateName = detail.State.StateName,
                                CountryName = detail.Country.CountryName,
                                IsPrimaryAddress = detail.IsPrimaryAddress

                            }).FirstOrDefault();

            }
            catch (Exception ex)
            {

            }
            return Response;
        }

        public async Task<List<UserShippingAddressDto>> GetUserAllShippingAddress()
        {
            long UserId = AbpSession.UserId.Value;
            List<UserShippingAddressDto> Response = new List<UserShippingAddressDto>();
            try
            {

                await _connectionUtility.EnsureConnectionOpenAsync();

                IEnumerable<UserShippingAddressDto> ResponseData = await _db.QueryAsync<UserShippingAddressDto>(@"SELECT UserShippingAddress.Id,FirstName,LastName,StateId, UserShippingAddress.CountryId, City,StreetAddress,PostCode, BusinessEmail,
                                                                   CompanyName,MobileNumber,States.StateName,Countries.CountryName
                                                                   FROM[dbo].[UserShippingAddress] 
																   INNER JOIN AbpUsers ON  UserShippingAddress.UserId = AbpUsers.Id
																   INNER JOIN  States  ON  States.Id =UserShippingAddress.StateId  
	                                                               INNER JOIN Countries ON Countries.Id =  UserShippingAddress.CountryId
	                                                               where UserShippingAddress.IsDeleted=0  and UserId = " + UserId + "", new
                {
                }, commandType: System.Data.CommandType.Text);
                Response = ResponseData.ToList();

            }
            catch (Exception ex)
            {

            }
            return Response;
        }


        public async Task<List<UserAdditionalShippingAddressDto>> GetUserAllAdditionalShippingAddress(long? ShippingAddressId)
        {

            long UserId = AbpSession.UserId.Value;
            List<UserAdditionalShippingAddressDto> Response = new List<UserAdditionalShippingAddressDto>();
            try
            {


                await _connectionUtility.EnsureConnectionOpenAsync();

                IEnumerable<UserAdditionalShippingAddressDto> ResponseDataList = await _db.QueryAsync<UserAdditionalShippingAddressDto>
                    (@"SELECT UserAdditionalShippingAddress.Id,FirstName,LastName,StateId, UserAdditionalShippingAddress.CountryId,ShippingAddressId, City,StreetAddress,PostCode, BusinessEmail,
                                                                   CompanyName,MobileNumber,States.StateName,Countries.CountryName
                                                                   FROM[dbo].[UserAdditionalShippingAddress] 
																   INNER JOIN AbpUsers ON UserAdditionalShippingAddress.UserId = AbpUsers.Id
																   INNER JOIN  States  ON  States.Id =UserAdditionalShippingAddress.StateId  
                                                                   INNER JOIN Countries ON Countries.Id =  UserAdditionalShippingAddress.CountryId and UserId = " + UserId + "", new
                    {
                    }, commandType: System.Data.CommandType.Text);

                if (ShippingAddressId.HasValue)
                {
                    ResponseDataList = ResponseDataList.Where(i => i.ShippingAddressId == ShippingAddressId.Value).ToList();
                }

                Response = ResponseDataList.ToList();
            }
            catch (Exception ex)
            {

            }
            return Response;
        }

        #region Save file in www root location of solution
        private string SaveFileInWWWRootLocation(byte[] ImgStr, string ImgName, string WebrootPath)
        {
            string FileLocation = string.Empty;
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

                System.IO.File.WriteAllBytes(imgPath, imageBytes);
                FileLocation = WebrootPath + "//" + imageName;
            }
            catch (Exception ex)
            {


            }
            return FileLocation;
        }
        #endregion

        public async Task DeleteBulkUserShippingAddress(List<long> Ids)
        {
            try
            {
                var UserShippingAddress = _userShippingAddressRepository.GetAllList(p => Ids.Any(p2 => p2 == p.Id)).ToList();

                if (UserShippingAddress != null)
                {

                    _userShippingAddressRepository.BulkDelete(UserShippingAddress);
                    var UserAdditionalShippingAddress = _userAdditionalShippingRepository.GetAllList(p => UserShippingAddress.Any(p2 => p2.Id == p.ShippingAddressId)).ToList();

                    if (UserAdditionalShippingAddress != null)
                    {
                        _userAdditionalShippingRepository.BulkDelete(UserAdditionalShippingAddress);
                    }
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task DeleteBulkUserAdditionalAddress(List<long> Ids)
        {
            var UserShippingAddress = _userAdditionalShippingRepository.GetAllList(p => Ids.Any(p2 => p2 == p.Id)).ToList();

            if (UserShippingAddress != null)
            {
                _userAdditionalShippingRepository.BulkDelete(UserShippingAddress);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAdditionalAddress(long Id)
        {
            var Address = _userAdditionalShippingRepository.GetAllList(i => i.Id == Id).FirstOrDefault();
            if (Address != null)
            {
                await _userAdditionalShippingRepository.DeleteAsync(Address);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        public async Task DeleteUserShippingAddress(long Id)
        {
            var Address = _userShippingAddressRepository.GetAllList(i => i.Id == Id).FirstOrDefault();
            if (Address != null)
            {
                await _userShippingAddressRepository.DeleteAsync(Address);

                var AdditionalAddress = _userAdditionalShippingRepository.GetAllList(i => i.ShippingAddressId == Address.Id).ToList();

                if (AdditionalAddress.Count > 0)
                {
                    _userAdditionalShippingRepository.BulkDelete(AdditionalAddress);
                }
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        public async Task<UserShippingAddressDto> GetUserShippingAddressById(long Id)
        {
            long UserId = AbpSession.UserId.Value;
            UserShippingAddressDto Response = new UserShippingAddressDto();
            UserShippingAddressDto productDetailsDto = new UserShippingAddressDto();
            try
            {
                var UserDetails = _userShippingAddressRepository.GetAllList(i => i.Id == Id).FirstOrDefault();
                productDetailsDto = ObjectMapper.Map<UserShippingAddressDto>(UserDetails);
                productDetailsDto.CountryName = _countriesRepository.GetAllList(i => i.Id == UserDetails.CountryId).Select(i => i.CountryName).FirstOrDefault();
                productDetailsDto.StateName = _statesRepository.GetAllList(i => i.Id == UserDetails.StateId).Select(i => i.StateName).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }
            return productDetailsDto;
        }

        public async Task<UserAdditionalShippingAddressDto> GetUserAdditionalAddressById(long Id)
        {
            long UserId = AbpSession.UserId.Value;
            UserAdditionalShippingAddressDto productDetailsDto = new UserAdditionalShippingAddressDto();
            try
            {
                var UserDetails = _userAdditionalShippingRepository.GetAllList(i => i.Id == Id).FirstOrDefault();
                productDetailsDto = ObjectMapper.Map<UserAdditionalShippingAddressDto>(UserDetails);
                productDetailsDto.CountryName = _countriesRepository.GetAllList(i => i.Id == UserDetails.CountryId).Select(i => i.CountryName).FirstOrDefault();
                productDetailsDto.StateName = _statesRepository.GetAllList(i => i.Id == UserDetails.StateId).Select(i => i.StateName).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return productDetailsDto;
        }


        public async Task TrialApi()
        {
            try
            {
                string Token = "B64A6D78FD67F55D147C81DEAA7C3F3AF8AC9A75";
                var URL = "https://orso.biz/api/?token=B64A6D78FD67F55D147C81DEAA7C3F3AF8AC9A75&command=products&created_at={{created_at}}&sort=created_at";

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataa = response.Content.ReadAsStringAsync().Result;
                    //var dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObject>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                    //foreach (var d in dataObjects)
                    //{
                    //    Console.WriteLine("{0}", d.Data);
                    //}
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }

                // Make any other calls using HttpClient here.

                // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
                client.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        public async Task<UserBusinessSettingsDto> GetUserSettingsData()
        {
            var TenantId = Convert.ToInt32(AbpSession.TenantId);
            var data = new UserBusinessSettingsDto();
            try
            {
                var DepartmentUser = (from department in _departmentMasterRepository.GetAllList()
                                      select new DepartmentUsersDto
                                      {
                                          DepartmentName = department.DepartmentName,
                                          IsActive = department.IsActive,
                                          Id = department.Id

                                      }).ToList();
                var userShippingDetails = await _db.QueryAsync<UserShippingAddressDto>(@"select Id, TenantId ,UserId ,StreetAddress ,StateId ,CountryId ,City ,PostCode ,IsPrimaryAddress ,
                BusinessEmail ,CompanyName ,FirstName ,LastName ,MobileNumber,
                AddressType FROM UserShippingAddress where TenantId = " + TenantId + " and IsDeleted=0", new
                {

                }, commandType: System.Data.CommandType.Text);
                var WareHouseMaster = (from wareHouse in _wareHouseMasterRepository.GetAllList()
                                       join settings in _userBusinessSettingsRepository.GetAllList() on wareHouse.SettingsId equals settings.Id
                                       join states in _statesRepository.GetAllList() on wareHouse.StateId equals states.Id
                                       join country in _countriesRepository.GetAllList() on wareHouse.CountryId equals country.Id
                                       select new WareHouseMasterDto
                                       {
                                           Id= wareHouse.Id,
                                           WarehouseTitle = wareHouse.WarehouseTitle,
                                           StreetAddress = wareHouse.StreetAddress,
                                           City = wareHouse.City,
                                           StateId = new StateDto
                                           {
                                               StateName = string.IsNullOrEmpty(states.StateName) ? "" : (states.StateName),
                                               CountryId = states.CountryId.ToString() == "" ? 0 : Convert.ToInt64(states.CountryId),
                                               IsActive = states.IsActive
                                           },
                                           PostCode = wareHouse.PostCode,
                                           CountryId = new CountryDto
                                           {
                                               CountryName = string.IsNullOrEmpty(wareHouse.Country.CountryName) ? "" : (wareHouse.Country.CountryName),
                                               IsActive = wareHouse.Country.IsActive
                                           },
                                           TimezoneId = wareHouse.TimezoneId,
                                           IsActive = wareHouse.IsActive  
                                       }).ToList();
                var StoreMaster = (from storeMaterModel in _storeMasterRepository.GetAllList()
                                   select new StoreMasterDto
                                   {
                                       Id= storeMaterModel.Id,
                                       StoreName = storeMaterModel.StoreName,
                                       StoreLocation = storeMaterModel.StoreLocation,
                                       IsActive = storeMaterModel.IsActive,
                                       StoreOpeningTimings = (from master in _storeMasterRepository.GetAllList()
                                                              join timing in _storeOpeningTimingsRepository.GetAllList() on master.Id equals timing.StoreMasterId
                                                              where timing.StoreMasterId == storeMaterModel.Id
                                                              select new StoreOpeningTimingsDto
                                                              {
                                                                  WeekDay = timing.WeekDay,
                                                                  OpeningTime = timing.OpeningTime,
                                                                  ClosingTime = timing.ClosingTime,
                                                                  IsCompleteDay = timing.IsCompleteDay,
                                                                  IsActive = timing.IsActive,
                                                                  Id = timing.Id
                                                              }).ToList()
                                   }).ToList();
                var userBusinessSettingsModel = await _db.QueryFirstOrDefaultAsync<UserBusinessSettingCustomDto>("ups_GetUserSettingDetails", new
                {
                    TenantId = TenantId
                }, commandType: System.Data.CommandType.StoredProcedure);



                data = new UserBusinessSettingsDto()
                {
                    Id = userBusinessSettingsModel.Id,
                    AccountName = userBusinessSettingsModel.AccountName,
                    BankName = userBusinessSettingsModel.BankName,
                    BranchAddress = userBusinessSettingsModel.BranchAddress,
                    BusinessEmail = userBusinessSettingsModel.BusinessEmail,
                    BusinessName = userBusinessSettingsModel.BusinessName,
                    BusinessWebsite = userBusinessSettingsModel.BusinessWebsite,
                    CompanyNumber = userBusinessSettingsModel.CompanyNumber,
                    ConnectDomain = userBusinessSettingsModel.ConnectDomain,
                    CurrencyMasterId = userBusinessSettingsModel.CurrencyMasterId,
                    DepartmentUser = DepartmentUser,
                    EmailAddress = userBusinessSettingsModel.EmailAddress,
                    FacebookLink = userBusinessSettingsModel.FacebookLink,
                    FirstName = userBusinessSettingsModel.FirstName,
                    LastName = userBusinessSettingsModel.LastName,
                    InstagramLink = userBusinessSettingsModel.InstagramLink,
                    IsActive = userBusinessSettingsModel.IsActive,
                    IsChargeMinimumOrderFee = userBusinessSettingsModel.IsChargeMinimumOrderFee,
                    IsChargeTaxGstorVst = userBusinessSettingsModel.IsChargeTaxGstorVst,
                    IsChargeTaxOnShipping = userBusinessSettingsModel.IsChargeTaxOnShipping,
                    IsPayPalAccount = userBusinessSettingsModel.IsPayPalAccount,
                    IsShipingRatesIncludingTax = userBusinessSettingsModel.IsShipingRatesIncludingTax,
                    IsShowPriceTax = userBusinessSettingsModel.IsShowPriceTax,
                    IsShowPriceWithoutAccount = userBusinessSettingsModel.IsShowPriceWithoutAccount,
                    IsStripeAccount = userBusinessSettingsModel.IsStripeAccount,
                    MeasurementId = userBusinessSettingsModel.MeasurementId,
                    MOQFee = userBusinessSettingsModel.MOQFee,
                    MOQFeeSKU = userBusinessSettingsModel.MOQFeeSKU,
                    NumberOfDaysQuoteIsValid = userBusinessSettingsModel.NumberOfDaysQuoteIsValid,
                    PinterestLink = userBusinessSettingsModel.PinterestLink,
                    PositionId = userBusinessSettingsModel.PositionId,
                    ProductPricePrefix = userBusinessSettingsModel.ProductPricePrefix,
                    QuoteTerms = userBusinessSettingsModel.QuoteTerms,
                    SwiftCode = userBusinessSettingsModel.SwiftCode,
                    TaxCode = userBusinessSettingsModel.TaxCode,
                    TaxPercentOnAddProduct = userBusinessSettingsModel.TaxPercentOnAddProduct,
                    TwitterLink = userBusinessSettingsModel.TwitterLink,
                    UserMobileNumber = userBusinessSettingsModel.UserMobileNumber,
                    UserPhoneNumber = userBusinessSettingsModel.UserPhoneNumber,
                    ValidNoOfDays = userBusinessSettingsModel.ValidNoOfDays,
                    UserShippingAddress = userShippingDetails.Where(i=>i.AddressType == 1).FirstOrDefault(),
                    UserBillingAddress = userShippingDetails.Where(i => i.AddressType == 2).FirstOrDefault(),
                    WareHouseMaster = WareHouseMaster,
                    StoreMaster = StoreMaster,
                    TermsAndCondition = userBusinessSettingsModel.TermsAndCondition,
                    businessPhoneNumber = userBusinessSettingsModel.BusinessContactNumber,
                    BSB = userBusinessSettingsModel.BSB,
                    AccountNumber = userBusinessSettingsModel.AccountNumber,
                    IsHideContactDetails = userBusinessSettingsModel.IsHideContactDetails,
                    BusinessSettingsLogo = new ProductImageType //new BusinessImageLogoDto
                    {
                        ImagePath = userBusinessSettingsModel.BusinessLogoUrl,
                        Url = userBusinessSettingsModel.BusinessLogoUrl,
                        FileName = userBusinessSettingsModel.LogoName,
                        Name = userBusinessSettingsModel.LogoName,
                        Ext = userBusinessSettingsModel.LogoExt,
                        Size = userBusinessSettingsModel.LogoSize,
                        Type = userBusinessSettingsModel.LogoType
                    },
                    BusinessSettingsFavicon = new ProductImageType
                    {
                        Ext = userBusinessSettingsModel.FaviconExt,
                        FileName = userBusinessSettingsModel.FaviconImageName,
                        ImagePath = userBusinessSettingsModel.FaviconImageURL,
                        Size = userBusinessSettingsModel.FaviconSize,
                        Url = userBusinessSettingsModel.FaviconImageURL,
                        Type = userBusinessSettingsModel.FaviconType,
                        Name= userBusinessSettingsModel.FaviconImageName
                    }
                };

            }
            catch (Exception ex)
            {
            }

            return data;


        }

        public async Task UpdateWarehouseById(WareHouseMasterDto Model)
        {
            try
            {
                var WarehouseData = _wareHouseMasterRepository.GetAllList(i => i.Id == Model.Id).FirstOrDefault();
                if(WarehouseData != null)
                {
                    WarehouseData.WarehouseTitle = Model.WarehouseTitle;
                    WarehouseData.StreetAddress = Model.StreetAddress;
                    WarehouseData.City = Model.City;
                    if (Model.CountryId.Id > 0)
                    {
                        WarehouseData.CountryId = Model.CountryId.Id;
                    }
                    if (Model.StateId.Id> 0)
                    {
                        WarehouseData.StateId = Model.StateId.Id;
                    }
                    WarehouseData.PostCode = Model.PostCode;
                    WarehouseData.TimezoneId = Model.TimezoneId;
                    await _wareHouseMasterRepository.UpdateAsync(WarehouseData);
                    
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
