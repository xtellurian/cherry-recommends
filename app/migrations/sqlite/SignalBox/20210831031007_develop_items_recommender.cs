using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_items_recommender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductRecommender_Products_ProductsId",
                table: "ProductProductRecommender");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendations_Products_ProductId",
                table: "ProductRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_Products_DefaultProductId",
                table: "Recommenders");

            migrationBuilder.DropTable(
                name: "ExperimentOffer");

            migrationBuilder.DropTable(
                name: "Iteration");

            migrationBuilder.DropTable(
                name: "OfferOfferRecommendation");

            migrationBuilder.DropTable(
                name: "PresentationOutcomes");

            migrationBuilder.DropTable(
                name: "Experiments");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Recommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "RecommendableItems");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CommonId",
                table: "RecommendableItems",
                newName: "IX_RecommendableItems_CommonId");

            migrationBuilder.AddColumn<long>(
                name: "DefaultItemId",
                table: "Recommenders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfItemsToRecommend",
                table: "Recommenders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "RecommendableItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "Product");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecommendableItems",
                table: "RecommendableItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ItemsRecommendations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecommenderId = table.Column<long>(type: "INTEGER", nullable: true),
                    Scores = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    RecommenderType = table.Column<string>(type: "TEXT", nullable: true),
                    TrackedUserId = table.Column<long>(type: "INTEGER", nullable: true),
                    RecommendationCorrelatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    Version = table.Column<string>(type: "TEXT", nullable: true),
                    ModelInput = table.Column<string>(type: "TEXT", nullable: true),
                    ModelInputType = table.Column<string>(type: "TEXT", nullable: true),
                    ModelOutput = table.Column<string>(type: "TEXT", nullable: true),
                    ModelOutputType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsRecommendations_RecommendationCorrelators_RecommendationCorrelatorId",
                        column: x => x.RecommendationCorrelatorId,
                        principalTable: "RecommendationCorrelators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemsRecommendations_Recommenders_RecommenderId",
                        column: x => x.RecommenderId,
                        principalTable: "Recommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ItemsRecommendations_TrackedUsers_TrackedUserId",
                        column: x => x.TrackedUserId,
                        principalTable: "TrackedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemsRecommenderRecommendableItem",
                columns: table => new
                {
                    ItemsId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecommendersId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsRecommenderRecommendableItem", x => new { x.ItemsId, x.RecommendersId });
                    table.ForeignKey(
                        name: "FK_ItemsRecommenderRecommendableItem_RecommendableItems_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "RecommendableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemsRecommenderRecommendableItem_Recommenders_RecommendersId",
                        column: x => x.RecommendersId,
                        principalTable: "Recommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemsRecommendationRecommendableItem",
                columns: table => new
                {
                    ItemsId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecommendationsId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsRecommendationRecommendableItem", x => new { x.ItemsId, x.RecommendationsId });
                    table.ForeignKey(
                        name: "FK_ItemsRecommendationRecommendableItem_ItemsRecommendations_RecommendationsId",
                        column: x => x.RecommendationsId,
                        principalTable: "ItemsRecommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemsRecommendationRecommendableItem_RecommendableItems_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "RecommendableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recommenders_DefaultItemId",
                table: "Recommenders",
                column: "DefaultItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsRecommendationRecommendableItem_RecommendationsId",
                table: "ItemsRecommendationRecommendableItem",
                column: "RecommendationsId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsRecommendations_RecommendationCorrelatorId",
                table: "ItemsRecommendations",
                column: "RecommendationCorrelatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsRecommendations_RecommenderId",
                table: "ItemsRecommendations",
                column: "RecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsRecommendations_TrackedUserId",
                table: "ItemsRecommendations",
                column: "TrackedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsRecommenderRecommendableItem_RecommendersId",
                table: "ItemsRecommenderRecommendableItem",
                column: "RecommendersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductRecommender_RecommendableItems_ProductsId",
                table: "ProductProductRecommender",
                column: "ProductsId",
                principalTable: "RecommendableItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendations_RecommendableItems_ProductId",
                table: "ProductRecommendations",
                column: "ProductId",
                principalTable: "RecommendableItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_RecommendableItems_DefaultItemId",
                table: "Recommenders",
                column: "DefaultItemId",
                principalTable: "RecommendableItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_RecommendableItems_DefaultProductId",
                table: "Recommenders",
                column: "DefaultProductId",
                principalTable: "RecommendableItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductRecommender_RecommendableItems_ProductsId",
                table: "ProductProductRecommender");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendations_RecommendableItems_ProductId",
                table: "ProductRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_RecommendableItems_DefaultItemId",
                table: "Recommenders");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_RecommendableItems_DefaultProductId",
                table: "Recommenders");

            migrationBuilder.DropTable(
                name: "ItemsRecommendationRecommendableItem");

            migrationBuilder.DropTable(
                name: "ItemsRecommenderRecommendableItem");

            migrationBuilder.DropTable(
                name: "ItemsRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_Recommenders_DefaultItemId",
                table: "Recommenders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecommendableItems",
                table: "RecommendableItems");

            migrationBuilder.DropColumn(
                name: "DefaultItemId",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "NumberOfItemsToRecommend",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "RecommendableItems");

            migrationBuilder.RenameTable(
                name: "RecommendableItems",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_RecommendableItems_CommonId",
                table: "Products",
                newName: "IX_Products_CommonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Experiments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConcurrentOffers = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cost = table.Column<double>(type: "REAL", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    DiscountCode = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommonUserId = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ExperimentId = table.Column<long>(type: "INTEGER", nullable: false),
                    Features = table.Column<string>(type: "TEXT", nullable: true),
                    IterationId = table.Column<string>(type: "TEXT", nullable: true),
                    IterationOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModelInput = table.Column<string>(type: "TEXT", nullable: true),
                    ModelInputType = table.Column<string>(type: "TEXT", nullable: true),
                    ModelOutput = table.Column<string>(type: "TEXT", nullable: true),
                    ModelOutputType = table.Column<string>(type: "TEXT", nullable: true),
                    RecommendationCorrelatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    RecommenderType = table.Column<string>(type: "TEXT", nullable: true),
                    TrackedUserId = table.Column<long>(type: "INTEGER", nullable: true),
                    Version = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recommendations_RecommendationCorrelators_RecommendationCorrelatorId",
                        column: x => x.RecommendationCorrelatorId,
                        principalTable: "RecommendationCorrelators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recommendations_TrackedUsers_TrackedUserId",
                        column: x => x.TrackedUserId,
                        principalTable: "TrackedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Iteration",
                columns: table => new
                {
                    ExperimentId = table.Column<long>(type: "INTEGER", nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iteration", x => new { x.ExperimentId, x.Id });
                    table.ForeignKey(
                        name: "FK_Iteration_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentOffer",
                columns: table => new
                {
                    ExperimentsId = table.Column<long>(type: "INTEGER", nullable: false),
                    OffersId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentOffer", x => new { x.ExperimentsId, x.OffersId });
                    table.ForeignKey(
                        name: "FK_ExperimentOffer_Experiments_ExperimentsId",
                        column: x => x.ExperimentsId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentOffer_Offers_OffersId",
                        column: x => x.OffersId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferOfferRecommendation",
                columns: table => new
                {
                    OffersId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecommendationsId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferOfferRecommendation", x => new { x.OffersId, x.RecommendationsId });
                    table.ForeignKey(
                        name: "FK_OfferOfferRecommendation_Offers_OffersId",
                        column: x => x.OffersId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferOfferRecommendation_Recommendations_RecommendationsId",
                        column: x => x.RecommendationsId,
                        principalTable: "Recommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PresentationOutcomes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ExperimentId = table.Column<long>(type: "INTEGER", nullable: true),
                    IterationId = table.Column<string>(type: "TEXT", nullable: true),
                    IterationOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    OfferId = table.Column<long>(type: "INTEGER", nullable: true),
                    Outcome = table.Column<string>(type: "TEXT", nullable: true),
                    RecommendationId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationOutcomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentationOutcomes_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresentationOutcomes_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresentationOutcomes_Recommendations_RecommendationId",
                        column: x => x.RecommendationId,
                        principalTable: "Recommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentOffer_OffersId",
                table: "ExperimentOffer",
                column: "OffersId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferOfferRecommendation_RecommendationsId",
                table: "OfferOfferRecommendation",
                column: "RecommendationsId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationOutcomes_ExperimentId",
                table: "PresentationOutcomes",
                column: "ExperimentId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationOutcomes_OfferId",
                table: "PresentationOutcomes",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationOutcomes_RecommendationId",
                table: "PresentationOutcomes",
                column: "RecommendationId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_RecommendationCorrelatorId",
                table: "Recommendations",
                column: "RecommendationCorrelatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_TrackedUserId",
                table: "Recommendations",
                column: "TrackedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductRecommender_Products_ProductsId",
                table: "ProductProductRecommender",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendations_Products_ProductId",
                table: "ProductRecommendations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_Products_DefaultProductId",
                table: "Recommenders",
                column: "DefaultProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
