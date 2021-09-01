using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_items_recommender_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_ModelRegistrations_ModelRegistrationId",
                table: "Recommenders");

            migrationBuilder.AddColumn<long>(
                name: "ModelRegistrationId1",
                table: "RecommendationCorrelators",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationCorrelators_ModelRegistrationId1",
                table: "RecommendationCorrelators",
                column: "ModelRegistrationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationCorrelators_ModelRegistrations_ModelRegistrationId1",
                table: "RecommendationCorrelators",
                column: "ModelRegistrationId1",
                principalTable: "ModelRegistrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_ModelRegistrations_ModelRegistrationId",
                table: "Recommenders",
                column: "ModelRegistrationId",
                principalTable: "ModelRegistrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationCorrelators_ModelRegistrations_ModelRegistrationId1",
                table: "RecommendationCorrelators");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_ModelRegistrations_ModelRegistrationId",
                table: "Recommenders");

            migrationBuilder.DropIndex(
                name: "IX_RecommendationCorrelators_ModelRegistrationId1",
                table: "RecommendationCorrelators");

            migrationBuilder.DropColumn(
                name: "ModelRegistrationId1",
                table: "RecommendationCorrelators");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_ModelRegistrations_ModelRegistrationId",
                table: "Recommenders",
                column: "ModelRegistrationId",
                principalTable: "ModelRegistrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
