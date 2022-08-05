using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_BrandingMethod_AdditionalPrice_Details_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBrandingPriceVariants_ProductMaster_ProductMasterId",
                table: "ProductBrandingPriceVariants");

            migrationBuilder.DropIndex(
                name: "IX_ProductBrandingPriceVariants_ProductMasterId",
                table: "ProductBrandingPriceVariants");

            migrationBuilder.DropColumn(
                name: "ProductMasterId",
                table: "ProductBrandingPriceVariants");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ProductColourMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BrandingUnitPrice",
                table: "ProductBrandingPriceVariants",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostPerItem",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Ext",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsChargeTextOnThis",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMethodHasAdditionalPriceVariant",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMethodHasQuantityPriceVariant",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecificFontStyle",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecificFontTypeFace",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MethodDescripition",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MethodSKU",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Profit",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Width",
                table: "BrandingMethodMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BrandingMethodAdditionalPrice",
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
                    QtyPlus50 = table.Column<int>(nullable: false),
                    QtyPlus00 = table.Column<int>(nullable: false),
                    QtyPlus250 = table.Column<int>(nullable: false),
                    QtyPlus500 = table.Column<int>(nullable: false),
                    QtyPlus1000 = table.Column<int>(nullable: false),
                    QtyPlus10000 = table.Column<int>(nullable: false),
                    Price = table.Column<string>(nullable: true),
                    BrandingMethodId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingMethodAdditionalPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAdditionalPrice_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandingMethodDetails",
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
                    UniversalBrnadingArray = table.Column<string>(nullable: true),
                    BrandingColorArray = table.Column<string>(nullable: true),
                    BrandingEquipmentArray = table.Column<string>(nullable: true),
                    BrandingTagArray = table.Column<string>(nullable: true),
                    ProductVendorArray = table.Column<string>(nullable: true),
                    SpecificFontStyleArray = table.Column<string>(nullable: true),
                    SpecificFontTypeFaceArray = table.Column<string>(nullable: true),
                    BrandingMethodId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingMethodDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingMethodDetails_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentBrandingMaster",
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
                    EquipmentTitle = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentBrandingMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UniversalBrandingMaster",
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
                    UniversalBrandingTitle = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniversalBrandingMaster", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAdditionalPrice_BrandingMethodId",
                table: "BrandingMethodAdditionalPrice",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodDetails_BrandingMethodId",
                table: "BrandingMethodDetails",
                column: "BrandingMethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandingMethodAdditionalPrice");

            migrationBuilder.DropTable(
                name: "BrandingMethodDetails");

            migrationBuilder.DropTable(
                name: "EquipmentBrandingMaster");

            migrationBuilder.DropTable(
                name: "UniversalBrandingMaster");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductColourMaster");

            migrationBuilder.DropColumn(
                name: "BrandingUnitPrice",
                table: "ProductBrandingPriceVariants");

            migrationBuilder.DropColumn(
                name: "CostPerItem",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "Ext",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "IsChargeTextOnThis",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "IsMethodHasAdditionalPriceVariant",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "IsMethodHasQuantityPriceVariant",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "IsSpecificFontStyle",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "IsSpecificFontTypeFace",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "MethodDescripition",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "MethodSKU",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "Profit",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "BrandingMethodMaster");

            migrationBuilder.AddColumn<long>(
                name: "ProductMasterId",
                table: "ProductBrandingPriceVariants",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrandingPriceVariants_ProductMasterId",
                table: "ProductBrandingPriceVariants",
                column: "ProductMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBrandingPriceVariants_ProductMaster_ProductMasterId",
                table: "ProductBrandingPriceVariants",
                column: "ProductMasterId",
                principalTable: "ProductMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
