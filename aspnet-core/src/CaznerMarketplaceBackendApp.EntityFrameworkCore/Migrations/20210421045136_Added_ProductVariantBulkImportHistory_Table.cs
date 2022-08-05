using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ProductVariantBulkImportHistory_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductVariantBulkImportHistory",
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
                    VariantDataId = table.Column<long>(nullable: true),
                    VariantOptionId = table.Column<long>(nullable: true),
                    BulkUploadVariationsId = table.Column<long>(nullable: true),
                    VariantdataImageId = table.Column<long>(nullable: true),
                    VariantWarehouseId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantBulkImportHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantBulkImportHistory_ProductBulkUploadVariations_BulkUploadVariationsId",
                        column: x => x.BulkUploadVariationsId,
                        principalTable: "ProductBulkUploadVariations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductVariantBulkImportHistory_ProductVariantsData_VariantDataId",
                        column: x => x.VariantDataId,
                        principalTable: "ProductVariantsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductVariantBulkImportHistory_ProductVariantOptionValues_VariantOptionId",
                        column: x => x.VariantOptionId,
                        principalTable: "ProductVariantOptionValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductVariantBulkImportHistory_ProductVariantWarehouse_VariantWarehouseId",
                        column: x => x.VariantWarehouseId,
                        principalTable: "ProductVariantWarehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductVariantBulkImportHistory_ProductVariantdataImages_VariantdataImageId",
                        column: x => x.VariantdataImageId,
                        principalTable: "ProductVariantdataImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantWarehouse_ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse",
                column: "ProductVariantBulkImportHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantOptionValues_ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues",
                column: "ProductVariantBulkImportHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantdataImages_ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages",
                column: "ProductVariantBulkImportHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantBulkImportHistory_BulkUploadVariationsId",
                table: "ProductVariantBulkImportHistory",
                column: "BulkUploadVariationsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantBulkImportHistory_VariantDataId",
                table: "ProductVariantBulkImportHistory",
                column: "VariantDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantBulkImportHistory_VariantOptionId",
                table: "ProductVariantBulkImportHistory",
                column: "VariantOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantBulkImportHistory_VariantWarehouseId",
                table: "ProductVariantBulkImportHistory",
                column: "VariantWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantBulkImportHistory_VariantdataImageId",
                table: "ProductVariantBulkImportHistory",
                column: "VariantdataImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantdataImages_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages",
                column: "ProductVariantBulkImportHistoryId",
                principalTable: "ProductVariantBulkImportHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantOptionValues_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues",
                column: "ProductVariantBulkImportHistoryId",
                principalTable: "ProductVariantBulkImportHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantWarehouse_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse",
                column: "ProductVariantBulkImportHistoryId",
                principalTable: "ProductVariantBulkImportHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantdataImages_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantOptionValues_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantWarehouse_ProductVariantBulkImportHistory_ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse");

            migrationBuilder.DropTable(
                name: "ProductVariantBulkImportHistory");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantWarehouse_ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantOptionValues_ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantdataImages_ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages");

            migrationBuilder.DropColumn(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantWarehouse");

            migrationBuilder.DropColumn(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantOptionValues");

            migrationBuilder.DropColumn(
                name: "ProductVariantBulkImportHistoryId",
                table: "ProductVariantdataImages");
        }
    }
}
