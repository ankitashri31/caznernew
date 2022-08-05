using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_fk_Assignedvendors_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedVendors_UserDetails_VendorUserId",
                table: "ProductAssignedVendors");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedVendors_AbpUsers_VendorUserId",
                table: "ProductAssignedVendors",
                column: "VendorUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedVendors_AbpUsers_VendorUserId",
                table: "ProductAssignedVendors");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedVendors_UserDetails_VendorUserId",
                table: "ProductAssignedVendors",
                column: "VendorUserId",
                principalTable: "UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
