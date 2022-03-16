using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_predicates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW View_MaxHistoricCustomerMetricVersion"); // manually added
            migrationBuilder.Sql("DROP VIEW View_CustomerMetricDailyStringAggregate"); // manually added
            migrationBuilder.Sql("DROP VIEW View_CustomerMetricDailyNumericAggregate"); // manually added

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricTrackedUserFeatures_TrackedUsers_TrackedUserId",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.DropIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId_TrackedUserId_Version",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.RenameColumn(
                name: "TrackedUserId",
                table: "HistoricTrackedUserFeatures",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricTrackedUserFeatures_TrackedUserId",
                table: "HistoricTrackedUserFeatures",
                newName: "IX_HistoricTrackedUserFeatures_CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId_CustomerId_Version",
                table: "HistoricTrackedUserFeatures",
                columns: new[] { "MetricId", "CustomerId", "Version" },
                unique: true,
                filter: "[CustomerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricTrackedUserFeatures_TrackedUsers_CustomerId",
                table: "HistoricTrackedUserFeatures",
                column: "CustomerId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql(
                @"CREATE VIEW View_MaxHistoricCustomerMetricVersion AS
                    (
                        SELECT Id HistoricCustomerMetricId, CustomerId, Version MaxVersion, MetricId, NumericValue, StringValue
                        FROM HistoricTrackedUserFeatures allValues
                        JOIN(
                            SELECT max(Id) HistoricCustomerMetricId, CustomerId as cIs, MetricId as mId, max(Version) MaxVersion
                            FROM HistoricTrackedUserFeatures
                            GROUP BY CustomerId, MetricId
                        ) n
                        ON allValues.Id = n.HistoricCustomerMetricId
                    )"
                );
            migrationBuilder.Sql(
            @"CREATE VIEW View_CustomerMetricDailyStringAggregate            
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
                            [CustomerId]
                            ,[MetricId]					AS [MetricId]
                            ,[StringValue]				AS [StringValue]
                            ,COUNT(1)					AS [ValueCount]
                            ,CAST([Created] AS DATE)	AS [Created]
                        FROM HistoricTrackedUserFeatures
                        WHERE [StringValue] IS NOT NULL
                    GROUP BY [CustomerId]
                            ,[MetricId]
                            ,[StringValue]
                            ,CAST([Created] AS DATE)
                    ) DT
            INNER JOIN Calendar CL
                ON CL.CalendarDate = DT.Created"
            );

            migrationBuilder.Sql(
                @"
            CREATE VIEW View_CustomerMetricDailyNumericAggregate           
            AS
            SELECT
                CAST(CL.CalendarDate AS DATETIMEOFFSET) AS [CalendarDate]
                ,CAST(CL.FirstOfWeek AS DATETIMEOFFSET) AS [FirstOfWeek]
                ,CAST(CL.LastOfWeek AS DATETIMEOFFSET)	AS [LastOfWeek]
                ,DT.CustomerId
                ,DT.MetricId
                ,DT.NumericValue
                ,CAST(DT.StartDate AS DATETIMEOFFSET)   AS [StartDate]
                ,CAST(DT.EndDate AS DATETIMEOFFSET)     AS [EndDate]
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
                        ,COALESCE(DATEADD(DAY,-1, LEAD([StartDate],1)OVER(PARTITION BY [CustomerId], [MetricId] ORDER BY [StartDate])), CONVERT (date, CURRENT_TIMESTAMP)) AS EndDate
                    FROM (
                            /*
                                Get the average metric value that's generated within the same Created (StartDate) date
                            */
                            SELECT
                                    [CustomerId]
                                    ,[MetricId]					AS [MetricId]
                                    ,AVG([NumericValue])		AS [NumericValue]
                                    ,CAST([Created] AS DATE)	AS [StartDate]
                                FROM HistoricTrackedUserFeatures
                                WHERE [NumericValue] IS NOT NULL
                            GROUP BY [CustomerId]
                                    ,[MetricId]
                                    ,CAST([Created] AS DATE)
                            ) MAIN
                    ) DT
            INNER JOIN Calendar CL
                /*
                Only get the past 6 months
                */
                ON CL.[CalendarDate] >= DATEADD(MONTH, -6, GETDATE())
                AND CL.[CalendarDate] BETWEEN DT.[StartDate] AND DT.[EndDate]"
            );
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW View_MaxHistoricCustomerMetricVersion"); // manually added
            migrationBuilder.Sql("DROP VIEW View_CustomerMetricDailyStringAggregate"); // manually added
            migrationBuilder.Sql("DROP VIEW View_CustomerMetricDailyNumericAggregate"); // manually added

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricTrackedUserFeatures_TrackedUsers_CustomerId",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.DropIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId_CustomerId_Version",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "HistoricTrackedUserFeatures",
                newName: "TrackedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricTrackedUserFeatures_CustomerId",
                table: "HistoricTrackedUserFeatures",
                newName: "IX_HistoricTrackedUserFeatures_TrackedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId_TrackedUserId_Version",
                table: "HistoricTrackedUserFeatures",
                columns: new[] { "MetricId", "TrackedUserId", "Version" },
                unique: true,
                filter: "[TrackedUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricTrackedUserFeatures_TrackedUsers_TrackedUserId",
                table: "HistoricTrackedUserFeatures",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // recreate old versions
            migrationBuilder.Sql(
              @"CREATE VIEW View_MaxHistoricCustomerMetricVersion AS
                     SELECT max(Id) HistoricCustomerMetricId, TrackedUserId as CustomerId, MetricId, max(Version) MaxVersion 
                     FROM HistoricTrackedUserFeatures GROUP BY TrackedUserId, MetricId");

            migrationBuilder.Sql(
            @"CREATE VIEW View_CustomerMetricDailyStringAggregate            
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
                ON CL.CalendarDate = DT.Created"
            );

            migrationBuilder.Sql(
                @"
            CREATE VIEW View_CustomerMetricDailyNumericAggregate           
            AS
            SELECT
                CAST(CL.CalendarDate AS DATETIMEOFFSET) AS [CalendarDate]
                ,CAST(CL.FirstOfWeek AS DATETIMEOFFSET) AS [FirstOfWeek]
                ,CAST(CL.LastOfWeek AS DATETIMEOFFSET)	AS [LastOfWeek]
                ,DT.CustomerId
                ,DT.MetricId
                ,DT.NumericValue
                ,CAST(DT.StartDate AS DATETIMEOFFSET)   AS [StartDate]
                ,CAST(DT.EndDate AS DATETIMEOFFSET)     AS [EndDate]
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
                        ,COALESCE(DATEADD(DAY,-1, LEAD([StartDate],1)OVER(PARTITION BY [CustomerId], [MetricId] ORDER BY [StartDate])), CONVERT (date, CURRENT_TIMESTAMP)) AS EndDate
                    FROM (
                            /*
                                Get the average metric value that's generated within the same Created (StartDate) date
                            */
                            SELECT
                                    [TrackedUserId]				AS [CustomerId]
                                    ,[MetricId]					AS [MetricId]
                                    ,AVG([NumericValue])		AS [NumericValue]
                                    ,CAST([Created] AS DATE)	AS [StartDate]
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
                ON CL.[CalendarDate] >= DATEADD(MONTH, -6, GETDATE())
                AND CL.[CalendarDate] BETWEEN DT.[StartDate] AND DT.[EndDate]"
            );
        }
    }
}
