using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_view_customer_metric_weekly_string_aggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var script = @"
            CREATE VIEW View_CustomerMetricWeeklyStringAggregate
            AS
            /*
            Generate the weekly metric aggregate based on FirstOfWeek and MetricId.
            */
            SELECT
                    FirstOfWeek
                    ,LastOfWeek
                    ,MetricId
                    ,StringValue
                    ,SUM(ValueCount) AS WeeklyValueCount
                    ,COUNT(DISTINCT CustomerId) AS WeeklyDistinctCustomerCount
            FROM View_CustomerMetricDailyStringAggregate
            GROUP BY 
                    FirstOfWeek
                    ,LastOfWeek
                    ,MetricId
                    ,StringValue";
            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var script = @"DROP VIEW View_CustomerMetricWeeklyStringAggregate";
            migrationBuilder.Sql(script);
        }
    }
}
