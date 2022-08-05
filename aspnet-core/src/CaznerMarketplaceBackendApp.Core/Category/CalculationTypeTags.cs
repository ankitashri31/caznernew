using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Category
{
    public class CalculationTypeTags : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string TypeTitle { get; set; }
        public bool IsActive { get; set; }
    }
}
