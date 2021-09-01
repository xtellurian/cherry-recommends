-- this should run before the migration
DELETE from dbo.InvokationLogEntry
DELETE from dbo.ProductRecommendations

-- this may or may not be needed
-- DELETE from dbo.Products