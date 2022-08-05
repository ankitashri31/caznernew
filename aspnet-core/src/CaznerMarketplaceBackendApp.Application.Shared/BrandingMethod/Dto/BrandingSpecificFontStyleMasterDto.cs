using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethod.Dto
{
    public class BrandingSpecificFontStyleMasterDto: Entity<long>
    {
        public int TenantId { get; set; }
        public string FontStyleTitle { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public long AssignmentId { get; set; }
    }
}
