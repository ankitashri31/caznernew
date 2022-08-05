using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ECatalogue_Table_SequenceNo_HideContactDetails_Cols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHideContactDetails",
                table: "UserBusinessSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SequenceNumber",
                table: "CategoryGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ECatalogueMaster",
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
                    Title = table.Column<string>(nullable: true),
                    CatalogueUrl = table.Column<string>(nullable: true),
                    Ext = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECatalogueMaster", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ECatalogueMaster");

            migrationBuilder.DropColumn(
                name: "IsHideContactDetails",
                table: "UserBusinessSettings");

            migrationBuilder.DropColumn(
                name: "SequenceNumber",
                table: "CategoryGroups");
        }
    }
}
