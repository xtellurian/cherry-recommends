
import logging
import json
import azure.functions as func

def error(error, status_code=400):
    logging.error(error)
    j = json.dumps({'error': error})
    return func.HttpResponse(j, status_code=status_code, headers={'Content-Type': 'application/json'})


def success(content):
    j = json.dumps(content)
    return func.HttpResponse(j, headers={'Content-Type': 'application/json'})
