import unittest
import uuid
import json

from .data import test_serialized_distribution, test_invokation_payload

from shared_code import populations

test_items = [
    {'commonId': str(uuid.uuid1())},
    {'commonId': str(uuid.uuid1())},
    {'commonId': str(uuid.uuid1())},
    {'commonId': str(uuid.uuid1())},
    {'commonId': str(uuid.uuid1())},
    {'commonId': str(uuid.uuid1())}
]


class TestFunction(unittest.TestCase):
    def test_is_valid_invoke_optimiser_request_body_fails(self):
        payload = test_invokation_payload()
        population_id = populations.calculate_population_id(payload)
        self.assertIsNotNone(population_id)
    
    def test_can_load_test_distribution(self):
        loaded = populations.PopulationDistributionCollection(dict=json.loads(test_serialized_distribution()))
        self.assertIsNotNone(loaded)
        self.assertIsNotNone(loaded.default_item)
        self.assertIsNotNone(loaded.populations)
        self.assertTrue(len(loaded.populations) > 0)

    def test_serialize_deserialize_population(self):
        population = populations.PopulationItemDistribution(
            str(uuid.uuid1()), test_items[0])

        population.add_items(test_items[1:2])
        serialized = json.dumps(population.to_dict())
        self.assertIsNotNone(serialized)
        self.assertTrue(len(serialized) > 0)
        deserialized = populations.PopulationItemDistribution(
            dict=json.loads(serialized))

        self.assertEqual(
            population.default_item['commonId'], deserialized.default_item['commonId'])

    def test_serialize_deserialize_population_collection(self):
        pop1 = populations.PopulationItemDistribution(
            str(uuid.uuid1()), test_items[0])
        pop1.add_items(test_items[1:2])
        pop2 = populations.PopulationItemDistribution(
            str(uuid.uuid1()), test_items[0])
        pop2.add_items(test_items[1:2])

        collection = populations.PopulationDistributionCollection(populations=[
                                                                  pop1, pop2])
        serialized = json.dumps(collection.to_dict())
        self.assertTrue(len(serialized) > 0)
        print(serialized)
        deserialized = populations.PopulationDistributionCollection(
            dict=json.loads(serialized))

        self.assertEqual(len(collection.populations),
                         len(deserialized.populations))

    def test_population_probability_sum_one(self):

        population = populations.PopulationItemDistribution(
            str(uuid.uuid1()), test_items[0])

        population.add_items(test_items[1:2])

        total_p = 0
        for k in population.probabilities:
            total_p += population.probabilities[k]
        self.assertAlmostEqual(total_p, 1)

        population.add_items([test_items[3]])

        total_p = 0
        for k in population.probabilities:
            total_p += population.probabilities[k]

        self.assertAlmostEqual(total_p, 1)

        chosen_items = population.choose_items(items = test_items[4:5], n_items = 1)
        self.assertEqual(len(chosen_items), 1)
        chosen_item = chosen_items[0]
        self.assertIsNotNone(chosen_item)
        self.assertTrue(chosen_item['commonId'] == test_items[4]['commonId']
                        or chosen_item['commonId'] == test_items[5]['commonId'])
