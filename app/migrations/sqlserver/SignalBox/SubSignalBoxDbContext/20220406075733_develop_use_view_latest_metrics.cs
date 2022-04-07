using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_use_view_latest_metrics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW View_MaxHistoricCustomerMetricVersion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW View_MaxHistoricCustomerMetricVersion AS
            (
                SELECT Id HistoricCustomerMetricId, CustomerId, BusinessId, Version MaxVersion, MetricId, NumericValue, StringValue
                FROM HistoricTrackedUserFeatures allValues
                JOIN(
                    SELECT max(Id) HistoricCustomerMetricId, CustomerId as cIs, BusinessId as bIs, MetricId as mId, max(Version) MaxVersion
                    FROM HistoricTrackedUserFeatures
                    GROUP BY CustomerId, BusinessId, MetricId
                ) n
                ON allValues.Id = n.HistoricCustomerMetricId
            )
            ");
        }
    }
}
