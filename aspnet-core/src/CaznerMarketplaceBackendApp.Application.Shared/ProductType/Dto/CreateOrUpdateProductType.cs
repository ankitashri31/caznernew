using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductType.Dto
{
   public  class CreateOrUpdateProductType
    {
        public string ProductTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
