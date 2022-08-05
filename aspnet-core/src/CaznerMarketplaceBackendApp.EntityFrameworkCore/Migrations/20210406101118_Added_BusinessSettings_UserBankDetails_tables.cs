using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_BusinessSettings_UserBankDetails_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SettingsId",
                table: "WareHouseMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CurrencyMaster",
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
                    CurrencyName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBusinessSettings",
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
                    UserId = table.Column<long>(nullable: false),
                    CurrencyMasterId = table.Column<long>(nullable: false),
                    MeasurementId = table.Column<long>(nullable: false),
                    IsChargeMinimumOrderFee = table.Column<double>(nullable: false),
                    IsShowPriceWithoutAccount = table.Column<bool>(nullable: false),
                    IsChargeTaxGstorVst = table.Column<bool>(nullable: false),
                    TaxPercentOnAddProduct = table.Column<string>(nullable: true),
                    TaxCode = table.Column<string>(nullable: true),
                    ProductPricePrefix = table.Column<string>(nullable: true),
                    IsShipingRatesIncludingTax = table.Column<bool>(nullable: false),
                    IsChargeTaxOnShipping = table.Column<bool>(nullable: false),
                    IsShowPriceTax = table.Column<bool>(nullable: false),
                    ValidNoOfDays = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBusinessSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBusinessSettings_CurrencyMaster_CurrencyMasterId",
                        column: x => x.CurrencyMasterId,
                        principalTable: "CurrencyMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBusinessSettings_ProductSizeMaster_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "ProductSizeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBusinessSettings_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBankDetails",
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
                    UserId = table.Column<long>(nullable: false),
                    BankName = table.Column<string>(nullable: true),
                    SwiftCode = table.Column<string>(nullable: true),
                    AccountName = table.Column<string>(nullable: true),
                    BranchAddress = table.Column<string>(nullable: true),
                    SettingsId = table.Column<long>(nullable: true),
                    IsStripeAccount = table.Column<long>(nullable: false),
                    IsPayPalAccount = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBankDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBankDetails_UserBusinessSettings_SettingsId",
                        column: x => x.SettingsId,
                        principalTable: "UserBusinessSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserBankDetails_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseMaster_SettingsId",
                table: "WareHouseMaster",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBankDetails_SettingsId",
                table: "UserBankDetails",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBankDetails_UserId",
                table: "UserBankDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBusinessSettings_CurrencyMasterId",
                table: "UserBusinessSettings",
                column: "CurrencyMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBusinessSettings_MeasurementId",
                table: "UserBusinessSettings",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBusinessSettings_UserId",
                table: "UserBusinessSettings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouseMaster_UserBusinessSettings_SettingsId",
                table: "WareHouseMaster",
                column: "SettingsId",
                principalTable: "UserBusinessSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WareHouseMaster_UserBusinessSettings_SettingsId",
                table: "WareHouseMaster");

            migrationBuilder.DropTable(
                name: "UserBankDetails");

            migrationBuilder.DropTable(
                name: "UserBusinessSettings");

            migrationBuilder.DropTable(
                name: "CurrencyMaster");

            migrationBuilder.DropIndex(
                name: "IX_WareHouseMaster_SettingsId",
                table: "WareHouseMaster");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "WareHouseMaster");
        }
    }
}
