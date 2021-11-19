import logging
import json

import azure.functions as func
from shared_code import request_validation, responses, populations

# this is the function that is public. record is an "output binding"


def main(req: func.HttpRequest, record: func.Out[str],  outputBlob: func.Out[str]) -> func.HttpResponse:
    logging.info("Creating a Categorical Optimiser")
    req_body = {}
    try:
        req_body = req.get_json()
    except:
        # you can return as error with this method.
        logging.info(req.get_body().decode('utf-8'))
        return responses.bad_request("Body of request must be JSON xx")

    # get data you require from the caller
    is_valid, reason = request_validation.is_valid_create_optimiser_request_body(
        req_body)
    if not is_valid:
        return responses.bad_request(reason)

    id = req_body['id']
    name = req_body['name']
    tenant = req.route_params.get('tenant')
    n_items_to_recommend = req_body["nItemsToRecommend"]

    # Compute number of popluations (used for copies of distributions)
    # TODO: make these populations real
    population_ids = [populations.constant_population_id]

    # create some intitial distribution used in model
    items = req_body['items']
    default_item = req_body['defaultItem']

    distributions = populations.PopulationDistributionCollection(
        default_item=default_item, n_items_to_recommend=n_items_to_recommend)

    for p in population_ids:
        # distribution_specific_pop = populations.new_distribution_for_population(items, default_item_id)
        pop_distribution = populations.PopulationItemDistribution(
            p, default_item, items)
        pop_distribution.normalize_probabilities()
        distributions.add(pop_distribution)

    # save it to a blob for use later.
    outputBlob.set(json.dumps(distributions.to_dict()))
    logging.info("Distribution created and stored")

    # create a dict of the data you need to store
    record_data = {
        "PartitionKey": tenant,
        "RowKey": id,
        "tenant": tenant,
        "id": id,
        "name": name,
        "nItemsToRecommend": n_items_to_recommend,
        "nPopulations": len(population_ids),
        "defaultItem": default_item,
        "defaultItemCommonId": default_item['commonId']
    }

    # this writes a row to the table.
    record.set(json.dumps(record_data))

    # this returns the row data to the HTTP caller.
    return responses.success(record_data)
