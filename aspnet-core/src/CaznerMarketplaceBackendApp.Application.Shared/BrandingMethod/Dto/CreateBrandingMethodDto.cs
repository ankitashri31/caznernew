using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.Product.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethod.Dto
{
    public class CreateBrandingMethodDto : EntityDto<long>
    {
        public string MethodName { get; set; }
        public string ImageUrl { get; set; }
     
        public ImageObj ImageObj { get; set; }
        [Required]
        public string MethodSKU { get; set; }
        [Required]
        public string MethodDescripition { get; set; }
        public string UnitPrice { get; set; }
        public string CostPerItem { get; set; }
        public string Profit { get; set; }
        public bool IsChargeTaxOnThis  { get; set; }
        public bool IsMethodHasQuantityPriceVariant { get; set; }
        public bool IsMethodHasAdditionalPriceVariant { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Notes { get; set; }
        public bool IsSpecificFontStyle { get; set; }
        public bool IsSpecificFontTypeFace { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
        public long UniqueNumber { get; set; }
        [Required]
        public BrandingMethodDetailsDto BrandingMethodDetails { get; set; }
        public List<ProductBrandingPriceVariantsDto> ProductBrandingPriceVariants { get; set; }
        // public List<BrandingMethodAdditionalPriceDto> BrandingMethodAdditionalPrice { get; set; }

        public List<BrandingAdditionalModel> BrandingMethodAdditionalPrice { get; set; }
        public long? MeasurementId { get; set; }

        public int ColorSelectionType { get; set; }
        public string ColorSelectionTypeTitle { get; set; }
        public string NumberOfStiches { get; set; }

    }

    public class ImageObj
    {
        public string ImageName { get; set; }
        public string Url { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
    public class BrandingMethodModel 
    {
        public string MethodName { get; set; }
        public string ImageUrl { get; set; }
        public ImageObj ImageObj { get; set; }
        [Required]
        public string MethodSKU { get; set; }
        [Required]
        public string MethodDescripition { get; set; }
        public string UnitPrice { get; set; }
        public string CostPerItem { get; set; }
        public string Profit { get; set; }
        public bool IsChargeTaxOnThis { get; set; }
        public bool IsMethodHasQuantityPriceVariant { get; set; }
        public bool IsMethodHasAdditionalPriceVariant { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Notes { get; set; }
        public bool IsSpecificFontStyle { get; set; }
        public bool IsSpecificFontTypeFace { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
        public long UniqueNumber { get; set; }

        [Required]
        public BrandingDetailComboDto BrandingMethodDetails { get; set; }
        public List<ProductBrandingPriceVariantsDto> ProductBrandingPriceVariants { get; set; }
        // public List<BrandingMethodAdditionalPriceDto> BrandingMethodAdditionalPrice { get; set; }
        public List<BrandingAdditionalModel> BrandingMethodAdditionalPrice { get; set; }
        public long? MeasurementId { get; set; }
        public string MeasurementTitle { get; set; }
        public int NumberOfStiches { get; set; }
    }

    public class BrandingDetailComboDto
    {
        public long Id { get; set; }
        public BrandingCombo[] UniversalBrandingArray { get; set; }
        public BrandingCombo[] BrandingColorArray { get; set; }
        public BrandingCombo[] BrandingEquipmentArray { get; set; }
        public BrandingCombo[] BrandingTagArray { get; set; }
        public BrandingCombo[] BrandingVendorArray { get; set; }
        public BrandingCombo[] SpecificFontStyleArray { get; set; }
        public BrandingCombo[] SpecificFontTypeFaceArray { get; set; }
        public ColorSelection ColorSelectionType { get; set; }
        public long BrandingMethodId { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfStiches { get; set; }
    }
    public class BrandingCombo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long AssignmentId { get; set; }
    }
}
