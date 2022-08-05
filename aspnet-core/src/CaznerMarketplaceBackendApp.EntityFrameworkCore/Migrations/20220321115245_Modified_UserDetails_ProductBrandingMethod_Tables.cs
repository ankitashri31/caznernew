using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_UserDetails_ProductBrandingMethod_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RequestApprovalDate",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MethodCustomizedColor",
                table: "ProductBrandingMethods",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestApprovalDate",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "MethodCustomizedColor",
                table: "ProductBrandingMethods");
        }
    }
}
