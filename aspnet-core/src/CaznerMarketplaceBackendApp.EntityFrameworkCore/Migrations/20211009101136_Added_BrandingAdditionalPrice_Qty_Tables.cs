using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_BrandingAdditionalPrice_Qty_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrandingAdditionalQuantities",
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
                    BrandingMethodId = table.Column<long>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingAdditionalQuantities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingAdditionalQuantities_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BrandingAdditionalQtyPrices",
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
                    AdditionalQtyId = table.Column<long>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingAdditionalQtyPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingAdditionalQtyPrices_BrandingAdditionalQuantities_AdditionalQtyId",
                        column: x => x.AdditionalQtyId,
                        principalTable: "BrandingAdditionalQuantities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandingAdditionalQtyPrices_AdditionalQtyId",
                table: "BrandingAdditionalQtyPrices",
                column: "AdditionalQtyId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingAdditionalQuantities_BrandingMethodId",
                table: "BrandingAdditionalQuantities",
                column: "BrandingMethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandingAdditionalQtyPrices");

            migrationBuilder.DropTable(
                name: "BrandingAdditionalQuantities");
        }
    }
}
