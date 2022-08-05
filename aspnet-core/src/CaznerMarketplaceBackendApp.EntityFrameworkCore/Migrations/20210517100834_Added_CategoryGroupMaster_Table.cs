using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_CategoryGroupMaster_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryGroupId",
                table: "CategoryMaster",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "CategoryGroupMaster",
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
                    GroupTitle = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGroupMaster", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryMaster_CategoryGroupId",
                table: "CategoryMaster",
                column: "CategoryGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryMaster_CategoryGroupMaster_CategoryGroupId",
                table: "CategoryMaster",
                column: "CategoryGroupId",
                principalTable: "CategoryGroupMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryMaster_CategoryGroupMaster_CategoryGroupId",
                table: "CategoryMaster");

            migrationBuilder.DropTable(
                name: "CategoryGroupMaster");

            migrationBuilder.DropIndex(
                name: "IX_CategoryMaster_CategoryGroupId",
                table: "CategoryMaster");

            migrationBuilder.DropColumn(
                name: "CategoryGroupId",
                table: "CategoryMaster");
        }
    }
}
