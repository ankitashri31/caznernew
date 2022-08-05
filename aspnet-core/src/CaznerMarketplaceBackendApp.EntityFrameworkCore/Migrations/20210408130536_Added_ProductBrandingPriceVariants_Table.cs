using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ProductBrandingPriceVariants_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductBrandingPriceVariants",
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
                    BrandingMethodId = table.Column<long>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    ProductMasterId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBrandingPriceVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBrandingPriceVariants_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductBrandingPriceVariants_ProductMaster_ProductMasterId",
                        column: x => x.ProductMasterId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrandingPriceVariants_BrandingMethodId",
                table: "ProductBrandingPriceVariants",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrandingPriceVariants_ProductMasterId",
                table: "ProductBrandingPriceVariants",
                column: "ProductMasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBrandingPriceVariants");
        }
    }
}
