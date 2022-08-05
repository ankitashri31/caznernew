using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductBrand.Dto
{
   public class CreateOrUpdateProductBrand
    {
        public string ProductBrandName { get; set; }
        public bool IsActive { get; set; }
    }
}
