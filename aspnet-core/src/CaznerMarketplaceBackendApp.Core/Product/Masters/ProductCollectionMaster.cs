using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductCollectionMaster : FullAuditedEntity<long>
    {
        public string ProductCollectionName { get; set; }
        public bool IsActive { get; set; }
    }
}
