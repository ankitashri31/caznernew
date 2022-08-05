using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_VariantOptions_VariantWareHouse_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantsData_WareHouseMaster_WareHouseMasterId",
                table: "ProductVariantsData");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantsData_WareHouseMasterId",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "LocationA",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "LocationB",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "LocationC",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "QuantityThisLocation",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "StockAlertQuantity",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "WareHouseMasterId",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "style",
                table: "ProductVariantsData");

            migrationBuilder.CreateTable(
                name: "ProductVariantOptionValues",
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
                    ProductVariantId = table.Column<long>(nullable: false),
                    VariantOptionId = table.Column<long>(nullable: false),
                    VariantOptionValue = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantOptionValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantOptionValues_ProductVariantsData_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariantsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariantOptionValues_ProductOptionsMaster_VariantOptionId",
                        column: x => x.VariantOptionId,
                        principalTable: "ProductOptionsMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantWarehouse",
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
                    WareHouseMasterId = table.Column<long>(nullable: true),
                    WarehouseId = table.Column<long>(nullable: false),
                    ProductVariantId = table.Column<long>(nullable: false),
                    LocationA = table.Column<string>(nullable: true),
                    LocationB = table.Column<string>(nullable: true),
                    LocationC = table.Column<string>(nullable: true),
                    StockAlertQuantity = table.Column<double>(nullable: false),
                    QuantityThisLocation = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantWarehouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantWarehouse_ProductVariantsData_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariantsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariantWarehouse_WareHouseMaster_WareHouseMasterId",
                        column: x => x.WareHouseMasterId,
                        principalTable: "WareHouseMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantOptionValues_ProductVariantId",
                table: "ProductVariantOptionValues",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantOptionValues_VariantOptionId",
                table: "ProductVariantOptionValues",
                column: "VariantOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantWarehouse_ProductVariantId",
                table: "ProductVariantWarehouse",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantWarehouse_WareHouseMasterId",
                table: "ProductVariantWarehouse",
                column: "WareHouseMasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVariantOptionValues");

            migrationBuilder.DropTable(
                name: "ProductVariantWarehouse");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ProductVariantsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationA",
                table: "ProductVariantsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationB",
                table: "ProductVariantsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationC",
                table: "ProductVariantsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "ProductVariantsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QuantityThisLocation",
                table: "ProductVariantsData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ProductVariantsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StockAlertQuantity",
                table: "ProductVariantsData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "WareHouseMasterId",
                table: "ProductVariantsData",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WarehouseId",
                table: "ProductVariantsData",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "style",
                table: "ProductVariantsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantsData_WareHouseMasterId",
                table: "ProductVariantsData",
                column: "WareHouseMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantsData_WareHouseMaster_WareHouseMasterId",
                table: "ProductVariantsData",
                column: "WareHouseMasterId",
                principalTable: "WareHouseMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
