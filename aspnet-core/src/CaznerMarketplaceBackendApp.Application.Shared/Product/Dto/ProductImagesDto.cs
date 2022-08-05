using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductImagesDto : EntityDto<long>
    {
        public string ImageName { get; set; }
        public long ProductId { get; set; }
        public string ImagePath { get; set; }
        public string ImageExtension { get; set; }
        public bool IsActive { get; set; }
        public string Msg  { get; set; }

        public int MediaType { get; set; }
    }
}
