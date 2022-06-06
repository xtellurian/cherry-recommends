CREATE PROCEDURE [dbo].[sp_ARPO]
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
            ,ISNULL(D.TotalGrossRevenue, 0) AS TotalGrossRevenue
            ,ISNULL(D.MeanGrossRevenue, 0) AS MeanGrossRevenue
            ,ISNULL(D.BaselineMeanGrossRevenue, 0) AS BaselineMeanGrossRevenue
            ,ISNULL(D.OfferCount, 0) AS OfferCount
            ,ISNULL(D.DistinctCustomerCount, 0) AS DistinctCustomerCount
    FROM    Calendar C
    LEFT JOIN (
        SELECT  DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) AS [Date]
                ,SUM(O.GrossRevenue) AS TotalGrossRevenue
                ,AVG(O.GrossRevenue) AS MeanGrossRevenue
                ,ISNULL(AVG(CASE WHEN R.BaselineItemId = O.RedeemedPromotionId THEN O.GrossRevenue END), 0) AS BaselineMeanGrossRevenue
                ,COUNT(O.Id) AS OfferCount
                ,COUNT(DISTINCT IR.CustomerId) AS DistinctCustomerCount
        FROM    Offers O
        INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
        INNER JOIN Recommenders R ON R.Id = IR.RecommenderId
        WHERE   IR.RecommenderId = @id
                AND O.Created >= @beforeStartDate
                AND O.[State] = 'Redeemed'
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

    SELECT  C.FirstOfWeek AS StartDate
            ,C.LastOfWeek AS EndDate
            ,ISNULL(SUM(D.GrossRevenue), 0) AS TotalGrossRevenue
            ,ISNULL(AVG(D.GrossRevenue), 0) AS MeanGrossRevenue
            ,ISNULL(AVG(CASE WHEN D.IsBaseline = 1 THEN D.GrossRevenue END), 0) AS BaselineMeanGrossRevenue
            ,COUNT(D.Id) AS OfferCount
            ,COUNT(DISTINCT D.CustomerId) AS DistinctCustomerCount
    FROM    Calendar C
    LEFT JOIN (
        SELECT  DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) AS [Date]
                ,O.GrossRevenue
                ,O.Id
                ,IR.CustomerId
                ,CASE WHEN R.BaselineItemId = O.RedeemedPromotionId THEN 1 ELSE 0 END AS IsBaseline
        FROM    Offers O
        INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
        INNER JOIN Recommenders R ON R.Id = IR.RecommenderId
        WHERE   IR.RecommenderId = @id
                AND O.Created >= @firstOfWeek
                AND O.[State] = 'Redeemed'
                AND ((O.EnvironmentId IS NULL AND @environmentId IS NULL) OR O.EnvironmentId = @environmentId)
    ) D ON D.[Date] = C.CalendarDate
    WHERE   C.CalendarDate >= @firstOfWeek
            AND C.CalendarDate <= @today
    GROUP BY
            C.FirstOfWeek
            ,C.LastOfWeek
END
ELSE IF (@period = 2) -- Monthly
BEGIN
    DECLARE @firstOfMonth DATE;

    SELECT  TOP 1 @firstOfMonth = C.FirstOfMonth
    FROM    Calendar C
    WHERE   @startDate BETWEEN C.FirstOfMonth AND C.LastOfMonth;

    SELECT  C.FirstOfMonth AS StartDate
            ,C.LastOfMonth AS EndDate
            ,ISNULL(SUM(D.GrossRevenue), 0) AS TotalGrossRevenue
            ,ISNULL(AVG(D.GrossRevenue), 0) AS MeanGrossRevenue
            ,ISNULL(AVG(CASE WHEN D.IsBaseline = 1 THEN D.GrossRevenue END), 0) AS BaselineMeanGrossRevenue
            ,COUNT(D.Id) AS OfferCount
            ,COUNT(DISTINCT D.CustomerId) AS DistinctCustomerCount
    FROM    Calendar C
    LEFT JOIN (
        SELECT  DATEADD(DAY, DATEDIFF(DAY, 0, O.Created), 0) AS [Date]
                ,O.GrossRevenue
                ,O.Id
                ,IR.CustomerId
                ,CASE WHEN R.BaselineItemId = O.RedeemedPromotionId THEN 1 ELSE 0 END AS IsBaseline
        FROM    Offers O
        INNER JOIN ItemsRecommendations IR ON IR.Id = O.RecommendationId
        INNER JOIN Recommenders R ON R.Id = IR.RecommenderId
        WHERE   IR.RecommenderId = @id
                AND O.Created >= @firstOfMonth
                AND O.[State] = 'Redeemed'
                AND ((O.EnvironmentId IS NULL AND @environmentId IS NULL) OR O.EnvironmentId = @environmentId)
    ) D ON D.[Date] = C.CalendarDate
    WHERE   C.CalendarDate >= @firstOfMonth
            AND C.CalendarDate <= @today
    GROUP BY
            C.FirstOfMonth
            ,C.LastOfMonth
END