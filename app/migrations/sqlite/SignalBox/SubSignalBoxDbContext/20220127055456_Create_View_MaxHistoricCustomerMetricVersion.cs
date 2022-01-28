using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class Create_View_MaxHistoricCustomerMetricVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE VIEW View_MaxHistoricCustomerMetricVersion AS
                     SELECT max(Id) HistoricCustomerMetricId, TrackedUserId as CustomerId, MetricId, max(Version) MaxVersion 
                     FROM HistoricTrackedUserFeatures GROUP BY TrackedUserId, MetricId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW View_MaxHistoricCustomerMetricVersion");
        }
    }
}
