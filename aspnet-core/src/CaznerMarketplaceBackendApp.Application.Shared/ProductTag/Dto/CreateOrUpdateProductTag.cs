using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductTag.Dto
{
    public class CreateOrUpdateProductTag
    {
        public string ProductTagName { get; set; }
        public bool IsActive { get; set; }
    }
}
