using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class fix_sqlitemigrationsfordbconstraintschange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Environments_EnvironmentId",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsRecommendations_Environments_EnvironmentId",
                table: "ItemsRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Environments_EnvironmentId",
                table: "Parameters");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_Environments_EnvironmentId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommenderPerformanceReports_Environments_EnvironmentId",
                table: "RecommenderPerformanceReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Touchpoints_Environments_EnvironmentId",
                table: "Touchpoints");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUsers_Environments_EnvironmentId",
                table: "TrackedUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_WebhookReceivers_Environments_EnvironmentId",
                table: "WebhookReceivers");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Environments_EnvironmentId",
                table: "Features",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsRecommendations_Environments_EnvironmentId",
                table: "ItemsRecommendations",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Environments_EnvironmentId",
                table: "Parameters",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_Environments_EnvironmentId",
                table: "ParameterSetRecommendations",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommenderPerformanceReports_Environments_EnvironmentId",
                table: "RecommenderPerformanceReports",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Touchpoints_Environments_EnvironmentId",
                table: "Touchpoints",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUsers_Environments_EnvironmentId",
                table: "TrackedUsers",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WebhookReceivers_Environments_EnvironmentId",
                table: "WebhookReceivers",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Environments_EnvironmentId",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsRecommendations_Environments_EnvironmentId",
                table: "ItemsRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Environments_EnvironmentId",
                table: "Parameters");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_Environments_EnvironmentId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommenderPerformanceReports_Environments_EnvironmentId",
                table: "RecommenderPerformanceReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Touchpoints_Environments_EnvironmentId",
                table: "Touchpoints");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUsers_Environments_EnvironmentId",
                table: "TrackedUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_WebhookReceivers_Environments_EnvironmentId",
                table: "WebhookReceivers");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Environments_EnvironmentId",
                table: "Features",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsRecommendations_Environments_EnvironmentId",
                table: "ItemsRecommendations",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Environments_EnvironmentId",
                table: "Parameters",
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

            migrationBuilder.AddForeignKey(
                name: "FK_RecommenderPerformanceReports_Environments_EnvironmentId",
                table: "RecommenderPerformanceReports",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Touchpoints_Environments_EnvironmentId",
                table: "Touchpoints",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUsers_Environments_EnvironmentId",
                table: "TrackedUsers",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WebhookReceivers_Environments_EnvironmentId",
                table: "WebhookReceivers",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
