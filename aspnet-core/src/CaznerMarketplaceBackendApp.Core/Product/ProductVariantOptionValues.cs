using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductVariantOptionValues : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual ProductVariantsData ProductVariantsData { get; set; }
        [ForeignKey("ProductVariantsData")]
        public long ProductVariantId { get; set; }
        public virtual ProductOptionsMaster ProductOptions { get; set; }
        [ForeignKey("ProductOptions")]
        public long VariantOptionId { get; set; }
        public string VariantOptionValue { get; set; }
        public bool IsActive { get; set; }

    }
}
