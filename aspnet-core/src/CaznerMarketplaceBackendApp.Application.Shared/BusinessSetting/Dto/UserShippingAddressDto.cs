using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BusinessSetting.Dto
{
    public class UserShippingAddressDto : EntityDto<long>
    {
        public long Id { get; set; }
        public long? ShippingAddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessEmail { get; set; }
        public string CompanyName { get; set; }
        public string MobileNumber { get; set; }
        public string StreetAddress { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public long? StateId { get; set; }
       
        public long? CountryId { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string PONumber { get; set; }
        public bool IsPrimaryAddress { get; set; }
        public int AddressType { get; set; }
    }
}
