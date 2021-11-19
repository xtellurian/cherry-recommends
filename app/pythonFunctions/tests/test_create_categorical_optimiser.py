import unittest

import azure.functions as func
import json

from .mocks import MockFunctionOutput
from .data import test_create_body
from V1_CreateCategoricalOptimiser import main
from shared_code import request_validation, populations


class TestFunction(unittest.TestCase):
    def test_is_valid_create_optimiser_request_body_fails(self):
        is_valid, reason = request_validation.is_valid_create_optimiser_request_body({
        })
        self.assertFalse(is_valid)
        self.assertIsNotNone(reason)

        is_valid, reason = request_validation.is_valid_create_optimiser_request_body({
            'items': [{
                'id': 45,
                'commonId': "bcjadbf"
            }]
        })
        self.assertFalse(is_valid)
        self.assertIsNotNone(reason)

    def test_create_categorical_optimiser(self):
        is_valid, reason = request_validation.is_valid_create_optimiser_request_body(
            test_create_body())
        self.assertTrue(is_valid)
        self.assertIsNone(reason)

    def test_is_valid_request_body(self):
        # Construct a mock HTTP request.
        mockBlob = MockFunctionOutput()
        mockRecord = MockFunctionOutput()
        body = json.dumps(test_create_body()).encode('utf8')
        req = func.HttpRequest(
            method='POST',
            body=body,
            url='v1/testtenant/categorical',
            params={})

        # SUT
        response = main(req, mockRecord, mockBlob)
        ###
        self.assertIsNotNone(response.get_body())
        self.assertEqual(response.status_code, 200)
        responseData = json.loads(response.get_body())
        self.assertTrue('PartitionKey' in responseData)
        self.assertTrue('RowKey' in responseData)
        self.assertTrue('id' in responseData)
        self.assertTrue('tenant' in responseData)
        self.assertTrue('name' in responseData)
        self.assertTrue('nItemsToRecommend' in responseData)
        self.assertTrue('nPopulations' in responseData)
        self.assertTrue('defaultItemCommonId' in responseData)

        self.assertIsNotNone(mockRecord)
        self.assertEqual(json.loads(mockRecord.value), responseData)

        all_distributions = populations.PopulationDistributionCollection(
            deserialized_dict=json.loads(mockBlob.value))
        self.assertIsNotNone(all_distributions)
        self.assertTrue(len(all_distributions.populations) > 0)
        for pop in all_distributions.populations:
            print(pop)
            self.assertIsNotNone(pop.population_id)
            self.assertTrue(len(pop.probabilities) > 0)
