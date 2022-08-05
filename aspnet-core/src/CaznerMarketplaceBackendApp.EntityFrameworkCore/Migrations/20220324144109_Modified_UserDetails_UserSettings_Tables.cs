using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_UserDetails_UserSettings_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBusinessSettings_CurrencyMaster_CurrencyMasterId",
                table: "UserBusinessSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBusinessSettings_ProductSizeMaster_MeasurementId",
                table: "UserBusinessSettings");

            migrationBuilder.DropIndex(
                name: "IX_UserBusinessSettings_MeasurementId",
                table: "UserBusinessSettings");

            migrationBuilder.AddColumn<bool>(
                name: "IsRequestApprovedByAdmin",
                table: "UserDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "MeasurementId",
                table: "UserBusinessSettings",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CurrencyMasterId",
                table: "UserBusinessSettings",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBusinessSettings_CurrencyMaster_CurrencyMasterId",
                table: "UserBusinessSettings",
                column: "CurrencyMasterId",
                principalTable: "CurrencyMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBusinessSettings_CurrencyMaster_CurrencyMasterId",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "IsRequestApprovedByAdmin",
                table: "UserDetails");

            migrationBuilder.AlterColumn<long>(
                name: "MeasurementId",
                table: "UserBusinessSettings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CurrencyMasterId",
                table: "UserBusinessSettings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBusinessSettings_MeasurementId",
                table: "UserBusinessSettings",
                column: "MeasurementId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBusinessSettings_CurrencyMaster_CurrencyMasterId",
                table: "UserBusinessSettings",
                column: "CurrencyMasterId",
                principalTable: "CurrencyMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBusinessSettings_ProductSizeMaster_MeasurementId",
                table: "UserBusinessSettings",
                column: "MeasurementId",
                principalTable: "ProductSizeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
