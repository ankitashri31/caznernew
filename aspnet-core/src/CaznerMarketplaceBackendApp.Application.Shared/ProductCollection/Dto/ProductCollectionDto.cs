using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductCollection.Dto
{
    public class ProductCollectionDto : EntityDto<long>
    {
        public string ProductCollectionName { get; set; }
        public bool IsActive { get; set; }

    }

    public class ProductCollectionResultRequestDto : PagedResultRequestDto
    {
        public string ProductCollectionName { get; set; }
        public string Sorting { get; set; }

    }
}
