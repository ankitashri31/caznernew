using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Authorization.Users;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethods
{
   public class BrandingMethodAssignedVendors : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public long VendorUserId { get; set; }
        public virtual BrandingMethodMaster BrandingMethodMaster { get; set; }
        [ForeignKey("BrandingMethodMaster")]
        public long BrandingMethodId { get; set; }
        public bool IsActive { get; set; }
    }
}
