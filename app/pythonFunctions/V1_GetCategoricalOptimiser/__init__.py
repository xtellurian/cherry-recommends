import logging
import json
import azure.functions as func

# record is a string


def main(req: func.HttpRequest, record: str) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')
    record_data = json.loads(record)
    logging.info(json.dumps(record_data))
    return func.HttpResponse(
        json.dumps(record_data),
        status_code=200,
        headers={'Content-Type': 'application/json'}
    )
