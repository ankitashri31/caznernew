using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_Cols_BusinessSettings_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "UserBusinessSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BSB",
                table: "UserBusinessSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "BSB",
                table: "UserBusinessSettings");
        }
    }
}
