using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CaznerMarketplaceBackendApp.Product.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethods
{
    public class BrandingMethodDetails : FullAuditedEntity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string UniversalBrandingArray { get; set; }
        public string BrandingColorArray { get; set; }
        public string BrandingEquipmentArray { get; set; }
        public string BrandingTagArray { get; set; }
        public string ProductVendorArray { get; set; }
        public string SpecificFontStyleArray { get; set; }
        public string SpecificFontTypeFaceArray { get; set; }
        public virtual BrandingMethodMaster BrandingMethodMaster { get; set; }
        [ForeignKey("BrandingMethodMaster")]
        public long BrandingMethodId { get; set; }
        public bool IsActive { get; set; }
    }
}
