using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Country;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Authorization.Users
{
    public class UserAdditionalShippingAddress : FullAuditedEntity<long>, IMustHaveTenant
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessEmail { get; set; }
        public string CompanyName { get; set; }
        public string MobileNumber { get; set; }
        public int TenantId { get; set; }

        public virtual UserShippingAddress UserShippingAddress { get; set; }
        [ForeignKey("UserShippingAddress")]
        public long? ShippingAddressId { get; set; }

        public virtual User User { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public string StreetAddress { get; set; }
        public virtual States State { get; set; }
        [ForeignKey("State")]
        public long? StateId { get; set; }
        public virtual Countries Country { get; set; }
        [ForeignKey("Country")]
        public long? CountryId { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public bool IsPrimaryAddress { get; set; }

    }
}
