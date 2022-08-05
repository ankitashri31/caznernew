using System;
using System.Collections.Generic;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ProductCustomDataDto
    {
        public long Id { get; set; }

        public long TurnAroundTimeId { get; set; }
        public string ProductSKU { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescripition { get; set; }
        public string ShortDescripition { get; set; }
        public string ProductNotes { get; set; }
        public bool IsActive { get; set; }
        public bool IsProductHasMultipleOptions { get; set; }
        public bool IsPhysicalProduct { get; set; }
        public bool IsIndentOrder { get; set; }
        public string Features { get; set; }
        public bool IsPublished { get; set; }
        public double Profit { get; set; }
        public double UnitPrice { get; set; }
        public double CostPerItem { get; set; }
        public double MarginIncreaseOnSalePrice { get; set; }
        public double SalePrice { get; set; }
        public bool OnSale { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public double DepositRequired { get; set; }
        public bool ChargeTaxOnThis { get; set; }
        public bool ProductHasPriceVariant { get; set; }
        public double DiscountPercentage { get; set; }
        public double DiscountPercentageDraft { get; set; }
        public int UnitOfMeasure { get; set; }
        public string BrandingUnitOfMeasure { get; set; }
        public int BrandingUnitOfMeasureId { get; set; }
        public string TurnAroundTime { get; set; }
        public int InventoryId { get; set; }
        public string PalletNote { get; set; }
        public string PalletWeight { get; set; }
        public double CartonPerPallet { get; set; }
        public double UnitPerPallet { get; set; }
        public string CartonHeight { get; set; }
        public string CartonWeight { get; set; }
        public string CartonWidth { get; set; }
        public string CartonLength { get; set; }
        public string CartonPackaging { get; set; }
        public string CartonNote { get; set; }
        public string CartonCubicWeightKG { get; set; }
        public long CartonUnitOfMeasureId { get; set; }
        public long CartonWeightMeasureId { get; set; }
        public string ProductHeight { get; set; }
        public string ProductWidth { get; set; }
        public string ProductLength { get; set; }
        public string UnitWeight { get; set; }
        public string ProductPackaging { get; set; }
        public double UnitPerProduct { get; set; }
        public string ProductDiameter { get; set; }
        public string ProductDimensionNotes { get; set; }
        public long ProductUnitMeasureId { get; set; }
        public long ProductWeightMeasureId { get; set; }
        public string ProductUnitMeasureTitle { get; set; }
        public string ProductWeightMeasureTitle { get; set; }
        public string CartonUnitOfMeasureTitle { get; set; }
        public string CartonWeightMeasureTitle { get; set; }
        public string BrandStringArray { get; set; }
        public string TypeStringArray { get; set; }
        public string MaterialStringArray { get; set; }
        public string CollectionStringArray { get; set; }
        public string TagStringArray { get; set; }
        public string VendorStringArray { get; set; }
        public string Barcode { get; set; }
        public long TotalNumberAvailable { get; set; }
        public int AlertRestockNumber { get; set; }
        public bool IsHasSubProducts { get; set; }
        public int NumberOfSubProducts { get; set; }
        public decimal ArtWorkUnitPrice { get; set; }
        public decimal ArtWorkHandlingCharges { get; set; }
        public bool IsArtworkEnabled { get; set; }
        public bool IsProductHasCompartmentBuilder { get; set; }
        public bool IsProductCompartmentType { get; set; }
        public string CompartmentTitle { get; set; }
        public string CompartmentDescription { get; set; }
        public string CompartmentBaseImageUrl { get; set; }
        public string CompartmentBaseImageName { get; set; }
        public string CompartmentBaseImageType { get; set; }
        public string CompartmentBaseImageSize { get; set; }
        public string CompartmentBaseImageExt { get; set; }

        public string CompartmentBaseFileName { get; set; }
        public string CompartmentBaseImagePath { get; set; }

        public string CompartmentBuilderTitle { get; set; }
        public bool IsShowPriceWithoutAccount { get; set; }

        public string NumberOfPieces { get; set; }

    }
}
