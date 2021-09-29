import logging
import json
import random
import azure.functions as func
import numpy as np
import pandas as pd

def error_response(error, status_code=400):
    logging.error(error)
    j = json.dumps({'error': error})
    return func.HttpResponse(j, status_code=status_code, headers={'Content-Type': 'application/json'})


def success_response(content):
    j = json.dumps(content)
    return func.HttpResponse(j, headers={'Content-Type': 'application/json'})


def is_valid_request_body(req_body):

    if 'payload' not in req_body:
        return (False, "the payload property is required")
    if ('features' not in req_body['payload']) & ('arguments' not in req_body['payload']):
        return (False, "At least one feature or argument is required")
    else:
        return (True, None)

    return (True, None)

def distribution_update_features_are_items(user_features, live_distribution):

    for key in user_features:

        if user_features[key] in live_distribution['ItemId'].tolist():
            entry_index = np.where(live_distribution['ItemId'] == user_features[key])[0][0]
            live_distribution['Probability'][entry_index] = live_distribution['Probability'][entry_index] + 0.2
        else:
            ## Not an error but worth tracking
            logging.info('Couldnt update live distribution ' + key)

    return live_distribution



def main(req: func.HttpRequest, inputblob: str, record: str) -> func.HttpResponse:
    
    logging.info('Invoking a categorical optimiser.')

    req_body = {}
    try:
        req_body = req.get_json()
    except:
        # you can return as error with this method.
        return error_response("Body of request must be JSON")

    try:
            
        # get data you require from the caller
        is_valid, reason = is_valid_request_body(req_body)
        if not is_valid:
            return error_response(reason)

        user_features = req_body['payload']['features']
        
        logging.info(user_features)

        population_id = user_features['PopulationId'] 

        # the model data saved when created
        model_parameters = json.loads(inputblob)
        # the id of the optimiser model
        id = req.route_params.get('id')

        if population_id in model_parameters['SourceDistributionList'].keys():
            source_distribution = pd.DataFrame.from_dict(model_parameters['SourceDistributionList'][population_id])
            logging.info('Loaded distribution')
        else: 
            source_distribution = pd.DataFrame.from_dict(model_parameters['SourceDistributionList']["A"])
            logging.info('Using default distribution')
        number_of_items = model_parameters['NumberOfItems']

        ###### SECTION NEEDED ######
        #UPDATE LIVEDDISTRIBUTION AND PROBABILITY WITH NEW INCOMING ITEMS
        live_distribution = source_distribution
        live_distribution = distribution_update_features_are_items(user_features, live_distribution)

        ## Ensure live distribution is a true probability distribution
        live_distribution['Probability'] = live_distribution['Probability']/sum(live_distribution['Probability'])
        recommendedProductArray = np.random.choice(live_distribution['ItemId'], size=number_of_items, replace=False, p = live_distribution['Probability'])
        logging.info('Got the product array')

        productIdList = recommendedProductArray.tolist()
        recommendedProducts = []
        
        for i in range(number_of_items):
            recommendedProducts.append({'itemCommonId':productIdList[i],'Score':number_of_items-i})

        response = {"recommendedProducts": recommendedProducts}
        return success_response(response)

    except:
        return error_response("An error occurred in the model")
