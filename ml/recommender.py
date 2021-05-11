from flask import Flask, redirect, url_for, request
from recommender_funcs import recommend_offer
app = Flask(__name__)

#Flask app for recommending best offer from given features

@app.route('/recommend', methods=['POST'])
def recommend():
    """ takes JSON object when called: must contian keys 'features' and 'offers'
    calls external function recommend_offer and returns that value
    """
    request_data = request.get_json()

    features = request_data['features']
    offers = request_data['offers']

    return recommend_offer(features, offers)

if __name__ == '__main__':
   app.run(debug = True)
