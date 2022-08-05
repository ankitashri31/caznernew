using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.BusinessSetting.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Authorization.Users.Dto
{
    public class UserDetailsDto : EntityDto<long>
    {
        public int TenantId { get; set; }

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
    }


    public class UserResponseDto 
    {
        public int TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public long UserId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string BusinessEmail { get; set; }

        public string CompanyName { get; set; }

        public string BusinessTradingName { get; set; }

        public string BusinessPhoneNumber { get; set; }

        public string RegistrationBusinessNumber { get; set; }

        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public string Title { get; set; }
        public string WebsiteUrl { get; set; }

        public string MobileNumber { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }
        public long? StateId { get; set; }
        public long? CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string PostCode { get; set; }

        public string CompanyPrivateEmail { get; set; }

        public string TimeZoneId { get; set; }

        public string CompanyComments { get; set; }

        public string CompanyPhoneNumber { get; set; }
        public string CompanyPublicEmail { get; set; }
        public bool Status { get; set; }
        public bool IsRejected { get; set; }
        public UserShippingAddressDto ShippingAddress  { get; set; }
    }
}
