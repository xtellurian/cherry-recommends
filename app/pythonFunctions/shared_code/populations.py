import random
from typing import List
from .request_validation import is_valid_invoke_payload


def renormalize_dict(dict_values: dict):
    dict_values = dict_values.copy()
    total_p = 0
    for item_id, probability in dict_values.items():
        total_p += probability
    for item_id in dict_values:
        dict_values[item_id] = dict_values[item_id] / total_p

    return dict_values


class PopulationItemDistribution():
    def __init__(self, population_id: str = None, default_item: dict = None, items: List[dict] = None, probabilities: dict = None, dict: dict = None) -> None:
        if dict is None:
            self.population_id = population_id
            self.default_item = default_item
            if items == None:
                self.items = [default_item]
            else:
                self.items = items

            if probabilities == None:
                # 100% on the default when constructing
                self.probabilities = {self.default_item['commonId']: 1}
            else:
                self.probabilities = probabilities
        else:
            self.population_id = dict['population_id']
            self.default_item = dict['default_item']
            self.items = dict['items']
            self.probabilities = dict['probabilities']

    def to_dict(self):
        return self.__dict__

    def get_item(self, item_id):
        if item_id == None:
            return None
        items = [e for e in self.items if e['commonId'] == item_id]
        if len(items) == 0:
            return None
        else:
            return items[0]

    def add_items(self, items):
        for item in items:
            if self.get_item(item['commonId']) != None:
                continue  # don't add if already exists
            self.items.append(item)
        for item in items:
            self.probabilities[item['commonId']] = 1/len(self.items)
        self.normalize_probabilities()

    def normalize_probabilities(self):
        self.probabilities = renormalize_dict(self.probabilities)

    def choose_items(self, items, n_items) -> List[dict]:
        if len(items) == 0:
            raise Exception("items must not be empty")
        item_ids = []
        for item in self.items:
            item_ids.append(item['commonId'])

        probabilities = {}
        for item in items:
            probabilities[item['commonId']] = self.probabilities[item['commonId']
                                                                 ] if item['commonId'] in self.probabilities else 1/len(items)

        print(items)
        print(probabilities)
        probabilities = renormalize_dict(probabilities)
        choices = sorted(items, key=lambda x: x['commonId'])
        probability_array = []
        for key, p in sorted(probabilities.items()):
            probability_array.append(p)
        print(probabilities)
        print(probability_array)
        print(choices)
        print(n_items)
        return random.choices(choices, weights=probability_array, k=n_items)


class PopulationDistributionCollection():
    def __init__(self, default_item: dict = None, populations=None, n_items: int = None, dict: dict = None):
        if dict is not None:
            self.default_item = dict['defaultItem']
            self.n_items = dict['nItems']
            self.populations = []
            for pop_d in dict['populations']:
                self.populations.append(PopulationItemDistribution(dict=pop_d))
        else:
            self.n_items = n_items
            self.default_item = default_item
            self.populations: List[PopulationItemDistribution] = populations if populations is not None else [
            ]

    def get_population(self, population_id):
        for p in self.populations:
            if p.population_id == population_id:
                return p

    def add(self, pop_item_distribution: PopulationItemDistribution):
        self.populations.append(pop_item_distribution)

    def to_dict(self):
        populations = []
        for p in self.populations:
            populations.append( p.to_dict())
        return {
            'populations': populations,
            'nItems': self.n_items,
            'defaultItem': self.default_item
        }


def calculate_population_id(payload):
    is_valid, message = is_valid_invoke_payload(payload)
    if not is_valid:
        raise Exception(message)

    features = payload['features']
    population = ""
    for key, value in features.items():
        population += key
        population += str(value)
    return population
