using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_view_customer_metric_daily_numeric_aggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
                    Purpose: Customer metric daily aggregate value
            Version History:
                 2022-02-08: EP - First Iteration
                 2022-02-08: Eph - Convert to sqlite
            */
            var script = @"
            CREATE VIEW View_CustomerMetricDailyNumericAggregate            
            AS
            SELECT
                CL.CalendarDate
                ,CL.FirstOfWeek
                ,CL.LastOfWeek
                ,DT.CustomerId
                ,DT.MetricId
                ,DT.NumericValue
                ,DT.StartDate
                ,DT.EndDate
                /*
                Unpack the value of the StartDate and EndDate using the Calendar Table.
                The logic will generate daily level data grouped by CustomerId and MetricId.
                */
            FROM (
                    SELECT
                        [CustomerId]
                        ,[MetricId]
                        ,[NumericValue]
                        ,[StartDate]
                        /*
                            Derive the EndDate based on the next available StartDate for the same CustomerId and MetricId combination. Then, subtract 1 day from the value.
                            If there's no next available StartDate, use the CURRENT_TIMESTAMP.
                        */
                        ,COALESCE(DATE(LEAD([StartDate],1)OVER(PARTITION BY [CustomerId], [MetricId] ORDER BY [StartDate]), '-1 day'), CURRENT_TIMESTAMP) AS EndDate
                    FROM (
                            SELECT
                                    [TrackedUserId]         AS [CustomerId]
                                    ,[MetricId]			  AS [MetricId]
                                    ,AVG([NumericValue])     AS [NumericValue]
                                    ,CAST([Created] AS DATE) AS [StartDate]
                                FROM HistoricTrackedUserFeatures
                                WHERE [NumericValue] IS NOT NULL
                            GROUP BY [TrackedUserId]
                                    ,[MetricId]
                                    ,CAST([Created] AS DATE)
                            ) MAIN
                    ) DT
            INNER JOIN Calendar CL
                /*
                Only get the past 6 months
                */
                ON CL.[CalendarDate] >= DATE('now', '-6 month')
                AND CL.[CalendarDate] BETWEEN DT.[StartDate] AND DT.[EndDate]";
            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var script = @"DROP VIEW View_CustomerMetricDailyNumericAggregate";
            migrationBuilder.Sql(script);
        }
    }
}
