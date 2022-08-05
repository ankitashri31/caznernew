using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Category
{
    public class CalculationTypes : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
    }
}
