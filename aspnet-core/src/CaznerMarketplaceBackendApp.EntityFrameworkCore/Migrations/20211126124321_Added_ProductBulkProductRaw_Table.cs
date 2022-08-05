using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ProductBulkProductRaw_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                  name: "ProductBulkImportRawData",
                  columns: table => new
         {
            Id = table.Column<long>(nullable: false)
           .Annotation("SqlServer:Identity", "1, 1"),
            CreationTime = table.Column<DateTime>(nullable: false),
            CreatorUserId = table.Column<long>(nullable: true),
            LastModificationTime = table.Column<DateTime>(nullable: true),
            LastModifierUserId = table.Column<long>(nullable: true),
            IsDeleted = table.Column<bool>(nullable: false),
            DeleterUserId = table.Column<long>(nullable: true),
            DeletionTime = table.Column<DateTime>(nullable: true),
            ProductSN = table.Column<string>(nullable: true),
            MainorVariant = table.Column<string>(nullable: true),
            ParentSKU = table.Column<string>(nullable: true),
            VariantSKU = table.Column<string>(nullable: true),
            Name = table.Column<string>(nullable: true),
            Groupcategory = table.Column<string>(nullable: true),
            Productcategory = table.Column<string>(nullable: true),
            ProductCollections = table.Column<string>(nullable: true),
            ShortDescription = table.Column<string>(nullable: true),
            Description = table.Column<string>(nullable: true),
            Features = table.Column<string>(nullable: true),
            Caznerproducttypes = table.Column<string>(nullable: true),
            ProductSize = table.Column<string>(nullable: true),
            Colour = table.Column<string>(nullable: true),
            ProductMaterial = table.Column<string>(nullable: true),
            Style = table.Column<string>(nullable: true),
            ProductBrands = table.Column<string>(nullable: true),
            ProductTags = table.Column<string>(nullable: true),
            ProductVendors = table.Column<string>(nullable: true),
            UnitPrice = table.Column<string>(nullable: true),
            CostPerItem = table.Column<string>(nullable: true),
            IsOnSale = table.Column<string>(nullable: true),
            DiscountPercentage = table.Column<string>(nullable: true),
            SalePrice = table.Column<string>(nullable: true),
            Freigtnote = table.Column<string>(nullable: true),
            DepositRequired = table.Column<string>(nullable: true),
            IsChargeTax = table.Column<string>(nullable: true),
            IsProductHasPriceVariant = table.Column<string>(nullable: true),
            Barcode = table.Column<string>(nullable: true),
            TotalNoAvailable = table.Column<string>(nullable: true),
            AlertRestockAtThisNumber = table.Column<string>(nullable: true),
            IsTrackQuantity = table.Column<string>(nullable: true),
            IsStopSellingStockZero = table.Column<string>(nullable: true),
            ProductHeight = table.Column<string>(nullable: true),
            ProductWidth = table.Column<string>(nullable: true),
            Productlength = table.Column<string>(nullable: true),
            ProductDiameter = table.Column<string>(nullable: true),
            ProductUOM = table.Column<string>(nullable: true),
            ProductUnitWeight = table.Column<string>(nullable: true),
            WeightUOM = table.Column<string>(nullable: true),
            VolumeValue = table.Column<string>(nullable: true),
            VolumeUOM = table.Column<string>(nullable: true),
            ProductDimensionNotes = table.Column<string>(nullable: true),
            IfsetOtherproducttitleanddimensoionsinthisset = table.Column<string>(nullable: true),
            Productpackaging = table.Column<string>(nullable: true),
            Counrtyoforigin = table.Column<string>(nullable: true),
            IsPhysicalProduct = table.Column<string>(nullable: true),
            CartonQuantity = table.Column<string>(nullable: true),
            CartonLength = table.Column<string>(nullable: true),
            CartonWidth = table.Column<string>(nullable: true),
            CartonHeight = table.Column<string>(nullable: true),
            CartonUOM = table.Column<string>(nullable: true),
            CartonWeight = table.Column<string>(nullable: true),
            CartonWeightUOM = table.Column<string>(nullable: true),
            CartonPackaging = table.Column<string>(nullable: true),
            CartonNote = table.Column<string>(nullable: true),
            CartonCubicWeight = table.Column<string>(nullable: true),
            PalletWeight = table.Column<string>(nullable: true),
            CartonsPerPallet = table.Column<string>(nullable: true),
            UnitsPerPallet = table.Column<string>(nullable: true),
            PalletNote = table.Column<string>(nullable: true),
            MainProductImage = table.Column<string>(nullable: true),
            ProductImages = table.Column<string>(nullable: true),
            LineMediaArtImages = table.Column<string>(nullable: true),
            LifeStyleImages = table.Column<string>(nullable: true),
            OtherMediaImages = table.Column<string>(nullable: true),
            NumberofPieces = table.Column<string>(nullable: true),
            MOQ = table.Column<string>(nullable: true),
            Q1 = table.Column<string>(nullable: true),
            Q2 = table.Column<string>(nullable: true),
            Q3 = table.Column<string>(nullable: true),
            Q4 = table.Column<string>(nullable: true),
            Q5 = table.Column<string>(nullable: true),
            Q6 = table.Column<string>(nullable: true),
            Q7 = table.Column<string>(nullable: true),
            Q8 = table.Column<string>(nullable: true),
            P1 = table.Column<string>(nullable: true),
            P2 = table.Column<string>(nullable: true),
            P3 = table.Column<string>(nullable: true),
            P4 = table.Column<string>(nullable: true),
            P5 = table.Column<string>(nullable: true),
            P6 = table.Column<string>(nullable: true),
            P7 = table.Column<string>(nullable: true),
            P8 = table.Column<string>(nullable: true),
            status = table.Column<string>(nullable: true),
            IsIndentOrder = table.Column<string>(nullable: true),
            Published = table.Column<string>(nullable: true),
            TurnAroundTime = table.Column<string>(nullable: true),
            RelatedProducts = table.Column<string>(nullable: true),
            AlternativeProducts = table.Column<string>(nullable: true),
            ColourFamily = table.Column<string>(nullable: true),
            PMSColourCode = table.Column<string>(nullable: true),
            VideoURL = table.Column<string>(nullable: true),
            Image360Degrees = table.Column<string>(nullable: true),
            ProductViews = table.Column<string>(nullable: true),
            NextShipmentDate = table.Column<string>(nullable: true),
            NextShipmentQuantity = table.Column<string>(nullable: true),
            ExtraSetupFee = table.Column<string>(nullable: true),
            BrandingMethodsseparatedbyacommarelevanttothisproduct = table.Column<string>(nullable: true),
            BrandingMethodNote = table.Column<string>(nullable: true),
            BrandingUOM = table.Column<string>(nullable: true),
            Branding_Location_Title_1 = table.Column<string>(nullable: true),
            Position_Max_Width_1 = table.Column<string>(nullable: true),
            Position_Max_Height_1 = table.Column<string>(nullable: true),
            Branding_Location_Image_1 = table.Column<string>(nullable: true),
            Branding_Location_Title_2 = table.Column<string>(nullable: true),
            Position_Max_Width_2 = table.Column<string>(nullable: true),
            Position_Max_Height_2 = table.Column<string>(nullable: true),
            Branding_Location_Image_2 = table.Column<string>(nullable: true),
            Branding_Location_Title_3 = table.Column<string>(nullable: true),
            Position_Max_Width_3 = table.Column<string>(nullable: true),
            Position_Max_Height_3 = table.Column<string>(nullable: true),
            Branding_Location_Image_3 = table.Column<string>(nullable: true),
            Branding_Location_Title_4 = table.Column<string>(nullable: true),
            Position_Max_Width_4 = table.Column<string>(nullable: true),
            Position_Max_Height_4 = table.Column<string>(nullable: true),
            Branding_Location_Image_4 = table.Column<string>(nullable: true),
            Branding_Location_Title_5 = table.Column<string>(nullable: true),
            Position_Max_Width_5 = table.Column<string>(nullable: true),
            Position_Max_Height_5 = table.Column<string>(nullable: true),
            Branding_Location_Image_5 = table.Column<string>(nullable: true),
            Branding_Location_Title_6 = table.Column<string>(nullable: true),
            Position_Max_Width_6 = table.Column<string>(nullable: true),
            Position_Max_Height_6 = table.Column<string>(nullable: true),
            Branding_Location_Image_6 = table.Column<string>(nullable: true),
            Branding_Location_Title_7 = table.Column<string>(nullable: true),
            Position_Max_Width_7 = table.Column<string>(nullable: true),
            Position_Max_Height_7 = table.Column<string>(nullable: true),
            Branding_Location_Image_7 = table.Column<string>(nullable: true),
            Branding_Location_Title_8 = table.Column<string>(nullable: true),
            Position_Max_Width_8 = table.Column<string>(nullable: true),
            Position_Max_Height_8 = table.Column<string>(nullable: true),
            Branding_Location_Image_8 = table.Column<string>(nullable: true),
            Branding_Location_Title_9 = table.Column<string>(nullable: true),
            Position_Max_Width_9 = table.Column<string>(nullable: true),
            Position_Max_Height_9 = table.Column<string>(nullable: true),
            Branding_Location_Image_9 = table.Column<string>(nullable: true),
            Branding_Location_Title_10 = table.Column<string>(nullable: true),
            Position_Max_Width_10 = table.Column<string>(nullable: true),
            Position_Max_Height_10 = table.Column<string>(nullable: true),
            Branding_Location_Image_10 = table.Column<string>(nullable: true),
            Branding_Location_Title_11 = table.Column<string>(nullable: true),
            Position_Max_Width_11 = table.Column<string>(nullable: true),
            Position_Max_Height_11 = table.Column<string>(nullable: true),
            Branding_Location_Image_11 = table.Column<string>(nullable: true),
            Branding_Location_Title_12 = table.Column<string>(nullable: true),
            Position_Max_Width_12 = table.Column<string>(nullable: true),
            Position_Max_Height_12 = table.Column<string>(nullable: true),
            Branding_Location_Image_12 = table.Column<string>(nullable: true),
            Branding_Location_Title_13 = table.Column<string>(nullable: true),
            Position_Max_Width_13 = table.Column<string>(nullable: true),
            Position_Max_Height_13 = table.Column<string>(nullable: true),
            Branding_Location_Image_13 = table.Column<string>(nullable: true),
            Branding_Location_Title_14 = table.Column<string>(nullable: true),
            Position_Max_Width_14 = table.Column<string>(nullable: true),
            Position_Max_Height_14 = table.Column<string>(nullable: true),
            Branding_Location_Image_14 = table.Column<string>(nullable: true),
            Branding_Location_Title_15 = table.Column<string>(nullable: true),
            Position_Max_Width_15 = table.Column<string>(nullable: true),
            Position_Max_Height_15 = table.Column<string>(nullable: true),
            Branding_Location_Image_15 = table.Column<string>(nullable: true),
            Branding_Location_Title_16 = table.Column<string>(nullable: true),
            Position_Max_Width_16 = table.Column<string>(nullable: true),
            Position_Max_Height_16 = table.Column<string>(nullable: true),
            Branding_Location_Image_16 = table.Column<string>(nullable: true),
            Branding_Location_Title_17 = table.Column<string>(nullable: true),
            Position_Max_Width_17 = table.Column<string>(nullable: true),
            Position_Max_Height_17 = table.Column<string>(nullable: true),
            Branding_Location_Image_17 = table.Column<string>(nullable: true),
            Branding_Location_Title_18 = table.Column<string>(nullable: true),
            Position_Max_Width_18 = table.Column<string>(nullable: true),
            Position_Max_Height_18 = table.Column<string>(nullable: true),
            Branding_Location_Image_18 = table.Column<string>(nullable: true),
            Branding_Location_Title_19 = table.Column<string>(nullable: true),
            Position_Max_Width_19 = table.Column<string>(nullable: true),
            Position_Max_Height_19 = table.Column<string>(nullable: true),
            Branding_Location_Image_19 = table.Column<string>(nullable: true),
            Branding_Location_Title_20 = table.Column<string>(nullable: true),
            Position_Max_Width_20 = table.Column<string>(nullable: true),
            Position_Max_Height_20 = table.Column<string>(nullable: true),
            Branding_Location_Image_20 = table.Column<string>(nullable: true),
            TenantId = table.Column<int>(nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_ProductBulkImportRawData", x => x.Id);
        });

      //------------------------------------------------------------------//



            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductBulkImportRawData",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductBulkImportRawData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
             name: "ProductBulkImportRawData");

            //---------------------------------

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "ProductBulkImportRawData",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ProductBulkImportRawData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "ProductBulkImportRawData",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "ProductBulkImportRawData",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ProductBulkImportRawData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductBulkImportRawData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ProductBulkImportRawData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "ProductBulkImportRawData",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ProductBulkImportRawData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductBulkImportRawData",
                table: "ProductBulkImportRawData",
                column: "Id");
        }
    }
}
