using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_DepartmentMasterDepartmentUsers_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DepartmentMaster",
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
                    DepartmentName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentUsers",
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
                    DepartmentMasterId = table.Column<long>(nullable: false),
                    SettingsId = table.Column<long>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentUsers_DepartmentMaster_DepartmentMasterId",
                        column: x => x.DepartmentMasterId,
                        principalTable: "DepartmentMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentUsers_UserBusinessSettings_SettingsId",
                        column: x => x.SettingsId,
                        principalTable: "UserBusinessSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentUsers_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVolumeDiscountVariant_ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant",
                column: "ProductBulkImportDataHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentUsers_DepartmentMasterId",
                table: "DepartmentUsers",
                column: "DepartmentMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentUsers_SettingsId",
                table: "DepartmentUsers",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentUsers_UserId",
                table: "DepartmentUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVolumeDiscountVariant_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant",
                column: "ProductBulkImportDataHistoryId",
                principalTable: "ProductBulkImportDataHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVolumeDiscountVariant_ProductBulkImportDataHistory_ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant");

            migrationBuilder.DropTable(
                name: "DepartmentUsers");

            migrationBuilder.DropTable(
                name: "DepartmentMaster");

            migrationBuilder.DropIndex(
                name: "IX_ProductVolumeDiscountVariant_ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant");

            migrationBuilder.DropColumn(
                name: "ProductBulkImportDataHistoryId",
                table: "ProductVolumeDiscountVariant");
        }
    }
}
