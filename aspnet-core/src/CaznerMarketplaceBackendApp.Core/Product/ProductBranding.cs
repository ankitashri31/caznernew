using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
   public class ProductBranding : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }

        public virtual BrandingMethodMaster BrandingMethodMaster { get; set; }
        [ForeignKey("BrandingMethodMaster")]
        public long ProductMethodId { get; set; }

        public int BrandingCounts { get; set; }
        public string ImageUrl { get; set; }
    }
}
