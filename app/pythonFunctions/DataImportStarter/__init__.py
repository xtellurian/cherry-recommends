# This function an HTTP starter function for Durable Functions.
# Before running this sample, please:
# - create a Durable orchestration function
# - create a Durable activity function (default name is "Hello")
# - add azure-functions-durable to requirements.txt
# - run pip install -r requirements.txt

import logging

import azure.functions as func
import azure.durable_functions as df
import json

available_activities = [
    'DataImport'.lower()
]


async def main(req: func.HttpRequest, starter: str) -> func.HttpResponse:
    client = df.DurableOrchestrationClient(starter)
    activity = req.route_params["functionName"].lower()
    if activity in available_activities:
        instance_id = await client.start_new(activity, None, req.get_json())
        logging.info(f"Started orchestration with ID = '{instance_id}'.")

        return client.create_check_status_response(req, instance_id)
    else:
        logging.error(f'{activity} was not an activity')
        return func.HttpResponse(body=json.dumps({'error': f'{activity} is an invalid activity'}), status_code=400)
