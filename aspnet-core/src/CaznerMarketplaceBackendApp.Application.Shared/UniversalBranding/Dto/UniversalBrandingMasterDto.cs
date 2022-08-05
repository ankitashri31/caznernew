using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.UniversalBranding.Dto
{
    public class UniversalBrandingMasterDto : EntityDto<long>
    {
        public int TenantId { get; set; }
        public string Code { get; set; }
        public string UniversalBrandingTitle { get; set; }
        public bool IsActive { get; set; }
        public long AssignmentId { get; set; }
    }
    public class UniversalBrandingResultRequestDto : PagedResultRequestDto
    {
        public string UniversalBrandingTitle { get; set; }
        public string Sorting { get; set; }
    }
}
