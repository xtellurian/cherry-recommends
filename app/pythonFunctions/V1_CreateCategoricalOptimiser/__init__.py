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

# this is the function that is public. record is an "output binding"


def is_valid_request_body(req_body):
    if 'id' not in req_body:
        return (False, "the id property is required")
    if 'name' not in req_body:
        return (False, "the name property is required")
    else:
        return (True, None)


def main(req: func.HttpRequest, record: func.Out[str],  outputBlob: func.Out[str]) -> func.HttpResponse:
    logging.info("Creating a Categorical Optimiser")
    req_body = {}
    try:
        req_body = req.get_json()
    except:
        # you can return as error with this method.
        return error_response("Body of request must be JSON")

    # get data you require from the caller
    is_valid, reason = is_valid_request_body(req_body)
    if not is_valid:
        return error_response(reason)

    id = req_body['id']
    name = req_body['name']
    tenant = req.route_params.get('tenant')

    # create a dict of the data you need to store
    record_data = {
        # "PartitionKey": tenant,
        "tenant": tenant,
        "RowKey": id,
        "id": id,
        "name": name
    }

    # create some intitial data used in model.
    model_data = req_body['SourceDistributionList']

    # save it to a blob for use later.
    outputBlob.set(json.dumps(req_body))
    logging.info("Distribution stored")

    # this writes a row to the table.
    record.set(json.dumps(record_data))

    # this returns the row data to the HTTP caller.
    return success_response(record_data)
