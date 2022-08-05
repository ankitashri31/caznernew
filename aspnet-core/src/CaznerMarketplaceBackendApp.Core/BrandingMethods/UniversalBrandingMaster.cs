using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethods
{
    public class UniversalBrandingMaster:FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string UniversalBrandingTitle { get; set; }
        public bool IsActive { get; set; }
    }
}
