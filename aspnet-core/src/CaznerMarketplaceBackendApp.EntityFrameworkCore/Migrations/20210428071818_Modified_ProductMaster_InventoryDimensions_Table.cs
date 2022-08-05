using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_ProductMaster_InventoryDimensions_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantdataImages_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantOptionValues_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantWarehouse_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantWarehouse_ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantOptionValues_ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantdataImages_ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages");

            migrationBuilder.DropColumn(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse");

            migrationBuilder.DropColumn(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues");

            migrationBuilder.DropColumn(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages");

            migrationBuilder.AddColumn<double>(
                name: "DiscountPercentage",
                table: "ProductMaster",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CartonQuantity",
                table: "ProductDimensionsInventory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CartonWeight",
                table: "ProductDimensionsInventory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "CartonQuantity",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropColumn(
                name: "CartonWeight",
                table: "ProductDimensionsInventory");

            migrationBuilder.AddColumn<long>(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantWarehouse_ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse",
                column: "ProductVariantBulkImportHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantOptionValues_ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues",
                column: "ProductVariantBulkImportHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantdataImages_ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages",
                column: "ProductVariantBulkImportHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantdataImages_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages",
                column: "ProductVariantBulkImportHistoryId",
                principalTable: "ProductVariantBulkImportHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantOptionValues_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues",
                column: "ProductVariantBulkImportHistoryId",
                principalTable: "ProductVariantBulkImportHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantWarehouse_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse",
                column: "ProductVariantBulkImportHistoryId",
                principalTable: "ProductVariantBulkImportHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
