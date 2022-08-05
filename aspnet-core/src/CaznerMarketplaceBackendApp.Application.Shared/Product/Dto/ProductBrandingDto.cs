using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductBrandingDto : EntityDto<long>
    {
        public long ProductId { get; set; }
        public long BrandingCounts { get; set; }
        public long ImageUrl { get; set; }
        public long MaxWidth { get; set; }
        public long MaxHeight { get; set; }
    }
}
