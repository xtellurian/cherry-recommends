using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class Remove_Products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_RecommendableItems_DefaultProductId",
                table: "Recommenders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommenderTargetVariableValue_Recommenders_RecommenderEntityBaseId",
                table: "RecommenderTargetVariableValue");

            migrationBuilder.DropTable(
                name: "ProductProductRecommender");

            migrationBuilder.DropTable(
                name: "ProductRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_RecommenderTargetVariableValue_RecommenderEntityBaseId",
                table: "RecommenderTargetVariableValue");

            migrationBuilder.DropIndex(
                name: "IX_Recommenders_DefaultProductId",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "RecommenderEntityBaseId",
                table: "RecommenderTargetVariableValue");

            migrationBuilder.DropColumn(
                name: "DefaultProductId",
                table: "Recommenders");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "RecommendableItems",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "Product",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Product");

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "ParameterSetRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "ItemsRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSetRecommendations_EnvironmentId",
                table: "ParameterSetRecommendations",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsRecommendations_EnvironmentId",
                table: "ItemsRecommendations",
                column: "EnvironmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsRecommendations_Environments_EnvironmentId",
                table: "ItemsRecommendations",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_Environments_EnvironmentId",
                table: "ParameterSetRecommendations",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemsRecommendations_Environments_EnvironmentId",
                table: "ItemsRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_Environments_EnvironmentId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_ParameterSetRecommendations_EnvironmentId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_ItemsRecommendations_EnvironmentId",
                table: "ItemsRecommendations");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "ItemsRecommendations");

            migrationBuilder.AddColumn<long>(
                name: "RecommenderEntityBaseId",
                table: "RecommenderTargetVariableValue",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DefaultProductId",
                table: "Recommenders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "RecommendableItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Product",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "Product");

            migrationBuilder.CreateTable(
                name: "ProductProductRecommender",
                columns: table => new
                {
                    ProductRecommendersId = table.Column<long>(type: "bigint", nullable: false),
                    ProductsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductRecommender", x => new { x.ProductRecommendersId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ProductProductRecommender_RecommendableItems_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "RecommendableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductRecommender_Recommenders_ProductRecommendersId",
                        column: x => x.ProductRecommendersId,
                        principalTable: "Recommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRecommendations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModelInput = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelInputType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelOutput = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelOutputType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    RecommendationCorrelatorId = table.Column<long>(type: "bigint", nullable: true),
                    RecommenderId = table.Column<long>(type: "bigint", nullable: true),
                    RecommenderType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackedUserId = table.Column<long>(type: "bigint", nullable: true),
                    Trigger = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRecommendations_RecommendableItems_ProductId",
                        column: x => x.ProductId,
                        principalTable: "RecommendableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRecommendations_RecommendationCorrelators_RecommendationCorrelatorId",
                        column: x => x.RecommendationCorrelatorId,
                        principalTable: "RecommendationCorrelators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRecommendations_Recommenders_RecommenderId",
                        column: x => x.RecommenderId,
                        principalTable: "Recommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductRecommendations_TrackedUsers_TrackedUserId",
                        column: x => x.TrackedUserId,
                        principalTable: "TrackedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecommenderTargetVariableValue_RecommenderEntityBaseId",
                table: "RecommenderTargetVariableValue",
                column: "RecommenderEntityBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommenders_DefaultProductId",
                table: "Recommenders",
                column: "DefaultProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductRecommender_ProductsId",
                table: "ProductProductRecommender",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_ProductId",
                table: "ProductRecommendations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_RecommendationCorrelatorId",
                table: "ProductRecommendations",
                column: "RecommendationCorrelatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_RecommenderId",
                table: "ProductRecommendations",
                column: "RecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_TrackedUserId",
                table: "ProductRecommendations",
                column: "TrackedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_RecommendableItems_DefaultProductId",
                table: "Recommenders",
                column: "DefaultProductId",
                principalTable: "RecommendableItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommenderTargetVariableValue_Recommenders_RecommenderEntityBaseId",
                table: "RecommenderTargetVariableValue",
                column: "RecommenderEntityBaseId",
                principalTable: "Recommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
