using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_view_customer_metric_weekly_numeric_aggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
                    Purpose: Metric daily aggregate value
            Version History:
                 2022-02-08: EP - First Iteration
                 2022-02-09: JT - Fix aggregation inaccuracy
            */
            var script = @"
            CREATE VIEW View_CustomerMetricWeeklyNumericAggregate
            AS
            /*
            Generate the weekly metric aggregate based on FirstOfWeek and MetricId.
            */
            SELECT  DA.FirstOfWeek
                    ,DA.LastOfWeek
                    ,DA.MetricId
                    ,AVG(NumericValue) AS [WeeklyMeanNumericValue]
                    ,COUNT(DISTINCT CustomerId) AS [WeeklyDistinctCustomerCount]
            FROM (
                    /*
                    Get the last record for the week
                    */
                    SELECT  TOP 1 WITH TIES
                            FirstOfWeek
                            ,LastOfWeek
                            ,MetricId
                            ,CustomerId
                            ,NumericValue
                            ,StartDate
                            ,EndDate
                            ,ROW_NUMBER() OVER (PARTITION BY FirstOfWeek, MetricId, CustomerId ORDER BY StartDate DESC) AS ROWNUM
                    FROM    View_CustomerMetricDailyNumericAggregate
                    ORDER BY
                            ROW_NUMBER() OVER (PARTITION BY FirstOfWeek, MetricId, CustomerId ORDER BY StartDate DESC)
            ) DA
            GROUP BY
                    DA.FirstOfWeek
                    ,DA.LastOfWeek
		            ,DA.MetricId";
            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var script = @"DROP VIEW View_CustomerMetricWeeklyNumericAggregate";
            migrationBuilder.Sql(script);
        }
    }
}
