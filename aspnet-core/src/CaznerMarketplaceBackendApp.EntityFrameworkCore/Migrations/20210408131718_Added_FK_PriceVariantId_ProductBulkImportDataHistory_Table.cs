using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_FK_PriceVariantId_ProductBulkImportDataHistory_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVolumeDiscountVariant_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant");

            migrationBuilder.DropIndex(
                name: "IX_ProductVolumeDiscountVariant_ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant");

            migrationBuilder.DropColumn(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant");

            migrationBuilder.AddColumn<long>(
                name: "PriceVariantsId",
                table: "ProductBulkImportDataHistory",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBulkImportDataHistory_PriceVariantsId",
                table: "ProductBulkImportDataHistory",
                column: "PriceVariantsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrandingPriceVariants_ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants",
                column: "ProductBulkImportDataHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBrandingPriceVariants_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants",
                column: "ProductBulkImportDataHistoryId",
                principalTable: "ProductBulkImportDataHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBulkImportDataHistory_ProductBrandingPriceVariants_PriceVariantsId",
                table: "ProductBulkImportDataHistory",
                column: "PriceVariantsId",
                principalTable: "ProductBrandingPriceVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBrandingPriceVariants_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBulkImportDataHistory_ProductBrandingPriceVariants_PriceVariantsId",
                table: "ProductBulkImportDataHistory");

            migrationBuilder.DropIndex(
                name: "IX_ProductBulkImportDataHistory_PriceVariantsId",
                table: "ProductBulkImportDataHistory");

            migrationBuilder.DropIndex(
                name: "IX_ProductBrandingPriceVariants_ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants");

            migrationBuilder.DropColumn(
                name: "PriceVariantsId",
                table: "ProductBulkImportDataHistory");

            migrationBuilder.DropColumn(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductBrandingPriceVariants");

            migrationBuilder.AddColumn<long>(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVolumeDiscountVariant_ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant",
                column: "ProductBulkImportDataHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVolumeDiscountVariant_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant",
                column: "ProductBulkImportDataHistoryId",
                principalTable: "ProductBulkImportDataHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
