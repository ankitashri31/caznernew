using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductMaterial.Dto
{
    public class CreateOrUpdateProductMaterial
    {
        public string ProductMaterialName { get; set; }
        public bool IsActive { get; set; }
    }
}
