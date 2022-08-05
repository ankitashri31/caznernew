using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_Images_CategoryGroupMaster_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupImageUrl",
                table: "CategoryGroupMaster",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "CategoryGroupMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "CategoryGroupMaster",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupImageUrl",
                table: "CategoryGroupMaster");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "CategoryGroupMaster");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "CategoryGroupMaster");
        }
    }
}
