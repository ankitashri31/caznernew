using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_Columns_VariantImagesData_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ext",
                table: "ProductVariantdataImages",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultImage",
                table: "ProductVariantdataImages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductVariantdataImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ProductVariantdataImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ProductVariantdataImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ext",
                table: "ProductVariantdataImages");

            migrationBuilder.DropColumn(
                name: "IsDefaultImage",
                table: "ProductVariantdataImages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductVariantdataImages");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductVariantdataImages");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ProductVariantdataImages");
        }
    }
}
