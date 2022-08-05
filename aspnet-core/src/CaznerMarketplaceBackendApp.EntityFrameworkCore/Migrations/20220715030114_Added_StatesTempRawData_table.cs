using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_StatesTempRawData_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatesTempRawData",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryISOCode = table.Column<string>(nullable: true),
                    CountryName = table.Column<string>(nullable: true),
                    StateName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatesTempRawData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatesTempRawData");
        }
    }
}
