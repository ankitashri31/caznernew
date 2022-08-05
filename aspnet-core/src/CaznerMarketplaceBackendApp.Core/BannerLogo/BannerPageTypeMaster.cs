using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BannerLogo
{
    public class BannerPageTypeMaster : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}
