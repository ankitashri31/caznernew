using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethod.Dto
{
   public class ProductColourMasterDto : EntityDto<long>
    {
        public string ProductColourName { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
        public long AssignmentId { get; set; }
    }
}
