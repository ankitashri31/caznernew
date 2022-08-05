using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_BrandingMethodMaster_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorSelectionType",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfStiches",
                table: "BrandingMethodMaster",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorSelectionType",
                table: "BrandingMethodMaster");

            migrationBuilder.DropColumn(
                name: "NumberOfStiches",
                table: "BrandingMethodMaster");
        }
    }
}
