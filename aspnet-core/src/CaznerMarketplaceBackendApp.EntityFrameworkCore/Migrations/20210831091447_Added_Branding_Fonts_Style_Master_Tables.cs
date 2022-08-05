using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_Branding_Fonts_Style_Master_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniversalBrnadingArray",
                table: "BrandingMethodDetails");

            migrationBuilder.AddColumn<string>(
                name: "UniversalBrandingArray",
                table: "BrandingMethodDetails",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BrandingSpecificFontStyleMaster",
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
                    FontStyleTitle = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingSpecificFontStyleMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandingSpecificFontTypeFaceMaster",
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
                    FontTypeFaceTitle = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingSpecificFontTypeFaceMaster", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandingSpecificFontStyleMaster");

            migrationBuilder.DropTable(
                name: "BrandingSpecificFontTypeFaceMaster");

            migrationBuilder.DropColumn(
                name: "UniversalBrandingArray",
                table: "BrandingMethodDetails");

            migrationBuilder.AddColumn<string>(
                name: "UniversalBrnadingArray",
                table: "BrandingMethodDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
