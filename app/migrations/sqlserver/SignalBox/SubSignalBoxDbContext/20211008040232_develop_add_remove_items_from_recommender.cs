using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_add_remove_items_from_recommender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationCorrelators_Recommenders_RecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationCorrelators_Recommenders_RecommenderId",
                table: "RecommendationCorrelators",
                column: "RecommenderId",
                principalTable: "Recommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationCorrelators_Recommenders_RecommenderId",
                table: "RecommendationCorrelators");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationCorrelators_Recommenders_RecommenderId",
                table: "RecommendationCorrelators",
                column: "RecommenderId",
                principalTable: "Recommenders",
                principalColumn: "Id");
        }
    }
}
