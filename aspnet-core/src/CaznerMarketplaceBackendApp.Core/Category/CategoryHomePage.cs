using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Category
{
    public class CategoryHomePage : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual CategoryGroupMaster CategoryGroupMaster { get; set; }
        [ForeignKey("CategoryGroupMaster")]
        public long CategoryId { get; set; }
        public int SequenceNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
