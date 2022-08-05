using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_IsVariantHasMixMatch_Field_Product_VariantData_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVariantHasMixAndMatch",
                table: "ProductVariantsData",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProductHasMixAndMatch",
                table: "ProductMaster",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVariantHasMixAndMatch",
                table: "ProductVariantsData");

            migrationBuilder.DropColumn(
                name: "IsProductHasMixAndMatch",
                table: "ProductMaster");
        }
    }
}
