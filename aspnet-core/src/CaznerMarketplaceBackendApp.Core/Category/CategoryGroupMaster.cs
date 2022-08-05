using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CaznerMarketplaceBackendApp.Category
{
    public class CategoryGroupMaster : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string GroupTitle { get; set; }
        public bool IsActive { get; set; }
        public string GroupImageUrl { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int SequenceNumber { get; set; }

    }
}
