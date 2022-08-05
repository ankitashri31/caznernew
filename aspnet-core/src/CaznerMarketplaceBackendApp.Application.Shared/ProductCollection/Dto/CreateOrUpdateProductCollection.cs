using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.ProductCollection.Dto
{
    public class CreateOrUpdateProductCollection
    {
        public string ProductCollectionName { get; set; }
        public bool IsActive { get; set; }
    }
}
