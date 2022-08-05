using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_ProductImagesTable_BrandingMethodMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "ProductMediaImages",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsIndentOrder",
                table: "ProductMaster",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "ProductImages",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UniqueNumber",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "ProductMediaImages");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "UniqueNumber",
                table: "BrandingMethodMaster");

            migrationBuilder.AlterColumn<string>(
                name: "IsIndentOrder",
                table: "ProductMaster",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
