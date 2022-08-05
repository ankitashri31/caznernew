using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ImportBulkProductStockLocationDto
    {
        [Display(Name = "Parent Product SKU")]
        public string ProductParentSKU { get; set; }

        public List<WareHouseLocationDataDto> StockLocationList { get; set; }

    }

    public class WareHouseLocationDataDto
    {
        [Display(Name = "Parent Product SKU")]
        public string ProductParentSKU { get; set; }

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
