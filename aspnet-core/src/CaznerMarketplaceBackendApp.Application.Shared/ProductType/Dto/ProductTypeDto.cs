using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductType.Dto
{
    public class ProductTypeDto : EntityDto<long>
    {
        public string Code { get; set; }
        public string ProductTypeName { get; set; }
        public bool IsActive { get; set; }
    }
    public class ProductTypeResultRequestDto : PagedResultRequestDto
    {
        public string ProductTypeName { get; set; }
        public string Sorting { get; set; }

    }
}
