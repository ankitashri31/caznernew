using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethod.Dto
{
    public class BrandingMethodDetailsDto : EntityDto<long>
    {
        public int TenantId { get; set; }
        public BrandingCombo[] UniversalBrandingArray { get; set; }
        public BrandingCombo[] BrandingColorArray { get; set; }
        public BrandingCombo[] BrandingEquipmentArray { get; set; }
        public BrandingCombo[] BrandingTagArray { get; set; }
        public BrandingCombo[] BrandingVendorArray { get; set; }
        public BrandingCombo[] SpecificFontStyleArray { get; set; }
        public BrandingCombo[] SpecificFontTypeFaceArray { get; set; }
        public long BrandingMethodId { get; set; }
        public bool IsActive { get; set; }
    }
}
