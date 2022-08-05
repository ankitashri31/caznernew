using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Updated_userBusinesssetting_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsStripeAccount",
                table: "UserBankDetails",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPayPalAccount",
                table: "UserBankDetails",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "IsStripeAccount",
                table: "UserBankDetails",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<long>(
                name: "IsPayPalAccount",
                table: "UserBankDetails",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
