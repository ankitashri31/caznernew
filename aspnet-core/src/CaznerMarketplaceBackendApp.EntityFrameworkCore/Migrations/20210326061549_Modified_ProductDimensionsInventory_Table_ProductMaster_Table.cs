using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_ProductDimensionsInventory_Table_ProductMaster_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductandPackageDimension");

            migrationBuilder.DropColumn(
                name: "PriceChargeTax",
                table: "ProductMaster");

            migrationBuilder.AddColumn<bool>(
                name: "ChargeTaxOnThis",
                table: "ProductMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ProductHasPriceVariant",
                table: "ProductMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProductDimensionsInventory",
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
                    ProductId = table.Column<long>(nullable: false),
                    ProductHeight = table.Column<string>(nullable: true),
                    ProductWidth = table.Column<string>(nullable: true),
                    ProductLength = table.Column<string>(nullable: true),
                    UnitWeight = table.Column<string>(nullable: true),
                    ProductPackaging = table.Column<string>(nullable: true),
                    UnitPerProduct = table.Column<double>(nullable: false),
                    CartonHeight = table.Column<string>(nullable: true),
                    CartonWidth = table.Column<string>(nullable: true),
                    CartonLength = table.Column<string>(nullable: true),
                    UnitPerCarton = table.Column<double>(nullable: false),
                    CartonPackaging = table.Column<string>(nullable: true),
                    CartonNote = table.Column<string>(nullable: true),
                    PalletWeight = table.Column<string>(nullable: true),
                    CartonPerPallet = table.Column<double>(nullable: false),
                    UnitPerPallet = table.Column<double>(nullable: false),
                    PalletNote = table.Column<string>(nullable: true),
                    StockkeepingUnit = table.Column<int>(nullable: false),
                    Barcode = table.Column<string>(nullable: true),
                    TotalNumberAvailable = table.Column<long>(nullable: false),
                    AlertRestockNumber = table.Column<int>(nullable: false),
                    IsTrackQuantity = table.Column<bool>(nullable: false),
                    IsStopSellingStockZero = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDimensionsInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDimensionsInventory_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDimensionsInventory_ProductId",
                table: "ProductDimensionsInventory",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDimensionsInventory");

            migrationBuilder.DropColumn(
                name: "ChargeTaxOnThis",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "ProductHasPriceVariant",
                table: "ProductMaster");

            migrationBuilder.AddColumn<int>(
                name: "PriceChargeTax",
                table: "ProductMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductandPackageDimension",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlertRestockNumber = table.Column<int>(type: "int", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CartonQuantity = table.Column<long>(type: "bigint", nullable: false),
                    CartonWeight = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsStopSellingStockZero = table.Column<bool>(type: "bit", nullable: false),
                    IsTrackQuantity = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageHeight = table.Column<long>(type: "bigint", nullable: false),
                    PackageLength = table.Column<long>(type: "bigint", nullable: false),
                    PackageWidth = table.Column<long>(type: "bigint", nullable: false),
                    ProductHeight = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductLength = table.Column<long>(type: "bigint", nullable: false),
                    ProductPackaging = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductWidth = table.Column<long>(type: "bigint", nullable: false),
                    StockkeepingUnit = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TotalNumberAvailable = table.Column<int>(type: "int", nullable: false),
                    UnitPerCarton = table.Column<long>(type: "bigint", nullable: false),
                    UnitWeight = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductandPackageDimension", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductandPackageDimension_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductandPackageDimension_ProductId",
                table: "ProductandPackageDimension",
                column: "ProductId");
        }
    }
}
