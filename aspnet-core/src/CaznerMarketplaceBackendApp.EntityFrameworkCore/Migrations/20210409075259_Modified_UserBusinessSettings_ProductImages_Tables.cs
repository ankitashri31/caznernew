using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_UserBusinessSettings_ProductImages_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectDomain",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookLink",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FaviconImageData",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaviconImageName",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaviconImageURL",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramLink",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PinterestLink",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterLink",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultImage",
                table: "ProductMediaImages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultImage",
                table: "ProductImages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectDomain",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "FacebookLink",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "FaviconImageData",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "FaviconImageName",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "FaviconImageURL",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "InstagramLink",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "PinterestLink",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "TwitterLink",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "IsDefaultImage",
                table: "ProductMediaImages");

            migrationBuilder.DropColumn(
                name: "IsDefaultImage",
                table: "ProductImages");
        }
    }
}
