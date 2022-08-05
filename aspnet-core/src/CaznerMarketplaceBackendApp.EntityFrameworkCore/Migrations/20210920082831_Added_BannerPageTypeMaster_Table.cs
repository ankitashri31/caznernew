using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_BannerPageTypeMaster_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageType",
                table: "UserBannerLogoData");

            migrationBuilder.AddColumn<long>(
                name: "PageTypeId",
                table: "UserBannerLogoData",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BannerPageTypeMaster",
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
                    Title = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerPageTypeMaster", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBannerLogoData_PageTypeId",
                table: "UserBannerLogoData",
                column: "PageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBannerLogoData_BannerPageTypeMaster_PageTypeId",
                table: "UserBannerLogoData",
                column: "PageTypeId",
                principalTable: "BannerPageTypeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBannerLogoData_BannerPageTypeMaster_PageTypeId",
                table: "UserBannerLogoData");

            migrationBuilder.DropTable(
                name: "BannerPageTypeMaster");

            migrationBuilder.DropIndex(
                name: "IX_UserBannerLogoData_PageTypeId",
                table: "UserBannerLogoData");

            migrationBuilder.DropColumn(
                name: "PageTypeId",
                table: "UserBannerLogoData");

            migrationBuilder.AddColumn<int>(
                name: "PageType",
                table: "UserBannerLogoData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
