using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_historic_features_view : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE VIEW View_MaxHistoricTrackedUserFeatureVersion AS
                    SELECT max(Id) HistoricTrackedUserFeatureId, TrackedUserId, FeatureId, max(Version) MaxVersion 
                    FROM HistoricTrackedUserFeatures GROUP BY TrackedUserId, FeatureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP VIEW View_MaxHistoricTrackedUserFeatureVersion");
        }
    }
}
