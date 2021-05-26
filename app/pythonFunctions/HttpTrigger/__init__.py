import azure.functions as func
import logging
import json
import boto3
import pandas as pd
import io


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')
    # this needs a body
    req_body = req.get_json()
    access_key = req_body.get('accessKey')
    secret = req_body.get('secret')
    file_name = req_body.get('file')
    bucket = req_body.get('bucket')

    if(access_key is None):
        return func.HttpResponse(error_json('accessKey is required'), status_code=400)
    if(secret is None):
        return func.HttpResponse(error_json('secret is required'), status_code=400)
    if(file_name is None):
        return func.HttpResponse(error_json('file_name is required'), status_code=400)
    if(bucket is None):
        return func.HttpResponse(error_json('bucket is required'), status_code=400)

    logging.info(f'Bucket: {bucket}, File: {file_name}')
    session = create_session(access_key, secret)
    df = s3_file_to_df(session, bucket, file_name)
    logging.info(df.info())
    return func.HttpResponse(f"{df.info()}")

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

def convert_dataframe(df: pd.DataFrame):
    
    return df