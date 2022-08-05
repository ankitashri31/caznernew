using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ProductVariantQuantityPrices_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DiscountPercentage",
                table: "ProductVariantsData",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DiscountPercentageDraft",
                table: "ProductVariantsData",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "OnSale",
                table: "ProductVariantsData",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "ProductVariantsData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SaleUnitPrice",
                table: "ProductVariantsData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ProductVariantQuantityPrices",
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
                    QuantityFrom = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantQuantityPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantQuantityPrices_ProductVariantsData_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariantsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantQuantityPrices_ProductVariantId",
                table: "ProductVariantQuantityPrices",
                column: "ProductVariantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVariantQuantityPrices");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "DiscountPercentageDraft",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "OnSale",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "SaleUnitPrice",
                table: "ProductVariantsData");
        }
    }
}
