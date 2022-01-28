using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class Drop_View_MaxHistoricTrackedUserFeatureVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP VIEW View_MaxHistoricTrackedUserFeatureVersion");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE VIEW View_MaxHistoricTrackedUserFeatureVersion AS
                    SELECT max(Id) HistoricTrackedUserFeatureId, TrackedUserId, FeatureId, max(Version) MaxVersion 
                    FROM HistoricTrackedUserFeatures GROUP BY TrackedUserId, FeatureId");
        }

    }
}
