using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_CollectionMaster_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectionName",
                table: "CategoryCollections");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "CategoryCollections");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "CategoryCollections");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CategoryCollections");

            migrationBuilder.AddColumn<long>(
                name: "CollectionId",
                table: "CategoryCollections",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "CollectionMaster",
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
                    CollectionName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionMaster", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCollections_CollectionId",
                table: "CategoryCollections",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCollections_CollectionMaster_CollectionId",
                table: "CategoryCollections",
                column: "CollectionId",
                principalTable: "CollectionMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCollections_CollectionMaster_CollectionId",
                table: "CategoryCollections");

            migrationBuilder.DropTable(
                name: "CollectionMaster");

            migrationBuilder.DropIndex(
                name: "IX_CategoryCollections_CollectionId",
                table: "CategoryCollections");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "CategoryCollections");

            migrationBuilder.AddColumn<string>(
                name: "CollectionName",
                table: "CategoryCollections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "CategoryCollections",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "CategoryCollections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CategoryCollections",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
