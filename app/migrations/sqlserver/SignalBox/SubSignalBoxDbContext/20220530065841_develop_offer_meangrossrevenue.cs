using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_offer_meangrossrevenue : Migration
    {
        public static readonly string sp_OfferMeanGrossRevenue = @"
            CREATE PROCEDURE [dbo].[sp_OfferMeanGrossRevenue]
                @id BIGINT
                ,@type INTEGER 
                ,@startDate DATETIMEOFFSET
                ,@state NVARCHAR(20)
            AS
            IF (@type = 0) -- Daily
            BEGIN
                SELECT  AD.StartDate
                        ,AD.EndDate
                        ,AD.TotalGrossRevenue
                        ,AD.MeanGrossRevenue
                        ,ISNULL(BD.BaselineMeanGrossRevenue, 0) AS BaselineMeanGrossRevenue
                        ,AD.OfferCount
                        ,AD.DistinctCustomerCount
                FROM (
                    SELECT  
                        C.CalendarDate AS StartDate
                        ,C.CalendarDate AS EndDate
                        ,ISNULL(SUM(O.GrossRevenue), 0) AS TotalGrossRevenue
                        ,ISNULL(AVG(O.GrossRevenue), 0) AS MeanGrossRevenue
                        ,COUNT(DISTINCT O.Id) AS OfferCount
                        ,COUNT(DISTINCT IR.CustomerId) AS DistinctCustomerCount
                    FROM Calendar C
                    LEFT JOIN Offers O ON DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) = C.CalendarDate -- Date only comparison
                    LEFT JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
                    WHERE   
                        C.[CalendarDate] >= @startDate
                        AND C.[CalendarDate] <= GETDATE()
                        AND (O.Id IS NULL OR (IR.RecommenderId = @id AND O.[State] = @state))
                    GROUP BY
                        C.CalendarDate
                ) AD
                LEFT JOIN (
                    SELECT 
                        C.CalendarDate AS StartDate
                        ,C.CalendarDate AS EndDate
                        ,AVG(O.GrossRevenue) AS BaselineMeanGrossRevenue
                    FROM   Calendar C
                    INNER JOIN Offers O ON DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) = C.CalendarDate -- Date only comparison
                    INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
                    INNER JOIN Recommenders R ON R.Id = IR.RecommenderId AND R.BaselineItemId = O.RedeemedPromotionId
                    WHERE   
                        C.[CalendarDate] >= @startDate
                        AND C.[CalendarDate] <= GETDATE()
                        AND R.Id = @id
                    GROUP BY
                        C.CalendarDate
                ) BD ON BD.StartDate = AD.StartDate AND BD.EndDate = AD.EndDate
            END
            ELSE IF (@type = 1) -- Weekly
            BEGIN
                SELECT  AD.StartDate
                        ,AD.EndDate
                        ,AD.TotalGrossRevenue
                        ,AD.MeanGrossRevenue
                        ,ISNULL(BD.BaselineMeanGrossRevenue, 0) AS BaselineMeanGrossRevenue
                        ,AD.OfferCount
                        ,AD.DistinctCustomerCount
                FROM (
                    SELECT
                        D.FirstOfWeek AS StartDate
                        ,D.LastOfWeek AS EndDate
                        ,ISNULL(SUM(O.GrossRevenue), 0) AS TotalGrossRevenue
                        ,ISNULL(AVG(O.GrossRevenue), 0) AS MeanGrossRevenue
                        ,COUNT(O.Id) AS OfferCount
                        ,COUNT(DISTINCT IR.CustomerId) AS DistinctCustomerCount
                    FROM (
                        SELECT DISTINCT
                            C.FirstOfWeek
                            ,C.LastOfWeek
                            ,OD.Id
                        FROM
                            Calendar C
                            LEFT JOIN (
                                SELECT
                                    O.Id,
                                    O.Created
                                FROM Offers O
                                INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
                                WHERE
                                    O.[State] = @state
                                    AND IR.RecommenderId = @id
                            ) OD ON DATEADD(DAY, DATEDIFF(DAY, 0, OD.Created), 0) BETWEEN C.FirstOfWeek AND C.LastOfWeek
                        WHERE
                            C.CalendarDate >= @startDate
                            AND C.CalendarDate <= GETDATE()
                    ) D
                    LEFT JOIN Offers O ON O.Id = D.Id
                    LEFT JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
                    GROUP BY
                        D.FirstOfWeek
                        ,D.LastOfWeek
                ) AD
                LEFT JOIN (
                    SELECT 
                        C.FirstOfWeek AS StartDate
                        ,C.LastOfWeek AS EndDate
                        ,AVG(O.GrossRevenue) AS BaselineMeanGrossRevenue
                    FROM   Calendar C
                    INNER JOIN Offers O ON DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) BETWEEN C.FirstOfWeek AND C.LastOfWeek
                    INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
                    INNER JOIN Recommenders R ON R.Id = IR.RecommenderId AND R.BaselineItemId = O.RedeemedPromotionId
                    WHERE   
                        C.[CalendarDate] >= @startDate
                        AND C.[CalendarDate] <= GETDATE()
                        AND R.Id = @id
                    GROUP BY
                        C.FirstOfWeek
                        ,C.LastOfWeek
                ) BD ON BD.StartDate = AD.StartDate AND BD.EndDate = AD.EndDate
            END
            ELSE IF (@type = 2) -- Monthly
            BEGIN
                SELECT  AD.StartDate
                        ,AD.EndDate
                        ,AD.TotalGrossRevenue
                        ,AD.MeanGrossRevenue
                        ,ISNULL(BD.BaselineMeanGrossRevenue, 0) AS BaselineMeanGrossRevenue
                        ,AD.OfferCount
                        ,AD.DistinctCustomerCount
                FROM (
                    SELECT
                        D.FirstOfMonth AS StartDate
                        ,D.LastOfMonth AS EndDate
                        ,ISNULL(SUM(O.GrossRevenue), 0) AS TotalGrossRevenue
                        ,ISNULL(AVG(O.GrossRevenue), 0) AS MeanGrossRevenue
                        ,COUNT(O.Id) AS OfferCount
                        ,COUNT(DISTINCT IR.CustomerId) AS DistinctCustomerCount
                    FROM (
                        SELECT DISTINCT
                            C.FirstOfMonth
                            ,C.LastOfMonth
                            ,OD.Id
                        FROM
                            Calendar C
                            LEFT JOIN (
                                SELECT
                                    O.Id,
                                    O.Created
                                FROM Offers O
                                INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
                                WHERE
                                    O.[State] = @state
                                    AND IR.RecommenderId = @id
                            ) OD ON DATEADD(DAY, DATEDIFF(DAY, 0, OD.Created), 0) BETWEEN C.FirstOfMonth AND C.LastOfMonth
                        WHERE
                            C.CalendarDate >= @startDate
                            AND C.CalendarDate <= GETDATE()
                    ) D
                    LEFT JOIN  Offers O ON O.Id = D.Id
                    LEFT JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
                    GROUP BY
                        D.FirstOfMonth
                        ,D.LastOfMonth
                ) AD
                LEFT JOIN (
                    SELECT 
                        C.FirstOfMonth AS StartDate
                        ,C.LastOfMonth AS EndDate
                        ,AVG(O.GrossRevenue) AS BaselineMeanGrossRevenue
                    FROM   Calendar C
                    INNER JOIN Offers O ON DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) BETWEEN C.FirstOfMonth AND C.LastOfMonth
                    INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
                    INNER JOIN Recommenders R ON R.Id = IR.RecommenderId AND R.BaselineItemId = O.RedeemedPromotionId
                    WHERE   
                        C.[CalendarDate] >= @startDate
                        AND C.[CalendarDate] <= GETDATE()
                        AND R.Id = @id
                    GROUP BY
                        C.FirstOfMonth
                        ,C.LastOfMonth
                ) BD ON BD.StartDate = AD.StartDate AND BD.EndDate = AD.EndDate
            END";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sp_OfferMeanGrossRevenue);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[sp_OfferMeanGrossRevenue]");
        }
    }
}
