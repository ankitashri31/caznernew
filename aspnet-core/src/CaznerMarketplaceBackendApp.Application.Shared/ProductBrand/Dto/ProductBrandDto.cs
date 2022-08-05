using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductBrand.Dto
{
    public class ProductBrandDto : EntityDto<long>
    {
        public string Code { get; set; }
        public string ProductBrandName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductBrandResultRequestDto : PagedResultRequestDto
    {
        public string ProductBrandName { get; set; }
        public string Sorting { get; set; }

    }
}
