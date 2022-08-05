using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_ObjectBinary_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReadDone",
                table: "AppBinaryObjects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRows",
                table: "AppBinaryObjects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReadDone",
                table: "AppBinaryObjects");

            migrationBuilder.DropColumn(
                name: "NumberOfRows",
                table: "AppBinaryObjects");
        }
    }
}
