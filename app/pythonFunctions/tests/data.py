import uuid
import json

from shared_code import populations as pops


def test_create_body():
    return {
        "id": 1,
        "name": "NewRecommender",
        "nItemsToRecommend": 1,
        "defaultItem": {"commonId": "Test1"},
        "items": [{
            "commonId": "Test1"
        }, {
            "commonId": "Test2"
        }
        ]
    }


def test_invokation_payload():
    return {
        'features': {

        },
        'arguments': {

        },
        'parameterBounds': [],
        'items': [{
            'commonId': 'test_default_item_id'
        }, {
            'commonId': 'test_frequent_id'
        }, {
            'commonId': str(uuid.uuid1())
        }]
    }


def test_serialized_distribution():
    default_item = {'commonId': 'test_default_item_id'}
    items = [{
        'commonId': str(uuid.uuid1())
    }, {
        'commonId': str(uuid.uuid1())
    }, {
        'commonId': str(uuid.uuid1())
    }]
    collection = pops.PopulationDistributionCollection(default_item=default_item, n_items_to_recommend=1,
                                                       populations=[
                                                           pops.PopulationItemDistribution(
                                                               pops.constant_population_id, default_item, items=items)
                                                       ])

    return json.dumps(collection.to_dict())
