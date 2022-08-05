using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
   public  class ProductAssignedTagsDto : EntityDto<long>
    {
        public long ProductTagId { get; set; }
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
