using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductTagMaster : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string ProductTagName { get; set; }
        public bool IsActive { get; set; }
    }
}
