using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.Store.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BusinessSetting.Dto
{
    public class UserBusinessSettingCustomDto : EntityDto<long>
    {
        public long? CurrencyMasterId { get; set; }
        public int? MeasurementId { get; set; }
        public string IsChargeMinimumOrderFee { get; set; }
        public bool IsShowPriceWithoutAccount { get; set; }
        public bool IsChargeTaxGstorVst { get; set; }
        public string TaxPercentOnAddProduct { get; set; }
        public string TaxCode { get; set; }
        public string ProductPricePrefix { get; set; }
        public bool IsShipingRatesIncludingTax { get; set; }
        public bool IsChargeTaxOnShipping { get; set; }
        public bool IsShowPriceTax { get; set; }
        public string ValidNoOfDays { get; set; }
        public bool IsActive { get; set; }

        //bankdetails
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string AccountName { get; set; }
        public string BranchAddress { get; set; }
        public bool IsStripeAccount { get; set; }
        public bool IsPayPalAccount { get; set; }
        //connect domain
        public string ConnectDomain { get; set; }
        //social
        public string InstagramLink { get; set; }
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string PinterestLink { get; set; }
        public string FaviconImageName { get; set; }
        public double MOQFee { get; set; }
        public string MOQFeeSKU { get; set; }
        public int NumberOfDaysQuoteIsValid { get; set; }
        public string QuoteTerms { get; set; }

        public string BusinessName { get; set; }
        public string CompanyNumber { get; set; }
        public string BusinessWebsite { get; set; }
        public string UserPhoneNumber { get; set; }
        public string BusinessEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserMobileNumber { get; set; }
        public long PositionId { get; set; }
        public string BusinessLogoUrl { get; set; }
        public string LogoExt { get; set; }
        public string LogoName { get; set; }
        public string LogoSize { get; set; }
        public string LogoType { get; set; }
        public string FaviconExt { get; set; }
        public string FaviconSize { get; set; }
        public string FaviconType { get; set; }
        public string FaviconImageURL { get; set; }
        public string TermsAndCondition { get; set; }
        public string BusinessContactNumber { get; set; }
        public string BSB { get; set; }
        public string AccountNumber { get; set; }
        public bool IsHideContactDetails { get; set; }
    }


}
