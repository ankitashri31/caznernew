using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_SubProducts_Cols_ProductMaster_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHasSubProducts",
                table: "ProductMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSubProducts",
                table: "ProductMaster",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHasSubProducts",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "NumberOfSubProducts",
                table: "ProductMaster");
        }
    }
}
