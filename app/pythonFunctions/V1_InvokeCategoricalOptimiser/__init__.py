import collections
import logging
import json
import azure.functions as func
import numpy as np
import pandas as pd

from shared_code import responses, request_validation, populations


def main(req: func.HttpRequest, inputblob: str, record: str) -> func.HttpResponse:

    logging.info('Invoking a categorical optimiser.')
    req_body = {}
    try:
        req_body = req.get_json()
    except:
        # you can return as error with this method.
        return responses.bad_request("Body of request must be JSON")

    record = json.loads(record)

    # get data you require from the caller
    is_valid, reason = request_validation.is_valid_invoke_optimiser_request_body(
        req_body)

    if not is_valid:
        return responses.bad_request(reason)

    payload = req_body['payload']
    items = payload['items']
    # Pull the relevant distribution for this model
    collection = populations.PopulationDistributionCollection(
        deserialized_dict=json.loads(inputblob))

    # Get the population id given specific personalisation features
    population_id = populations.calculate_population_id(payload)

    relevant_population = collection.get_population(population_id)

    if relevant_population is None:
        logging.warning("Population not found")
        relevant_population = populations.PopulationItemDistribution(
            population_id, collection.default_item, items)
    else:
        logging.info(f'Found population, id = {population_id}')

    # Draw the items to recommend
    chosen_items = relevant_population.choose_items(
        items=items, n_items=collection.n_items_to_recommend)
    # set the scores
    for i in chosen_items:
        i['score'] = relevant_population.get_item_probability(i['commonId'])
    response = {"scoredItems": chosen_items}
    return responses.success(response)
