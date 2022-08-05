using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_CategoryCollections_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "CategoryCollections",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "CategoryCollections",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CategoryCollections",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "CategoryCollections");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "CategoryCollections");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CategoryCollections");
        }
    }
}
