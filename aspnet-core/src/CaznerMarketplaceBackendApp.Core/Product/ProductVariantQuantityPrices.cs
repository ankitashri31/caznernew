using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductVariantQuantityPrices : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual ProductVariantsData ProductVariantsData { get; set; }
        [ForeignKey("ProductVariantsData")]
        public long ProductVariantId { get; set; }
        public int QuantityFrom { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
