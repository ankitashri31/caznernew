using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_SubCategories_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubCategoryMaster",
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
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    Ext = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategoryMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategorySubCategories",
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
                    CategoryId = table.Column<long>(nullable: false),
                    SubCategoryId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategorySubCategories_CategoryMaster_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategorySubCategories_SubCategoryMaster_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategoryMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAssignedSubCategories",
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
                    SubCategoryId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssignedSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAssignedSubCategories_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAssignedSubCategories_SubCategoryMaster_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategoryMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategorySubCategories_CategoryId",
                table: "CategorySubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySubCategories_SubCategoryId",
                table: "CategorySubCategories",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedSubCategories_ProductId",
                table: "ProductAssignedSubCategories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedSubCategories_SubCategoryId",
                table: "ProductAssignedSubCategories",
                column: "SubCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategorySubCategories");

            migrationBuilder.DropTable(
                name: "ProductAssignedSubCategories");

            migrationBuilder.DropTable(
                name: "SubCategoryMaster");
        }
    }
}
