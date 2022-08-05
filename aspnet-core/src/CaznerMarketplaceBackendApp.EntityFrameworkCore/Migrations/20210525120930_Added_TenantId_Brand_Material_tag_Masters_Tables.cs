using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_TenantId_Brand_Material_tag_Masters_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ProductTagMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ProductMaterialMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ProductBrandMaster",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductTagMaster");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductMaterialMaster");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductBrandMaster");
        }
    }
}
