using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductBrandingPriceVariants : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual BrandingMethodMaster BrandingMethod { get; set; }
        [ForeignKey("BrandingMethod")]
        public long? BrandingMethodId { get; set; }

        public string Quantity { get; set; }

        public string Price { get; set; }
        public string Price1 { get; set; }
        public string Price2 { get; set; }
        public string Price3 { get; set; }

        public string BrandingUnitPrice { get; set; }

        public bool IsActive { get; set; }

    }
}
