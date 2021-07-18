from typing import List
from .client_functions import get, post, delete
from .pagination import pageQuery, PaginatedResponse
from .exceptions import SignalBoxException


def create_parameterset_recommender(access_token: str, base_url: str, common_id: str, name: str, parameters: List[str], bounds, arguments):
    json_params = {
        "commonId": common_id,
        "name": name,
        "parameters": parameters,
        "bounds": bounds,
        "arguments": arguments,
    }
    r = post(f'{base_url}/api/recommenders/ParameterSetRecommenders',
             json_params, access_token)
    if r.ok:
        return r.json()
    else:
        raise SignalBoxException(r.text)


def get_parameterset_recommender(access_token: str, base_url: str, common_id: str):
    r = get(
        f'{base_url}/api/recommenders/ParameterSetRecommenders/{common_id}', access_token)
    if r.ok:
        return r.json()
    else:
        raise SignalBoxException(r.text)


def delete_parameterset_recommender(access_token: str, base_url: str, id):
    r = delete(
        f'{base_url}/api/recommenders/ParameterSetRecommenders/{id}', access_token)
    if r.ok:
        return r.json()
    else:
        raise SignalBoxException(r.text)
