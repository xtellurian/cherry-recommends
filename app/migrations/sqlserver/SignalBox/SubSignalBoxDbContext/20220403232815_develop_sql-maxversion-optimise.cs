using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_sqlmaxversionoptimise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE VIEW [dbo].[View_LatestMetrics]
                AS
        (
            SELECT HistoricCustomerMetricId
                  ,CustomerId
                  ,BusinessId
                  ,MaxVersion
                  ,MetricId
                  ,NumericValue
                  ,StringValue
              FROM
                  (
                  SELECT
                          Id AS HistoricCustomerMetricId
                          ,CustomerId
                          ,BusinessId
                          ,Version AS MaxVersion
                          ,MetricId
                          ,NumericValue
                          ,StringValue
                          ,ROW_NUMBER() OVER(PARTITION BY MetricId, CustomerId, BusinessId ORDER BY Version DESC) AS RowNumber
                      FROM HistoricTrackedUserFeatures
                  ) DT
              WHERE RowNumber = 1
        )");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DROP VIEW [dbo].[View_LatestMetrics]
        )");
        }
    }
}
