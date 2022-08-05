using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Country;
using CaznerMarketplaceBackendApp.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.WareHouse
{
    public class WareHouseMaster : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string WarehouseTitle { get; set; }
        public string StreetAddress{ get; set; }
        public string City { get; set; }
        public virtual States State { get; set; }
        [ForeignKey("State")]
        public long StateId { get; set; }
        public string PostCode { get; set; }
        public virtual Countries Country { get; set; }
        [ForeignKey("Country")]
        public long CountryId { get; set; }
        public virtual UserBusinessSettings UserBusinessSettings { get; set; }
        [ForeignKey("UserBusinessSettings")]
        public long? SettingsId { get; set; }
        public string TimezoneId { get; set; }
        public bool IsActive { get; set; }


}
}
