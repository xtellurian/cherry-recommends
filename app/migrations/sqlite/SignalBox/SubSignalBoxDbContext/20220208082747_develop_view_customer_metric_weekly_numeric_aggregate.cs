using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_view_customer_metric_weekly_numeric_aggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
                    Purpose: Metric daily aggregate value
            Version History:
                 2022-02-08: EP - First Iteration
                 2022-02-09: JT - Out of sync with sqlserver version. Result will differ.
            */
            var script = @"
            CREATE VIEW View_CustomerMetricWeeklyNumericAggregate
            AS
                /*
                Generate the weekly metric aggregate based on FirstOfWeek and MetricId.
                */
            SELECT
                    FirstOfWeek
                    ,LastOfWeek
                    ,MetricId
                    ,AVG(NumericValue) AS WeeklyMeanNumericValue
                    ,COUNT(DISTINCT CustomerId) AS WeeklyDistinctCustomerCount
                FROM View_CustomerMetricDailyNumericAggregate
            GROUP BY 
                    FirstOfWeek
                    ,LastOfWeek
                    ,MetricId";
            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var script = @"DROP VIEW View_CustomerMetricWeeklyNumericAggregate";
            migrationBuilder.Sql(script);
        }
    }
}
