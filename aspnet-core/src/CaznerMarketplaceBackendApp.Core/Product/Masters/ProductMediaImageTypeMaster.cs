using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductMediaImageTypeMaster : FullAuditedEntity<long>
    {
        public string ProductMediaImageTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
