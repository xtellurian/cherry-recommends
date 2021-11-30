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
MIGRATION=$2

if [ -z "$DATABASE" ]
then
      echo "Usage: $0 <database-name> <migration>"
      exit 1
fi


if [ -z "$MIGRATION" ]
then
      echo "Usage: $0 <database-name> <migration>"
      exit 1
fi

CONTEXT="SignalBoxDbContext"

dotnet ef database update $MIGRATION --context $CONTEXT --project ../migrations/sqlite -- --Provider sqlite --Hosting:SingleTenantDatabaseName $DATABASE --Hosting:Multitenant "false"
