using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaznerMarketplaceBackendApp.Category
{
    public class CategoryMaster : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public string CategoryTitle { get; set; }

        public string CategoryImageUrl { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }

        public bool IsActive { get; set; }

        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

    }
}
