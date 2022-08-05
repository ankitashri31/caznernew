using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ProductBrandingMethods_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChargeTextOnThis",
                table: "BrandingMethodMaster");

            migrationBuilder.AddColumn<bool>(
                name: "IsChargeTaxOnThis",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProductBrandingMethods",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ProductMasterId = table.Column<long>(nullable: false),
                    BrandingMethodId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBrandingMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBrandingMethods_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductBrandingMethods_ProductMaster_ProductMasterId",
                        column: x => x.ProductMasterId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrandingMethods_BrandingMethodId",
                table: "ProductBrandingMethods",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrandingMethods_ProductMasterId",
                table: "ProductBrandingMethods",
                column: "ProductMasterId");

            // migration to add MeasurementId in BrandingAdditionalPrice table

            migrationBuilder.DropColumn(
                name: "QtyPlus00",
                table: "BrandingMethodAdditionalPrice");

            migrationBuilder.AddColumn<long>(
                name: "MeasurementId",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QtyPlus100",
                table: "BrandingMethodAdditionalPrice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodMaster_MeasurementId",
                table: "BrandingMethodMaster",
                column: "MeasurementId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandingMethodMaster_ProductSizeMaster_MeasurementId",
                table: "BrandingMethodMaster",
                column: "MeasurementId",
                principalTable: "ProductSizeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);


        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBrandingMethods");

            migrationBuilder.DropColumn(
                name: "IsChargeTaxOnThis",
                table: "BrandingMethodMaster");

            migrationBuilder.AddColumn<bool>(
                name: "IsChargeTextOnThis",
                table: "BrandingMethodMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // migration to add MeasurementId in BrandingAdditionalPrice table

            migrationBuilder.DropForeignKey(
               name: "FK_BrandingMethodMaster_ProductSizeMaster_MeasurementId",
               table: "BrandingMethodMaster");

            migrationBuilder.DropIndex(
                name: "IX_BrandingMethodMaster_MeasurementId",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "MeasurementId",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "QtyPlus100",
                table: "BrandingMethodAdditionalPrice");

            migrationBuilder.AddColumn<int>(
                name: "QtyPlus00",
                table: "BrandingMethodAdditionalPrice",
                type: "int",
                nullable: false,
                defaultValue: 0);

      
        }
    }
}
