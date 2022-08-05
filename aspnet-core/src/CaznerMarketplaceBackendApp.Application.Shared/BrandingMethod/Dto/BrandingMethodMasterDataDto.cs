using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.EquipmentsBranding.Dto;
using CaznerMarketplaceBackendApp.Product.Dto;
using CaznerMarketplaceBackendApp.ProductSize.Dto;
using CaznerMarketplaceBackendApp.Tag.Dto;
using CaznerMarketplaceBackendApp.UniversalBranding.Dto;

namespace CaznerMarketplaceBackendApp.BrandingMethod.Dto
{
    public class BrandingMethodMasterDataDto
    {
        public List<BrandingAttributeData> BrandingAttribute { get; set; }
        public List<EquipmentBrandingMasterDto> EquipmenBrandingData { get; set; }
        public List<UniversalBrandingMasterDto> UniversalBrandingData { get; set; }

        public List<ProductTagDto> ProductTagData { get; set; }

        public List<ColorSelection> BrandingColorData { get; set; }
        public List<BrandingSpecificFontStyleMasterDto> BrandingSpecificFontStyleData { get; set; }

        public List<BrandingSpecificFontTypeFaceMasterDto> BrandingSpecificFontTypeFaceData { get; set; }
        public List<SupplierListDto> SupplierListData { get; set; }
        public List<ProductSizeDto> MeasurementSizeData { get; set; }

        public List<AppConsts.ColorSelectionType> ColorSelectionType { get; set; }
    }

    public class ColorSelection
    {
        public string SelectionTypeTitle { get; set; }
        public string Code { get; set; }
        public int Id { get; set; }
        public List<ProductColourMasterDto> BrandingColorData { get; set; }
    }
  
}
