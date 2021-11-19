import unittest

import azure.functions as func
import json

from shared_code import populations
from .data import test_serialized_distribution, test_invokation_payload
from V1_InvokeCategoricalOptimiser import main
from shared_code import request_validation


class TestInvoke(unittest.TestCase):
    def test_is_valid_invoke_optimiser_request_body_fails(self):
        is_valid, reason = request_validation.is_valid_invoke_optimiser_request_body({
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

    def test_is_valid_invoke_optimiser_request_body_success(self):
        payload = test_invokation_payload()
        is_valid, reason = request_validation.is_valid_invoke_optimiser_request_body(
            {'payload': payload})
        self.assertTrue(is_valid)
        self.assertIsNone(reason)

    def test_invoke_categorical_optimiser(self):
        # Construct a mock HTTP request.
        inputBlob = test_serialized_distribution()
        inputRecord = json.dumps({
            'defaultItemCommonId': 'test1'
        })
        payload = test_invokation_payload()
        body = json.dumps({'payload': payload}).encode('utf8')
        req = func.HttpRequest(
            method='POST',
            body=body,
            url='v1/testtenant/categorical/1/invoke',
            params={})

        ## SUT
        response = main(req, inputBlob, inputRecord)
        ##
        self.assertIsNotNone(response.get_body())
        self.assertEqual(response.status_code, 200)
        responseData = json.loads(response.get_body())
