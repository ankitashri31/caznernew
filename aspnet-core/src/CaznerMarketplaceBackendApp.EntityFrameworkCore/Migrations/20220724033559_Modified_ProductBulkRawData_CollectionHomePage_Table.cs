using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_ProductBulkRawData_CollectionHomePage_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Groupcategory",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "Productcategory",
                table: "ProductBulkImportRawData");

            migrationBuilder.AddColumn<string>(
                name: "GroupCategory1",
                table: "ProductBulkImportRawData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupCategory2",
                table: "ProductBulkImportRawData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupCategory3",
                table: "ProductBulkImportRawData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubCategory1",
                table: "ProductBulkImportRawData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubCategory2",
                table: "ProductBulkImportRawData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubCategory3",
                table: "ProductBulkImportRawData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubSubCategory1",
                table: "ProductBulkImportRawData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubSubCategory2",
                table: "ProductBulkImportRawData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubSubCategory3",
                table: "ProductBulkImportRawData",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CollectionHomePage",
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
                    CollectionId = table.Column<long>(nullable: false),
                    NumberOfProducts = table.Column<int>(nullable: false),
                    SequenceNumber = table.Column<int>(nullable: false),
                    VideoUrl = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionHomePage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionHomePage_CollectionMaster_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "CollectionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectionHomePage_CollectionId",
                table: "CollectionHomePage",
                column: "CollectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectionHomePage");

            migrationBuilder.DropColumn(
                name: "GroupCategory1",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "GroupCategory2",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "GroupCategory3",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "SubCategory1",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "SubCategory2",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "SubCategory3",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "SubSubCategory1",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "SubSubCategory2",
                table: "ProductBulkImportRawData");

            migrationBuilder.DropColumn(
                name: "SubSubCategory3",
                table: "ProductBulkImportRawData");

            migrationBuilder.AddColumn<string>(
                name: "Groupcategory",
                table: "ProductBulkImportRawData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Productcategory",
                table: "ProductBulkImportRawData",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
