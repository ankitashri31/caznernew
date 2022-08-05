using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductVariantDataDto : EntityDto<long>
    {
        
        public string Variant { get; set; }
        public string VariantDetails { get; set; }
        public double VariantQuantity { get; set; }

        //    variant pricing
        public string VariantPrice { get; set; }
        public string ComparePrice { get; set; }
        public string CostPerItem { get; set; }
        public string NextShipment { get; set; }
        public decimal IncomingQuantity { get; set; }
        public string Margin { get; set; }
        public string ProfitCurrencySymbol { get; set; }
        public string Profit { get; set; }
        public bool IsChargeTaxVariant { get; set; }

        //    variant Inventory
        public string VariantSKU { get; set; }
        public string VariantBarCode { get; set; }
        public bool IsTrackQuantity { get; set; }  
        public bool IsActive { get; set; }
        public string Shape { get; set; }
        public string ImageName { get; set; }
        public string ImageURL { get; set; }
        public bool IsMultiColorVariant { get; set; }
        public ProductVariantWarehouseDto VariantWarehouse { get; set; }
        //variant warehouse
        //public List<ProductVariantWarehouseDto> ProductVariantWarehouse { get; set; }
    }

    public class ProductVariantValueDto : EntityDto<long>
    {
        public long ProductId { get; set; }
        public string Variant { get; set; }
        public string VariantSKU { get; set; }
        public string VariantBarCode { get; set; }
        public double VariantQuantity { get; set; }
        public string VariantPrice { get; set; }

        public string VariantDetails { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Margin { get; set; }
     
        public string Material { get; set; }
        public bool IsChargeTaxVariant { get; set; }

        public string ProfitCurrencySymbol { get; set; }
  
        public bool IsTrackQuantity { get; set; }
        public bool IsActive { get; set; }
        public string Shape { get; set; }
        public string Style { get; set; }
        public bool IsVariantHasMixAndMatch { get; set; }
        public long VariantWareHouseId { get; set; }
        public string WareHouse { get; set; }
        public string LocationA { get; set; }
        public string LocationB { get; set; }
        public string LocationC { get; set; }
        public double QuantityStockUnit { get; set; }
        public string ImageURL { get; set; }
        public string NextShipment { get; set; }
        public decimal IncomingQuantity { get; set; }
        public bool IsMultiColorVariant { get; set; }
        public VariantPriceModel VariantPriceModel { get; set; }

        public List<ProductVariantWarehouseDto> ProductVariantWarehouse { get; set; }
        public List<ProductVariantImagesDto> ProductVariantImages { get; set; }

        public List<ProductVariantQuantityPricesDto> ProductQuantityPrices { get; set; }

    }

    public class VariantPriceModel
    {
        public decimal? SalePrice { get; set; }
        public bool OnSale { get; set; }
        public double DiscountPercentage { get; set; }
        public double DiscountPercentageDraft { get; set; }
        public decimal? SaleUnitPrice { get; set; }
        public string CostPerItem { get; set; }
        public decimal Profit { get; set; }
        public string ComparePrice { get; set; }
    }

    }
