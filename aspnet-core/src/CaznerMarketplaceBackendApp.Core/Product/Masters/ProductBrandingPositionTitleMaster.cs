using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductBrandingPositionTitleMaster : FullAuditedEntity<long>
    {
        public string ProductBrandingPositionTitleName { get; set; }
        public bool IsActive { get; set; }
    }
}
