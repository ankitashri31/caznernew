using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_AssignedSubCategories_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedSubCategories_SubCategoryMaster_SubCategoryId",
                table: "ProductAssignedSubCategories");

            migrationBuilder.DropTable(
                name: "ProductAssignedCategories");

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "ProductAssignedSubCategories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductAssignedCategoryMaster",
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
                    ProductId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssignedCategoryMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAssignedCategoryMaster_CategoryGroupMaster_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryGroupMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAssignedCategoryMaster_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAssignedSubSubCategories",
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
                    SubSubCategoryId = table.Column<long>(nullable: false),
                    SubCategoryId = table.Column<long>(nullable: true),
                    ProductId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssignedSubSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAssignedSubSubCategories_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAssignedSubSubCategories_CategoryMaster_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "CategoryMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductAssignedSubSubCategories_SubCategoryMaster_SubSubCategoryId",
                        column: x => x.SubSubCategoryId,
                        principalTable: "SubCategoryMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedSubCategories_CategoryId",
                table: "ProductAssignedSubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedCategoryMaster_CategoryId",
                table: "ProductAssignedCategoryMaster",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedCategoryMaster_ProductId",
                table: "ProductAssignedCategoryMaster",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedSubSubCategories_ProductId",
                table: "ProductAssignedSubSubCategories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedSubSubCategories_SubCategoryId",
                table: "ProductAssignedSubSubCategories",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedSubSubCategories_SubSubCategoryId",
                table: "ProductAssignedSubSubCategories",
                column: "SubSubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedSubCategories_CategoryGroupMaster_CategoryId",
                table: "ProductAssignedSubCategories",
                column: "CategoryId",
                principalTable: "CategoryGroupMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedSubCategories_CategoryMaster_SubCategoryId",
                table: "ProductAssignedSubCategories",
                column: "SubCategoryId",
                principalTable: "CategoryMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedSubCategories_CategoryGroupMaster_CategoryId",
                table: "ProductAssignedSubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAssignedSubCategories_CategoryMaster_SubCategoryId",
                table: "ProductAssignedSubCategories");

            migrationBuilder.DropTable(
                name: "ProductAssignedCategoryMaster");

            migrationBuilder.DropTable(
                name: "ProductAssignedSubSubCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductAssignedSubCategories_CategoryId",
                table: "ProductAssignedSubCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ProductAssignedSubCategories");

            migrationBuilder.CreateTable(
                name: "ProductAssignedCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssignedCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAssignedCategories_CategoryMaster_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAssignedCategories_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedCategories_CategoryId",
                table: "ProductAssignedCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedCategories_ProductId",
                table: "ProductAssignedCategories",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAssignedSubCategories_SubCategoryMaster_SubCategoryId",
                table: "ProductAssignedSubCategories",
                column: "SubCategoryId",
                principalTable: "SubCategoryMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
