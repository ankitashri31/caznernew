using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CaznerMarketplaceBackendApp.Category;
using CaznerMarketplaceBackendApp.SubCategory;

namespace CaznerMarketplaceBackendApp.Product
{
  public  class ProductAssignedSubSubCategories : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual SubCategoryMaster SubCategoryMaster { get; set; }
        [ForeignKey("SubCategoryMaster")]
        public long SubSubCategoryId { get; set; }
        public virtual CategoryMaster CategoryMaster { get; set; }
        [ForeignKey("CategoryMaster")]
        public long? SubCategoryId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
