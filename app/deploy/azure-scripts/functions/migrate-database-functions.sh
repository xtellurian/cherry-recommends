set -e

if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../..
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi

home_dir=$(pwd)
cd $APP_PATH/azure

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
FUNCTIONAPPNAME=$(pulumi stack output DotnetFunctionAppName)
FUNCTIONKEY=$(pulumi stack output --show-secrets DotnetFunctionAppDefaultKey)

echo "Checking dependency for function $FUNCTIONAPPNAME..."
check_dependency=`curl --request POST -H "Content-Length: 0" --url "https://$FUNCTIONAPPNAME.azurewebsites.net/api/testboot?code=$FUNCTIONKEY"`
echo "Dependency check result: $check_dependency"
if [ "$check_dependency" = "Dependencies are OK" ]
then
    echo "Dependencies OK. Continuing"
else
    echo "Dependencies bad. Waiting 30s and retrying."
    sleep 30
    echo "Retrying checking dependency for function $FUNCTIONAPPNAME..."
    check_dependency=`curl --request POST -H "Content-Length: 0" --url "https://$FUNCTIONAPPNAME.azurewebsites.net/api/testboot?code=$FUNCTIONKEY"`
    echo "Dependency check result: $check_dependency"
    if [ "$check_dependency" = "Dependencies are OK" ]
    then
        echo "Dependencies OK after second attempt."
    else
        echo "Dependency check failed. abort."
        exit 1
    fi
fi

# do the migration
echo "Running migration..."
migration=`curl -LI --request POST -H "Content-Length: 0" --url "https://$FUNCTIONAPPNAME.azurewebsites.net/api/tenants/*/migrations?code=$FUNCTIONKEY" | head -n 1 | cut -d ' ' -f2`
if [ -z "$migration" ] || [ -z $migration=200 ]
then
    echo "Database migration failed!"
    exit 1
fi
echo "Successfully migrated tenants!"
