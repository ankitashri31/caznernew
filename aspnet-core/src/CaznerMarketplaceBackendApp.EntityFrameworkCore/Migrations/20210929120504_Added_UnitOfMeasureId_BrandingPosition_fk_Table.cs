using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_UnitOfMeasureId_BrandingPosition_fk_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
            name: "UnitOfMeasureId",
            table: "ProductBrandingPosition",
            nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UnitOfMeasureId",
                table: "ProductBrandingPosition",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrandingPosition_UnitOfMeasureId",
                table: "ProductBrandingPosition",
                column: "UnitOfMeasureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBrandingPosition_ProductSizeMaster_UnitOfMeasureId",
                table: "ProductBrandingPosition",
                column: "UnitOfMeasureId",
                principalTable: "ProductSizeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
             name: "UnitOfMeasureId",
             table: "ProductBrandingPosition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBrandingPosition_ProductSizeMaster_UnitOfMeasureId",
                table: "ProductBrandingPosition");

            migrationBuilder.DropIndex(
                name: "IX_ProductBrandingPosition_UnitOfMeasureId",
                table: "ProductBrandingPosition");

            migrationBuilder.AlterColumn<string>(
                name: "UnitOfMeasureId",
                table: "ProductBrandingPosition",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
