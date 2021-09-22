from typing import List
import requests
import warnings
from .exceptions import CredentialsException, SignalBoxException
from .client_functions import batch, construct_user, construct_event, post, put
from .experiments import create_experiment, get_experiment, get_offers_in_experiment, query_experiments
from .products import create_product, query_products
from .trackedusers import create_user, create_or_update_users
from .touchpoints import create_touchpoint_on_tracked_user, get_touchpoint_on_tracked_user, get_touchpoint
from .parameters import create_parameter
from .parameter_recommenders import create_parameterset_recommender, get_parameterset_recommender, delete_parameterset_recommender
from .product_recommenders import create_product_recommender, get_product_recommender, delete_product_recommender, invoke_product_recommender, create_recommender_target_variable_value, get_recommender_target_variable_values
from .features import get_feature, create_feature, get_feature_value, set_feature_value
from .items_recommenders import create_items_recommender, get_items_recommender, invoke_items_recommender, delete_items_recommender


class Configuration:
    def __init__(self, host='localhost', port=5001, protocol='https'):
        self.host = host
        self.port = port
        self.protocol = protocol
        if "localhost" in host:
            warnings.filterwarnings(
                "ignore", message="Unverified HTTPS request is being made to host")

    def base_url(self):
        return f'{self.protocol}://{self.host}:{self.port}'


class Credentials:
    def __init__(self, api_key=None, access_token=None):
        self.api_key = api_key
        self.access_token = access_token
        if api_key is None and access_token is None:
            raise CredentialsException(
                "one of api_key or access_token must not be null")

    def fetch_token(self, configuration: Configuration):
        if self.access_token is None:
            api_endpoint = f'{configuration.base_url()}/api/apikeys/exchange'
            api_headers = {"Content-Type": "application/json"}
            json_params = {
                "apiKey": self.api_key,
            }

            r = requests.post(url=api_endpoint, json=json_params,
                              headers=api_headers, verify=False)

            if r.ok:
                self.access_token = r.json()['access_token']
                if self.access_token == None:
                    raise CredentialsException("Access Token is None")

            else:
                raise CredentialsException(
                    f'Server returned: {r.status_code}, with body: {r.text}')
        return self.access_token


