using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductDetails : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string ProductTypeArray { get; set; }
        public string ProductBrandArray { get; set; }
        public string ProductCollectionArray { get; set; }
        public string ProductMaterialArray { get; set; }
        public string ProductTagArray { get; set; }      
        public string ProductVendorArray { get; set; }
        public string ProductCategoryArray { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
