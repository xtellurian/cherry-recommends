using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_offer_conversionrate : Migration
    {
        private string sp_OfferConversionRate = @"
            CREATE PROCEDURE [dbo].[sp_OfferConversionRate]
                @id BIGINT
                ,@period INTEGER 
                ,@startDate DATE
                ,@environmentId BIGINT NULL
            AS
            DECLARE @today DATE = DATEADD(DAY, 0, DATEDIFF(DAY, 0, GETDATE()));
            DECLARE @beforeStartDate DATE = DATEADD(DAY, -1, @startDate); -- for improved filtering only
            IF (@period = 0) -- Daily
            BEGIN
                SELECT  C.CalendarDate AS StartDate
                        ,C.CalendarDate AS EndDate
                        ,ISNULL(D.RedeemedCount, 0) AS RedeemedCount
                        ,ISNULL(D.TotalCount, 0) AS TotalCount
                        ,CASE WHEN TotalCount > 0 THEN CONVERT(FLOAT, D.RedeemedCount)/D.TotalCount ELSE 0 END AS ConversionRate
                FROM    Calendar C
                LEFT JOIN (
                    SELECT  DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) AS [Date]
                            ,SUM(CASE WHEN O.[State] = 'Redeemed' THEN 1 ELSE 0 END) AS RedeemedCount
                            ,COUNT(O.Id) AS TotalCount
                    FROM    Offers O
                    INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
                    WHERE   IR.RecommenderId = @id
                            AND O.Created >= @beforeStartDate
                            AND ((O.EnvironmentId IS NULL AND @environmentId IS NULL) OR 
                                O.EnvironmentId = @environmentId)
                    GROUP BY
                            DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0)
                ) D ON D.[Date] = C.CalendarDate
                WHERE   C.CalendarDate >= @startDate
                        AND C.CalendarDate <= @today
            END
            ELSE IF (@period = 1) -- Weekly
            BEGIN
                DECLARE @firstOfWeek DATE;

                SELECT  TOP 1 @firstOfWeek = C.FirstOfWeek
                FROM    Calendar C
                WHERE   @startDate BETWEEN C.FirstOfWeek AND C.LastOfWeek;

                SELECT  StartDate
                        ,EndDate
                        ,RedeemedCount
                        ,TotalCount
                        ,CASE WHEN TotalCount > 0 THEN CONVERT(FLOAT, RedeemedCount)/TotalCount ELSE 0 END AS ConversionRate
                FROM (
                    SELECT  C.FirstOfWeek AS StartDate
                            ,C.LastOfWeek AS EndDate
                            ,ISNULL(SUM(CASE WHEN D.[State] = 'Redeemed' THEN 1 ELSE 0 END), 0) AS RedeemedCount
                            ,ISNULL(COUNT(DISTINCT D.Id), 0) AS TotalCount
                    FROM    Calendar C
                    LEFT JOIN (
                        SELECT  DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) AS [Date]
                                ,O.[State]
                                ,O.Id
                        FROM    Offers O
                        INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
                        WHERE   IR.RecommenderId = @id
                                AND O.Created >= @firstOfWeek
                                AND ((O.EnvironmentId IS NULL AND @environmentId IS NULL) OR O.EnvironmentId = @environmentId)
                    ) D ON D.[Date] = C.CalendarDate
                    WHERE   C.CalendarDate >= @firstOfWeek
                            AND C.CalendarDate <= @today
                    GROUP BY
                            C.FirstOfWeek
                            ,C.LastOfWeek
                ) D
            END
            ELSE IF (@period = 2) -- Monthly
            BEGIN
                DECLARE @firstOfMonth DATE;

                SELECT  TOP 1 @firstOfMonth = C.FirstOfMonth
                FROM    Calendar C
                WHERE   @startDate BETWEEN C.FirstOfMonth AND C.LastOfMonth;

                SELECT  StartDate
                        ,EndDate
                        ,RedeemedCount
                        ,TotalCount
                        ,CASE WHEN TotalCount > 0 THEN CONVERT(FLOAT, RedeemedCount)/TotalCount ELSE 0 END AS ConversionRate
                FROM (
                    SELECT  C.FirstOfMonth AS StartDate
                            ,C.LastOfMonth AS EndDate
                            ,ISNULL(SUM(CASE WHEN D.[State] = 'Redeemed' THEN 1 ELSE 0 END), 0) AS RedeemedCount
                            ,ISNULL(COUNT(DISTINCT D.Id), 0) AS TotalCount
                    FROM    Calendar C
                    LEFT JOIN (
                        SELECT  DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) AS [Date]
                                ,O.[State]
                                ,O.Id
                        FROM    Offers O
                        INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
                        WHERE   IR.RecommenderId = @id
                                AND O.Created >= @firstOfMonth
                                AND ((O.EnvironmentId IS NULL AND @environmentId IS NULL) OR O.EnvironmentId = @environmentId)
                    ) D ON D.[Date] = C.CalendarDate
                    WHERE   C.CalendarDate >= @firstOfMonth
                            AND C.CalendarDate <= @today
                    GROUP BY
                            C.FirstOfMonth
                            ,C.LastOfMonth
                ) D
            END";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sp_OfferConversionRate);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[sp_OfferConversionRate]");
        }
    }
}
