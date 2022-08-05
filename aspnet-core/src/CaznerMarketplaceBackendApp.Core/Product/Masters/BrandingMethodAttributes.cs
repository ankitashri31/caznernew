using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Masters
{
    public class BrandingMethodAttributes : FullAuditedEntity<long>
    {
        public string AttributeName { get; set; }
        public bool IsActive { get; set; }
    }
}
