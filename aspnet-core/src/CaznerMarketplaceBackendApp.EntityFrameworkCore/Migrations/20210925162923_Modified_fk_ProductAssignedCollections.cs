using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_fk_ProductAssignedCollections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
           name: "FK_ProductAssignedCollections_ProductCollectionMaster_CollectionId",
           table: "ProductAssignedCollections");

            migrationBuilder.AddForeignKey(
        name: "FK_ProductAssignedCollections_CollectionMaster_CollectionId",
        table: "ProductAssignedCollections",
        column: "CollectionId",
        principalTable: "CollectionMaster",
        principalColumn: "Id",
        onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
          name: "FK_ProductAssignedCollections_CollectionMaster_CollectionId",
          table: "ProductAssignedCollections");



            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedCollections_ProductCollectionMaster_CollectionId",
                table: "ProductAssignedCollections",
                column: "CollectionId",
                principalTable: "ProductCollectionMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
