using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_view_customer_metric_daily_string_aggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var script = @"
            CREATE VIEW View_CustomerMetricDailyStringAggregate        
            AS
            SELECT
                CAST(CL.CalendarDate AS DATETIMEOFFSET) AS [CalendarDate]
                ,CAST(CL.FirstOfWeek AS DATETIMEOFFSET) AS [FirstOfWeek]
                ,CAST(CL.LastOfWeek AS DATETIMEOFFSET)	AS [LastOfWeek]
                ,DT.CustomerId
                ,DT.MetricId
                ,DT.StringValue
                ,DT.ValueCount
                ,CAST(DT.Created AS DATETIMEOFFSET)	    AS [Created]
                /*
                Unpack the value of the StartDate and EndDate using the Calendar Table.
                The logic will generate daily level data grouped by CustomerId, MetricId and StringValue.
                */
            FROM (
                    /*
                        Get the metric value count that's generated within the same Created (StartDate) date
                    */
                    SELECT
                            [TrackedUserId]				AS [CustomerId]
                            ,[MetricId]					AS [MetricId]
                            ,[StringValue]				AS [StringValue]
                            ,COUNT(1)					AS [ValueCount]
                            ,CAST([Created] AS DATE)	AS [Created]
                        FROM HistoricTrackedUserFeatures
                        WHERE [StringValue] IS NOT NULL
                    GROUP BY [TrackedUserId]
                            ,[MetricId]
                            ,[StringValue]
                            ,CAST([Created] AS DATE)
                    ) DT
            INNER JOIN Calendar CL
                ON CL.CalendarDate = DT.Created";
            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var script = @"DROP VIEW View_CustomerMetricDailyStringAggregate";
            migrationBuilder.Sql(script);
        }
    }
}
