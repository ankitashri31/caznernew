using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_StoreMaster_StoreOpeningTiming_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreMaster",
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
                    TenantId = table.Column<int>(nullable: false),
                    StoreName = table.Column<string>(nullable: true),
                    StoreLocation = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreOpeningTimings",
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
                    TenantId = table.Column<int>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    StoreMasterId = table.Column<long>(nullable: false),
                    WeekDay = table.Column<string>(nullable: true),
                    OpeningTime = table.Column<string>(nullable: true),
                    ClosingTime = table.Column<string>(nullable: true),
                    IsCompleteDay = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreOpeningTimings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreOpeningTimings_StoreMaster_StoreMasterId",
                        column: x => x.StoreMasterId,
                        principalTable: "StoreMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreOpeningTimings_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreOpeningTimings_StoreMasterId",
                table: "StoreOpeningTimings",
                column: "StoreMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreOpeningTimings_UserId",
                table: "StoreOpeningTimings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreOpeningTimings");

            migrationBuilder.DropTable(
                name: "StoreMaster");
        }
    }
}
