# This function is not intended to be invoked directly. Instead it will be
# triggered by an orchestrator function.
# Before running this sample, please:
# - create a Durable orchestration function
# - create a Durable HTTP starter function
# - add azure-functions-durable to requirements.txt
# - run pip install -r requirements.txt

import logging
import json
import boto3
import pandas as pd
import numpy as np
import io

# tenants
from . import mosh


def main(input: dict) -> object:
    tenant = input.get('tenant')
    if(tenant is None):
        raise Exception('tenant is required')

    source_config = input.get('source')
    access_key = source_config.get('accessKey')
    secret = source_config.get('secret')
    file_name = source_config.get('file')
    bucket = source_config.get('bucket')

    if(access_key is None):
        raise Exception('accessKey is required')
    if(secret is None):
        raise Exception('secret is required')
    if(file_name is None):
        raise Exception('file is required')
    if(bucket is None):
        raise Exception('bucket is required')
    if(bucket is None):
        raise Exception('bucket is required')

    logging.info(f'Connecting to bucket: {bucket}, File: {file_name}')
    session = create_session(access_key, secret)
    df = s3_file_to_df(session, bucket, file_name).replace({np.nan: None})
    if tenant.lower() == 'mosh':
        return mosh.handle(input, df)
    else:
        raise Exception(f'Unknown tenant: {tenant}')

def error_json(reason):
    return json.dumps({'reason': reason})


def create_session(access_key, secret):
    return boto3.Session(
        aws_access_key_id=access_key,
        aws_secret_access_key=secret,
    )

def s3_file_to_df(session, bucket, file):
    s3 = session.client('s3')
    logging.info('Created a BOTO3 session.')
    obj = s3.get_object(Bucket=bucket, Key=file)
    logging.info('S3 object reference created.')
    return pd.read_json(io.BytesIO(obj['Body'].read()))

