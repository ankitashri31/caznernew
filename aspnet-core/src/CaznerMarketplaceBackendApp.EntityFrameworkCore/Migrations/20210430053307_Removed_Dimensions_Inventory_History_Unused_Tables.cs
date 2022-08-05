using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Removed_Dimensions_Inventory_History_Unused_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBrandingPriceVariants_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductBulkImportDataHistory");

            migrationBuilder.DropTable(
                name: "ProductInventory");

            migrationBuilder.DropTable(
                name: "ProductPackageDimension");

            migrationBuilder.DropTable(
                name: "ProductVariantBulkImportHistory");

            migrationBuilder.DropTable(
                name: "ProductDimension");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductBulkImportDataHistoryId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductBrandingPriceVariants_ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants");

            migrationBuilder.DropColumn(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductImages",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductDimension",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartonQuantity = table.Column<int>(type: "int", nullable: false),
                    CartonWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Width = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDimension", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDimension_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductInventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlertRestockNumber = table.Column<int>(type: "int", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsStopSellingStockZero = table.Column<bool>(type: "bit", nullable: false),
                    IsTrackQuantity = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    StockkeepingUnit = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TotalNumberAvailable = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInventory_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPackageDimension",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Height = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductPackaging = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    UnitPerCarton = table.Column<long>(type: "bigint", nullable: false),
                    Width = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPackageDimension", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPackageDimension_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantBulkImportHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BulkUploadVariationsId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    VariantDataId = table.Column<long>(type: "bigint", nullable: true),
                    VariantOptionId = table.Column<long>(type: "bigint", nullable: true),
                    VariantWarehouseId = table.Column<long>(type: "bigint", nullable: true),
                    VariantdataImageId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantBulkImportHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantBulkImportHistory_ProductBulkUploadVariations_BulkUploadVariationsId",
                        column: x => x.BulkUploadVariationsId,
                        principalTable: "ProductBulkUploadVariations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariantBulkImportHistory_ProductVariantsData_VariantDataId",
                        column: x => x.VariantDataId,
                        principalTable: "ProductVariantsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariantBulkImportHistory_ProductVariantOptionValues_VariantOptionId",
                        column: x => x.VariantOptionId,
                        principalTable: "ProductVariantOptionValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariantBulkImportHistory_ProductVariantWarehouse_VariantWarehouseId",
                        column: x => x.VariantWarehouseId,
                        principalTable: "ProductVariantWarehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductVariantBulkImportHistory_ProductVariantdataImages_VariantdataImageId",
                        column: x => x.VariantdataImageId,
                        principalTable: "ProductVariantdataImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductBulkImportDataHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsImportDone = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    PriceVariantsId = table.Column<long>(type: "bigint", nullable: true),
                    ProductDetailsId = table.Column<long>(type: "bigint", nullable: true),
                    ProductDimensionId = table.Column<long>(type: "bigint", nullable: true),
                    ProductImagesId = table.Column<long>(type: "bigint", nullable: true),
                    ProductInventoryId = table.Column<long>(type: "bigint", nullable: true),
                    ProductMasterId = table.Column<long>(type: "bigint", nullable: false),
                    ProductMediaImagesId = table.Column<long>(type: "bigint", nullable: true),
                    ProductVolumeDiscountVariantId = table.Column<long>(type: "bigint", nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBulkImportDataHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBulkImportDataHistory_ProductBrandingPriceVariants_PriceVariantsId",
                        column: x => x.PriceVariantsId,
                        principalTable: "ProductBrandingPriceVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductBulkImportDataHistory_ProductDetails_ProductDetailsId",
                        column: x => x.ProductDetailsId,
                        principalTable: "ProductDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductBulkImportDataHistory_ProductDimension_ProductDimensionId",
                        column: x => x.ProductDimensionId,
                        principalTable: "ProductDimension",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductBulkImportDataHistory_ProductImages_ProductImagesId",
                        column: x => x.ProductImagesId,
                        principalTable: "ProductImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductBulkImportDataHistory_ProductDimensionsInventory_ProductInventoryId",
                        column: x => x.ProductInventoryId,
                        principalTable: "ProductDimensionsInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductBulkImportDataHistory_ProductMaster_ProductMasterId",
                        column: x => x.ProductMasterId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductBulkImportDataHistory_ProductMediaImages_ProductMediaImagesId",
                        column: x => x.ProductMediaImagesId,
                        principalTable: "ProductMediaImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductBulkImportDataHistory_ProductVolumeDiscountVariant_ProductVolumeDiscountVariantId",
                        column: x => x.ProductVolumeDiscountVariantId,
                        principalTable: "ProductVolumeDiscountVariant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductBulkImportDataHistoryId",
                table: "ProductImages",
                column: "ProductBulkImportDataHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrandingPriceVariants_ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants",
                column: "ProductBulkImportDataHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBulkImportDataHistory_PriceVariantsId",
                table: "ProductBulkImportDataHistory",
                column: "PriceVariantsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBulkImportDataHistory_ProductDetailsId",
                table: "ProductBulkImportDataHistory",
                column: "ProductDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBulkImportDataHistory_ProductDimensionId",
                table: "ProductBulkImportDataHistory",
                column: "ProductDimensionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBulkImportDataHistory_ProductImagesId",
                table: "ProductBulkImportDataHistory",
                column: "ProductImagesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBulkImportDataHistory_ProductInventoryId",
                table: "ProductBulkImportDataHistory",
                column: "ProductInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBulkImportDataHistory_ProductMasterId",
                table: "ProductBulkImportDataHistory",
                column: "ProductMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBulkImportDataHistory_ProductMediaImagesId",
                table: "ProductBulkImportDataHistory",
                column: "ProductMediaImagesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBulkImportDataHistory_ProductVolumeDiscountVariantId",
                table: "ProductBulkImportDataHistory",
                column: "ProductVolumeDiscountVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDimension_ProductId",
                table: "ProductDimension",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventory_ProductId",
                table: "ProductInventory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPackageDimension_ProductId",
                table: "ProductPackageDimension",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantBulkImportHistory_BulkUploadVariationsId",
                table: "ProductVariantBulkImportHistory",
                column: "BulkUploadVariationsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantBulkImportHistory_VariantDataId",
                table: "ProductVariantBulkImportHistory",
                column: "VariantDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantBulkImportHistory_VariantOptionId",
                table: "ProductVariantBulkImportHistory",
                column: "VariantOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantBulkImportHistory_VariantWarehouseId",
                table: "ProductVariantBulkImportHistory",
                column: "VariantWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantBulkImportHistory_VariantdataImageId",
                table: "ProductVariantBulkImportHistory",
                column: "VariantdataImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBrandingPriceVariants_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants",
                column: "ProductBulkImportDataHistoryId",
                principalTable: "ProductBulkImportDataHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductImages",
                column: "ProductBulkImportDataHistoryId",
                principalTable: "ProductBulkImportDataHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
