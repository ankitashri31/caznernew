using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_UserBusinessSettings_ShippingAddress_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressType",
                table: "UserShippingAddress",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BusinessContactNumber",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessEmail",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessLogoUrl",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessWebsite",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyNumber",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaviconExt",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaviconName",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaviconSize",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaviconType",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaviconUrl",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoExt",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoName",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoSize",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoType",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MOQFee",
                table: "UserBusinessSettings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "MOQFeeSKU",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDaysQuoteIsValid",
                table: "UserBusinessSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PositionId",
                table: "UserBusinessSettings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "QuoteTerms",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserMobileNumber",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPhoneNumber",
                table: "UserBusinessSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressType",
                table: "UserShippingAddress");

            migrationBuilder.DropColumn(
                name: "BusinessContactNumber",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "BusinessEmail",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "BusinessLogoUrl",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "BusinessWebsite",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "CompanyNumber",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "FaviconExt",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "FaviconName",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "FaviconSize",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "FaviconType",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "FaviconUrl",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "LogoExt",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "LogoName",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "LogoSize",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "LogoType",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "MOQFee",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "MOQFeeSKU",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "NumberOfDaysQuoteIsValid",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "QuoteTerms",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "UserMobileNumber",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "UserPhoneNumber",
                table: "UserBusinessSettings");
        }
    }
}
