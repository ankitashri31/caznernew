using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductDesignHubDto : EntityDto<long>
    {
        public long ProductId { get; set; }
        public string BrandingPositionTitle { get; set; }
        public string ImageUrl { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
