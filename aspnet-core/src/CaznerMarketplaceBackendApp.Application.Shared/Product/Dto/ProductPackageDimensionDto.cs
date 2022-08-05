using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
   public  class ProductPackageDimensionDto : EntityDto<long>
    {
        public long ProductId { get; set; }
        public long Height { get; set; }
        public long Width { get; set; }
        public long Length { get; set; }
        public long UnitPerCarton { get; set; }
        public string ProductPackaging { get; set; }
        public string Note { get; set; }
    }
}
