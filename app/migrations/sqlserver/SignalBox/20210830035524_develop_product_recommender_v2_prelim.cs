using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_product_recommender_v2_prelim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvokationLogEntry_ParameterSetRecommenders_ParameterSetRecommenderId",
                table: "InvokationLogEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterParameterSetRecommender_ParameterSetRecommenders_ParameterSetRecommendersId",
                table: "ParameterParameterSetRecommender");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_ParameterSetRecommenders_RecommenderId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommenders_ModelRegistrations_ModelRegistrationId",
                table: "ParameterSetRecommenders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationCorrelators_ParameterSetRecommenders_ParameterSetRecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommenderTargetVariableValue_ParameterSetRecommenders_ParameterSetRecommenderId",
                table: "RecommenderTargetVariableValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParameterSetRecommenders",
                table: "ParameterSetRecommenders");

            migrationBuilder.RenameTable(
                name: "ParameterSetRecommenders",
                newName: "Recommenders");

            migrationBuilder.RenameIndex(
                name: "IX_ParameterSetRecommenders_ModelRegistrationId",
                table: "Recommenders",
                newName: "IX_Recommenders_ModelRegistrationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recommenders",
                table: "Recommenders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvokationLogEntry_Recommenders_ParameterSetRecommenderId",
                table: "InvokationLogEntry",
                column: "ParameterSetRecommenderId",
                principalTable: "Recommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterParameterSetRecommender_Recommenders_ParameterSetRecommendersId",
                table: "ParameterParameterSetRecommender",
                column: "ParameterSetRecommendersId",
                principalTable: "Recommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_Recommenders_RecommenderId",
                table: "ParameterSetRecommendations",
                column: "RecommenderId",
                principalTable: "Recommenders",
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
                name: "FK_Recommenders_ModelRegistrations_ModelRegistrationId",
                table: "Recommenders",
                column: "ModelRegistrationId",
                principalTable: "ModelRegistrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommenderTargetVariableValue_Recommenders_ParameterSetRecommenderId",
                table: "RecommenderTargetVariableValue",
                column: "ParameterSetRecommenderId",
                principalTable: "Recommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvokationLogEntry_Recommenders_ParameterSetRecommenderId",
                table: "InvokationLogEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterParameterSetRecommender_Recommenders_ParameterSetRecommendersId",
                table: "ParameterParameterSetRecommender");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_Recommenders_RecommenderId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationCorrelators_Recommenders_ParameterSetRecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_ModelRegistrations_ModelRegistrationId",
                table: "Recommenders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommenderTargetVariableValue_Recommenders_ParameterSetRecommenderId",
                table: "RecommenderTargetVariableValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recommenders",
                table: "Recommenders");

            migrationBuilder.RenameTable(
                name: "Recommenders",
                newName: "ParameterSetRecommenders");

            migrationBuilder.RenameIndex(
                name: "IX_Recommenders_ModelRegistrationId",
                table: "ParameterSetRecommenders",
                newName: "IX_ParameterSetRecommenders_ModelRegistrationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParameterSetRecommenders",
                table: "ParameterSetRecommenders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvokationLogEntry_ParameterSetRecommenders_ParameterSetRecommenderId",
                table: "InvokationLogEntry",
                column: "ParameterSetRecommenderId",
                principalTable: "ParameterSetRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterParameterSetRecommender_ParameterSetRecommenders_ParameterSetRecommendersId",
                table: "ParameterParameterSetRecommender",
                column: "ParameterSetRecommendersId",
                principalTable: "ParameterSetRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_ParameterSetRecommenders_RecommenderId",
                table: "ParameterSetRecommendations",
                column: "RecommenderId",
                principalTable: "ParameterSetRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommenders_ModelRegistrations_ModelRegistrationId",
                table: "ParameterSetRecommenders",
                column: "ModelRegistrationId",
                principalTable: "ModelRegistrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationCorrelators_ParameterSetRecommenders_ParameterSetRecommenderId",
                table: "RecommendationCorrelators",
                column: "ParameterSetRecommenderId",
                principalTable: "ParameterSetRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommenderTargetVariableValue_ParameterSetRecommenders_ParameterSetRecommenderId",
                table: "RecommenderTargetVariableValue",
                column: "ParameterSetRecommenderId",
                principalTable: "ParameterSetRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
