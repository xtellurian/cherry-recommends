

# this is an actual entry script
import json
import pickle
import random
import os

possible_outcomes = [
    {
        'max': 5,
        'min': 3,
        'total': 7
    },
    {
        'max': 10,
        'min': 5,
        'total': 55
    },
    {
        'max': 20,
        'min': 10,
        'total': 15
    }
]
def init():
    global model_parameters
    with open(os.path.join(os.getenv('AZUREML_MODEL_DIR'), 'tutorial_model.pkl'), 'rb') as handle:
        model_parameters = pickle.load(handle)['parameters']

# request should have: payload: {},  version: str
def run(request):
    print(request)
    body = json.loads(request)
    version = body['version']
    ## check we can handle the version
    if version is str:
        print(f'Can handle version {version}')

    # Run inference
    recommendation = recommend(body['payload'])
    print('recommendation:', recommendation)
    wrapper = {
        'recommendedParameters': recommendation
    }
    return wrapper

# assuming this is a paremeter-set recommender
# payload is json with arguments, parameters (both objects)
def recommend(r):
    # sort the offers by ID
    parameterBounds = r['parameterBounds']
    print(parameterBounds)
    arguments = r['arguments']
    print(arguments)
    # you can load parameters from the model if you need ot.
    print('model params:', model_parameters['weights'])
    # random.choices() returns a list of k length
    return random.choices(possible_outcomes, model_parameters['weights'], k=1)[0] 

