set -e

if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../../../
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi

MIGRATION=$1

if [ -z "$MIGRATION" ]
then
      echo "Usage: $0 <migration> <database>"
      exit 1
fi

DATABASE=$2

if [ -z "$DATABASE" ]
then
      echo "Usage: $0 <database> <database> "
      exit 1
fi

cd $APP_PATH/web


MIGRATIONS_DIR="SignalBox"
CONTEXT="SignalBoxDbContext"
PROVIDER="sqlserver"


dotnet ef database update $MIGRATION --context $CONTEXT --project ../migrations/sqlserver -- --Provider sqlserver --Hosting:SingleTenantDatabaseName $DATABASE
