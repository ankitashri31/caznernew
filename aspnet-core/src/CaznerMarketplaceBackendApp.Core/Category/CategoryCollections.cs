using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Category
{
    public class CategoryCollections : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual CategoryMaster CategoryMaster { get; set; }
        [ForeignKey("CategoryMaster")]
        public long CategoryId { get; set; }
        public virtual CollectionMaster CollectionMaster { get; set; }
        [ForeignKey("CollectionMaster")]
        public long CollectionId { get; set; }
        public bool IsActive { get; set; }
    }
}
