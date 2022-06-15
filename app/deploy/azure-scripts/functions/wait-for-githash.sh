#!/bin/bash

set -e
if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../..
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi

GITHASH=$(git rev-parse --short HEAD)
echo "Local Githash: $GITHASH"

home_dir=$(pwd)
cd $APP_PATH/azure

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
FUNCTIONAPPNAME=$(pulumi stack output DotnetFunctionAppName)
FUNCTIONKEY=$(pulumi stack output --show-secrets DotnetFunctionAppDefaultKey)

echo "Checking dependency for function $FUNCTIONAPPNAME..."

REMOTE_GITHASH=""

COUNTER=0
while [[ $COUNTER -lt 20 && $REMOTE_GITHASH != $GITHASH ]]
do
REMOTE_GITHASH=`curl --request GET -H "Content-Length: 0" --url "https://$FUNCTIONAPPNAME.azurewebsites.net/api/githash?code=$FUNCTIONKEY"`

echo "($COUNTER)|Remote Githash: $REMOTE_GITHASH"
echo "($COUNTER)|Local Githash: $GITHASH"
COUNTER=$((COUNTER+1))

sleep 10

done

if [ "$REMOTE_GITHASH" == "$GITHASH" ]
then
    echo "GITHASH MATCH. Continue"
else
    echo "Githash did not match after $COUNTER tries. Aborting."
    exit 1
fi
