using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ProductVariantTables_WareHouseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WareHouseMaster",
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
                    WarehouseTitle = table.Column<string>(nullable: true),
                    StreetAddress = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    StateId = table.Column<long>(nullable: false),
                    PostCode = table.Column<string>(nullable: true),
                    CountryId = table.Column<long>(nullable: false),
                    TimezoneId = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WareHouseMaster_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id"                        );
                    table.ForeignKey(
                        name: "FK_WareHouseMaster_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id"
                        );
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantsData",
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
                    Variant = table.Column<string>(nullable: true),
                    VariantMasterIds = table.Column<string>(nullable: true),
                    SKU = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Material = table.Column<string>(nullable: true),
                    style = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    ComparePrice = table.Column<decimal>(nullable: false),
                    CostPerItem = table.Column<decimal>(nullable: false),
                    Margin = table.Column<string>(nullable: true),
                    ProfitCurrencySymbol = table.Column<int>(nullable: false),
                    Profit = table.Column<decimal>(nullable: false),
                    BarCode = table.Column<string>(nullable: true),
                    WareHouseMasterId = table.Column<long>(nullable: true),
                    WarehouseId = table.Column<long>(nullable: true),
                    LocationA = table.Column<string>(nullable: true),
                    LocationB = table.Column<string>(nullable: true),
                    LocationC = table.Column<string>(nullable: true),
                    QuantityStockUnit = table.Column<double>(nullable: false),
                    QuantityThisLocation = table.Column<double>(nullable: false),
                    StockAlertQuantity = table.Column<double>(nullable: false),
                    IsTrackQuantity = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Shape = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantsData_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id"
                        );
                    table.ForeignKey(
                        name: "FK_ProductVariantsData_WareHouseMaster_WareHouseMasterId",
                        column: x => x.WareHouseMasterId,
                        principalTable: "WareHouseMaster",
                        principalColumn: "Id"
                        );
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantdataImages",
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
                    ProductId = table.Column<long>(nullable: false),
                    ImageFileData = table.Column<byte[]>(nullable: true),
                    ImageFileName = table.Column<string>(nullable: true),
                    ImageURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantdataImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantdataImages_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariantdataImages_ProductVariantsData_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariantsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantdataImages_ProductId",
                table: "ProductVariantdataImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantdataImages_ProductVariantId",
                table: "ProductVariantdataImages",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantsData_ProductId",
                table: "ProductVariantsData",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantsData_WareHouseMasterId",
                table: "ProductVariantsData",
                column: "WareHouseMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseMaster_CountryId",
                table: "WareHouseMaster",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseMaster_StateId",
                table: "WareHouseMaster",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVariantdataImages");

            migrationBuilder.DropTable(
                name: "ProductVariantsData");

            migrationBuilder.DropTable(
                name: "WareHouseMaster");
        }
    }
}
