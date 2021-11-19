import random
from typing import List
from .request_validation import is_valid_invoke_payload

constant_population_id = 1


def renormalize_dict(dict_values: dict):
    dict_values = dict_values.copy()
    total_p = 0
    for item_id, probability in dict_values.items():
        total_p += probability
    for item_id in dict_values:
        # round to 4 decimal places
        dict_values[item_id] = round(dict_values[item_id] / total_p, 4)

    return dict_values


class PopulationItemDistribution():
    def __init__(self, population_id: str = None, default_item: dict = None, items: List[dict] = None, probabilities: dict = None, deserialized_dict: dict = None) -> None:
        if deserialized_dict is None:
            self.population_id = population_id
            self.default_item = default_item
            if items == None:
                self.items = [default_item]
            else:
                self.items = items

            if probabilities == None:
                probs = {}
                if items is None:
                    raise Exception("items is required if not deserializing")
                n_items = len(items)
                for i in items:
                    probs[i['commonId']] = 1 / n_items
                # 100% on the default when constructing
                probs[default_item['commonId']] = 1
                self.probabilities = probs
            else:
                self.probabilities = probabilities
        else:
            self.population_id = deserialized_dict['population_id']
            self.default_item = deserialized_dict['default_item']
            self.items = deserialized_dict['items']
            self.probabilities = deserialized_dict['probabilities']

    def to_dict(self):
        return self.__dict__

    def get_item_probability(self, item_id):
        if item_id in self.probabilities:
            return self.probabilities[item_id]
        else:
            return 0

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

        new_probabilities = {}
        for item in items:
            new_probabilities[item['commonId']] = self.probabilities[item['commonId']
                                                                     ] if item['commonId'] in self.probabilities else 1/len(items)

        self.probabilities = renormalize_dict(new_probabilities)
        choices = sorted(items, key=lambda x: x['commonId'])
        probability_array = []
        for key, p in sorted(self.probabilities.items()):
            probability_array.append(p)

        return random.choices(choices, weights=probability_array, k=n_items)


class PopulationDistributionCollection():
    def __init__(self, default_item: dict = None, populations=None, n_items_to_recommend: int = None, deserialized_dict: dict = None):
        if deserialized_dict is not None:
            self.default_item = deserialized_dict['defaultItem']
            self.n_items_to_recommend = deserialized_dict['nItemsToRecommend']
            self.populations = []
            for pop_d in deserialized_dict['populations']:
                self.populations.append(
                    PopulationItemDistribution(deserialized_dict=pop_d))
        else:
            self.n_items_to_recommend = n_items_to_recommend
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
            populations.append(p.to_dict())
        return {
            'populations': populations,
            'nItemsToRecommend': self.n_items_to_recommend,
            'defaultItem': self.default_item
        }


def calculate_population_id(payload):
    is_valid, message = is_valid_invoke_payload(payload)
    if not is_valid:
        raise Exception(message)

    return constant_population_id
