using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_ProductBulkImportRawData_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsImportPerformed",
                table: "ProductBulkImportRawData",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ProductBulkImportRawData",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsImportPerformed",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductBulkImportRawData");
        }
    }
}
