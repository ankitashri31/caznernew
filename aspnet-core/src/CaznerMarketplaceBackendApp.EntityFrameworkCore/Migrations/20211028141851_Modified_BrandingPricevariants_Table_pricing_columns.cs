using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_BrandingPricevariants_Table_pricing_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Price1",
                table: "ProductBrandingPriceVariants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Price2",
                table: "ProductBrandingPriceVariants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Price3",
                table: "ProductBrandingPriceVariants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price1",
                table: "ProductBrandingPriceVariants");

            migrationBuilder.DropColumn(
                name: "Price2",
                table: "ProductBrandingPriceVariants");

            migrationBuilder.DropColumn(
                name: "Price3",
                table: "ProductBrandingPriceVariants");
        }
    }
}
