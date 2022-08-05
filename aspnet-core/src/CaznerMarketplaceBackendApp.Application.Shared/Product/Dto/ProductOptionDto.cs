using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductOptionDto : EntityDto<long>
    {
        public string Code { get; set; }
        public string OptionName { get; set; }
        public bool IsActive { get; set; }
    }
}
