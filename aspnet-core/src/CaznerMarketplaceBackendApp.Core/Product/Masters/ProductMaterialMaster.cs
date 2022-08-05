using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    public class ProductMaterialMaster:FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string ProductMaterialName { get; set; }
        public bool IsActive { get; set; }
    }
}
