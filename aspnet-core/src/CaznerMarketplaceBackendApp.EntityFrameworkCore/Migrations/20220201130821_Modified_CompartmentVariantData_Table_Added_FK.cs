using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_CompartmentVariantData_Table_Added_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductVarientId",
                table: "CompartmentVariantData",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompartmentVariantData_ProductVarientId",
                table: "CompartmentVariantData",
                column: "ProductVarientId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompartmentVariantData_ProductVariantsData_ProductVarientId",
                table: "CompartmentVariantData",
                column: "ProductVarientId",
                principalTable: "ProductVariantsData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompartmentVariantData_ProductVariantsData_ProductVarientId",
                table: "CompartmentVariantData");

            migrationBuilder.DropIndex(
                name: "IX_CompartmentVariantData_ProductVarientId",
                table: "CompartmentVariantData");

            migrationBuilder.DropColumn(
                name: "ProductVarientId",
                table: "CompartmentVariantData");
        }
    }
}
