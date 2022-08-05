using Abp.Application.Services.Dto;
using CaznerMarketplaceBackendApp.Product.Dto;
using CaznerMarketplaceBackendApp.Tag.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethod.Dto
{
    public class BrandingMethodDto : EntityDto<long>
    {
        public string MethodName { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Notes { get; set; }
        public long UniqueNumber { get; set; }
        public bool IsActive { get; set; }
        // public List<BrandingAttributeData> BrandingAttribute { get; set; }

        public long AssignmentId { get; set; }
        public string SelectedColor { get; set; }
        public string SelectedColorDraft { get; set; }
        public string BrandingUnitOfMeasure { get; set; }
        public int ColorSelectionType { get; set; }
        public int NumberOfStitches { get; set; }
    }

    public class BrandingMethodResultRequestDto : PagedResultRequestDto
    {
        public string MethodName { get; set; }
        public string ImageUrl { get; set; }
        public byte[] ImageData { get; set; }
        public string Sorting { get; set; }

    }


    public class BrandingAttributeData
    {
        public long Id { get; set; }
        public string AttributeTitle { get; set; }
        public string AttributeValue { get; set; }
        public string Value { get; set; }
        public long AssignmentId { get; set; }
    }

    public class BrandingMethodPriceModel
    {
        public long Id { get; set; }
        public string MethodName { get; set; }
        public decimal UnitPrice { get; set; }
        public string SelectedColor { get; set; }
        public string MaxSelectedColor { get; set; }
        public bool IsColorSelected { get; set; }
        public List<ProductBrandingPriceVariantsDto> PriceModel { get; set; }
    }
}
