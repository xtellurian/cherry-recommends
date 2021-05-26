# This function is not intended to be invoked directly. Instead it will be
# triggered by an HTTP starter function.
# Before running this sample, please:
# - create a Durable activity function (default name is "Hello")
# - create a Durable HTTP starter function
# - add azure-functions-durable to requirements.txt
# - run pip install -r requirements.txt

import logging

import azure.functions as func
import azure.durable_functions as durable


def orchestrator_function(context: durable.DurableOrchestrationContext):
    logging.info('Starting DataImport orchestrator')
    input = context.get_input()
    source = input.get('source')
    source_type = source.get('type')
    if(source_type.lower() == 's3'):
        result1 = yield context.call_activity('ImportFromS3', input)
    else:
        raise Exception(f'Unknown source: {source_type}')

    return [result1]

main = durable.Orchestrator.create(orchestrator_function)