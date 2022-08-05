using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Category
{
    public class CategoryGroups : FullAuditedEntity<long>, IMustHaveTenant
    {
        public virtual CategoryGroupMaster CategoryGroupMaster { get; set; }
        [ForeignKey("CategoryGroupMaster")]
        public long CategoryGroupId { get; set; }

        public virtual CategoryMaster CategoryMaster { get; set; }
        [ForeignKey("CategoryMaster")]
        public long CategoryMasterId { get; set; }
        //public int SequenceNumber { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get ; set ; }
    }
}
