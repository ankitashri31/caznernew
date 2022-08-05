using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductVariantImagesDto : EntityDto<long>
    {
        public long ProductVariantId { get; set; }
        public long ProductId { get; set; }
        public byte[] ImageFileData { get; set; }
        public string ImageFileName { get; set; }
        public string FileName { get; set; }
        public string ImageURL { get; set; }
        public string Url { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
