CREATE PROCEDURE [dbo].[sp_OfferSensitivityCurve]
    @id BIGINT
    ,@startDate DATE
AS
SELECT  PromotionId
        ,PromotionName
        ,TotalGrossRevenue
        ,MeanGrossRevenue
        ,TotalCount
        ,RedeemedCount
        ,CASE WHEN TotalCount > 0 THEN CONVERT(FLOAT, RedeemedCount)/TotalCount ELSE 0 END AS ConversionRate
FROM (
    SELECT  P.Id AS PromotionId
            ,P.Name AS PromotionName
            ,ISNULL(SUM(D.GrossRevenue), 0) AS TotalGrossRevenue
            ,ISNULL(AVG(D.GrossRevenue), 0) AS MeanGrossRevenue
            ,ISNULL(COUNT(D.ItemId), 0) AS TotalCount
            ,ISNULL(SUM(CASE WHEN D.[State] = 'Redeemed' AND D.ItemId = P.Id AND D.ItemId = D.RedeemedPromotionId THEN 1 ELSE 0 END), 0) AS RedeemedCount
    FROM    Recommenders R 
    INNER JOIN ItemsRecommenderRecommendableItem RI ON RI.RecommendersId = R.Id
    INNER JOIN RecommendableItems P ON P.Id = RI.ItemsId
    LEFT JOIN (
        SELECT  IR.Id AS RecommendationId
                ,IR.RecommenderId
                ,IRRI.ItemsId AS ItemId
                ,O.Id
                ,O.[State]
                ,O.GrossRevenue
                ,O.RedeemedPromotionId
        FROM ItemsRecommendations IR
        INNER JOIN ItemsRecommendationRecommendableItem IRRI ON IRRI.RecommendationsId = IR.Id
        INNER JOIN Offers O ON O.RecommendationId = IR.Id 
        WHERE   O.Created >= @startDate
    ) D ON D.RecommenderId = R.Id AND D.ItemId = P.Id
    WHERE   R.Id = @id
    GROUP BY
            P.Id
            ,P.Name
) D