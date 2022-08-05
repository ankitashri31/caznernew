using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_ArtworkMaster_ArtworkImages_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtWorkMaster",
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
                    ArtworkUniqueId = table.Column<string>(nullable: true),
                    ArtworkSKU = table.Column<string>(nullable: true),
                    ArtworkFeeTitle = table.Column<string>(nullable: true),
                    ArtworkDescription = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    HandlingCharge = table.Column<decimal>(nullable: false),
                    ApprovalDescription = table.Column<string>(nullable: true),
                    ArtworkNote = table.Column<string>(nullable: true),
                    IsEnableForMockups = table.Column<bool>(nullable: false),
                    MockupSKU = table.Column<string>(nullable: true),
                    MockupUniqueId = table.Column<string>(nullable: true),
                    MockupTitle = table.Column<string>(nullable: true),
                    MockupDescription = table.Column<string>(nullable: true),
                    MockupPrice = table.Column<decimal>(nullable: false),
                    MaxNumberOfMockUpCanOrder = table.Column<double>(nullable: false),
                    IsArtworkEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtWorkMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArtworkImages",
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
                    ImageName = table.Column<string>(nullable: true),
                    ArtworkId = table.Column<long>(nullable: false),
                    ImagePath = table.Column<string>(nullable: true),
                    ImageSize = table.Column<string>(nullable: true),
                    ImageFileData = table.Column<byte[]>(nullable: true),
                    ImageExtension = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Ext = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtworkImages_ArtWorkMaster_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "ArtWorkMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ArtworkMockupImages",
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
                    ImageName = table.Column<string>(nullable: true),
                    ArtworkId = table.Column<long>(nullable: false),
                    ImagePath = table.Column<string>(nullable: true),
                    ImageSize = table.Column<string>(nullable: true),
                    ImageFileData = table.Column<byte[]>(nullable: true),
                    ImageExtension = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Ext = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkMockupImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtworkMockupImages_ArtWorkMaster_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "ArtWorkMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkImages_ArtworkId",
                table: "ArtworkImages",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkMockupImages_ArtworkId",
                table: "ArtworkMockupImages",
                column: "ArtworkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtworkImages");

            migrationBuilder.DropTable(
                name: "ArtworkMockupImages");

            migrationBuilder.DropTable(
                name: "ArtWorkMaster");
        }
    }
}
