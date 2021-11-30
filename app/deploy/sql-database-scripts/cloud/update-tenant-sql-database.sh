set -e

if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../../
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi

cd $APP_PATH/azure

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
CS=$(pulumi stack output TenantDatabaseConnectionString --show-secrets)

cd ../web

CONTEXT="MultiTenantDbContext"

dotnet ef database update --context $CONTEXT --connection "$CS" --project ../migrations/sqlserver -- --Provider sqlserver
