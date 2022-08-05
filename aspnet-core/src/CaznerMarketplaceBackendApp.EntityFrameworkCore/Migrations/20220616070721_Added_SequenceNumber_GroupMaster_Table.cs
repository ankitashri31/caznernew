using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_SequenceNumber_GroupMaster_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SequenceNumber",
                table: "CategoryGroups");

            migrationBuilder.AddColumn<int>(
                name: "SequenceNumber",
                table: "CategoryGroupMaster",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SequenceNumber",
                table: "CategoryGroupMaster");

            migrationBuilder.AddColumn<int>(
                name: "SequenceNumber",
                table: "CategoryGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
