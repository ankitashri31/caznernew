using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethods
{
    public class BrandingMethodAssignedUniversals:FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual UniversalBrandingMaster UniversalBrandingMaster { get; set; }
        [ForeignKey("UniversalBrandingMaster")]
        public long UniversalId { get; set; }
        public virtual BrandingMethodMaster BrandingMethodMaster { get; set; }
        [ForeignKey("BrandingMethodMaster")]
        public long BrandingMethodId { get; set; }
        public bool IsActive { get; set; }
    }
}
