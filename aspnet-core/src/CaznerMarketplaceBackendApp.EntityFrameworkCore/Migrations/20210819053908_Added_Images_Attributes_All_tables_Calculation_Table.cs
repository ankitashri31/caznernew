using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_Images_Attributes_All_tables_Calculation_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ext",
                table: "CollectionMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CollectionMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "CollectionMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CollectionMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "CollectionMaster",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TypeAttributeId",
                table: "CollectionCalculations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ext",
                table: "CategoryMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CategoryMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "CategoryMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CategoryMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "CategoryMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ext",
                table: "CategoryGroupMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CategoryGroupMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "CategoryGroupMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CategoryGroupMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "CategoryGroupMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CalculationTypeAttributes",
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
                    TypeId = table.Column<long>(nullable: false),
                    TypeMatchId = table.Column<long>(nullable: false),
                    IsAssigned = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationTypeAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculationTypeAttributes_CalculationTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CalculationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalculationTypeAttributes_CalculationTypeTags_TypeMatchId",
                        column: x => x.TypeMatchId,
                        principalTable: "CalculationTypeTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCalculations_TypeAttributeId",
                table: "CollectionCalculations",
                column: "TypeAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationTypeAttributes_TypeId",
                table: "CalculationTypeAttributes",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationTypeAttributes_TypeMatchId",
                table: "CalculationTypeAttributes",
                column: "TypeMatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionCalculations_CalculationTypeAttributes_TypeAttributeId",
                table: "CollectionCalculations",
                column: "TypeAttributeId",
                principalTable: "CalculationTypeAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionCalculations_CalculationTypeAttributes_TypeAttributeId",
                table: "CollectionCalculations");

            migrationBuilder.DropTable(
                name: "CalculationTypeAttributes");

            migrationBuilder.DropIndex(
                name: "IX_CollectionCalculations_TypeAttributeId",
                table: "CollectionCalculations");

            migrationBuilder.DropColumn(
                name: "Ext",
                table: "CollectionMaster");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CollectionMaster");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "CollectionMaster");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CollectionMaster");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "CollectionMaster");

            migrationBuilder.DropColumn(
                name: "TypeAttributeId",
                table: "CollectionCalculations");

            migrationBuilder.DropColumn(
                name: "Ext",
                table: "CategoryMaster");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CategoryMaster");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "CategoryMaster");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CategoryMaster");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "CategoryMaster");

            migrationBuilder.DropColumn(
                name: "Ext",
                table: "CategoryGroupMaster");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CategoryGroupMaster");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "CategoryGroupMaster");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CategoryGroupMaster");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "CategoryGroupMaster");
        }
    }
}
