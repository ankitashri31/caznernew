using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_ProductImages_MediaImages_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ext",
                table: "ProductMediaImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductMediaImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ProductMediaImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ProductMediaImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ext",
                table: "ProductImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ProductImages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ProductImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ext",
                table: "ProductMediaImages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductMediaImages");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductMediaImages");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ProductMediaImages");

            migrationBuilder.DropColumn(
                name: "Ext",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ProductImages");
        }
    }
}
