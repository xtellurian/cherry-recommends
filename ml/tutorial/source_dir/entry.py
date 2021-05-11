
# this is an actual entry script
import json
import pickle
import random
import os

def init():
    global parameters
    with open(os.path.join(os.getenv('AZUREML_MODEL_DIR'), 'tutorial_model.pkl'), 'rb') as handle:
        parameters = pickle.load(handle)['parameters']

# request should have: offers: [],  features: {}
def run(request):
    print(request)
    recommendaton_request = json.loads(request)
    # Run inference
    recomendation = recommend(recommendaton_request)
    print(recomendation)
    return recomendation

def recommend(recommendaton_request):
    # sort the offers by ID
    offers = sorted(recommendaton_request['offers'], key=lambda o: o['id'])
    print(offers)
    features = recommendaton_request['features']
    return random.choices(offers, parameters['weights'] )
