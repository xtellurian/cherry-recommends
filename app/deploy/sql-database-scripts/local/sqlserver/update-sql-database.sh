set -e

CONTEXT=SignalBoxDbContext

DATABASE=$1

if [ -z "$DATABASE" ]
then
      echo "Setting DATABASE=signalbox"
      DATABASE=signalbox
fi

echo "Using Database = $DATABASE"
CS="Server=127.0.0.1,1433;Database=$DATABASE;User Id=SA;Password=YourStrong@Passw0rd"

if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../../../
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi



cd $APP_PATH/web

CONTEXT="SignalBoxDbContext"

dotnet ef database update --context $CONTEXT --connection "$CS" --project ../migrations/sqlserver -- --Provider sqlserver --Hosting:SingleTenantDatabaseName $DATABASE  --Hosting:Multitenant "false"
