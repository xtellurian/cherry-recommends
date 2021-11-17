InvokeCategoricalOptimiser invokes a new recommender. It takes
- name
- id
- payload (time varying inputs incorporating items, features, and arguments
-- items (The items that can be recommended by this recommender. This does not have to be the same as the initial_items_list)
-- features (customer descriptors that can include "targeted_features)
-- arguments (direct customer inputs from the client side)



Sample input is
{
	"id": 1,
	"name": "NameOfRecommender",
  "version": "prod_1",
  "payload": {
    "features": {
			"targeted_features": {
				"lifetime_revenue":2,
				"account_status": 1
			},
			"non_targeted_features": {
				"gender":"Male"
			}
		},
		"items":[
			{
				"ItemId": "Test1"
			},
			{
				"ItemId": "Test2"
			},
			{
				"ItemId": "Test4"
			}

		]
	}
}