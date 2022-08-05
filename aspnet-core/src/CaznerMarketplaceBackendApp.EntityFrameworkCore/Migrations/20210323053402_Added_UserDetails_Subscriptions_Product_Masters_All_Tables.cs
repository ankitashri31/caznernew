using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaznerMarketplaceBackendApp.Migrations
{
    public partial class Added_UserDetails_Subscriptions_Product_Masters_All_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
              name: "Countries",
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
                  CountryName = table.Column<string>(nullable: true),
                  IsActive = table.Column<bool>(nullable: false)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Countries", x => x.Id);
              });

            migrationBuilder.CreateTable(
                name: "PositionMaster",
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
                    PositionName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
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
                    StateName = table.Column<string>(nullable: true),
                    CountryId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
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
                    UserId = table.Column<long>(nullable: false),
                    BusinessEmail = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    BusinessTradingName = table.Column<string>(nullable: true),
                    BusinessPhoneNumber = table.Column<string>(nullable: true),
                    RegistrationBusinessNumber = table.Column<string>(nullable: true),
                    PositionId = table.Column<long>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    WebsiteUrl = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    StreetAddress = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    StateId = table.Column<long>(nullable: true),
                    CountryId = table.Column<long>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    CompanyPrivateEmail = table.Column<string>(nullable: true),
                    TimeZoneId = table.Column<string>(nullable: true),
                    CompanyComments = table.Column<string>(nullable: true),
                    CompanyPhoneNumber = table.Column<string>(nullable: true),
                    CompanyPublicEmail = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDetails_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserDetails_PositionMaster_PositionId",
                        column: x => x.PositionId,
                        principalTable: "PositionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserDetails_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserDetails_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                table: "States",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_CountryId",
                table: "UserDetails",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_PositionId",
                table: "UserDetails",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_StateId",
                table: "UserDetails",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails",
                column: "UserId");



            migrationBuilder.CreateTable(
                name: "BrandingMethodAttributes",
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
                    AttributeName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingMethodAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandingMethodMaster",
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
                    MethodName = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingMethodMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductBrandingPositionTitleMaster",
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
                    ProductBrandingPositionTitleName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBrandingPositionTitleMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductBrandMaster",
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
                    ProductBrandName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBrandMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCollectionMaster",
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
                    ProductCollectionName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCollectionMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductColourMaster",
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
                    ProductColourName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColourMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductMaster",
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
                    ProductSKU = table.Column<string>(nullable: true),
                    ProductUniqueId = table.Column<string>(nullable: true),
                    ProductTitle = table.Column<string>(nullable: true),
                    ProductDescripition = table.Column<string>(nullable: true),
                    ShortDescripition = table.Column<string>(nullable: true),
                    Profit = table.Column<decimal>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    CostPerItem = table.Column<decimal>(nullable: false),
                    MarginIncreaseOnSalePrice = table.Column<decimal>(nullable: false),
                    SalePrice = table.Column<decimal>(nullable: false),
                    OnSale = table.Column<bool>(nullable: false),
                    UnitOfMeasure = table.Column<int>(nullable: false),
                    MinimumOrderQuantity = table.Column<int>(nullable: false),
                    DepositRequired = table.Column<decimal>(nullable: false),
                    PriceChargeTax = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsProductHasMultipleOptions = table.Column<bool>(nullable: false),
                    IsPhysicalProduct = table.Column<bool>(nullable: false),
                    ProductNotes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductMaterialMaster",
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
                    ProductMaterialName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMaterialMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductMediaImageTypeMaster",
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
                    ProductMediaImageTypeName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMediaImageTypeMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionsMaster",
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
                    OptionName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionsMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductSizeMaster",
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
                    ProductSizeName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSizeMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTagMaster",
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
                    ProductTagName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTagMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypeMaster",
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
                    ProductTypeName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypeMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsMasters",
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
                    Question = table.Column<string>(nullable: true),
                    UserType = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionFeaturesMaster",
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
                    FeatureTitle = table.Column<string>(nullable: true),
                    FeatureUniqueId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionFeaturesMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPlanMaster",
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
                    PlanTitle = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    CurrencyType = table.Column<int>(nullable: false),
                    CurrencySymbol = table.Column<int>(nullable: false),
                    UserType = table.Column<int>(nullable: false),
                    BillingTypeFrequency = table.Column<int>(nullable: false),
                    IsMostPopularPlan = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlanMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandingAttributeAssignment",
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
                    BrandingMethodId = table.Column<long>(nullable: false),
                    BrandingMethodAttributeId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandingAttributeAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrandingAttributeAssignment_BrandingMethodAttributes_BrandingMethodAttributeId",
                        column: x => x.BrandingMethodAttributeId,
                        principalTable: "BrandingMethodAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BrandingAttributeAssignment_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductandPackageDimension",
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
                    ProductHeight = table.Column<long>(nullable: false),
                    ProductWidth = table.Column<long>(nullable: false),
                    ProductLength = table.Column<long>(nullable: false),
                    UnitWeight = table.Column<long>(nullable: false),
                    CartonWeight = table.Column<long>(nullable: false),
                    CartonQuantity = table.Column<long>(nullable: false),
                    PackageHeight = table.Column<long>(nullable: false),
                    PackageWidth = table.Column<long>(nullable: false),
                    PackageLength = table.Column<long>(nullable: false),
                    UnitPerCarton = table.Column<long>(nullable: false),
                    ProductPackaging = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    StockkeepingUnit = table.Column<int>(nullable: false),
                    Barcode = table.Column<string>(nullable: true),
                    TotalNumberAvailable = table.Column<int>(nullable: false),
                    AlertRestockNumber = table.Column<int>(nullable: false),
                    IsTrackQuantity = table.Column<bool>(nullable: false),
                    IsStopSellingStockZero = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductandPackageDimension", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductandPackageDimension_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductAssignedBrands",
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
                    ProductBrandId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssignedBrands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAssignedBrands_ProductBrandMaster_ProductBrandId",
                        column: x => x.ProductBrandId,
                        principalTable: "ProductBrandMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductAssignedBrands_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductAssignedCollections",
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
                    ProductCollectionId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssignedCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAssignedCollections_ProductCollectionMaster_ProductCollectionId",
                        column: x => x.ProductCollectionId,
                        principalTable: "ProductCollectionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAssignedCollections_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAssignedVendors",
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
                    VendorUserId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssignedVendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAssignedVendors_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAssignedVendors_UserDetails_VendorUserId",
                        column: x => x.VendorUserId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductBranding",
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
                    ProductMethodId = table.Column<long>(nullable: false),
                    BrandingCounts = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBranding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBranding_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductBranding_BrandingMethodMaster_ProductMethodId",
                        column: x => x.ProductMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductBrandingPosition",
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
                    ProductId = table.Column<long>(nullable: true),
                    LayerNumber = table.Column<string>(nullable: true),
                    LayerTitle = table.Column<string>(nullable: true),
                    PostionMaxwidth = table.Column<int>(nullable: false),
                    PostionMaxHeight = table.Column<int>(nullable: false),
                    ImageName = table.Column<string>(nullable: true),
                    ImageFileData = table.Column<byte[]>(nullable: true),
                    ImageFileURL = table.Column<string>(nullable: true),
                    IsProductSubmissionDone = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBrandingPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBrandingPosition_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductDesignHub",
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
                    BrandingPositionTitle = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDesignHub", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDesignHub_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetails",
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
                    ProductTypeArray = table.Column<string>(nullable: true),
                    ProductBrandArray = table.Column<string>(nullable: true),
                    ProductCollectionArray = table.Column<string>(nullable: true),
                    ProductMaterialArray = table.Column<string>(nullable: true),
                    ProductTagArray = table.Column<string>(nullable: true),
                    ProductVendorArray = table.Column<string>(nullable: true),
                    ProductId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetails_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDimension",
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
                    ProductId = table.Column<long>(nullable: false),
                    Height = table.Column<long>(nullable: false),
                    Width = table.Column<long>(nullable: false),
                    Length = table.Column<long>(nullable: false),
                    UnitWeight = table.Column<long>(nullable: false),
                    CartonWeight = table.Column<long>(nullable: false),
                    CartonQuantity = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDimension", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDimension_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
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
                    ProductId = table.Column<long>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    ImageSize = table.Column<string>(nullable: true),
                    ImageFileData = table.Column<byte[]>(nullable: true),
                    ImageExtension = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsProductSubmissionDone = table.Column<bool>(nullable: false),
                    ProductImageGroupId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductInventory",
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
                    StockkeepingUnit = table.Column<int>(nullable: false),
                    Barcode = table.Column<string>(nullable: true),
                    TotalNumberAvailable = table.Column<int>(nullable: false),
                    AlertRestockNumber = table.Column<int>(nullable: false),
                    IsTrackQuantity = table.Column<bool>(nullable: false),
                    IsStopSellingStockZero = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInventory_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPackageDimension",
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
                    Height = table.Column<long>(nullable: false),
                    Width = table.Column<long>(nullable: false),
                    Length = table.Column<long>(nullable: false),
                    UnitPerCarton = table.Column<long>(nullable: false),
                    ProductPackaging = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPackageDimension", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPackageDimension_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPallet",
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
                    PalletWeigth = table.Column<long>(nullable: false),
                    CartonsPerPallet = table.Column<long>(nullable: false),
                    UnitsPerPallet = table.Column<long>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPallet_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductStockLocation",
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
                    WareHouseId = table.Column<long>(nullable: false),
                    QuantityAtLocation = table.Column<int>(nullable: false),
                    StockAlertQty = table.Column<int>(nullable: false),
                    LocationA = table.Column<string>(nullable: true),
                    LocationB = table.Column<string>(nullable: true),
                    LocationC = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStockLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStockLocation_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVolumeDiscountVariant",
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
                    QuantityFrom = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVolumeDiscountVariant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVolumeDiscountVariant_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAssignedMaterials",
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
                    ProductMaterialId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssignedMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAssignedMaterials_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAssignedMaterials_ProductMaterialMaster_ProductMaterialId",
                        column: x => x.ProductMaterialId,
                        principalTable: "ProductMaterialMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductMediaImages",
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
                    ImageName = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ProductId = table.Column<long>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    ImageSize = table.Column<string>(nullable: true),
                    ImageFileData = table.Column<byte[]>(nullable: true),
                    ProductMediaImageTypeId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsProductSubmissionDone = table.Column<bool>(nullable: false),
                    ProductMediaGroupId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMediaImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMediaImages_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductMediaImages_ProductMediaImageTypeMaster_ProductMediaImageTypeId",
                        column: x => x.ProductMediaImageTypeId,
                        principalTable: "ProductMediaImageTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductBulkUploadVariations",
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
                    productOptionId = table.Column<long>(nullable: false),
                    ProductOptionValue = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBulkUploadVariations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBulkUploadVariations_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductBulkUploadVariations_ProductOptionsMaster_productOptionId",
                        column: x => x.productOptionId,
                        principalTable: "ProductOptionsMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAssignedTags",
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
                    ProductTagId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssignedTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAssignedTags_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAssignedTags_ProductTagMaster_ProductTagId",
                        column: x => x.ProductTagId,
                        principalTable: "ProductTagMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAssignedTypes",
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
                    ProductTypeId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssignedTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAssignedTypes_ProductMaster_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAssignedTypes_ProductTypeMaster_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswersMasters",
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
                    QuestionId = table.Column<long>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswersMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswersMasters_QuestionsMasters_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QuestionsMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPlanFeatures",
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
                    SubscriptionPlanId = table.Column<long>(nullable: false),
                    SubscriptionFeatureId = table.Column<long>(nullable: false),
                    IsAccessAllowed = table.Column<bool>(nullable: true),
                    IsFreeTextExists = table.Column<bool>(nullable: false),
                    FreeText = table.Column<string>(nullable: true),
                    AllowedQuantity = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlanFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionPlanFeatures_SubscriptionFeaturesMaster_SubscriptionFeatureId",
                        column: x => x.SubscriptionFeatureId,
                        principalTable: "SubscriptionFeaturesMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscriptionPlanFeatures_SubscriptionPlanMaster_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "SubscriptionPlanMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscriptionPlan",
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
                    UserId = table.Column<long>(nullable: false),
                    SubscriptionPlanId = table.Column<long>(nullable: false),
                    SubscriptionStartDate = table.Column<DateTime>(nullable: true),
                    SubscriptionEndDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscriptionPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubscriptionPlan_SubscriptionPlanMaster_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "SubscriptionPlanMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSubscriptionPlan_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductMethodAttributeValues",
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
                    BrandingMethodId = table.Column<long>(nullable: false),
                    BrandingAttributeAssignmentId = table.Column<long>(nullable: false),
                    ProductMasterId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMethodAttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMethodAttributeValues_BrandingAttributeAssignment_BrandingAttributeAssignmentId",
                        column: x => x.BrandingAttributeAssignmentId,
                        principalTable: "BrandingAttributeAssignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductMethodAttributeValues_BrandingMethodMaster_BrandingMethodId",
                        column: x => x.BrandingMethodId,
                        principalTable: "BrandingMethodMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductMethodAttributeValues_ProductMaster_ProductMasterId",
                        column: x => x.ProductMasterId,
                        principalTable: "ProductMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswersMasters_QuestionId",
                table: "AnswersMasters",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingAttributeAssignment_BrandingMethodAttributeId",
                table: "BrandingAttributeAssignment",
                column: "BrandingMethodAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandingAttributeAssignment_BrandingMethodId",
                table: "BrandingAttributeAssignment",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductandPackageDimension_ProductId",
                table: "ProductandPackageDimension",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedBrands_ProductBrandId",
                table: "ProductAssignedBrands",
                column: "ProductBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedBrands_ProductId",
                table: "ProductAssignedBrands",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedCollections_ProductCollectionId",
                table: "ProductAssignedCollections",
                column: "ProductCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedCollections_ProductId",
                table: "ProductAssignedCollections",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedMaterials_ProductId",
                table: "ProductAssignedMaterials",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedMaterials_ProductMaterialId",
                table: "ProductAssignedMaterials",
                column: "ProductMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedTags_ProductId",
                table: "ProductAssignedTags",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedTags_ProductTagId",
                table: "ProductAssignedTags",
                column: "ProductTagId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedTypes_ProductId",
                table: "ProductAssignedTypes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedTypes_ProductTypeId",
                table: "ProductAssignedTypes",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedVendors_ProductId",
                table: "ProductAssignedVendors",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssignedVendors_VendorUserId",
                table: "ProductAssignedVendors",
                column: "VendorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBranding_ProductId",
                table: "ProductBranding",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBranding_ProductMethodId",
                table: "ProductBranding",
                column: "ProductMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrandingPosition_ProductId",
                table: "ProductBrandingPosition",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBulkUploadVariations_ProductId",
                table: "ProductBulkUploadVariations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBulkUploadVariations_productOptionId",
                table: "ProductBulkUploadVariations",
                column: "productOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDesignHub_ProductId",
                table: "ProductDesignHub",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductId",
                table: "ProductDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDimension_ProductId",
                table: "ProductDimension",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventory_ProductId",
                table: "ProductInventory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMediaImages_ProductId",
                table: "ProductMediaImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMediaImages_ProductMediaImageTypeId",
                table: "ProductMediaImages",
                column: "ProductMediaImageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMethodAttributeValues_BrandingAttributeAssignmentId",
                table: "ProductMethodAttributeValues",
                column: "BrandingAttributeAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMethodAttributeValues_BrandingMethodId",
                table: "ProductMethodAttributeValues",
                column: "BrandingMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMethodAttributeValues_ProductMasterId",
                table: "ProductMethodAttributeValues",
                column: "ProductMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPackageDimension_ProductId",
                table: "ProductPackageDimension",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPallet_ProductId",
                table: "ProductPallet",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStockLocation_ProductId",
                table: "ProductStockLocation",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVolumeDiscountVariant_ProductId",
                table: "ProductVolumeDiscountVariant",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlanFeatures_SubscriptionFeatureId",
                table: "SubscriptionPlanFeatures",
                column: "SubscriptionFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlanFeatures_SubscriptionPlanId",
                table: "SubscriptionPlanFeatures",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptionPlan_SubscriptionPlanId",
                table: "UserSubscriptionPlan",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptionPlan_UserId",
                table: "UserSubscriptionPlan",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "UserDetails");

            migrationBuilder.DropTable(
                name: "PositionMaster");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "AnswersMasters");

            migrationBuilder.DropTable(
                name: "ProductandPackageDimension");

            migrationBuilder.DropTable(
                name: "ProductAssignedBrands");

            migrationBuilder.DropTable(
                name: "ProductAssignedCollections");

            migrationBuilder.DropTable(
                name: "ProductAssignedMaterials");

            migrationBuilder.DropTable(
                name: "ProductAssignedTags");

            migrationBuilder.DropTable(
                name: "ProductAssignedTypes");

            migrationBuilder.DropTable(
                name: "ProductAssignedVendors");

            migrationBuilder.DropTable(
                name: "ProductBranding");

            migrationBuilder.DropTable(
                name: "ProductBrandingPosition");

            migrationBuilder.DropTable(
                name: "ProductBrandingPositionTitleMaster");

            migrationBuilder.DropTable(
                name: "ProductBulkUploadVariations");

            migrationBuilder.DropTable(
                name: "ProductColourMaster");

            migrationBuilder.DropTable(
                name: "ProductDesignHub");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "ProductDimension");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductInventory");

            migrationBuilder.DropTable(
                name: "ProductMediaImages");

            migrationBuilder.DropTable(
                name: "ProductMethodAttributeValues");

            migrationBuilder.DropTable(
                name: "ProductPackageDimension");

            migrationBuilder.DropTable(
                name: "ProductPallet");

            migrationBuilder.DropTable(
                name: "ProductSizeMaster");

            migrationBuilder.DropTable(
                name: "ProductStockLocation");

            migrationBuilder.DropTable(
                name: "ProductVolumeDiscountVariant");

            migrationBuilder.DropTable(
                name: "SubscriptionPlanFeatures");

            migrationBuilder.DropTable(
                name: "UserSubscriptionPlan");

            migrationBuilder.DropTable(
                name: "QuestionsMasters");

            migrationBuilder.DropTable(
                name: "ProductBrandMaster");

            migrationBuilder.DropTable(
                name: "ProductCollectionMaster");

            migrationBuilder.DropTable(
                name: "ProductMaterialMaster");

            migrationBuilder.DropTable(
                name: "ProductTagMaster");

            migrationBuilder.DropTable(
                name: "ProductTypeMaster");

            migrationBuilder.DropTable(
                name: "ProductOptionsMaster");

            migrationBuilder.DropTable(
                name: "ProductMediaImageTypeMaster");

            migrationBuilder.DropTable(
                name: "BrandingAttributeAssignment");

            migrationBuilder.DropTable(
                name: "ProductMaster");

            migrationBuilder.DropTable(
                name: "SubscriptionFeaturesMaster");

            migrationBuilder.DropTable(
                name: "SubscriptionPlanMaster");

            migrationBuilder.DropTable(
                name: "BrandingMethodAttributes");

            migrationBuilder.DropTable(
                name: "BrandingMethodMaster");
        }
    }
}
