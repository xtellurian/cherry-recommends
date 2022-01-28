CreateCategoricalOptimiser creates a new optimiser. It takes the following required arguments:

- name
- id
- items (The initial items created in the recommender - this forms the initial distribution)
- nItemsToRecommend (Number of items to recommend)
- defaultItemCommonId (This needs to come from initial_items_list and have same length as NumberOfItems)

Sample input is
{
	"id": "Id1",
	"name": "NewRecommender",
	"nItemsToRecommend": 1,
	"baselineItem": {"commonId": "Test1"},
	"items": [{
		"commonId": "Test1"
	}, {
		"commonId": "Test2"
	}]
}

Output:
- HttpResponse
{
    "PartitionKey": "newtenant",
    "RowKey": "20",
    "tenant": "newtenant",
    "id": 20,
    "name": "NewRecommender",
    "nItemsToRecommend": 1,
    "nPopulations": 1,
    "defaultItem": {
        "CommonId": "Test1",
    },
    "defaultItemCommonId": "Test1"
}
- Azure Table Storage row
- Azure Blob Storage 
{
  "populations": [
    {
      "population_id": 1,
      "default_item": { "commonId": "Test1" },
      "items": [
        { "commonId": "Test1" },
        { "commonId": "Test2" },
        { "commonId": "Test3" }
      ],
      "probabilities": { "Test1": 0.6, "Test2": 0.2, "Test3": 0.2 }
    }
  ],
  "nItemsToRecommend": 1,
  "defaultItem": { "commonId": "Test1" }
}



InvokeCategoricalOptimiser invokes a new recommender. It takes
- name
- id
- payload (time varying inputs incorporating items, metrics, and arguments
-- items (The items that can be recommended by this recommender. This does not have to be the same as the initial_items_list)
-- metrics (customer descriptors that can include "targeted_metrics")
-- arguments (direct customer inputs from the client side)



Output:
- HttpResponse
{
    "scoredItems": [
        {
            "commonId": "Test1",
            "score": 0.6
        }
    ]
}