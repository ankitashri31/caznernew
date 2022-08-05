using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_ProductBulkImportRawData_Table_Added_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "ProductBulkImportRawData",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductBulkImportRawData",
                table: "ProductBulkImportRawData",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductBulkImportRawData",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductBulkImportRawData");
        }
    }
}
