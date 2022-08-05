using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
   public  class ProductPriceDto : EntityDto<long>
    {
        public string Profit { get; set; }
        public string UnitPrice { get; set; }
        public string CostPerItem { get; set; }
        public string MarginIncreaseOnSalePrice { get; set; }
        public string SalePrice { get; set; }
        public bool OnSale { get; set; }
        public string UnitOfMeasure { get; set; }
        public string MinimumOrderQuantity { get; set; }
        public string DepositRequired { get; set; }
        public bool ChargeTaxOnThis { get; set; }
        public bool ProductHasPriceVariant { get; set; }
        public string DiscountPercentage { get; set; }
        public string DiscountPercentageDraft { get; set; }

        public decimal MinimumSalePrice { get; set; }
    }
}
