using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_CompartmentBulder_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProductHasCompartmentBuilder",
                table: "ProductMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CompartmentVariantData",
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
                    ProductId = table.Column<long>(nullable: false),
                    Compartment = table.Column<string>(nullable: true),
                    CompartmentMasterIds = table.Column<string>(nullable: true),
                    SKU = table.Column<string>(nullable: true),
                    CompartmentTitle = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Ext = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    ImageFileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompartmentVariantData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompartmentVariantData_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductCompartmentBaseImages",
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
                    ProductId = table.Column<long>(nullable: false),
                    Ext = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    ImageFileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCompartmentBaseImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCompartmentBaseImages_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CompartmentOptionValues",
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
                    CompartmentVariantId = table.Column<long>(nullable: false),
                    OptionId = table.Column<long>(nullable: false),
                    CompartmentOptionValue = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompartmentOptionValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompartmentOptionValues_CompartmentVariantData_CompartmentVariantId",
                        column: x => x.CompartmentVariantId,
                        principalTable: "CompartmentVariantData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CompartmentOptionValues_ProductOptionsMaster_OptionId",
                        column: x => x.OptionId,
                        principalTable: "ProductOptionsMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompartmentOptionValues_CompartmentVariantId",
                table: "CompartmentOptionValues",
                column: "CompartmentVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_CompartmentOptionValues_OptionId",
                table: "CompartmentOptionValues",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompartmentVariantData_ProductId",
                table: "CompartmentVariantData",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompartmentBaseImages_ProductId",
                table: "ProductCompartmentBaseImages",
                column: "ProductId");

            //----------------------------------------//

            migrationBuilder.AddColumn<string>(
              name: "CompartmentSubTitle",
              table: "CompartmentVariantData",
              nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompartmentOptionValues");

            migrationBuilder.DropTable(
                name: "ProductCompartmentBaseImages");

            migrationBuilder.DropTable(
                name: "CompartmentVariantData");

            migrationBuilder.DropColumn(
                name: "IsProductHasCompartmentBuilder",
                table: "ProductMaster");

            //--------------------------------------//

            migrationBuilder.DropColumn(
             name: "CompartmentSubTitle",
             table: "CompartmentVariantData");
        }
    }
}
