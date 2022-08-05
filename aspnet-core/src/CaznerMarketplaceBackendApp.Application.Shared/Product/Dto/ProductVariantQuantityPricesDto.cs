using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductVariantQuantityPricesDto : EntityDto<long>
    {       
        public long ProductVariantId { get; set; }
        public int QuantityFrom { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeletedOnUi { get; set; }
    }
}
