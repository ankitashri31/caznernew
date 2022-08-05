using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductSize.Dto
{
    public class ProductSizeDto : EntityDto<long>
    {
        public string ProductSizeName { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public long AssignmentId { get; set; }
    }

    public class ProductSizeResultRequestDto : PagedResultRequestDto
    {

        public string ProductSizeName { get; set; }
        public string Sorting { get; set; }

    }
}
