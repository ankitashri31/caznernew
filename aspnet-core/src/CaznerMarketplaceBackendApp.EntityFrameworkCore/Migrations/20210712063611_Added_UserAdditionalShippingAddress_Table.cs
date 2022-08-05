using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_UserAdditionalShippingAddress_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAdditionalShippingAddress",
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
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    BusinessEmail = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ShippingAddressId = table.Column<long>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    StreetAddress = table.Column<string>(nullable: true),
                    StateId = table.Column<long>(nullable: true),
                    CountryId = table.Column<long>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    IsPrimaryAddress = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdditionalShippingAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAdditionalShippingAddress_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAdditionalShippingAddress_UserShippingAddress_ShippingAddressId",
                        column: x => x.ShippingAddressId,
                        principalTable: "UserShippingAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAdditionalShippingAddress_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAdditionalShippingAddress_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalShippingAddress_CountryId",
                table: "UserAdditionalShippingAddress",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalShippingAddress_ShippingAddressId",
                table: "UserAdditionalShippingAddress",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalShippingAddress_StateId",
                table: "UserAdditionalShippingAddress",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdditionalShippingAddress_UserId",
                table: "UserAdditionalShippingAddress",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAdditionalShippingAddress");
        }
    }
}
