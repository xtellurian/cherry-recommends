using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class fix_environment_not_found : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "WebhookReceivers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "RecommendationDestinations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebhookReceivers_EnvironmentId",
                table: "WebhookReceivers",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationDestinations_EnvironmentId",
                table: "RecommendationDestinations",
                column: "EnvironmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationDestinations_Environments_EnvironmentId",
                table: "RecommendationDestinations",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebhookReceivers_Environments_EnvironmentId",
                table: "WebhookReceivers",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationDestinations_Environments_EnvironmentId",
                table: "RecommendationDestinations");

            migrationBuilder.DropForeignKey(
                name: "FK_WebhookReceivers_Environments_EnvironmentId",
                table: "WebhookReceivers");

            migrationBuilder.DropIndex(
                name: "IX_WebhookReceivers_EnvironmentId",
                table: "WebhookReceivers");

            migrationBuilder.DropIndex(
                name: "IX_RecommendationDestinations_EnvironmentId",
                table: "RecommendationDestinations");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "WebhookReceivers");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "RecommendationDestinations");
        }
    }
}
