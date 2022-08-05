using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CaznerMarketplaceBackendApp.Product.Dto
{
    public class ImportBulkProductDto
    {
        [Display(Name = "Main or Variant")]
        public string MainorVariant { get; set; }

        [Display(Name = "Parent SKU")]
        public string ParentSKU { get; set; }

        [Display(Name = "Variant SKU")]
        public string VariantSKU { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Category image")]
        public string CategoryImage { get; set; }

        [Display(Name = "Groupcategory")]
        public string Groupcategory { get; set; }

        [Display(Name = "Productcategory")]
        public string Productcategory { get; set; }

        [Display(Name = "Short description")]
        public string ShortDescription { get; set; }

        [Display(Name = "Long  description")]
        public string Description { get; set; }

        [Display(Name = "Features")]
        public string Features { get; set; }

        [Display(Name = "Caznerproducttypes")]
        public string Caznerproducttypes { get; set; }

        [Display(Name = "ProductSize ")]
        public string ProductSize { get; set; }

        [Display(Name = "Colour")]
        public string Colour { get; set; }

        [Display(Name = "Product brands")]
        public string ProductBrands { get; set; }

        [Display(Name = "Product Material")]
        public string ProductMaterial { get; set; }

        [Display(Name = "Product collections")]
        public string ProductCollections { get; set; }

        [Display(Name = "Style")]
        public string Style { get; set; }

        [Display(Name = "Product tags")]
        public string ProductTags { get; set; }

        [Display(Name = "Product vendors")]
        public string ProductVendors { get; set; }

        [Display(Name = "Colors Available")]
        public string ColorsAvailable { get; set; }

        [Display(Name = "Unit Price")]
        public string UnitPrice { get; set; }

        [Display(Name = "Cost per item")]
        public string CostPerItem { get; set; }

        [Display(Name = "Profit")]
        public string Profit { get; set; }

        [Display(Name = "Is on sale")]
        public string IsOnSale { get; set; }

        [Display(Name = "Discount percentage")]
        public string DiscountPercentage { get; set; }

        [Display(Name = "Sale price")]
        public string SalePrice { get; set; }

        [Display(Name = "Freigthnote")]
        public string Freigthnote { get; set; }

        [Display(Name = "Minimum order quantity")]
        public string MinimumOrderQuantity { get; set; }

        [Display(Name = "Deposit required")]
        public string DepositRequired { get; set; }

        [Display(Name = "Is charge tax")]
        public string IsChargeTax { get; set; }

        [Display(Name = "Is product has price variant")]
        public string IsProductHasPriceVariant { get; set; }

        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [Display(Name = "Total no. available")]
        public string TotalNoAvailable { get; set; }

        [Display(Name = "Alert restock at this number")]
        public string AlertRestockAtThisNumber { get; set; }

        [Display(Name = "Is track quantity")]
        public string IsTrackQuantity { get; set; }

        [Display(Name = "Is stop selling stock zero")]
        public string IsStopSellingStockZero { get; set; }

        [Display(Name = "Product height")]
        public string ProductHeight { get; set; }

        [Display(Name = "Product width")]
        public string ProductWidth { get; set; }

        [Display(Name = "Product length")]
        public string Productlength { get; set; }

        [Display(Name = "Product unit weight")]
        public string ProductUnitWeight { get; set; }

        [Display(Name = "Product Diameter")]
        public string ProductDiameter { get; set; }

        [Display(Name = "Product UOM")]
        public string ProductUOM { get; set; }

        [Display(Name = "Product packaging")]
        public string ProductPackaging { get; set; }

        [Display(Name = "WeightUOM")]
        public string WeightUOM { get; set; }

        [Display(Name = "Volume Value")]
        public string VolumeValue { get; set; }

        [Display(Name = "Volume UOM")]
        public string VolumeUOM { get; set; }

        [Display(Name = "Product Dimension Notes ")]
        public string ProductDimensionNotes { get; set; }

        [Display(Name = "If set Other product title and dimensoions in this set")]
        public string IfsetOtherproducttitleanddimensoionsinthisset { get; set; }

        [Display(Name = "Units per product")]
        public string UnitsPerProduct { get; set; }

        [Display(Name = "Product packaging")]
        public string Productpackaging { get; set; }

        [Display(Name = "Counrty of origin")]
        public string Counrtyoforigin { get; set; }

        [Display(Name = "Carton UOM")]
        public string CartonUOM { get; set; }


        [Display(Name = "Carton weight UOM")]
        public string CartonWeightUOM { get; set; }


        [Display(Name = "Is physical product")]
        public string IsPhysicalProduct { get; set; }

        [Display(Name = "Carton qty")]
        public string CartonQuantity { get; set; }

        [Display(Name = "Carton length")]
        public string CartonLength { get; set; }

        [Display(Name = "Carton width")]
        public string CartonWidth { get; set; }

        [Display(Name = "Carton height")]
        public string CartonHeight { get; set; }

        [Display(Name = "Carton weight")]
        public string CartonWeight { get; set; }

        [Display(Name = "Carton packaging")]
        public string CartonPackaging { get; set; }

        [Display(Name = "Carton Cubic Weight")]
        public string CartonCubicWeight { get; set; }

        [Display(Name = "Number of Pieces ")]
        public string NumberofPieces { get; set; }

        [Display(Name = "Carton note")]
        public string CartonNote { get; set; }

        [Display(Name = "Pallet weight")]
        public string PalletWeight { get; set; }

        [Display(Name = "Cartons per pallet")]
        public string CartonsPerPallet { get; set; }

        [Display(Name = "Units per pallet")]
        public string UnitsPerPallet { get; set; }

        [Display(Name = "Pallet note")]
        public string PalletNote { get; set; }

        [Display(Name = "Main product image")]
        public string MainProductImage { get; set; }

        #region commented images
        //[Display(Name = "Image 1")]
        //public string Image1 { get; set; }

        //[Display(Name = "Image 2")]
        //public string Image2 { get; set; }
        //[Display(Name = "Image 3")]
        //public string Image3 { get; set; }
        //[Display(Name = "Image 4")]
        //public string Image4 { get; set; }
        //[Display(Name = "Image 5")]
        //public string Image5 { get; set; }

        //[Display(Name = "Image 6")]
        //public string Image6 { get; set; }

        //[Display(Name = "Image 7")]
        //public string Image7 { get; set; }

        //[Display(Name = "Image 8")]
        //public string Image8 { get; set; }

        //[Display(Name = "Image 9")]
        //public string Image9 { get; set; }

        //[Display(Name = "Image 10")]
        //public string Image10 { get; set; }

        //[Display(Name = "Image 11")]
        //public string  Image11 { get; set; }

        //[Display(Name = "Image 12")]
        //public string  Image12 { get; set; }

        //[Display(Name = "Image 13")]
        //public string Image13 { get; set; }

        #endregion

        [Display(Name = "Product Images")]
        public string ProductImages { get; set; }

        [Display(Name = "Line media art images")]
        public string LineMediaArtImages { get; set; }

        [Display(Name = "Life style images")]
        public string LifeStyleImages { get; set; }

        [Display(Name = "Other media images")]
        public string OtherMediaImages { get; set; }


        [Display(Name = "Unit Of Measure")]
        public string UnitOfMeasure { get; set; }

        [Display(Name = "MOQ")]
        public string MOQ { get; set; }

        [Display(Name = "Is Indent Order")]
        public string IsIndentOrder { get; set; }

        [Display(Name = "status")]
        public string status { get; set; }

        [Display(Name = "Published")]
        public string Published { get; set; }

        [Display(Name = "Turn Around Time")]
        public string TurnAroundTime { get; set; }

        [Display(Name = "Related Products")]
        public string RelatedProducts { get; set; }

        [Display(Name = "Alternative Products")]
        public string AlternativeProducts { get; set; }

        [Display(Name = "Colour Family")]
        public string ColourFamily { get; set; }

        [Display(Name = "PMS Colour Code")]
        public string PMSColourCode { get; set; }

        [Display(Name = "Video URL")]
        public string VideoURL { get; set; }

        [Display(Name = "Image 360 Degrees")]
        public string Image360Degrees  { get; set; }


        [Display(Name = "Product Views")]
        public string ProductViews { get; set; }

        [Display(Name = "Next Shipment Date")]
        public string NextShipmentDate { get; set; }

        [Display(Name = "Next Shipment Quantity")]
        public string NextShipmentQuantity { get; set; }

        [Display(Name = "Extra Set up fee")]
        public string ExtraSetupFee { get; set; }

        [Display(Name = "Branding Methods separated by a comma relevant to this product")]
        public string BrandingMethodsseparatedbyacommarelevanttothisproduct { get; set; }

        [Display(Name = "Branding Method Note")]
        public string BrandingMethodNote { get; set; }

        [Display(Name = "Branding UOM")]
        public string BrandingUOM { get; set; }

        public string GroupCategory1 { get; set; }
        public string SubCategory1 { get; set; }
        public string SubSubCategory1 { get; set; }

        public string GroupCategory2 { get; set; }
        public string SubCategory2  { get; set; }
        public string SubSubCategory2 { get; set; }

        public string GroupCategory3 { get; set; }
        public string SubCategory3 { get; set; }
        public string SubSubCategory3 { get; set; }

        public List<QuantityPriceVariantModel> QuantityPriceVariantModel { get; set; }

        public List<BrandingLocationModel> BrandingLocationModel { get; set; }

        #region Variants
        //[Display(Name = "A1")]
        //public string A1 { get; set; }

        //[Display(Name = "A2")]
        //public string A2 { get; set; }
        //[Display(Name = "A3")]
        //public string A3 { get; set; }
        //[Display(Name = "A4")]
        //public string A4 { get; set; }
        //[Display(Name = "A5")]
        //public string A5 { get; set; }
        //[Display(Name = "A6")]
        //public string A6 { get; set; }
        //[Display(Name = "A7")]
        //public string A7 { get; set; }
        //[Display(Name = "A8")]
        //public string A8 { get; set; }

        //[Display(Name = "A9")]
        //public string A9 { get; set; }

        //[Display(Name = "A10")]
        //public string A10 { get; set; }


        //[Display(Name = "B1")]
        //public string B1 { get; set; }

        //[Display(Name = "B2")]
        //public string B2 { get; set; }

        //[Display(Name = "B3")]
        //public string B3 { get; set; }

        //[Display(Name = "B4")]
        //public string B4 { get; set; }

        //[Display(Name = "B5")]
        //public string B5 { get; set; }

        //[Display(Name = "B6")]
        //public string B6 { get; set; }

        //[Display(Name = "B7")]
        //public string B7 { get; set; }

        //[Display(Name = "B8")]
        //public string B8 { get; set; }

        //[Display(Name = "B9")]
        //public string B9 { get; set; }

        //[Display(Name = "B10")]
        //public string B10 { get; set; }

        //[Display(Name = "C1")]
        //public string C1 { get; set; }

        //[Display(Name = "C2")]
        //public string C2 { get; set; }

        //[Display(Name = "C3")]
        //public string C3 { get; set; }

        //[Display(Name = "C4")]
        //public string C4 { get; set; }

        //[Display(Name = "C5")]
        //public string C5 { get; set; }

        //[Display(Name = "C6")]
        //public string C6 { get; set; }

        //[Display(Name = "C7")]
        //public string C7 { get; set; }

        //[Display(Name = "C8")]
        //public string C8 { get; set; }


        //[Display(Name = "C9")]
        //public string C9 { get; set; }

        //[Display(Name = "C10")]
        //public string C10  { get; set; }

        #endregion

        public List<MethodVariantData> MethodVariants { get; set; }
    }
     

    public class ImportProductModel
    {
        public List<ImportBulkProductDto> BulkProducts { get; set; }
        public List<ImportColorVariantsDto> ColorVariants { get; set; }

    }

    public class QuantityPriceVariantModel
    {
        public string Quantity { get; set; }
        public string Price { get; set; }
    }

    public class BrandingLocationModel
    {
        public string Branding_Location_Title_ { get; set; }
        public string Position_Max_Width_ { get; set; }
        public string Position_Max_Height_ { get; set; }
        public string Branding_Location_Image_ { get; set; }
    }

    public class MethodVariantData
    {
        public string MethodName { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
    }

    public class RawPriceModel
    {
        public string Q { get; set; }
        public string P { get; set; }
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
    }

    public class RawBrandingModel
    {
        public string Branding_Location_Title_ { get; set; }
        public string Position_Max_Width_ { get; set; }
        public string Position_Max_Height_ { get; set; }
        public string Branding_Location_Image_ { get; set; }
        //---------------------------------------------------------------
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
    }
}
