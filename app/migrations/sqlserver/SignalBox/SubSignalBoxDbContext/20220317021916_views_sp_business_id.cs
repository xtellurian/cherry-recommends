using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class views_sp_business_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // View_MaxHistoricCustomerMetricVersion
            migrationBuilder.Sql(@"
            ALTER VIEW View_MaxHistoricCustomerMetricVersion AS
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

            // View_CustomerMetricDailyNumericAggregate
            migrationBuilder.Sql(@"
            ALTER VIEW View_CustomerMetricDailyNumericAggregate
            AS
            SELECT
                CAST(CL.CalendarDate AS DATETIMEOFFSET) AS [CalendarDate]
                ,CAST(CL.FirstOfWeek AS DATETIMEOFFSET) AS [FirstOfWeek]
                ,CAST(CL.LastOfWeek AS DATETIMEOFFSET)	AS [LastOfWeek]
                ,DT.CustomerId
                ,DT.BusinessId
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
                        ,[BusinessId]
                        ,[MetricId]
                        ,[NumericValue]
                        ,[StartDate]
                        /*
                            Derive the EndDate based on the next available StartDate for the same CustomerId and MetricId combination. Then, subtract 1 day from the value.
                            If there's no next available StartDate, use the CURRENT_TIMESTAMP.
                        */
                        ,COALESCE(DATEADD(DAY,-1, LEAD([StartDate],1)OVER(PARTITION BY [CustomerId], [BusinessId], [MetricId] ORDER BY [StartDate])), CONVERT (date, CURRENT_TIMESTAMP)) AS EndDate
                    FROM (
                            /*
                                Get the average metric value that's generated within the same Created (StartDate) date
                            */
                            SELECT
                                    [CustomerId]
                                    ,[BusinessId]
                                    ,[MetricId]					AS [MetricId]
                                    ,AVG([NumericValue])		AS [NumericValue]
                                    ,CAST([Created] AS DATE)	AS [StartDate]
                                FROM HistoricTrackedUserFeatures
                                WHERE [NumericValue] IS NOT NULL
                            GROUP BY [CustomerId]
                                    ,[BusinessId]
                                    ,[MetricId]
                                    ,CAST([Created] AS DATE)
                            ) MAIN
                    ) DT
            INNER JOIN Calendar CL
                /*
                Only get the past 6 months
                */
                ON CL.[CalendarDate] >= DATEADD(MONTH, -6, GETDATE())
                AND CL.[CalendarDate] BETWEEN DT.[StartDate] AND DT.[EndDate]
            ");

            // View_CustomerMetricDailyStringAggregate
            migrationBuilder.Sql(@"
            ALTER VIEW View_CustomerMetricDailyStringAggregate
            AS
            SELECT
                CAST(CL.CalendarDate AS DATETIMEOFFSET) AS [CalendarDate]
                ,CAST(CL.FirstOfWeek AS DATETIMEOFFSET) AS [FirstOfWeek]
                ,CAST(CL.LastOfWeek AS DATETIMEOFFSET)	AS [LastOfWeek]
                ,DT.CustomerId
                ,DT.BusinessId
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
                            ,[BusinessId]
							,[MetricId]
							,[StringValue]
							,[ValueCount]
							,[StartDate]
							/*
								Derive the EndDate based on the next available StartDate for the same CustomerId and MetricId combination. Then, subtract 1 day from the value.
								If there's no next available StartDate, use the CURRENT_TIMESTAMP.
							*/
							,COALESCE(DATEADD(DAY,-1, LEAD([StartDate],1)OVER(PARTITION BY [CustomerId], [BusinessId], [MetricId] ORDER BY [StartDate])), CONVERT (date, CURRENT_TIMESTAMP)) AS EndDate
					FROM (
							/*
								Get the metric value count that's generated within the same Created (StartDate) date
							*/
							SELECT
									[CustomerId]				AS [CustomerId]
                                    ,[BusinessId]               AS [BusinessId]
									,[MetricId]					AS [MetricId]
									,[StringValue]				AS [StringValue]
									,COUNT(1)					AS [ValueCount]
									,CAST([Created] AS DATE)	AS [StartDate]
								FROM HistoricTrackedUserFeatures
								WHERE [StringValue] IS NOT NULL
							GROUP BY [CustomerId]
                                    ,[BusinessId]
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
                AND CL.[CalendarDate] BETWEEN DT.[StartDate] AND DT.[EndDate]
            ");

            // View_CustomerMetricWeeklyNumericAggregate
            migrationBuilder.Sql(@"
            ALTER VIEW View_CustomerMetricWeeklyNumericAggregate
                AS
                /*
                Generate the weekly metric aggregate based on FirstOfWeek and MetricId.
                */
                SELECT  DA.FirstOfWeek
                        ,DA.LastOfWeek
                        ,DA.MetricId
                        ,AVG(NumericValue) AS [WeeklyMeanNumericValue]
                        ,COUNT(DISTINCT CustomerId) AS [WeeklyDistinctCustomerCount]
                        ,COUNT(DISTINCT BusinessId) AS [WeeklyDistinctBusinessCount]
                FROM (
                        /*
                        Get the last record for the week
                        */
                        SELECT  TOP 1 WITH TIES
                                FirstOfWeek
                                ,LastOfWeek
                                ,MetricId
                                ,CustomerId
                                ,BusinessId
                                ,NumericValue
                                ,StartDate
                                ,EndDate
                                ,ROW_NUMBER() OVER (PARTITION BY FirstOfWeek, MetricId, CustomerId, BusinessId ORDER BY StartDate DESC) AS ROWNUM
                        FROM    View_CustomerMetricDailyNumericAggregate
                        ORDER BY
                                ROW_NUMBER() OVER (PARTITION BY FirstOfWeek, MetricId, CustomerId, BusinessId ORDER BY StartDate DESC)
                ) DA
                GROUP BY
                        DA.FirstOfWeek
                        ,DA.LastOfWeek
                        ,DA.MetricId
            ");

            // View_CustomerMetricWeeklyStringAggregate
            migrationBuilder.Sql(@"
            ALTER VIEW [dbo].[View_CustomerMetricWeeklyStringAggregate]
            AS
            /*
            Generate the weekly metric aggregate based on FirstOfWeek and MetricId.
            */
            SELECT
                    DA.FirstOfWeek
                    ,DA.LastOfWeek
                    ,DA.MetricId
                    ,DA.StringValue
                    ,SUM(DA.ValueCount) AS WeeklyValueCount
                    ,COUNT(DISTINCT DA.CustomerId) AS WeeklyDistinctCustomerCount
                    ,COUNT(DISTINCT DA.BusinessId) AS WeeklyDistinctBusinessCount
            FROM (
                    /*
                    Get the last record for the week
                    */
                    SELECT  TOP 1 WITH TIES
                            FirstOfWeek
                            ,LastOfWeek
                            ,MetricId
                            ,CustomerId
                            ,BusinessId
                            ,StringValue
                            ,ValueCount
                            ,StartDate
                            ,EndDate
                            ,ROW_NUMBER() OVER (PARTITION BY FirstOfWeek, MetricId, CustomerId, BusinessId ORDER BY StartDate DESC) AS ROWNUM
                    FROM    View_CustomerMetricDailyStringAggregate
                    ORDER BY
                            ROW_NUMBER() OVER (PARTITION BY FirstOfWeek, MetricId, CustomerId, BusinessId ORDER BY StartDate DESC)
            ) DA
            GROUP BY
                    DA.FirstOfWeek
                    ,DA.LastOfWeek
                    ,DA.MetricId
                    ,DA.StringValue
            ");


            // sp_NumericMetricBinning
            migrationBuilder.Sql(@"
            ALTER PROCEDURE sp_NumericMetricBinning
                @MetricId INTEGER
                ,@BinNumber INTEGER
                AS
                SELECT
                BinFloor
                ,BinWidth
                ,CAST(BinFloor AS VARCHAR(10))+ ' - ' + CAST((BinFloor + BinWidth) AS VARCHAR(10)) AS BinRange
                ,CustomerCount
                ,BusinessCount
                FROM
                (
                    SELECT FLOOR(CA.NumericValue/BW.BinWidth)* BW.BinWidth AS BinFloor
                        ,BW.BinWidth
                        ,COUNT(CA.CustomerId)                            AS CustomerCount
                        ,COUNT(CA.BusinessId)                            AS BusinessCount
                    FROM View_CustomerMetricDailyNumericAggregate CA
                    CROSS JOIN
                        (
                            SELECT CASE
                                    WHEN MIN(NumericValue) = 0 AND MAX(NumericValue) = 0 THEN MIN(1)
                                    WHEN MIN(NumericValue) = MAX(NumericValue) THEN MIN(NumericValue)
                                    ELSE CEILING((MAX(NumericValue) - MIN(NumericValue))/@BinNumber)
                                    END AS BinWidth
                            FROM View_CustomerMetricDailyNumericAggregate
                            WHERE CalendarDate = CAST(CURRENT_TIMESTAMP AS DATE)
                                AND MetricId = @MetricId
                        ) BW
                    WHERE CA.CalendarDate = CAST(CURRENT_TIMESTAMP AS DATE)
                    AND CA.MetricId = @MetricId
                    GROUP BY FLOOR(CA.NumericValue/BW.BinWidth) * BW.BinWidth
                        ,BW.BinWidth
                ) DT
                ORDER BY BinFloor
                ;
            ");

            // sp_CategoricalMetricBinning
            migrationBuilder.Sql(@"
            ALTER PROCEDURE sp_CategoricalMetricBinning
                @MetricId INTEGER
                ,@TopKValue INTEGER
                AS
                SELECT TOP (@TopKValue)
                    StringValue
                    ,CustomerCount
                    ,BusinessCount
                FROM
                (
                    SELECT StringValue
                        ,COUNT(CustomerId) AS CustomerCount
                        ,COUNT(BusinessId) AS BusinessCount
                        FROM View_CustomerMetricDailyStringAggregate
                    WHERE CalendarDate = CAST(CURRENT_TIMESTAMP AS DATE)
                        AND MetricId = @MetricId
                    GROUP BY StringValue
                ) DT
                ORDER BY CustomerCount DESC
                ;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // View_MaxHistoricCustomerMetricVersion
            migrationBuilder.Sql(@"
            ALTER VIEW View_MaxHistoricCustomerMetricVersion AS
            (
                SELECT Id HistoricCustomerMetricId, CustomerId, Version MaxVersion, MetricId, NumericValue, StringValue
                FROM HistoricTrackedUserFeatures allValues
                JOIN(
                    SELECT max(Id) HistoricCustomerMetricId, CustomerId as cIs, MetricId as mId, max(Version) MaxVersion
                    FROM HistoricTrackedUserFeatures
                    GROUP BY CustomerId, MetricId
                ) n
                ON allValues.Id = n.HistoricCustomerMetricId
            )
            ");

            // View_CustomerMetricDailyNumericAggregate
            migrationBuilder.Sql(@"
            ALTER VIEW View_CustomerMetricDailyNumericAggregate
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
                AND CL.[CalendarDate] BETWEEN DT.[StartDate] AND DT.[EndDate]
            ");

            // View_CustomerMetricDailyStringAggregate
            migrationBuilder.Sql(@"
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
									[CustomerId]				AS [CustomerId]
									,[MetricId]					AS [MetricId]
									,[StringValue]				AS [StringValue]
									,COUNT(1)					AS [ValueCount]
									,CAST([Created] AS DATE)	AS [StartDate]
								FROM HistoricTrackedUserFeatures
								WHERE [StringValue] IS NOT NULL
							GROUP BY [CustomerId]
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
                AND CL.[CalendarDate] BETWEEN DT.[StartDate] AND DT.[EndDate]
            ");

            // View_CustomerMetricWeeklyNumericAggregate
            migrationBuilder.Sql(@"
            ALTER VIEW View_CustomerMetricWeeklyNumericAggregate
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
                        ,DA.MetricId
            ");

            // View_CustomerMetricWeeklyStringAggregate
            migrationBuilder.Sql(@"
            ALTER VIEW View_CustomerMetricWeeklyStringAggregate
            AS
            /*
            Generate the weekly metric aggregate based on FirstOfWeek and MetricId.
            */
            SELECT
                    DA.FirstOfWeek
                    ,DA.LastOfWeek
                    ,DA.MetricId
                    ,DA.StringValue
                    ,SUM(DA.ValueCount) AS WeeklyValueCount
                    ,COUNT(DISTINCT DA.CustomerId) AS WeeklyDistinctCustomerCount
            FROM (
                    /*
                    Get the last record for the week
                    */
                    SELECT  TOP 1 WITH TIES
                            FirstOfWeek
                            ,LastOfWeek
                            ,MetricId
                            ,CustomerId
                            ,StringValue
                            ,ValueCount
                            ,StartDate
                            ,EndDate
                            ,ROW_NUMBER() OVER (PARTITION BY FirstOfWeek, MetricId, CustomerId ORDER BY StartDate DESC) AS ROWNUM
                    FROM    View_CustomerMetricDailyStringAggregate
                    ORDER BY
                            ROW_NUMBER() OVER (PARTITION BY FirstOfWeek, MetricId, CustomerId ORDER BY StartDate DESC)
            ) DA
            GROUP BY
                    DA.FirstOfWeek
                    ,DA.LastOfWeek
                    ,DA.MetricId
                    ,DA.StringValue
            ");

            // sp_NumericMetricBinning
            migrationBuilder.Sql(@"
            ALTER PROCEDURE sp_NumericMetricBinning
                @MetricId INTEGER
                ,@BinNumber INTEGER
                AS
                SELECT
                BinFloor
                ,BinWidth
                ,CAST(BinFloor AS VARCHAR(10))+ ' - ' + CAST((BinFloor + BinWidth) AS VARCHAR(10)) AS BinRange
                ,CustomerCount
                FROM
                (
                    SELECT FLOOR(CA.NumericValue/BW.BinWidth)* BW.BinWidth AS BinFloor
                        ,BW.BinWidth
                        ,COUNT(CA.CustomerId)                            AS CustomerCount
                    FROM View_CustomerMetricDailyNumericAggregate CA
                    CROSS JOIN
                        (
                            SELECT CASE
                                    WHEN MIN(NumericValue) = 0 AND MAX(NumericValue) = 0 THEN MIN(1)
                                    WHEN MIN(NumericValue) = MAX(NumericValue) THEN MIN(NumericValue)
                                    ELSE CEILING((MAX(NumericValue) - MIN(NumericValue))/@BinNumber)
                                    END AS BinWidth
                            FROM View_CustomerMetricDailyNumericAggregate
                            WHERE CalendarDate = CAST(CURRENT_TIMESTAMP AS DATE)
                                AND MetricId = @MetricId
                        ) BW
                    WHERE CA.CalendarDate = CAST(CURRENT_TIMESTAMP AS DATE)
                    AND CA.MetricId = @MetricId
                    GROUP BY FLOOR(CA.NumericValue/BW.BinWidth) * BW.BinWidth
                        ,BW.BinWidth
                ) DT
                ORDER BY BinFloor
                ;
            ");

            // sp_CategoricalMetricBinning
            migrationBuilder.Sql(@"
            ALTER PROCEDURE sp_CategoricalMetricBinning
                @MetricId INTEGER
                ,@TopKValue INTEGER
                AS
                SELECT TOP (@TopKValue)
                    StringValue
                    ,CustomerCount
                FROM
                (
                    SELECT StringValue
                        ,COUNT(CustomerId) AS CustomerCount
                        FROM View_CustomerMetricDailyStringAggregate
                    WHERE CalendarDate = CAST(CURRENT_TIMESTAMP AS DATE)
                        AND MetricId = @MetricId
                    GROUP BY StringValue
                ) DT
                ORDER BY CustomerCount DESC
                ;
            ");
        }
    }
}
