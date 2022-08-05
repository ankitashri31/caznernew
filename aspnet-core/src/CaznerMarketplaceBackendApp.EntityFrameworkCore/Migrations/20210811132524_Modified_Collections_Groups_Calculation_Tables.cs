using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Modified_Collections_Groups_Calculation_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryMaster_CategoryGroupMaster_CategoryGroupId",
                table: "CategoryMaster");

            migrationBuilder.DropIndex(
                name: "IX_CategoryMaster_CategoryGroupId",
                table: "CategoryMaster");

            migrationBuilder.DropColumn(
                name: "CategoryGroupId",
                table: "CategoryMaster");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CollectionMaster",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsManualCalculation",
                table: "CollectionMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMatchAnyCondition",
                table: "CollectionMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeoEnabled",
                table: "CollectionMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "CollectionMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoPageTitle",
                table: "CollectionMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoUrl",
                table: "CollectionMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CalculationTypes",
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
                    Type = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalculationTypeTags",
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
                    TypeTitle = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationTypeTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryGroups",
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
                    CategoryGroupId = table.Column<long>(nullable: false),
                    CategoryMasterId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryGroups_CategoryGroupMaster_CategoryGroupId",
                        column: x => x.CategoryGroupId,
                        principalTable: "CategoryGroupMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryGroups_CategoryMaster_CategoryMasterId",
                        column: x => x.CategoryMasterId,
                        principalTable: "CategoryMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionCalculations",
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
                    CategoryMasterId = table.Column<long>(nullable: true),
                    CollectionId = table.Column<long>(nullable: false),
                    TypeMatchId = table.Column<long>(nullable: true),
                    TypeId = table.Column<long>(nullable: true),
                    EntityValue = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionCalculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionCalculations_CollectionMaster_CategoryMasterId",
                        column: x => x.CategoryMasterId,
                        principalTable: "CollectionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectionCalculations_CalculationTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CalculationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectionCalculations_CalculationTypeTags_TypeMatchId",
                        column: x => x.TypeMatchId,
                        principalTable: "CalculationTypeTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryGroups_CategoryGroupId",
                table: "CategoryGroups",
                column: "CategoryGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryGroups_CategoryMasterId",
                table: "CategoryGroups",
                column: "CategoryMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCalculations_CategoryMasterId",
                table: "CollectionCalculations",
                column: "CategoryMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCalculations_TypeId",
                table: "CollectionCalculations",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCalculations_TypeMatchId",
                table: "CollectionCalculations",
                column: "TypeMatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryGroups");

            migrationBuilder.DropTable(
                name: "CollectionCalculations");

            migrationBuilder.DropTable(
                name: "CalculationTypes");

            migrationBuilder.DropTable(
                name: "CalculationTypeTags");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CollectionMaster");

            migrationBuilder.DropColumn(
                name: "IsManualCalculation",
                table: "CollectionMaster");

            migrationBuilder.DropColumn(
                name: "IsMatchAnyCondition",
                table: "CollectionMaster");

            migrationBuilder.DropColumn(
                name: "IsSeoEnabled",
                table: "CollectionMaster");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "CollectionMaster");

            migrationBuilder.DropColumn(
                name: "SeoPageTitle",
                table: "CollectionMaster");

            migrationBuilder.DropColumn(
                name: "SeoUrl",
                table: "CollectionMaster");

            migrationBuilder.AddColumn<long>(
                name: "CategoryGroupId",
                table: "CategoryMaster",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryMaster_CategoryGroupId",
                table: "CategoryMaster",
                column: "CategoryGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryMaster_CategoryGroupMaster_CategoryGroupId",
                table: "CategoryMaster",
                column: "CategoryGroupId",
                principalTable: "CategoryGroupMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
