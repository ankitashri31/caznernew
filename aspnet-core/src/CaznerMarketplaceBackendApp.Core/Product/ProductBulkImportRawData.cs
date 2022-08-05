using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product
{
    
    public class ProductBulkImportRawData 
    {
        [Key]
        public long Id { get; set; }
        public string ProductSN { get; set; }
        public string MainorVariant { get; set; }
        public string ParentSKU { get; set; }
        public string VariantSKU { get; set; }
        public string Name { get; set; }
        // public string Groupcategory { get; set; }
        // public string Productcategory { get; set; }
        // GroupCategory3  SubCategory3   SubSubCategory3

        public string GroupCategory1 { get; set; }
        public string SubCategory1 { get; set; }
        public string SubSubCategory1 { get; set; }

        public string GroupCategory2 { get; set; }
        public string SubCategory2 { get; set; }
        public string SubSubCategory2 { get; set; }

        public string GroupCategory3 { get; set; }
        public string SubCategory3 { get; set; }
        public string SubSubCategory3 { get; set; }

        public string ProductCollections { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        public string Features { get; set; }
        public string Caznerproducttypes { get; set; }
        public string ProductSize { get; set; }
        public string Colour { get; set; }
        public string ProductMaterial { get; set; }
        public string Style { get; set; }
        public string ProductBrands { get; set; }
        public string ProductTags { get; set; }
        public string ProductVendors { get; set; }
        public string UnitPrice { get; set; }
        public string CostPerItem { get; set; }
        public string IsOnSale { get; set; }
        public string DiscountPercentage { get; set; }
        public string SalePrice { get; set; }
        public string Freigtnote { get; set; }
        public string DepositRequired { get; set; }
        public string IsChargeTax { get; set; }
        public string IsProductHasPriceVariant { get; set; }
        public string Barcode { get; set; }
        public string TotalNoAvailable { get; set; }
        public string AlertRestockAtThisNumber { get; set; }
        public string IsTrackQuantity { get; set; }
        public string IsStopSellingStockZero { get; set; }
        public string ProductHeight { get; set; }
        public string ProductWidth { get; set; }

        public string Productlength { get; set; }
        public string ProductDiameter { get; set; }
        public string ProductUOM { get; set; }
        public string ProductUnitWeight { get; set; }

        public string WeightUOM { get; set; }
        public string VolumeValue { get; set; }
        public string VolumeUOM { get; set; }
        public string ProductDimensionNotes { get; set; }
        public string IfsetOtherproducttitleanddimensoionsinthisset { get; set; }

        public string Productpackaging { get; set; }
        public string Counrtyoforigin { get; set; }

        public string IsPhysicalProduct { get; set; }

        public string CartonQuantity { get; set; }
        public string CartonLength { get; set; }
        public string CartonWidth { get; set; }
        public string CartonHeight { get; set; }
        public string CartonUOM { get; set; }

        public string CartonWeight { get; set; }
        public string CartonWeightUOM { get; set; }

        public string CartonPackaging { get; set; }

        public string CartonNote { get; set; }
        public string CartonCubicWeight { get; set; }

        public string PalletWeight { get; set; }
        public string CartonsPerPallet { get; set; }

        public string UnitsPerPallet { get; set; }
        public string PalletNote { get; set; }

        public string MainProductImage { get; set; }
        public string ProductImages { get; set; }
        public string LineMediaArtImages { get; set; }
        public string LifeStyleImages { get; set; }
        public string OtherMediaImages { get; set; }
        public string NumberofPieces { get; set; }
        public string MOQ { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }

        public string Q4 { get; set; }
        public string Q5 { get; set; }
        public string Q6 { get; set; }
        public string Q7 { get; set; }
        public string Q8 { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }
        public string P4 { get; set; }
        public string P5 { get; set; }
        public string P6 { get; set; }
        public string P7 { get; set; }
        public string P8 { get; set; }

        public string status { get; set; }
        public string IsIndentOrder { get; set; }

        public string Published { get; set; }
        public string TurnAroundTime { get; set; }
        public string RelatedProducts { get; set; }

        public string AlternativeProducts { get; set; }

        public string ColourFamily { get; set; }

        public string PMSColourCode { get; set; }
        public string VideoURL { get; set; }
        public string Image360Degrees { get; set; }
        public string ProductViews { get; set; }
        public string NextShipmentDate { get; set; }
        public string NextShipmentQuantity { get; set; }

        public string ExtraSetupFee { get; set; }
        public string BrandingMethodsseparatedbyacommarelevanttothisproduct { get; set; }
        public string BrandingMethodNote { get; set; }
        public string BrandingUOM { get; set; }

        public string Branding_Location_Title_1 { get; set; }
        public string Position_Max_Width_1 { get; set; }
        public string Position_Max_Height_1 { get; set; }
        public string Branding_Location_Image_1 { get; set; }
        //----------------------------------------------------------------------
        public string Branding_Location_Title_2 { get; set; }
        public string Position_Max_Width_2 { get; set; }
        public string Position_Max_Height_2 { get; set; }
        public string Branding_Location_Image_2 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_3 { get; set; }
        public string Position_Max_Width_3 { get; set; }
        public string Position_Max_Height_3 { get; set; }
        public string Branding_Location_Image_3 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_4 { get; set; }
        public string Position_Max_Width_4 { get; set; }
        public string Position_Max_Height_4 { get; set; }
        public string Branding_Location_Image_4 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_5 { get; set; }
        public string Position_Max_Width_5 { get; set; }
        public string Position_Max_Height_5 { get; set; }
        public string Branding_Location_Image_5 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_6 { get; set; }
        public string Position_Max_Width_6 { get; set; }
        public string Position_Max_Height_6 { get; set; }
        public string Branding_Location_Image_6 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_7 { get; set; }
        public string Position_Max_Width_7 { get; set; }
        public string Position_Max_Height_7 { get; set; }
        public string Branding_Location_Image_7 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_8 { get; set; }
        public string Position_Max_Width_8 { get; set; }
        public string Position_Max_Height_8 { get; set; }
        public string Branding_Location_Image_8 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_9 { get; set; }
        public string Position_Max_Width_9 { get; set; }
        public string Position_Max_Height_9 { get; set; }
        public string Branding_Location_Image_9 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_10 { get; set; }
        public string Position_Max_Width_10 { get; set; }
        public string Position_Max_Height_10 { get; set; }
        public string Branding_Location_Image_10 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_11 { get; set; }
        public string Position_Max_Width_11 { get; set; }
        public string Position_Max_Height_11 { get; set; }
        public string Branding_Location_Image_11 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_12 { get; set; }
        public string Position_Max_Width_12 { get; set; }
        public string Position_Max_Height_12 { get; set; }
        public string Branding_Location_Image_12 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_13 { get; set; }
        public string Position_Max_Width_13 { get; set; }
        public string Position_Max_Height_13 { get; set; }
        public string Branding_Location_Image_13 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_14 { get; set; }
        public string Position_Max_Width_14 { get; set; }
        public string Position_Max_Height_14 { get; set; }
        public string Branding_Location_Image_14 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_15 { get; set; }
        public string Position_Max_Width_15 { get; set; }
        public string Position_Max_Height_15 { get; set; }
        public string Branding_Location_Image_15 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_16 { get; set; }
        public string Position_Max_Width_16 { get; set; }
        public string Position_Max_Height_16 { get; set; }
        public string Branding_Location_Image_16 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_17 { get; set; }
        public string Position_Max_Width_17 { get; set; }
        public string Position_Max_Height_17 { get; set; }
        public string Branding_Location_Image_17 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_18 { get; set; }
        public string Position_Max_Width_18 { get; set; }
        public string Position_Max_Height_18 { get; set; }
        public string Branding_Location_Image_18 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_19 { get; set; }
        public string Position_Max_Width_19 { get; set; }
        public string Position_Max_Height_19 { get; set; }
        public string Branding_Location_Image_19 { get; set; }

        //----------------------------------------------------------------------
        public string Branding_Location_Title_20 { get; set; }
        public string Position_Max_Width_20 { get; set; }
        public string Position_Max_Height_20 { get; set; }
        public string Branding_Location_Image_20 { get; set; }

        //--------------------------------------------------------------------

        public string TenantId { get; set; }

        public string IsImportPerformed { get; set; }

    }
}
