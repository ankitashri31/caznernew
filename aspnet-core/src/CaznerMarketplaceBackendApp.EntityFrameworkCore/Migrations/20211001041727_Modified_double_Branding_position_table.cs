using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_double_Branding_position_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PostionMaxwidth",
                table: "ProductBrandingPosition",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "PostionMaxHeight",
                table: "ProductBrandingPosition",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PostionMaxwidth",
                table: "ProductBrandingPosition",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "PostionMaxHeight",
                table: "ProductBrandingPosition",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
