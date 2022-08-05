using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ImportColorVariantsDto
    {
        public List<ColorVariantsModel> VariantsData { get; set; }

        [Display(Name = "Parent Product SKU")]
        public string ParentProductId { get; set; }
    }

    public class ColorVariantsModel
    {
        public string Color { get; set; }
        public string Size  { get; set; }
        public string Material { get; set; }
        public string Style { get; set; }

        public string SKU { get; set; }

        public string BarCode { get; set; }
        public string QuantityStockUnit { get; set; }
        public string Price { get; set; }
        
        public string IsTrackQuantity { get; set; }
        public string IsChargeTaxVariant { get; set; }

        public string ComparePrice { get; set; }
        public string OnSale { get; set; }

        public string CostPerItem { get; set; }
        public string Margin { get; set; }
        public string ProfitCurrencySymbol { get; set; }

        public string Profit { get; set; }
        public string Shape { get; set; }

        [Display(Name = "Parent Product SKU")]
        public string ParentProductSKU { get; set; }
        public string Images { get; set; }
        public string NextShipment { get; set; }
        public string IncomingQuantity { get; set; }
        public string DiscountPercentage { get; set; }
        public string DiscountPercentageDraft { get; set; }
        public string SaleUnitPrice { get; set; }
        public string SalePrice { get; set; }
        public List<QuantityPriceVariantModel> PriceVariantModel { get; set; }
    }

}
