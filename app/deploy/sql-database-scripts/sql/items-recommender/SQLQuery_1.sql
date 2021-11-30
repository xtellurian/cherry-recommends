-- this should run before the migration

DELETE from dbo.InvokationLogEntry

-- these may or may not be needed, depending on the environment.
-- DELETE from dbo.ProductRecommendations
-- DELETE from dbo.ProductRecommenders
-- DELETE from dbo.Products