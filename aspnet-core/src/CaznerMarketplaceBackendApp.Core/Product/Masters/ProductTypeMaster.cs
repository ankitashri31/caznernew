using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductTypeMaster : FullAuditedEntity<long>
    {
        public string ProductTypeName { get; set; }
        public bool IsActive { get; set; }       

    }
}
