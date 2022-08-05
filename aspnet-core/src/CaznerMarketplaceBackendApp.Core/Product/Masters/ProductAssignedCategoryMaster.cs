using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Masters
{
    public class ProductAssignedCategoryMaster :  FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual CategoryGroupMaster CategoryGroupMaster { get; set; }
        [ForeignKey("CategoryGroupMaster")]
        public long CategoryId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
