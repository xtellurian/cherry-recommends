using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_rename_feature_to_metric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureDestinations_Features_FeatureId",
                table: "FeatureDestinations");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureGenerators_Features_FeatureId",
                table: "FeatureGenerators");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricTrackedUserFeatures_Features_FeatureId",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.RenameColumn(
                name: "FeatureId",
                table: "HistoricTrackedUserFeatures",
                newName: "MetricId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricTrackedUserFeatures_FeatureId_TrackedUserId_Version",
                table: "HistoricTrackedUserFeatures",
                newName: "IX_HistoricTrackedUserFeatures_MetricId_TrackedUserId_Version");

            migrationBuilder.RenameColumn(
                name: "FeatureId",
                table: "FeatureGenerators",
                newName: "MetricId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureGenerators_FeatureId",
                table: "FeatureGenerators",
                newName: "IX_FeatureGenerators_MetricId");

            migrationBuilder.RenameColumn(
                name: "FeatureId",
                table: "FeatureDestinations",
                newName: "MetricId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureDestinations_FeatureId",
                table: "FeatureDestinations",
                newName: "IX_FeatureDestinations_MetricId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureDestinations_Features_MetricId",
                table: "FeatureDestinations",
                column: "MetricId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureGenerators_Features_MetricId",
                table: "FeatureGenerators",
                column: "MetricId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricTrackedUserFeatures_Features_MetricId",
                table: "HistoricTrackedUserFeatures",
                column: "MetricId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureDestinations_Features_MetricId",
                table: "FeatureDestinations");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureGenerators_Features_MetricId",
                table: "FeatureGenerators");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricTrackedUserFeatures_Features_MetricId",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.RenameColumn(
                name: "MetricId",
                table: "HistoricTrackedUserFeatures",
                newName: "FeatureId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId_TrackedUserId_Version",
                table: "HistoricTrackedUserFeatures",
                newName: "IX_HistoricTrackedUserFeatures_FeatureId_TrackedUserId_Version");

            migrationBuilder.RenameColumn(
                name: "MetricId",
                table: "FeatureGenerators",
                newName: "FeatureId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureGenerators_MetricId",
                table: "FeatureGenerators",
                newName: "IX_FeatureGenerators_FeatureId");

            migrationBuilder.RenameColumn(
                name: "MetricId",
                table: "FeatureDestinations",
                newName: "FeatureId");

            migrationBuilder.RenameIndex(
                name: "IX_FeatureDestinations_MetricId",
                table: "FeatureDestinations",
                newName: "IX_FeatureDestinations_FeatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureDestinations_Features_FeatureId",
                table: "FeatureDestinations",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureGenerators_Features_FeatureId",
                table: "FeatureGenerators",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricTrackedUserFeatures_Features_FeatureId",
                table: "HistoricTrackedUserFeatures",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
