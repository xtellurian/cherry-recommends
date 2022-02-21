using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_metrichistogram : Migration
    {
        private readonly string sp_GetNumericMetricBinning = @"
            CREATE PROCEDURE dbo.sp_NumericMetricBinning
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
                FROM [dbo].[View_CustomerMetricDailyNumericAggregate] CA
                CROSS JOIN
                    (
                        SELECT CASE
                                WHEN MIN(NumericValue) = 0 AND MAX(NumericValue) = 0 THEN MIN(1)
                                WHEN MIN(NumericValue) = MAX(NumericValue) THEN MIN(NumericValue)
                                ELSE CEILING((MAX(NumericValue) - MIN(NumericValue))/@BinNumber)
                                END AS BinWidth
                        FROM [dbo].[View_CustomerMetricDailyNumericAggregate]
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
        ";

        private readonly string sp_GetCategoricalMetricBinning = @"
            CREATE PROCEDURE dbo.sp_CategoricalMetricBinning
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
                    FROM [dbo].[View_CustomerMetricDailyStringAggregate]
                WHERE CalendarDate = CAST(CURRENT_TIMESTAMP AS DATE)
                    AND MetricId = @MetricId
                GROUP BY StringValue
            ) DT
            ORDER BY CustomerCount DESC
            ;
        ";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sp_GetNumericMetricBinning);
            migrationBuilder.Sql(sp_GetCategoricalMetricBinning);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop procedure dbo.sp_NumericMetricBinning");
            migrationBuilder.Sql("drop procedure dbo.sp_CategoricalMetricBinning");
        }
    }
}
