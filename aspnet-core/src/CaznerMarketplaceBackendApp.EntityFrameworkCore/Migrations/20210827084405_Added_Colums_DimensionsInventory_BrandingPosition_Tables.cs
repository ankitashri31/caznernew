using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_Colums_DimensionsInventory_BrandingPosition_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "IncomingQuantity",
                table: "ProductVariantsData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "NextShipment",
                table: "ProductVariantsData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrandingLocationNote",
                table: "ProductBrandingPosition",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomingQuantity",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "NextShipment",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "BrandingLocationNote",
                table: "ProductBrandingPosition");
        }
    }
}
