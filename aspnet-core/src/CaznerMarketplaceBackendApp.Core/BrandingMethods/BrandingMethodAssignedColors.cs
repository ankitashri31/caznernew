using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethods
{
    public class BrandingMethodAssignedColors : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual ProductColourMaster ProductColourMaster { get; set; }
        [ForeignKey("ProductColourMaster")]
        public long ColorId { get; set; }
        public virtual BrandingMethodMaster BrandingMethodMaster { get; set; }
        [ForeignKey("BrandingMethodMaster")]
        public long BrandingMethodId { get; set; }
        public bool IsActive { get; set; }
    }
}
