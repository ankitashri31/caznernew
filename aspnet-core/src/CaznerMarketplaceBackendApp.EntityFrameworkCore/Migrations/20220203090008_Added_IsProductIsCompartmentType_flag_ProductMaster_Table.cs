using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_IsProductIsCompartmentType_flag_ProductMaster_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProductIsCompartmentType",
                table: "ProductMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
               name: "CompartmentBuilderTitle",
               table: "ProductMaster",
               nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProductIsCompartmentType",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "CompartmentBuilderTitle",
                table: "ProductMaster");
        }
    }
}
