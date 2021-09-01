using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_product_recommender_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvokationLogEntry_ProductRecommenders_ProductRecommenderId",
                table: "InvokationLogEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_InvokationLogEntry_Recommenders_ParameterSetRecommenderId",
                table: "InvokationLogEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductRecommender_ProductRecommenders_ProductRecommendersId",
                table: "ProductProductRecommender");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendations_ProductRecommenders_RecommenderId",
                table: "ProductRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationCorrelators_ProductRecommenders_ProductRecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationCorrelators_Recommenders_ParameterSetRecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommenderTargetVariableValue_ProductRecommenders_ProductRecommenderId",
                table: "RecommenderTargetVariableValue");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommenderTargetVariableValue_Recommenders_ParameterSetRecommenderId",
                table: "RecommenderTargetVariableValue");

            migrationBuilder.DropTable(
                name: "ProductRecommenders");

            migrationBuilder.DropIndex(
                name: "IX_RecommenderTargetVariableValue_ParameterSetRecommenderId",
                table: "RecommenderTargetVariableValue");

            migrationBuilder.DropIndex(
                name: "IX_RecommendationCorrelators_ParameterSetRecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropIndex(
                name: "IX_InvokationLogEntry_ParameterSetRecommenderId",
                table: "InvokationLogEntry");

            migrationBuilder.DropColumn(
                name: "ParameterSetRecommenderId",
                table: "RecommenderTargetVariableValue");

            migrationBuilder.DropColumn(
                name: "ParameterSetRecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropColumn(
                name: "ParameterSetRecommenderId",
                table: "InvokationLogEntry");

            migrationBuilder.RenameColumn(
                name: "ProductRecommenderId",
                table: "RecommenderTargetVariableValue",
                newName: "RecommenderEntityBaseId");

            migrationBuilder.RenameIndex(
                name: "IX_RecommenderTargetVariableValue_ProductRecommenderId",
                table: "RecommenderTargetVariableValue",
                newName: "IX_RecommenderTargetVariableValue_RecommenderEntityBaseId");

            migrationBuilder.RenameColumn(
                name: "ProductRecommenderId",
                table: "RecommendationCorrelators",
                newName: "RecommenderId");

            migrationBuilder.RenameIndex(
                name: "IX_RecommendationCorrelators_ProductRecommenderId",
                table: "RecommendationCorrelators",
                newName: "IX_RecommendationCorrelators_RecommenderId");

            migrationBuilder.RenameColumn(
                name: "ProductRecommenderId",
                table: "InvokationLogEntry",
                newName: "RecommenderEntityBaseId");

            migrationBuilder.RenameIndex(
                name: "IX_InvokationLogEntry_ProductRecommenderId",
                table: "InvokationLogEntry",
                newName: "IX_InvokationLogEntry_RecommenderEntityBaseId");

            migrationBuilder.AddColumn<long>(
                name: "DefaultProductId",
                table: "Recommenders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Recommenders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "ParameterSetRecommender");

            migrationBuilder.AddColumn<long>(
                name: "TouchpointId",
                table: "Recommenders",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recommenders_DefaultProductId",
                table: "Recommenders",
                column: "DefaultProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommenders_TouchpointId",
                table: "Recommenders",
                column: "TouchpointId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvokationLogEntry_Recommenders_RecommenderEntityBaseId",
                table: "InvokationLogEntry",
                column: "RecommenderEntityBaseId",
                principalTable: "Recommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductRecommender_Recommenders_ProductRecommendersId",
                table: "ProductProductRecommender",
                column: "ProductRecommendersId",
                principalTable: "Recommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendations_Recommenders_RecommenderId",
                table: "ProductRecommendations",
                column: "RecommenderId",
                principalTable: "Recommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationCorrelators_Recommenders_RecommenderId",
                table: "RecommendationCorrelators",
                column: "RecommenderId",
                principalTable: "Recommenders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_Products_DefaultProductId",
                table: "Recommenders",
                column: "DefaultProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_Touchpoints_TouchpointId",
                table: "Recommenders",
                column: "TouchpointId",
                principalTable: "Touchpoints",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvokationLogEntry_Recommenders_RecommenderEntityBaseId",
                table: "InvokationLogEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductRecommender_Recommenders_ProductRecommendersId",
                table: "ProductProductRecommender");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendations_Recommenders_RecommenderId",
                table: "ProductRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationCorrelators_Recommenders_RecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_Products_DefaultProductId",
                table: "Recommenders");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_Touchpoints_TouchpointId",
                table: "Recommenders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommenderTargetVariableValue_Recommenders_RecommenderEntityBaseId",
                table: "RecommenderTargetVariableValue");

            migrationBuilder.DropIndex(
                name: "IX_Recommenders_DefaultProductId",
                table: "Recommenders");

            migrationBuilder.DropIndex(
                name: "IX_Recommenders_TouchpointId",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "DefaultProductId",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "TouchpointId",
                table: "Recommenders");

            migrationBuilder.RenameColumn(
                name: "RecommenderEntityBaseId",
                table: "RecommenderTargetVariableValue",
                newName: "ProductRecommenderId");

            migrationBuilder.RenameIndex(
                name: "IX_RecommenderTargetVariableValue_RecommenderEntityBaseId",
                table: "RecommenderTargetVariableValue",
                newName: "IX_RecommenderTargetVariableValue_ProductRecommenderId");

            migrationBuilder.RenameColumn(
                name: "RecommenderId",
                table: "RecommendationCorrelators",
                newName: "ProductRecommenderId");

            migrationBuilder.RenameIndex(
                name: "IX_RecommendationCorrelators_RecommenderId",
                table: "RecommendationCorrelators",
                newName: "IX_RecommendationCorrelators_ProductRecommenderId");

            migrationBuilder.RenameColumn(
                name: "RecommenderEntityBaseId",
                table: "InvokationLogEntry",
                newName: "ProductRecommenderId");

            migrationBuilder.RenameIndex(
                name: "IX_InvokationLogEntry_RecommenderEntityBaseId",
                table: "InvokationLogEntry",
                newName: "IX_InvokationLogEntry_ProductRecommenderId");

            migrationBuilder.AddColumn<long>(
                name: "ParameterSetRecommenderId",
                table: "RecommenderTargetVariableValue",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParameterSetRecommenderId",
                table: "RecommendationCorrelators",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParameterSetRecommenderId",
                table: "InvokationLogEntry",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductRecommenders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DefaultProductId = table.Column<long>(type: "bigint", nullable: true),
                    ErrorHandling = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModelRegistrationId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TouchpointId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecommenders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRecommenders_ModelRegistrations_ModelRegistrationId",
                        column: x => x.ModelRegistrationId,
                        principalTable: "ModelRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRecommenders_Products_DefaultProductId",
                        column: x => x.DefaultProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductRecommenders_Touchpoints_TouchpointId",
                        column: x => x.TouchpointId,
                        principalTable: "Touchpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecommenderTargetVariableValue_ParameterSetRecommenderId",
                table: "RecommenderTargetVariableValue",
                column: "ParameterSetRecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationCorrelators_ParameterSetRecommenderId",
                table: "RecommendationCorrelators",
                column: "ParameterSetRecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_InvokationLogEntry_ParameterSetRecommenderId",
                table: "InvokationLogEntry",
                column: "ParameterSetRecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommenders_DefaultProductId",
                table: "ProductRecommenders",
                column: "DefaultProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommenders_ModelRegistrationId",
                table: "ProductRecommenders",
                column: "ModelRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommenders_TouchpointId",
                table: "ProductRecommenders",
                column: "TouchpointId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvokationLogEntry_ProductRecommenders_ProductRecommenderId",
                table: "InvokationLogEntry",
                column: "ProductRecommenderId",
                principalTable: "ProductRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvokationLogEntry_Recommenders_ParameterSetRecommenderId",
                table: "InvokationLogEntry",
                column: "ParameterSetRecommenderId",
                principalTable: "Recommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductRecommender_ProductRecommenders_ProductRecommendersId",
                table: "ProductProductRecommender",
                column: "ProductRecommendersId",
                principalTable: "ProductRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendations_ProductRecommenders_RecommenderId",
                table: "ProductRecommendations",
                column: "RecommenderId",
                principalTable: "ProductRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationCorrelators_ProductRecommenders_ProductRecommenderId",
                table: "RecommendationCorrelators",
                column: "ProductRecommenderId",
                principalTable: "ProductRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationCorrelators_Recommenders_ParameterSetRecommenderId",
                table: "RecommendationCorrelators",
                column: "ParameterSetRecommenderId",
                principalTable: "Recommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommenderTargetVariableValue_ProductRecommenders_ProductRecommenderId",
                table: "RecommenderTargetVariableValue",
                column: "ProductRecommenderId",
                principalTable: "ProductRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommenderTargetVariableValue_Recommenders_ParameterSetRecommenderId",
                table: "RecommenderTargetVariableValue",
                column: "ParameterSetRecommenderId",
                principalTable: "Recommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
