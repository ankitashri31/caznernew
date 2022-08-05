using System;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using CaznerMarketplaceBackendApp.MultiTenancy;

namespace CaznerMarketplaceBackendApp.MultiTenancy.Dto
{
    public class CreateTenantRegistration
    {
        //[Required]
        //[StringLength(AbpTenantBase.MaxTenancyNameLength)]
        //[RegularExpression(TenantConsts.TenancyNameRegex)]
        //public string TenancyName { get; set; }
        public long Id { get; set; }

        [Required]
        [StringLength(TenantConsts.MaxNameLength)]
        public string Name { get; set; }
        public string SurName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        [StringLength(AbpUserBase.MaxPasswordLength)]
        [DisableAuditing]
        public string AdminPassword { get; set; }

        //[MaxLength(AbpTenantBase.MaxConnectionStringLength)]
        //[DisableAuditing]
        //public string ConnectionString { get; set; }

        public bool ShouldChangePasswordOnNextLogin { get; set; }

        public bool SendActivationEmail { get; set; }

       // public int? EditionId { get; set; }

        public bool IsActive { get; set; }

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public bool IsInTrialPeriod { get; set; }

        //public string UserName { get; set; }
   
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }

        // public UserDetailsDto UserDetail { get; set; }

        public string BusinessEmail { get; set; }

        public string CompanyName { get; set; }
        public string BusinessTradingName { get; set; }

        public string BusinessPhoneNumber { get; set; }

        public string RegistrationBusinessNumber { get; set; }

        public long? PositionId { get; set; }
        public string Title { get; set; }
        public string WebsiteUrl { get; set; }

        public string MobileNumber { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }
        public long? StateId { get; set; }
        public long? CountryId { get; set; }

        public string PostCode { get; set; }

        public string CompanyPrivateEmail { get; set; }

        public string TimeZoneId { get; set; }

        public string CompanyComments { get; set; }

        public string CompanyPhoneNumber { get; set; }
        public string CompanyPublicEmail { get; set; }
        public bool Status { get; set; }
        public long SubscriptionPlanId { get; set; }
        public int TenantId { get; set; }
    }

    public class CreateDistributorRegistration
    {
       
        public long Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }

        public string AdminEmailAddress { get; set; }
     
        public string AdminPassword { get; set; }

        public bool ShouldChangePasswordOnNextLogin { get; set; }

        public bool SendActivationEmail { get; set; }

        public bool IsActive { get; set; }

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public bool IsInTrialPeriod { get; set; }

        //public string UserName { get; set; }

        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }

        // public UserDetailsDto UserDetail { get; set; }

        public string BusinessEmail { get; set; }

        public string CompanyName { get; set; }
        public string BusinessTradingName { get; set; }

        public string BusinessPhoneNumber { get; set; }

        public string RegistrationBusinessNumber { get; set; }

        public long? PositionId { get; set; }
        public string Title { get; set; }
        public string WebsiteUrl { get; set; }

        public string MobileNumber { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }
        public long? StateId { get; set; }
        public long? CountryId { get; set; }

        public string PostCode { get; set; }

        public string CompanyPrivateEmail { get; set; }

        public string TimeZoneId { get; set; }

        public string CompanyComments { get; set; }

        public string CompanyPhoneNumber { get; set; }
        public string CompanyPublicEmail { get; set; }
        public bool Status { get; set; }
        public long SubscriptionPlanId { get; set; }

        public string EncryptedTenantId { get; set; }
    }
}