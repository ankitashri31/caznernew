using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductBrandingPriceVariantsDto : EntityDto<long>
    {
        public long Id { get; set; }
        public long? BrandingMethodId { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Price1 { get; set; }
        public string Price2 { get; set; }
        public string Price3 { get; set; }
        public string BrandingUnitPrice { get; set; }
        public long ProductMasterId { get; set; }
        public bool IsActive { get; set; }
        public string MethodName { get; set; }
    }
}
