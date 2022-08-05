using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_Region_Table_Modified_Countries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "CurrencyMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISO2Code",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISO3Code",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RegionId",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorldBankIncomeGroup",
                table: "Countries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    RegionCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_RegionId",
                table: "Countries",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Regions_RegionId",
                table: "Countries",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Regions_RegionId",
                table: "Countries");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Countries_RegionId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "CurrencyMaster");

            migrationBuilder.DropColumn(
                name: "ISO2Code",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "ISO3Code",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "WorldBankIncomeGroup",
                table: "Countries");
        }
    }
}
