using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductMaterial.Dto
{
    public class ProductMaterialDto : EntityDto<long>
    {
        public string Code { get; set; }
        public string ProductMaterialName { get; set; }
        public bool IsActive { get; set; }
    }
    public class ProductMaterialResultRequestDto : PagedResultRequestDto
    {
        public string ProductMaterialName { get; set; }
      
        public string Sorting { get; set; }

    }
}
