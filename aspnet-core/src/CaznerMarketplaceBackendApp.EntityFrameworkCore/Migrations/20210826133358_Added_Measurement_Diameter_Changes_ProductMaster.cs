using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_Measurement_Diameter_Changes_ProductMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ProductSizeMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CartonCubicWeightKG",
                table: "ProductDimensionsInventory",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CartonUnitOfMeasureId",
                table: "ProductDimensionsInventory",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CartonWeightMeasureId",
                table: "ProductDimensionsInventory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PalletDimension",
                table: "ProductDimensionsInventory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductDiameter",
                table: "ProductDimensionsInventory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductDimensionNotes",
                table: "ProductDimensionsInventory",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductUnitMeasureId",
                table: "ProductDimensionsInventory",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductWeightMeasureId",
                table: "ProductDimensionsInventory",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductDimensionsInventory_CartonUnitOfMeasureId",
                table: "ProductDimensionsInventory",
                column: "CartonUnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDimensionsInventory_CartonWeightMeasureId",
                table: "ProductDimensionsInventory",
                column: "CartonWeightMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDimensionsInventory_ProductUnitMeasureId",
                table: "ProductDimensionsInventory",
                column: "ProductUnitMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDimensionsInventory_ProductWeightMeasureId",
                table: "ProductDimensionsInventory",
                column: "ProductWeightMeasureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDimensionsInventory_ProductSizeMaster_CartonUnitOfMeasureId",
                table: "ProductDimensionsInventory",
                column: "CartonUnitOfMeasureId",
                principalTable: "ProductSizeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDimensionsInventory_ProductSizeMaster_CartonWeightMeasureId",
                table: "ProductDimensionsInventory",
                column: "CartonWeightMeasureId",
                principalTable: "ProductSizeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDimensionsInventory_ProductSizeMaster_ProductUnitMeasureId",
                table: "ProductDimensionsInventory",
                column: "ProductUnitMeasureId",
                principalTable: "ProductSizeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDimensionsInventory_ProductSizeMaster_ProductWeightMeasureId",
                table: "ProductDimensionsInventory",
                column: "ProductWeightMeasureId",
                principalTable: "ProductSizeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDimensionsInventory_ProductSizeMaster_CartonUnitOfMeasureId",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDimensionsInventory_ProductSizeMaster_CartonWeightMeasureId",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDimensionsInventory_ProductSizeMaster_ProductUnitMeasureId",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDimensionsInventory_ProductSizeMaster_ProductWeightMeasureId",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropIndex(
                name: "IX_ProductDimensionsInventory_CartonUnitOfMeasureId",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropIndex(
                name: "IX_ProductDimensionsInventory_CartonWeightMeasureId",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropIndex(
                name: "IX_ProductDimensionsInventory_ProductUnitMeasureId",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropIndex(
                name: "IX_ProductDimensionsInventory_ProductWeightMeasureId",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductSizeMaster");

            migrationBuilder.DropColumn(
                name: "CartonCubicWeightKG",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropColumn(
                name: "CartonUnitOfMeasureId",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropColumn(
                name: "CartonWeightMeasureId",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropColumn(
                name: "PalletDimension",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropColumn(
                name: "ProductDiameter",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropColumn(
                name: "ProductDimensionNotes",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropColumn(
                name: "ProductUnitMeasureId",
                table: "ProductDimensionsInventory");

            migrationBuilder.DropColumn(
                name: "ProductWeightMeasureId",
                table: "ProductDimensionsInventory");
        }
    }
}
