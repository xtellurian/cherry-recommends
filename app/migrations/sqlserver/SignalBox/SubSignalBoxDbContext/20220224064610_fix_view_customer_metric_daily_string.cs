using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class fix_view_customer_metric_daily_string : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var script = @"
            ALTER VIEW [dbo].[View_CustomerMetricDailyStringAggregate]            
            AS
            SELECT
                CAST(CL.CalendarDate AS DATETIMEOFFSET) AS [CalendarDate]
                ,CAST(CL.FirstOfWeek AS DATETIMEOFFSET) AS [FirstOfWeek]
                ,CAST(CL.LastOfWeek AS DATETIMEOFFSET)	AS [LastOfWeek]
                ,DT.CustomerId
                ,DT.MetricId
                ,DT.StringValue
                ,DT.ValueCount
                ,CAST(DT.StartDate AS DATETIMEOFFSET)   AS [StartDate]
                ,CAST(DT.EndDate AS DATETIMEOFFSET)     AS [EndDate]
                /*
                Unpack the value of the StartDate and EndDate using the Calendar Table.
                The logic will generate daily level data grouped by CustomerId, MetricId and StringValue.
                */
            FROM (
					SELECT	[CustomerId]
							,[MetricId]
							,[StringValue]
							,[ValueCount]
							,[StartDate]
							/*
								Derive the EndDate based on the next available StartDate for the same CustomerId and MetricId combination. Then, subtract 1 day from the value.
								If there's no next available StartDate, use the CURRENT_TIMESTAMP.
							*/
							,COALESCE(DATEADD(DAY,-1, LEAD([StartDate],1)OVER(PARTITION BY [CustomerId], [MetricId] ORDER BY [StartDate])), CONVERT (date, CURRENT_TIMESTAMP)) AS EndDate
					FROM (
							/*
								Get the metric value count that's generated within the same Created (StartDate) date
							*/
							SELECT
									[TrackedUserId]				AS [CustomerId]
									,[MetricId]					AS [MetricId]
									,[StringValue]				AS [StringValue]
									,COUNT(1)					AS [ValueCount]
									,CAST([Created] AS DATE)	AS [StartDate]
								FROM HistoricTrackedUserFeatures
								WHERE [StringValue] IS NOT NULL
							GROUP BY [TrackedUserId]
									,[MetricId]
									,[StringValue]
									,CAST([Created] AS DATE)
						) MAIN
                    ) DT
            INNER JOIN Calendar CL
                /*
                Only get the past 6 months
                */
                ON CL.[CalendarDate] >= DATEADD(MONTH, -6, GETDATE())
                AND CL.[CalendarDate] BETWEEN DT.[StartDate] AND DT.[EndDate]";
            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var script = @"
            ALTER VIEW View_CustomerMetricDailyStringAggregate            
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
    }
}
