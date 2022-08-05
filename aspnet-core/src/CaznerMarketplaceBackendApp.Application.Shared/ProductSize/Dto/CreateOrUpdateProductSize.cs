using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductSize.Dto
{
    public class CreateOrUpdateProductSize
    {
        public string ProductSizeName { get; set; }
        public bool IsActive { get; set; }
    }
}
