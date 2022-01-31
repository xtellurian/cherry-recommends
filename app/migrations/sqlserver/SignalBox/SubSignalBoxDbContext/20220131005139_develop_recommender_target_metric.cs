using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_recommender_target_metric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TargetMetricId",
                table: "Recommenders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetMetricId",
                table: "ParameterSetRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetMetricId",
                table: "ItemsRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recommenders_TargetMetricId",
                table: "Recommenders",
                column: "TargetMetricId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSetRecommendations_TargetMetricId",
                table: "ParameterSetRecommendations",
                column: "TargetMetricId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsRecommendations_TargetMetricId",
                table: "ItemsRecommendations",
                column: "TargetMetricId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsRecommendations_Features_TargetMetricId",
                table: "ItemsRecommendations",
                column: "TargetMetricId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_Features_TargetMetricId",
                table: "ParameterSetRecommendations",
                column: "TargetMetricId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_Features_TargetMetricId",
                table: "Recommenders",
                column: "TargetMetricId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemsRecommendations_Features_TargetMetricId",
                table: "ItemsRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_Features_TargetMetricId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_Features_TargetMetricId",
                table: "Recommenders");

            migrationBuilder.DropIndex(
                name: "IX_Recommenders_TargetMetricId",
                table: "Recommenders");

            migrationBuilder.DropIndex(
                name: "IX_ParameterSetRecommendations_TargetMetricId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_ItemsRecommendations_TargetMetricId",
                table: "ItemsRecommendations");

            migrationBuilder.DropColumn(
                name: "TargetMetricId",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "TargetMetricId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "TargetMetricId",
                table: "ItemsRecommendations");
        }
    }
}
