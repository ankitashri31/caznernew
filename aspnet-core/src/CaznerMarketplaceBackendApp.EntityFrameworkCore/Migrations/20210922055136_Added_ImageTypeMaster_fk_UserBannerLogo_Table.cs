using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ImageTypeMaster_fk_UserBannerLogo_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "UserBannerLogoData");

            migrationBuilder.AddColumn<long>(
                name: "ImageTypeId",
                table: "UserBannerLogoData",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ImageTypeMaster",
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
                    table.PrimaryKey("PK_ImageTypeMaster", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBannerLogoData_ImageTypeId",
                table: "UserBannerLogoData",
                column: "ImageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBannerLogoData_ImageTypeMaster_ImageTypeId",
                table: "UserBannerLogoData",
                column: "ImageTypeId",
                principalTable: "ImageTypeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBannerLogoData_ImageTypeMaster_ImageTypeId",
                table: "UserBannerLogoData");

            migrationBuilder.DropTable(
                name: "ImageTypeMaster");

            migrationBuilder.DropIndex(
                name: "IX_UserBannerLogoData_ImageTypeId",
                table: "UserBannerLogoData");

            migrationBuilder.DropColumn(
                name: "ImageTypeId",
                table: "UserBannerLogoData");

            migrationBuilder.AddColumn<int>(
                name: "ImageType",
                table: "UserBannerLogoData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
