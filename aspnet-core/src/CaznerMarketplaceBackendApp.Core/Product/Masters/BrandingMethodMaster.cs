using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Masters
{
    public class BrandingMethodMaster : FullAuditedEntity<long>, IMustHaveTenant
    {
        public string MethodName { get; set; }
        public string ImageUrl { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }
        public string Url { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string MethodSKU { get; set; }
        public string MethodDescripition { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPerItem { get; set; }
        public decimal Profit { get; set; }
        public bool IsChargeTaxOnThis { get; set; }
        public bool IsMethodHasQuantityPriceVariant { get; set; }
        public bool IsMethodHasAdditionalPriceVariant { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public virtual ProductSizeMaster ProductSizeMaster { get; set; }
        [ForeignKey("ProductSizeMaster")]
        public long? MeasurementId { get; set; }
        public string Notes { get; set; }
        public bool IsSpecificFontStyle { get; set; }
        public bool IsSpecificFontTypeFace { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
        public long UniqueNumber { get; set; }
        public int ColorSelectionType { get; set; }
        public int NumberOfStiches { get; set; }

    }
}
