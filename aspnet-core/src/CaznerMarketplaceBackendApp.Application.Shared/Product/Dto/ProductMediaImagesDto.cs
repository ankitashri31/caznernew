using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductMediaImagesDto : EntityDto<long>
    {
        public long ProductId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }       
        public long ProductMediaImageTypeId { get; set; }
    }
}
