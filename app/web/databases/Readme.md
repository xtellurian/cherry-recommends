# Databases

This directory contains sqlite databases.

## Vizualising the database

```sh
cd databases
npm install
brew install graphviz # dependency
npm run viz # produces db.svg based on signalbox.db
```

## Database schema

| Database Schema | Table Name | Object Type | Description | Obsolete |
| ----------- | ----------- | ----------- | ----------- | ----------- |
| dbo | __EFMigrationsHistory | Base Table | Systems generated table inserted by `dotnet ef` | Y |
| dbo | ParameterParameterSetRecommender | Base Table | Link table between parameter and parameter set recommender | N |
| dbo | Parameters | Base Table | Parameter reference table | N |
| dbo | ParameterSetRecommendations | Base Table | Link table between parameter and recommender | N |
| dbo | RecommendableItems | Base Table | Recommender's possible numeric categorical options | N |
| dbo | RecommendationCorrelators | Base Table | Unique Id to a recommendation | N |
| dbo | RecommendationDestinations | Base Table | Send to external systems | N |
| dbo | Recommenders | Base Table | Recommender reference table | N |
| dbo | RecommenderTargetVariableValue | Base Table |  | Y |
| dbo | RewardSelectors | Base Table |  | Y |
| dbo | Segments | Base Table | Group customer population together | N |
| dbo | SegmentTrackedUser | Base Table | Links which segment the customer belongs | N |
| dbo | Touchpoints | Base Table |  | Y |
| dbo | TrackedUserActions | Base Table |  | Y |
| dbo | TrackedUserEvents | Base Table | List of the customer events | N |
| dbo | TrackedUsers | Base Table | List of the environment's customer table | N |
| dbo | TrackedUserTouchpoints | Base Table |  | Y |
| dbo | TrackUserSystemMaps | Base Table | Maps external system id to tracked user id | N |
| dbo | WebhookReceivers | Base Table | Endpoint that receives webhook | N |
| dbo | View_MaxHistoricTrackedUserFeatureVersion | View | Latest feature value version | N |
| sys | database_firewall_rules | View | System table | N |