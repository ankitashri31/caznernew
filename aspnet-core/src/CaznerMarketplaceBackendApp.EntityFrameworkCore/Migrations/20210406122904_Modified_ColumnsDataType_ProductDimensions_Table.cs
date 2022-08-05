using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_ColumnsDataType_ProductDimensions_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductImages",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Width",
                table: "ProductDimension",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "UnitWeight",
                table: "ProductDimension",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Length",
                table: "ProductDimension",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Height",
                table: "ProductDimension",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "CartonWeight",
                table: "ProductDimension",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "CartonQuantity",
                table: "ProductDimension",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductBulkImportDataHistoryId",
                table: "ProductImages",
                column: "ProductBulkImportDataHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductImages",
                column: "ProductBulkImportDataHistoryId",
                principalTable: "ProductBulkImportDataHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductBulkImportDataHistoryId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductImages");

            migrationBuilder.AlterColumn<long>(
                name: "Width",
                table: "ProductDimension",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UnitWeight",
                table: "ProductDimension",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Length",
                table: "ProductDimension",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Height",
                table: "ProductDimension",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CartonWeight",
                table: "ProductDimension",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CartonQuantity",
                table: "ProductDimension",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
