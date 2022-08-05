using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Category
{
    public class CollectionMaster : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string CollectionName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public byte[] ImageData { get; set; }
        public bool IsManualCalculation { get; set; }
        public bool IsMatchAnyCondition { get; set; }

        public bool IsSeoEnabled { get; set; }
        public string SeoPageTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoUrl { get; set; }

        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

    }
}
