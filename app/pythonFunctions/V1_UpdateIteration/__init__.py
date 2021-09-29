import logging
import json
import azure.functions as func


def error_response(error, status_code=400):
    logging.error(error)
    j = json.dumps({'error': error})
    return func.HttpResponse(j, status_code=status_code, headers={'Content-Type': 'application/json'})


def success_response(content):
    j = json.dumps(content)
    return func.HttpResponse(j, headers={'Content-Type': 'application/json'})


def main(req: func.HttpRequest, record: str) -> func.HttpResponse:
    logging.info('Starting Update Iteration function')
    # load the reward data from the blob passed by this trigger
    req_body = {}
    try:
        req_body = req.get_json()
    except:
        # you can return as error with this method.
        return error_response("Body of request must be JSON")

    # req_body will contain data about how to update the model
    # ... but what should it look like?

    return success_response({'message': 'done'})
