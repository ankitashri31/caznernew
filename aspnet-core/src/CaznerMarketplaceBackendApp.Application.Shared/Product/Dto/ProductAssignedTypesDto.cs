using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductAssignedTypesDto : EntityDto<long>
    {
        public long ProductTypeId { get; set; }     
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
