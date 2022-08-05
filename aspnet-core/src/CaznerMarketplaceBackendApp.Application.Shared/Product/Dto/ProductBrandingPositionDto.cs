using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductBrandingPositionDto : EntityDto<long>
    {
        public long ProductId { get; set; }
      //  public string LayerNumber { get; set; }
        public string LayerTitle { get; set; }
        public double? PositionMaxwidth { get; set; }
        public double? PositionMaxHeight { get; set; }

        public string ImageName { get; set; }
        public string Extension { get; set; }
       // public byte[] ImageFileData { get; set; }
        public string ImageFileURL { get; set; }
        public string BrandingLocationNote { get; set; }

        public bool IsDeletedFromUi { get; set; }

        public long? UnitOfMeasureId { get; set; }
        public string UnitOfMeasure { get; set; }
        public string FileName { get; set; }
        public int MediaType { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ProductImageType ImageObj { get; set; }
    }
}
