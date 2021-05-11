import requests
import warnings
warnings.filterwarnings("ignore", message="Unverified HTTPS request is being made to host")

def post_api_header(access_token):
        return {"Content-Type": "application/json", "Authorization": f'Bearer {access_token}'}
def get_api_header(access_token):
        return {"Authorization": f'Bearer {access_token}'}

class Credentials:
    def __init__(self, api_key):
        self.api_key = api_key

class Configuration:
    def __init__(self, host='localhost', port=5001, protocol='https'):
        self.host = host
        self.port = port
        self.protocol = protocol

class Four2Client:
    def __init__(self, credentials: Credentials, configuration: Configuration = None):

        if configuration is None:
            configuration = Configuration()

        self.base_url = f'{configuration.protocol}://{configuration.host}:{configuration.port}'

        api_endpoint = f'{self.base_url}/api/apikeys/exchange'

        api_headers = {"Content-Type": "application/json"}
        json_params = {  
            "apiKey": credentials.api_key,
        }

        r = requests.post(url = api_endpoint, json = json_params, headers = api_headers, verify=False)

        if r.ok:
            self.access_token = r.json()['access_token']
        else:
            raise Exception(r.text)

    def create_offer(self, name: str, currency: str, price: float, cost: float):
        api_endpoint = f'{self.base_url}/api/offers'
        api_headers = post_api_header(self.access_token)
        json_params = {
                            "name": name,
                            "currency": currency,
                            "price": price,
                            "cost": cost
                        }
        r = requests.post(url = api_endpoint, json = json_params, headers = api_headers, verify=False)

        if r.ok:
            return r.json()

        raise Exception(r.text)

    def create_user(self, name: str, externalId: str):
        api_endpoint = f'{self.base_url}/api/trackedusers'
        api_headers = post_api_header(self.access_token)
        json_params = {
                            "name": name,
                            "externalId": externalId
                        }
        r = requests.post(url = api_endpoint, json = json_params, headers = api_headers, verify=False)

        if r.ok:
            return r.json()

        raise Exception(r.text)

    def create_segment(self, name: str):
        api_endpoint = f'{self.base_url}/api/segments'
        api_headers = post_api_header(self.access_token)
        json_params = {
                            "name": name
                        }
        r = requests.post(url = api_endpoint, json = json_params, headers = api_headers, verify=False)

        if r.ok:
            return r.json()

        raise Exception(r.text)

    def create_experiment(self, offerIds: list, name: str, concurrentOffers: int = 1, segmentId: str = None):
        api_endpoint = f'{self.base_url}/api/experiments'
        api_headers = post_api_header(self.access_token)
        json_params = {
                            "offerIds": offerIds,
                            "segmentId": segmentId,
                            "name": name,
                            "concurrentOffers": concurrentOffers
                        }
        r = requests.post(url = api_endpoint, json = json_params, headers = api_headers, verify=False)

        if r.ok:
            return r.json()

        raise Exception(r.text)

    def get_experiment(self, experimentId: str):
        url = f'{self.base_url}/api/experiments/{experimentId}'

        api_headers = get_api_header(self.access_token)

        r = requests.get(url = url, headers = api_headers, verify=False)

        if r.ok:
            return r.json()

        raise Exception(r.text)

    def recommend_offer(self, experimentId: str, externalId: str, features: dict = None):

        url = f'{self.base_url}/api/experiments/{experimentId}/recommendation'

        api_headers = post_api_header(self.access_token)

        json_params = {
            "externalTrackedUserId" : externalId,
            "features": features
        }

        r = requests.post(url = url, headers = api_headers, json = json_params, verify=False)

        if r.ok:
            return r.json()

        raise Exception(r.text)

    def track_recommendation_outcome(self, recommendation, offerId, outcome):
        url = f'{self.base_url}/api/experiments/{recommendation["experimentId"]}/outcome'

        api_headers = post_api_header(self.access_token)

        json_params = {
            "externalTrackedUserId" :  recommendation['trackedUserExternalId'],
            'experimentId': recommendation['experimentId'],
            'iterationId': recommendation['iterationId'],
            'recommendationId': recommendation['recommendationId'],
            'offerId': offerId,
            'outcome': outcome
        }

        r = requests.post(url = url, headers = api_headers, json = json_params, verify=False)

        if r.ok:
            return r.json()

        raise Exception(r.text)
