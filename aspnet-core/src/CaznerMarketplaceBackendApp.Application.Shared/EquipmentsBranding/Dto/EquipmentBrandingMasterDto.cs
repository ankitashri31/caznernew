using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.EquipmentsBranding.Dto
{
    public class EquipmentBrandingMasterDto : EntityDto<long>
    {
        public int TenantId { get; set; }
        public string EquipmentTitle { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public long AssignmentId { get; set; }
    }
    public class EquipmentBrandingResultRequestDto : PagedResultRequestDto
    {
        public string EquipmentTitle { get; set; }
        public string Sorting { get; set; }
    }
}
