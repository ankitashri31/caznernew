using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
   public  class ProductVariantListCustomDto
    {

        public long Id { get; set; }
        public string VariantSKU { get; set; }
        public string Variant { get; set; }
        public float VariantPrice { get; set; }

        public ProductImageType ImageObj { get; set; }
       
        public string Color { get; set; }

       
    }
}
