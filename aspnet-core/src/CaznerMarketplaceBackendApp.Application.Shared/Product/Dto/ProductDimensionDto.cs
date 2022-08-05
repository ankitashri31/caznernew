using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductDimensionDto: EntityDto<long>
    {
        public long ProductId { get; set; }
        public long Height { get; set; }
        public long Width { get; set; }
        public long Length { get; set; }

        public long UnitWeight { get; set; }
        public long CartonWeight { get; set; }
        public long CartonQuantity { get; set; }
    }
}
