using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductBulkUploadVariations : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }       
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }
        public virtual ProductOptionsMaster ProductOptionMaster { get; set; }
        [ForeignKey("ProductOptionMaster")]
        public long productOptionId { get; set; }
        public string ProductOptionValue { get; set; }
        public bool IsActive { get; set; }
    }
}
