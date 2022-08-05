using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Currency;
using CaznerMarketplaceBackendApp.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Authorization.Users
{
    public class UserBusinessSettings : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }

        public virtual CurrencyMaster CurrencyMaster { get; set; }
        [ForeignKey("CurrencyMaster")]
        public long? CurrencyMasterId { get; set; }
       
        public int? MeasurementId { get; set; }

        public double IsChargeMinimumOrderFee { get; set; }
        public bool IsShowPriceWithoutAccount { get; set; }
        public bool IsChargeTaxGstorVst { get; set; }
        public string TaxPercentOnAddProduct { get; set; }
        public string TaxCode { get; set; }
        public string ProductPricePrefix { get; set; }
        public bool IsShipingRatesIncludingTax { get; set; }
        public bool IsChargeTaxOnShipping { get; set; }
        public bool IsShowPriceTax { get; set; }
        public int ValidNoOfDays { get; set; }
        public string ConnectDomain { get; set; }
        public string InstagramLink { get; set; }
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string PinterestLink { get; set; }
        public byte[] FaviconImageData { get; set; }
        public string FaviconImageName { get; set; }
        public string FaviconImageURL { get; set; }
        public string FaviconExt { get; set; }
        public string FaviconSize { get; set; }
        public string FaviconType { get; set; }
        public string FaviconName { get; set; }
        public string FaviconUrl { get; set; }
        public bool IsActive { get; set; }

        public string BusinessName { get; set; }
        public string CompanyNumber { get; set; }

        public string BusinessWebsite { get; set; }

        public string BusinessEmail { get; set; }

        public string BusinessContactNumber { get; set; }

        public string BusinessLogoUrl { get; set; }
        public string LogoExt { get; set; }
        public string LogoSize { get; set; }
        public string LogoType { get; set; }
        public string LogoName { get; set; }
        public string LogoUrl { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public long PositionId { get; set; }

        public string UserPhoneNumber { get; set; }

        public string UserMobileNumber { get; set; }

        public string MOQFeeSKU { get; set; }

        public double MOQFee { get; set; }

        public int NumberOfDaysQuoteIsValid { get; set; }

        public string QuoteTerms { get; set; }
        public string TermsAndCondition { get; set; }
        public string BSB { get; set; }
        public string AccountNumber { get; set; }
        public bool IsHideContactDetails { get; set; }
    }

}
