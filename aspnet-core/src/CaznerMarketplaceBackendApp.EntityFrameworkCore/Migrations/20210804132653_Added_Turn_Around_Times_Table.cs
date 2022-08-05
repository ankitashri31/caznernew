using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_Turn_Around_Times_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TurnAroundTime",
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
                    Time = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurnAroundTime", x => x.Id);
                });

            migrationBuilder.AddColumn<long>(
               name: "TurnAroundTimeId",
               table: "ProductMaster",
               nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaster_TurnAroundTimeId",
                table: "ProductMaster",
                column: "TurnAroundTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMaster_TurnAroundTime_TurnAroundTimeId",
                table: "ProductMaster",
                column: "TurnAroundTimeId",
                principalTable: "TurnAroundTime",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TurnAroundTime");

            migrationBuilder.DropForeignKey(
             name: "FK_ProductMaster_TurnAroundTime_TurnAroundTimeId",
             table: "ProductMaster");

            migrationBuilder.DropIndex(
                name: "IX_ProductMaster_TurnAroundTimeId",
                table: "ProductMaster");

            migrationBuilder.DropColumn(
                name: "TurnAroundTimeId",
                table: "ProductMaster");
        }
    }
}
