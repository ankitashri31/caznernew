using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ImageUrl_Fields_CategoryMaster_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryImageUrl",
                table: "CategoryMaster",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "CategoryMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "CategoryMaster",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryImageUrl",
                table: "CategoryMaster");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "CategoryMaster");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "CategoryMaster");
        }
    }
}
