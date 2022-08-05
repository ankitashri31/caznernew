using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductAssignedBrandsDto : EntityDto<long>
    {
        public long[] ProductBrandId { get; set; }       
      
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
