using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductAssignedVendorsDto : EntityDto<long>
    {
        public long VendorUserId { get; set; }      
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
