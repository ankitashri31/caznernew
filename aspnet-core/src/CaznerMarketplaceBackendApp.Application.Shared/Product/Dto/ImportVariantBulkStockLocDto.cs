using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ImportVariantBulkStockLocDto
    {
        [Display(Name = "Variant Product SKU")]
        public string VariantProductSKU { get; set; }

        [Display(Name = "Parent Product SKU")]
        public string ParentProductSKU { get; set; }
        public List<VariantBulkStockLocationDto> WareHouseList { get; set; }
    }

    public class VariantBulkStockLocationDto
    {
        [Display(Name = "Variant Product SKU")]
        public string VariantProductSKU { get; set; }

        [Display(Name = "Stock keeping unit")]
        public string StockKeepingUnit { get; set; }

        [Display(Name = "Qty this location")]
        public string QuantityAtLocation { get; set; }

        [Display(Name = "Stock alert qty")]
        public string StockAlertQty { get; set; }

        [Display(Name = "Location A")]
        public string LocationA { get; set; }

        [Display(Name = "Location B")]
        public string LocationB { get; set; }

        [Display(Name = "Location C")]
        public string LocationC { get; set; }
    }
}
