using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.UniversalCategory
{
    public class UniversalCategoryMaster : FullAuditedEntity<long>
    {
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

    }
}
