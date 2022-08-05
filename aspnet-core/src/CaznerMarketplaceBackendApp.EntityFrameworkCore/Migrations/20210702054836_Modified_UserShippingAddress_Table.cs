using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_UserShippingAddress_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessEmail",
                table: "UserShippingAddress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "UserShippingAddress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "UserShippingAddress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "UserShippingAddress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "UserShippingAddress",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessEmail",
                table: "UserShippingAddress");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "UserShippingAddress");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "UserShippingAddress");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "UserShippingAddress");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "UserShippingAddress");
        }
    }
}
