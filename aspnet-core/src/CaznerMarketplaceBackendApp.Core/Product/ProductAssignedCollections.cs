using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CaznerMarketplaceBackendApp.Category;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductAssignedCollections : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual CollectionMaster ProductCollectionMaster { get; set; }
        [ForeignKey("ProductCollectionMaster")]
        public long CollectionId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        [ForeignKey("ProductMaster")]
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