class SignalBoxClient:
    def __init__(self, credentials: Credentials, configuration: Configuration = None):
        if configuration is None:
            configuration = Configuration()
        self.access_token = credentials.fetch_token(
            configuration=configuration)
        self.base_url = configuration.base_url()

    def create_offer(self, name: str, currency: str, price: float, cost: float):
        json_params = {
            "name": name,
            "currency": currency,
            "price": price,
            "cost": cost
        }

        r = post(f'{self.base_url}/api/offers', json_params, self.access_token)
        if r.ok:
            return r.json()
        else:
            raise SignalBoxException(r.text)

    def create_user(self, name: str, common_user_id: str):
        return create_user(self.access_token, self.base_url, name=name, common_user_id=common_user_id)

    def create_touchpoint_on_tracked_user(self, tracked_user_id: str, touchpoint_id: str, values: dict):
        return create_touchpoint_on_tracked_user(
            self.access_token, self.base_url, tracked_user_id=tracked_user_id, touchpoint_id=touchpoint_id, values=values)

    def get_touchpoint_on_tracked_user(self, tracked_user_id: str, touchpoint_id: str, version: int = None):
        return get_touchpoint_on_tracked_user(
            self.access_token, self.base_url, tracked_user_id=tracked_user_id, touchpoint_id=touchpoint_id, version=version)

    def get_touchpoint(self, touchpoint_id: str):
        return get_touchpoint(self.access_token, self.base_url, touchpoint_id=touchpoint_id)

    def create_or_update_users(self, users):
        return create_or_update_users(self.access_token, self.base_url,  users)

    def create_product(self, common_id: str, name: str, list_price: float, direct_cost: float, description: str):
        return create_product(self.access_token, self.base_url, common_id, name, list_price, direct_cost, description)

    def query_products(self, page: int):
        return query_products(self.access_token, self.base_url, page)

    def create_segment(self, name: str):
        json_params = {
            "name": name
        }
        r = post(f'{self.base_url}/api/segments',
                 json_params, self.access_token)

        if r.ok:
            return r.json()
        else:
            raise SignalBoxException(r.text)

    def create_parameter(self, common_id: str, name: str, parameter_type: str = "Numerical", default_value=None, description: str = None):
        return create_parameter(self.access_token, self.base_url,
                                common_id, name, parameter_type, default_value, description)

    # features
    def create_feature(self, common_id, name):
        return create_feature(self.access_token, self.base_url, common_id, name)

    def get_feature(self, common_id):
        return get_feature(self.access_token, self.base_url, common_id)

    def get_feature_value(self, user_id, feature_id, version: int = None):
        return get_feature_value(self.access_token, self.base_url, user_id, feature_id, version)

    def set_feature_value(self, user_id, feature_id, value):
        return set_feature_value(self.access_token, self.base_url, user_id, feature_id, value)

    def create_experiment(self, offer_ids: list, name: str, concurrent_offers: int = 1, segment_id: str = None):
        return create_experiment(self.access_token, self.base_url,
                                 offer_ids, name, concurrent_offers, segment_id)

    def get_experiment(self, experiment_id: str):
        return get_experiment(self.access_token, self.base_url, experiment_id)

    def query_experiments(self, page: int = None):
        return query_experiments(self.access_token, self.base_url, page=page)

    def get_offers_in_experiment(self, experiment_id: str):
        return get_offers_in_experiment(self.access_token, self.base_url, experiment_id)

    # Recommenders

    # Product Recommenders
    def create_product_recommender(self, common_id: str, name: str, product_ids: List[str] = None):
        return create_product_recommender(self.access_token, self.base_url, common_id, name, product_ids)

    def get_product_recommender(self, recommender_id):
        return get_product_recommender(self.access_token, self.base_url, id=recommender_id)

    def delete_product_recommender(self, id):
        return delete_product_recommender(self.access_token, self.base_url, id)

    def invoke_product_recommender(self, recommender_id: int, common_user_id: str):
        return invoke_product_recommender(self.access_token, self.base_url, id=recommender_id, common_user_id=common_user_id)

    def create_product_recommender_target_variable_value(self, recommender_id: int, start, end, name, value):
        return create_recommender_target_variable_value(self.access_token, self.base_url, "ProductRecommenders", recommender_id, start, end, name, value)

    def get_product_recommender_target_variable_values(self, recommender_id: int, name: str = None):
        return get_recommender_target_variable_values(self.access_token, self.base_url, "ProductRecommenders", recommender_id, name)

    # Parameter Set Recommenders
    def create_parameterset_recommender(self, common_id: str, name: str, parameters: List[str], bounds, arguments):
        return create_parameterset_recommender(
            self.access_token, self.base_url, common_id, name, parameters, bounds, arguments)

    def get_parameterset_recommender(self, common_id: str):
        return get_parameterset_recommender(self.access_token, self.base_url, common_id)

    def delete_parameterset_recommender(self, id):
        return delete_parameterset_recommender(self.access_token, self.base_url, id)

    def create_parameterset_recommender_target_variable_value(self, recommender_id: int, start, end, name, value):
        return create_recommender_target_variable_value(self.access_token, self.base_url, "ParameterSetRecommenders", recommender_id, start, end, name, value)

    def get_parameterset_recommender_target_variable_values(self, recommender_id: int, name: str = None):
        return get_recommender_target_variable_values(self.access_token, self.base_url, "ParameterSetRecommenders", recommender_id, name)

    # Items Recommender
    def create_items_recommender(self, common_id: str, name: str, default_item_id: str, item_ids: List[str] = None, number_items_to_recommend: int = None):
        return create_items_recommender(self.access_token, self.base_url, common_id, name, default_item_id, item_ids, number_items_to_recommend)

    def get_items_recommender(self, recommender_id):
        return get_items_recommender(self.access_token, self.base_url, recommender_id)

    def invoke_items_recommender(self, recommender_id, common_user_id: str, arguments: dict = None):
        return invoke_items_recommender(self.access_token, self.base_url, recommender_id, common_user_id, arguments)

    def delete_items_recommender(self, recommender_id):
        return delete_items_recommender(self.access_token, self.base_url, recommender_id)

    def recommend_offer(self, experimentId: str, commonUserId: str, features: dict = None):
        json_params = {
            "commonUserId": commonUserId,
            "features": features
        }
        r = post(f'{self.base_url}/api/experiments/{experimentId}/recommendation',
                 json_params, self.access_token)

        if r.ok:
            return r.json()
        else:
            raise SignalBoxException(r.text)

    def track_recommendation_outcome(self, recommendation, offerId, outcome):
        json_params = {
            "commonUserId":  recommendation['commonUserId'],
            'experimentId': recommendation['experimentId'],
            'iterationId': recommendation['iterationId'],
            'recommendationId': recommendation['recommendationId'],
            'offerId': offerId,
            'outcome': outcome
        }
        r = post(
            f'{self.base_url}/api/experiments/{recommendation["experimentId"]}/outcome', json_params, self.access_token)

        if r.ok:
            return r.json()
        else:
            raise SignalBoxException(r.text)

    def construct_event(self, commonUserId, event_id, event_type, kind, properties, timestamp=None, source_system_id=None, recommendationCorrelatorId: int = None):
        return construct_event(commonUserId, event_id, event_type, kind, properties, timestamp, source_system_id, recommendationCorrelatorId)

    def construct_user(self, commonUserId: str, name: str = None, properties: str = None, integratedSystemId: int = None, userId: str = None):
        return construct_user(commonUserId, name=name, properties=properties, integratedSystemId=integratedSystemId, userId=userId)

    def log_events(self, events, batch_size=1000, callback=None):
        results = []
        for e in batch(events, n=batch_size):
            r = post(f'{self.base_url}/api/events', e, self.access_token)
            if(r.ok):
                if(callback != None):
                    callback(r.json())
                results.append(r.json())
            else:
                raise SignalBoxException(r.text)
        return results
