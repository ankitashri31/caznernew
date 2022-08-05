using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_rejection_Fields_UserDetails_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequestRejectedByAdmin",
                table: "UserDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestRejectionDate",
                table: "UserDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequestRejectedByAdmin",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "RequestRejectionDate",
                table: "UserDetails");
        }
    }
}
