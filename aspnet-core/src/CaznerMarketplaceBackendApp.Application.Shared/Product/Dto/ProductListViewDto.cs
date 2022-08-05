using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductListViewDto : EntityDto<long>
    {
        public string ProductSKU { get; set; }
        public string ProductUniqueId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescripition { get; set; }
        public string ShortDescripition { get; set; }

        public bool IsProductHasMultipleOptions { get; set; }
        public bool IsPhysicalProduct { get; set; }
        public bool IsActive { get; set; }
        //price
        public decimal Profit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostPerItem { get; set; }
        public decimal MarginIncreaseOnSalePrice { get; set; }
        public decimal SalePrice { get; set; }
        public bool OnSale { get; set; }
        public int UnitOfMeasure { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public decimal DepositRequired { get; set; }
        public bool ChargeTaxOnThis { get; set; }
        public bool IsPublished { get; set; }
        public bool ProductHasPriceVariant { get; set; }
        public string DefaultImagePath { get; set; }
        public string Url { get; set; }
        public string DefaultImageName { get; set; }
        public double DiscountPercentage { get; set; }
        //package dimension
        public string ProductHeight { get; set; }
        public string ProductWidth { get; set; }
        public string ProductLength { get; set; }
        public string UnitWeight { get; set; }
        public string ProductPackaging { get; set; }
        public double UnitPerProduct { get; set; }
        public string CartonWeight { get; set; }
        public string CartonQuantity { get; set; }
        //Carton dimension
        public string CartonHeight { get; set; }
        public string CartonWidth { get; set; }
        public string CartonLength { get; set; }
        public double UnitPerCarton { get; set; }
        public string CartonPackaging { get; set; }
        public string CartonNote { get; set; }

        //Pallet dimension        
        public string PalletWeight { get; set; }
        public double CartonPerPallet { get; set; }
        public double UnitPerPallet { get; set; }
        public string PalletNote { get; set; }
        //Inventory table
        public int StockkeepingUnit { get; set; }
        public string Barcode { get; set; }
        public long TotalNumberAvailable { get; set; }
        public int AlertRestockNumber { get; set; }
        public bool IsTrackQuantity { get; set; }
        public bool IsStopSellingStockZero { get; set; }

        public string ProductType { get; set; }

        //public ProductandPackageDimensionDto ProductandPackageDimension { get; set; }

        //public ProductandPackageDimensionDto ProductandPackageDimension { get; set; }

    }

    public class CollectionproductView
    {
        public string CollectionName { get; set; }
        public long Id { get; set; }
        public List<ProductView> ProductListView { get; set; }
    }

    public class ProductView
    {
        public long Id { get; set; }
        public string ProductSKU { get; set; }
        public string ProductUniqueId { get; set; }
        public string ProductDefaultImage { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescripition { get; set; }
        public string ShortDescripition { get; set; }
        public bool IsActive { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal MinimumSalePrice { get; set; }
        public decimal SalePrice { get; set; }
        public string DiscountPercentage { get; set; }
        public decimal CostPerItem { get; set; }
        public string DefaultImageName { get; set; }
        public string DefaultImagePath { get; set; }

        public bool IsShowPriceWithoutAccount { get; set; }

    }
}
