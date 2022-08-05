using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class GetVariantListDto
    {
        public long Id { get; set; }
        public string VariantSKU { get; set; }
        public string Variant { get; set; }
        public float VariantPrice { get; set; }
        public string ImageURL { get; set; }
        public string Ext { get; set; }

        public string Size { get; set; }

        public string Type { get; set; }
        public string Name { get; set; }
        public string ImageFileName { get; set; }

        public string ImageFileData { get; set; }
        public string Color { get; set; }

    }
}
