using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ContactUs_Master_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactUsMaster",
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
                    Name = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    MessageSubject = table.Column<string>(nullable: true),
                    MessageBody = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    IsRepliedByAdmin = table.Column<bool>(nullable: false),
                    IsAnswerMailSent = table.Column<bool>(nullable: false),
                    AnswerMessageBody = table.Column<string>(nullable: true),
                    AnsweredByUserId = table.Column<long>(nullable: true),
                    AnsweredOn = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUsMaster", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactUsMaster");
        }
    }
}
