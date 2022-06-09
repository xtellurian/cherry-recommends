ALTER PROCEDURE [dbo].[sp_OfferConversionRate]
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
            ,ISNULL(D.NonBaselineRedeemedCount, 0) AS NonBaselineRedeemedCount
            ,ISNULL(D.BaselineRedeemedCount, 0) AS BaselineRedeemedCount
            ,ISNULL(D.TotalCount, 0) AS TotalCount
            ,ISNULL(D.NonBaselineCount, 0) AS NonBaselineCount
            ,ISNULL(D.BaselineCount, 0) AS BaselineCount
            ,CASE WHEN D.TotalCount > 0 THEN CONVERT(FLOAT, D.RedeemedCount)/D.TotalCount ELSE 0 END AS ConversionRate
            ,CASE WHEN D.BaselineCount > 0 THEN CONVERT(FLOAT, D.NonBaselineRedeemedCount)/D.NonBaselineCount ELSE 0 END AS NonBaselineConversionRate
            ,CASE WHEN D.BaselineCount > 0 THEN CONVERT(FLOAT, D.BaselineRedeemedCount)/D.BaselineCount ELSE 0 END AS BaselineConversionRate
    FROM    Calendar C
    LEFT JOIN (
        SELECT  DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) AS [Date]
                ,SUM(CASE WHEN O.[State] = 'Redeemed' THEN 1 ELSE 0 END) AS RedeemedCount
                ,SUM(CASE WHEN O.[State] = 'Redeemed' AND R.BaselineItemId <> IRRI.ItemsId THEN 1 ELSE 0 END) AS NonBaselineRedeemedCount
                ,SUM(CASE WHEN O.[State] = 'Redeemed' AND R.BaselineItemId = IRRI.ItemsId THEN 1 ELSE 0 END) AS BaselineRedeemedCount
                ,COUNT(O.Id) AS TotalCount
                ,SUM(CASE WHEN IRRI.ItemsId <> R.BaselineItemId THEN 1 ELSE 0 END) AS NonBaselineCount
                ,SUM(CASE WHEN IRRI.ItemsId = R.BaselineItemId THEN 1 ELSE 0 END) AS BaselineCount
        FROM    Offers O
        INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
        INNER JOIN Recommenders R ON R.Id = IR.RecommenderId
        INNER JOIN ItemsRecommendationRecommendableItem IRRI ON IRRI.RecommendationsId = IR.Id
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
            ,NonBaselineRedeemedCount
            ,BaselineRedeemedCount
            ,TotalCount
            ,NonBaselineCount
            ,BaselineCount
            ,CASE WHEN TotalCount > 0 THEN CONVERT(FLOAT, RedeemedCount)/TotalCount ELSE 0 END AS ConversionRate
            ,CASE WHEN NonBaselineCount > 0 THEN CONVERT(FLOAT, NonBaselineRedeemedCount)/NonBaselineCount ELSE 0 END AS NonBaselineConversionRate
            ,CASE WHEN BaselineCount > 0 THEN CONVERT(FLOAT, BaselineRedeemedCount)/BaselineCount ELSE 0 END AS BaselineConversionRate
    FROM (
        SELECT  C.FirstOfWeek AS StartDate
                ,C.LastOfWeek AS EndDate
                ,ISNULL(SUM(CASE WHEN D.[State] = 'Redeemed' THEN 1 ELSE 0 END), 0) AS RedeemedCount
                ,ISNULL(SUM(CASE WHEN D.[State] = 'Redeemed' AND D.IsBaseline = 0 THEN 1 ELSE 0 END), 0) AS NonBaselineRedeemedCount
                ,ISNULL(SUM(CASE WHEN D.[State] = 'Redeemed' AND D.IsBaseline = 1 THEN 1 ELSE 0 END), 0) AS BaselineRedeemedCount
                ,ISNULL(COUNT(DISTINCT D.Id), 0) AS TotalCount
                ,ISNULL(SUM(CASE WHEN D.IsBaseline = 0 THEN 1 ELSE 0 END), 0) AS NonBaselineCount
                ,ISNULL(SUM(CASE WHEN D.IsBaseline = 1 THEN 1 ELSE 0 END), 0) AS BaselineCount
        FROM    Calendar C
        LEFT JOIN (
            SELECT  DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) AS [Date]
                    ,O.[State]
                    ,O.Id
                    ,CASE WHEN IRRI.ItemsId = R.BaselineItemId THEN 1 ELSE 0 END AS IsBaseline
            FROM    Offers O
            INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
            INNER JOIN Recommenders R ON R.Id = IR.RecommenderId
            INNER JOIN ItemsRecommendationRecommendableItem IRRI ON IRRI.RecommendationsId = IR.Id
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
            ,NonBaselineRedeemedCount
            ,BaselineRedeemedCount
            ,TotalCount
            ,NonBaselineCount
            ,BaselineCount
            ,CASE WHEN TotalCount > 0 THEN CONVERT(FLOAT, RedeemedCount)/TotalCount ELSE 0 END AS ConversionRate
            ,CASE WHEN NonBaselineCount > 0 THEN CONVERT(FLOAT, NonBaselineRedeemedCount)/NonBaselineCount ELSE 0 END AS NonBaselineConversionRate
            ,CASE WHEN BaselineCount > 0 THEN CONVERT(FLOAT, BaselineRedeemedCount)/BaselineCount ELSE 0 END AS BaselineConversionRate
    FROM (
        SELECT  C.FirstOfMonth AS StartDate
                ,C.LastOfMonth AS EndDate
                ,ISNULL(SUM(CASE WHEN D.[State] = 'Redeemed' THEN 1 ELSE 0 END), 0) AS RedeemedCount
                ,ISNULL(SUM(CASE WHEN D.[State] = 'Redeemed' AND D.IsBaseline = 0 THEN 1 ELSE 0 END), 0) AS NonBaselineRedeemedCount
                ,ISNULL(SUM(CASE WHEN D.[State] = 'Redeemed' AND D.IsBaseline = 1 THEN 1 ELSE 0 END), 0) AS BaselineRedeemedCount
                ,ISNULL(COUNT(DISTINCT D.Id), 0) AS TotalCount
                ,ISNULL(SUM(CASE WHEN D.IsBaseline = 0 THEN 1 ELSE 0 END), 0) AS NonBaselineCount
                ,ISNULL(SUM(CASE WHEN D.IsBaseline = 1 THEN 1 ELSE 0 END), 0) AS BaselineCount
        FROM    Calendar C
        LEFT JOIN (
            SELECT  DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) AS [Date]
                    ,O.[State]
                    ,O.Id
                    ,CASE WHEN IRRI.ItemsId = R.BaselineItemId THEN 1 ELSE 0 END AS IsBaseline
            FROM    Offers O
            INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
            INNER JOIN Recommenders R ON R.Id = IR.RecommenderId
            INNER JOIN ItemsRecommendationRecommendableItem IRRI ON IRRI.RecommendationsId = IR.Id
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
END