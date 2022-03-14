using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_business_metrics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId_TrackedUserId_Version",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.AddColumn<long>(
                name: "BusinessId",
                table: "HistoricTrackedUserFeatures",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricTrackedUserFeatures_BusinessId",
                table: "HistoricTrackedUserFeatures",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId_BusinessId_Version",
                table: "HistoricTrackedUserFeatures",
                columns: new[] { "MetricId", "BusinessId", "Version" },
                unique: true,
                filter: "[BusinessId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId_TrackedUserId_Version",
                table: "HistoricTrackedUserFeatures",
                columns: new[] { "MetricId", "TrackedUserId", "Version" },
                unique: true,
                filter: "[TrackedUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricTrackedUserFeatures_Businesses_BusinessId",
                table: "HistoricTrackedUserFeatures",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricTrackedUserFeatures_Businesses_BusinessId",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.DropIndex(
                name: "IX_HistoricTrackedUserFeatures_BusinessId",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.DropIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId_BusinessId_Version",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.DropIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId_TrackedUserId_Version",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId_TrackedUserId_Version",
                table: "HistoricTrackedUserFeatures",
                columns: new[] { "MetricId", "TrackedUserId", "Version" },
                unique: true);
        }
    }
}
