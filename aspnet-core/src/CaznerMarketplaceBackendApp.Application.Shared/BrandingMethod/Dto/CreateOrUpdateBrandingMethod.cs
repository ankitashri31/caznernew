using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.BrandingMethod.Dto
{
    public class CreateOrUpdateBrandingMethod
    {
        public string MethodName { get; set; }
        public string ImageUrl { get; set; }
        public byte[] ImageData { get; set; }
        public bool IsActive { get; set; }
    }
}
