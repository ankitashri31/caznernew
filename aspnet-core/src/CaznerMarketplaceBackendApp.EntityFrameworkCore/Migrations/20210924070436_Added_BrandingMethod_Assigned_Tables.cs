using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_BrandingMethod_Assigned_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedCategories_CategoryMaster_ProductCategoryId",
                table: "ProductAssignedCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedCollections_ProductCollectionMaster_ProductCollectionId",
                table: "ProductAssignedCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedMaterials_ProductMaterialMaster_ProductMaterialId",
                table: "ProductAssignedMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedTags_ProductTagMaster_ProductTagId",
                table: "ProductAssignedTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedTypes_ProductTypeMaster_ProductTypeId",
                table: "ProductAssignedTypes");

            migrationBuilder.DropTable(
                name: "BrandingMethodDetails");

            migrationBuilder.DropIndex(
                name: "IX_ProductAssignedTypes_ProductTypeId",
                table: "ProductAssignedTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductAssignedTags_ProductTagId",
                table: "ProductAssignedTags");

            migrationBuilder.DropIndex(
                name: "IX_ProductAssignedMaterials_ProductMaterialId",
                table: "ProductAssignedMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ProductAssignedCollections_ProductCollectionId",
                table: "ProductAssignedCollections");

            migrationBuilder.DropIndex(
                name: "IX_ProductAssignedCategories_ProductCategoryId",
                table: "ProductAssignedCategories");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                table: "ProductAssignedTypes");

            migrationBuilder.DropColumn(
                name: "ProductTagId",
                table: "ProductAssignedTags");

            migrationBuilder.DropColumn(
                name: "ProductMaterialId",
                table: "ProductAssignedMaterials");

            migrationBuilder.DropColumn(
                name: "ProductCollectionId",
                table: "ProductAssignedCollections");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "ProductAssignedCategories");

            migrationBuilder.AddColumn<long>(
                name: "TypeId",
                table: "ProductAssignedTypes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TagId",
                table: "ProductAssignedTags",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MaterialId",
                table: "ProductAssignedMaterials",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CollectionId",
                table: "ProductAssignedCollections",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "ProductAssignedCategories",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "BrandingMethodAssignedColors",
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
                    ColorId = table.Column<long>(nullable: false),
                    BrandingMethodId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingMethodAssignedColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedColors_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedColors_ProductColourMaster_ColorId",
                        column: x => x.ColorId,
                        principalTable: "ProductColourMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandingMethodAssignedEquipments",
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
                    EquipmentBrandingId = table.Column<long>(nullable: false),
                    BrandingMethodId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingMethodAssignedEquipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedEquipments_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedEquipments_EquipmentBrandingMaster_EquipmentBrandingId",
                        column: x => x.EquipmentBrandingId,
                        principalTable: "EquipmentBrandingMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandingMethodAssignedFontStyles",
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
                    BrandingSpecificFontStyleMasterId = table.Column<long>(nullable: true),
                    BrandingSpecificFontStyleId = table.Column<long>(nullable: false),
                    BrandingMethodId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingMethodAssignedFontStyles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedFontStyles_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedFontStyles_BrandingSpecificFontStyleMaster_BrandingSpecificFontStyleMasterId",
                        column: x => x.BrandingSpecificFontStyleMasterId,
                        principalTable: "BrandingSpecificFontStyleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BrandingMethodAssignedFontTypeFaces",
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
                    BrandingSpecificFontTypeFaceId = table.Column<long>(nullable: false),
                    BrandingMethodId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingMethodAssignedFontTypeFaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedFontTypeFaces_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedFontTypeFaces_BrandingSpecificFontTypeFaceMaster_BrandingSpecificFontTypeFaceId",
                        column: x => x.BrandingSpecificFontTypeFaceId,
                        principalTable: "BrandingSpecificFontTypeFaceMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandingMethodAssignedTags",
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
                    TagId = table.Column<long>(nullable: false),
                    BrandingMethodId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingMethodAssignedTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedTags_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedTags_ProductTagMaster_TagId",
                        column: x => x.TagId,
                        principalTable: "ProductTagMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandingMethodAssignedUniversals",
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
                    UniversalId = table.Column<long>(nullable: false),
                    BrandingMethodId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingMethodAssignedUniversals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedUniversals_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedUniversals_UniversalBrandingMaster_UniversalId",
                        column: x => x.UniversalId,
                        principalTable: "UniversalBrandingMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandingMethodAssignedVendors",
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
                    VendorUserId = table.Column<long>(nullable: false),
                    BrandingMethodId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingMethodAssignedVendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedVendors_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandingMethodAssignedVendors_AbpUsers_VendorUserId",
                        column: x => x.VendorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedTypes_TypeId",
                table: "ProductAssignedTypes",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedTags_TagId",
                table: "ProductAssignedTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedMaterials_MaterialId",
                table: "ProductAssignedMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedCollections_CollectionId",
                table: "ProductAssignedCollections",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedCategories_CategoryId",
                table: "ProductAssignedCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedColors_BrandingMethodId",
                table: "BrandingMethodAssignedColors",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedColors_ColorId",
                table: "BrandingMethodAssignedColors",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedEquipments_BrandingMethodId",
                table: "BrandingMethodAssignedEquipments",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedEquipments_EquipmentBrandingId",
                table: "BrandingMethodAssignedEquipments",
                column: "EquipmentBrandingId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedFontStyles_BrandingMethodId",
                table: "BrandingMethodAssignedFontStyles",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedFontStyles_BrandingSpecificFontStyleMasterId",
                table: "BrandingMethodAssignedFontStyles",
                column: "BrandingSpecificFontStyleMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedFontTypeFaces_BrandingMethodId",
                table: "BrandingMethodAssignedFontTypeFaces",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedFontTypeFaces_BrandingSpecificFontTypeFaceId",
                table: "BrandingMethodAssignedFontTypeFaces",
                column: "BrandingSpecificFontTypeFaceId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedTags_BrandingMethodId",
                table: "BrandingMethodAssignedTags",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedTags_TagId",
                table: "BrandingMethodAssignedTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedUniversals_BrandingMethodId",
                table: "BrandingMethodAssignedUniversals",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedUniversals_UniversalId",
                table: "BrandingMethodAssignedUniversals",
                column: "UniversalId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedVendors_BrandingMethodId",
                table: "BrandingMethodAssignedVendors",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodAssignedVendors_VendorUserId",
                table: "BrandingMethodAssignedVendors",
                column: "VendorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedCategories_CategoryMaster_CategoryId",
                table: "ProductAssignedCategories",
                column: "CategoryId",
                principalTable: "CategoryMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedCollections_ProductCollectionMaster_CollectionId",
                table: "ProductAssignedCollections",
                column: "CollectionId",
                principalTable: "ProductCollectionMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedMaterials_ProductMaterialMaster_MaterialId",
                table: "ProductAssignedMaterials",
                column: "MaterialId",
                principalTable: "ProductMaterialMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedTags_ProductTagMaster_TagId",
                table: "ProductAssignedTags",
                column: "TagId",
                principalTable: "ProductTagMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedTypes_ProductTypeMaster_TypeId",
                table: "ProductAssignedTypes",
                column: "TypeId",
                principalTable: "ProductTypeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedCategories_CategoryMaster_CategoryId",
                table: "ProductAssignedCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedCollections_ProductCollectionMaster_CollectionId",
                table: "ProductAssignedCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedMaterials_ProductMaterialMaster_MaterialId",
                table: "ProductAssignedMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedTags_ProductTagMaster_TagId",
                table: "ProductAssignedTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedTypes_ProductTypeMaster_TypeId",
                table: "ProductAssignedTypes");

            migrationBuilder.DropTable(
                name: "BrandingMethodAssignedColors");

            migrationBuilder.DropTable(
                name: "BrandingMethodAssignedEquipments");

            migrationBuilder.DropTable(
                name: "BrandingMethodAssignedFontStyles");

            migrationBuilder.DropTable(
                name: "BrandingMethodAssignedFontTypeFaces");

            migrationBuilder.DropTable(
                name: "BrandingMethodAssignedTags");

            migrationBuilder.DropTable(
                name: "BrandingMethodAssignedUniversals");

            migrationBuilder.DropTable(
                name: "BrandingMethodAssignedVendors");

            migrationBuilder.DropIndex(
                name: "IX_ProductAssignedTypes_TypeId",
                table: "ProductAssignedTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductAssignedTags_TagId",
                table: "ProductAssignedTags");

            migrationBuilder.DropIndex(
                name: "IX_ProductAssignedMaterials_MaterialId",
                table: "ProductAssignedMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ProductAssignedCollections_CollectionId",
                table: "ProductAssignedCollections");

            migrationBuilder.DropIndex(
                name: "IX_ProductAssignedCategories_CategoryId",
                table: "ProductAssignedCategories");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "ProductAssignedTypes");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "ProductAssignedTags");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "ProductAssignedMaterials");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "ProductAssignedCollections");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ProductAssignedCategories");

            migrationBuilder.AddColumn<long>(
                name: "ProductTypeId",
                table: "ProductAssignedTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductTagId",
                table: "ProductAssignedTags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductMaterialId",
                table: "ProductAssignedMaterials",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductCollectionId",
                table: "ProductAssignedCollections",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductCategoryId",
                table: "ProductAssignedCategories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "BrandingMethodDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandingColorArray = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandingEquipmentArray = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandingMethodId = table.Column<long>(type: "bigint", nullable: false),
                    BrandingTagArray = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    ProductVendorArray = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificFontStyleArray = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificFontTypeFaceArray = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    UniversalBrandingArray = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedTypes_ProductTypeId",
                table: "ProductAssignedTypes",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedTags_ProductTagId",
                table: "ProductAssignedTags",
                column: "ProductTagId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedMaterials_ProductMaterialId",
                table: "ProductAssignedMaterials",
                column: "ProductMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedCollections_ProductCollectionId",
                table: "ProductAssignedCollections",
                column: "ProductCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedCategories_ProductCategoryId",
                table: "ProductAssignedCategories",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingMethodDetails_BrandingMethodId",
                table: "BrandingMethodDetails",
                column: "BrandingMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedCategories_CategoryMaster_ProductCategoryId",
                table: "ProductAssignedCategories",
                column: "ProductCategoryId",
                principalTable: "CategoryMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedCollections_ProductCollectionMaster_ProductCollectionId",
                table: "ProductAssignedCollections",
                column: "ProductCollectionId",
                principalTable: "ProductCollectionMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedMaterials_ProductMaterialMaster_ProductMaterialId",
                table: "ProductAssignedMaterials",
                column: "ProductMaterialId",
                principalTable: "ProductMaterialMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedTags_ProductTagMaster_ProductTagId",
                table: "ProductAssignedTags",
                column: "ProductTagId",
                principalTable: "ProductTagMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedTypes_ProductTypeMaster_ProductTypeId",
                table: "ProductAssignedTypes",
                column: "ProductTypeId",
                principalTable: "ProductTypeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
