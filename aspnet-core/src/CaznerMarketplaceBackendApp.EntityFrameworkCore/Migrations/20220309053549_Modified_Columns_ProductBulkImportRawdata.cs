using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_Columns_ProductBulkImportRawdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "RegionCode",
            //    table: "Regions",
            //    maxLength: 3,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "ProductBulkImportRawData",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IsImportPerformed",
                table: "ProductBulkImportRawData",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "RegionCode",
            //    table: "Regions",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldMaxLength: 3,
            //    oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "ProductBulkImportRawData",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsImportPerformed",
                table: "ProductBulkImportRawData",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
