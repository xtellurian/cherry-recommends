using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_historic_features_prelim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUserFeatures_Features_FeatureId",
                table: "TrackedUserFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUserFeatures_TrackedUsers_TrackedUserId",
                table: "TrackedUserFeatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackedUserFeatures",
                table: "TrackedUserFeatures");

            migrationBuilder.RenameTable(
                name: "TrackedUserFeatures",
                newName: "HistoricTrackedUserFeatures");

            migrationBuilder.RenameIndex(
                name: "IX_TrackedUserFeatures_TrackedUserId",
                table: "HistoricTrackedUserFeatures",
                newName: "IX_HistoricTrackedUserFeatures_TrackedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TrackedUserFeatures_FeatureId_TrackedUserId_Version",
                table: "HistoricTrackedUserFeatures",
                newName: "IX_HistoricTrackedUserFeatures_FeatureId_TrackedUserId_Version");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoricTrackedUserFeatures",
                table: "HistoricTrackedUserFeatures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricTrackedUserFeatures_Features_FeatureId",
                table: "HistoricTrackedUserFeatures",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricTrackedUserFeatures_TrackedUsers_TrackedUserId",
                table: "HistoricTrackedUserFeatures",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricTrackedUserFeatures_Features_FeatureId",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricTrackedUserFeatures_TrackedUsers_TrackedUserId",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoricTrackedUserFeatures",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.RenameTable(
                name: "HistoricTrackedUserFeatures",
                newName: "TrackedUserFeatures");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricTrackedUserFeatures_TrackedUserId",
                table: "TrackedUserFeatures",
                newName: "IX_TrackedUserFeatures_TrackedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricTrackedUserFeatures_FeatureId_TrackedUserId_Version",
                table: "TrackedUserFeatures",
                newName: "IX_TrackedUserFeatures_FeatureId_TrackedUserId_Version");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackedUserFeatures",
                table: "TrackedUserFeatures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUserFeatures_Features_FeatureId",
                table: "TrackedUserFeatures",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUserFeatures_TrackedUsers_TrackedUserId",
                table: "TrackedUserFeatures",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
