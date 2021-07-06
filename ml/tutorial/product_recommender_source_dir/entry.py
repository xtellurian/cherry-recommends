

# this is an actual entry script
import json
import pickle
import random
import os

possible_outcomes = [
    {
        'productId': 5,
    },
    {
        'productId': 6,
    },
    {
        'productId': 7,
    },
]
def init():
    global model_parameters
    with open(os.path.join(os.getenv('AZUREML_MODEL_DIR'), 'product_recommender_tutorial.pkl'), 'rb') as handle:
        model_parameters = pickle.load(handle)['parameters']

# request should have: payload: {},  version: str
def run(request):
    print(request)
    body = json.loads(request)
    version = body['version']
    ## check we can handle the version
    print(f'Version: {version}')

    # Run inference
    recommendation = recommend(body['payload'])
    print('recommendation:', recommendation)
   
    return recommendation

# assuming this is a paremeter-set recommender
# payload is json with arguments, parameters (both objects)
def recommend(r):
    # sort the offers by ID
    arguments = r['arguments']
    commonUserId = r['commonUserId']
    print(arguments)
    # you can load parameters from the model if you need ot.
    print('model params:', model_parameters['weights'])
    # random.choices() returns a list of k length
    return random.choices(possible_outcomes, model_parameters['weights'], k=1)[0] 

