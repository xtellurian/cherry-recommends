set -e

echo "Updating cloud SQL database"

if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../../
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi

cd $APP_PATH/azure

DATABASE=$1

if [ -z "$DATABASE" ]
then
      echo "Usage: $0 <database-name>"
      exit 1
fi

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"

SERVER=$(pulumi stack output SqlServerName)
USER=$(pulumi stack output SqlServerUsername)
PW=$(pulumi stack output SqlServerPassword --show-secrets)

CS="Server=tcp:$SERVER.database.windows.net,1433;Initial Catalog=$DATABASE;User ID=$USER;Password=$PW;Min Pool Size=0;Max Pool Size=30;Persist Security Info=False;";

cd $APP_PATH/web

CONTEXT="SignalBoxDbContext"

dotnet ef database update --context $CONTEXT --connection "$CS" --project ../migrations/sqlserver -- --Provider sqlserver --Hosting:SingleTenantDatabaseName $DATABASE
