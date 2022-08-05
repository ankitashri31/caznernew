using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ImportBulkBrandingLocationDto
    {
        [Display(Name = "Parent Product SKU")]
        public string ProductParentSKU { get; set; }
        public List<BrandingLocationDto> BrandingLocationList { get; set; }
    }

    public class BrandingLocationDto
    {
        public string ProductParentSKU { get; set; }
        public string LayerTitle { get; set; }
        public string PositionMaxwidth { get; set; }
        public string PositionMaxHeight { get; set; }
        public string ImageName { get; set; }
        public string ImageFileURL { get; set; }

    }
}
