set -e
if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../../
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi

cd $APP_PATH/web

DATABASE=$1

if [ -z "$DATABASE" ]
then
      echo "Usage: $0 <database-name>"
      exit 1
fi

MIGRATIONS_DIR="SignalBox"
CONTEXT="SignalBoxDbContext"
PROVIDER="sqlite"

dotnet ef migrations list --context $CONTEXT --project "../migrations/sqlite" -- --Provider $PROVIDER --Hosting:SingleTenantDatabaseName $DATABASE --Hosting:Multitenant "false"