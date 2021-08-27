using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_revenue_charts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParameterSetRecommenderId",
                table: "RecommendationCorrelators",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductRecommenderId",
                table: "RecommendationCorrelators",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_RecommendationCorrelatorId",
                table: "TrackedUserEvents",
                column: "RecommendationCorrelatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_RecommendationCorrelatorId",
                table: "TrackedUserActions",
                column: "RecommendationCorrelatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationCorrelators_ParameterSetRecommenderId",
                table: "RecommendationCorrelators",
                column: "ParameterSetRecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationCorrelators_ProductRecommenderId",
                table: "RecommendationCorrelators",
                column: "ProductRecommenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationCorrelators_ParameterSetRecommenders_ParameterSetRecommenderId",
                table: "RecommendationCorrelators",
                column: "ParameterSetRecommenderId",
                principalTable: "ParameterSetRecommenders",
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
                name: "FK_TrackedUserActions_RecommendationCorrelators_RecommendationCorrelatorId",
                table: "TrackedUserActions",
                column: "RecommendationCorrelatorId",
                principalTable: "RecommendationCorrelators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUserEvents_RecommendationCorrelators_RecommendationCorrelatorId",
                table: "TrackedUserEvents",
                column: "RecommendationCorrelatorId",
                principalTable: "RecommendationCorrelators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationCorrelators_ParameterSetRecommenders_ParameterSetRecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationCorrelators_ProductRecommenders_ProductRecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUserActions_RecommendationCorrelators_RecommendationCorrelatorId",
                table: "TrackedUserActions");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUserEvents_RecommendationCorrelators_RecommendationCorrelatorId",
                table: "TrackedUserEvents");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUserEvents_RecommendationCorrelatorId",
                table: "TrackedUserEvents");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUserActions_RecommendationCorrelatorId",
                table: "TrackedUserActions");

            migrationBuilder.DropIndex(
                name: "IX_RecommendationCorrelators_ParameterSetRecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropIndex(
                name: "IX_RecommendationCorrelators_ProductRecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropColumn(
                name: "ParameterSetRecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropColumn(
                name: "ProductRecommenderId",
                table: "RecommendationCorrelators");
        }
    }
}
