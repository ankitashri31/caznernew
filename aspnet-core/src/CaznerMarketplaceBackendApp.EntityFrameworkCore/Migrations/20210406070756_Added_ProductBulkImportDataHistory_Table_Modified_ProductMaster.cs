using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ProductBulkImportDataHistory_Table_Modified_ProductMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorsAvailable",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductBulkImportDataHistory",
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
                    TenantId = table.Column<int>(nullable: false),
                    SKU = table.Column<string>(nullable: true),
                    IsImportDone = table.Column<bool>(nullable: false),
                    ProductMasterId = table.Column<long>(nullable: false),
                    ProductDetailsId = table.Column<long>(nullable: true),
                    ProductInventoryId = table.Column<long>(nullable: true),
                    ProductDimensionId = table.Column<long>(nullable: true),
                    ProductImagesId = table.Column<long>(nullable: true),
                    ProductMediaImagesId = table.Column<long>(nullable: true),
                    ProductVolumeDiscountVariantId = table.Column<long>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBulkImportDataHistory", x => x.Id);
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBulkImportDataHistory");

            migrationBuilder.DropColumn(
                name: "ColorsAvailable",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "Features",
                table: "ProductMaster");
        }
    }
}
