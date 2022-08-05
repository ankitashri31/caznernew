using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Store
{
    public class StoreMaster : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
        public bool IsActive { get; set; }
    }
}
