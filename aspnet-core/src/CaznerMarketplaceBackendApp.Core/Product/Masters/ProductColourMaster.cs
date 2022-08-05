using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductColourMaster : FullAuditedEntity<long>, IMustHaveTenant
    {
        public string ProductColourName { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get ; set ; }
    }
}
