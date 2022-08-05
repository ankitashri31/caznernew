using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_Related_Alternate_Products_ProductMaster_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "ProductAssignedCategories");

            migrationBuilder.AddColumn<string>(
                name: "BrandingMethodNote",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrandingUOM",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColourFamily",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CounrtyOfOrigin",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraSetUpFee",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrightNote",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IfSetOtherProductTitleAndDimensoionsInThisSet",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image360Degrees",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsIndentOrder",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextShipmentDate",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextShipmentQuantity",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumberOfPieces",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PMSColourCode",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoURL",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VolumeUOM",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VolumeValue",
                table: "ProductMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AlternativeProducts",
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
                    ProductSKU = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternativeProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlternativeProducts_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductViewImages",
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
                    ImageName = table.Column<string>(nullable: true),
                    ProductId = table.Column<long>(nullable: false),
                    ImagePath = table.Column<string>(nullable: true),
                    ImageSize = table.Column<string>(nullable: true),
                    ImageFileData = table.Column<byte[]>(nullable: true),
                    ImageExtension = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Ext = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductViewImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductViewImages_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RelativeProducts",
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
                    ProductSKU = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelativeProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelativeProducts_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlternativeProducts_ProductId",
                table: "AlternativeProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductViewImages_ProductId",
                table: "ProductViewImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RelativeProducts_ProductId",
                table: "RelativeProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlternativeProducts");

            migrationBuilder.DropTable(
                name: "ProductViewImages");

            migrationBuilder.DropTable(
                name: "RelativeProducts");

            migrationBuilder.DropColumn(
                name: "BrandingMethodNote",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "BrandingUOM",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "ColourFamily",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "CounrtyOfOrigin",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "ExtraSetUpFee",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "FrightNote",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "IfSetOtherProductTitleAndDimensoionsInThisSet",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "Image360Degrees",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "IsIndentOrder",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "NextShipmentDate",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "NextShipmentQuantity",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "NumberOfPieces",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "PMSColourCode",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "VideoURL",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "VolumeUOM",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "VolumeValue",
                table: "ProductMaster");

            //migrationBuilder.CreateTable(
            //    name: "ProductAssignedCategories",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
            //        DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
            //        DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
            //        ProductCategoryId = table.Column<long>(type: "bigint", nullable: false),
            //        ProductId = table.Column<long>(type: "bigint", nullable: false),
            //        TenantId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ProductAssignedCategories", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ProductAssignedCategories_CategoryMaster_ProductCategoryId",
            //            column: x => x.ProductCategoryId,
            //            principalTable: "CategoryMaster",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.NoAction);
            //        table.ForeignKey(
            //            name: "FK_ProductAssignedCategories_ProductMaster_ProductId",
            //            column: x => x.ProductId,
            //            principalTable: "ProductMaster",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.NoAction);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProductAssignedCategories_ProductCategoryId",
            //    table: "ProductAssignedCategories",
            //    column: "ProductCategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProductAssignedCategories_ProductId",
            //    table: "ProductAssignedCategories",
            //    column: "ProductId");
        }
    }
}
