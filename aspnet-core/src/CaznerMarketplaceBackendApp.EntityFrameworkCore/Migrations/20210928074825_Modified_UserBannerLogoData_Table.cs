using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_UserBannerLogoData_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ext",
                table: "UserBannerLogoData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserBannerLogoData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "UserBannerLogoData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "UserBannerLogoData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "UserBannerLogoData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ext",
                table: "UserBannerLogoData");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserBannerLogoData");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "UserBannerLogoData");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserBannerLogoData");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "UserBannerLogoData");
        }
    }
}
