using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaznerMarketplaceBackendApp.Category
{
    public class CollectionHomePage : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual CollectionMaster CollectionMaster { get; set; }
        [ForeignKey("CollectionMaster")]
        public long CollectionId { get; set; }
        public int NumberOfProducts { get; set; }
        public int SequenceNumber { get; set; }
        public string VideoUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
