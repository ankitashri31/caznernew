using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_Image_Cols_ProductBrandingPosition_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ext",
                table: "ProductBrandingPosition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductBrandingPosition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ProductBrandingPosition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ProductBrandingPosition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "ProductBrandingPosition",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ext",
                table: "ProductBrandingPosition");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductBrandingPosition");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductBrandingPosition");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ProductBrandingPosition");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "ProductBrandingPosition");
        }
    }
}
