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

MIGRATION=$1

if [ -z "$MIGRATION" ]
then
      echo "Usage: $0 <migration>"
      exit 1
fi

CONTEXT="MultiTenantDbContext"

dotnet ef database update $MIGRATION --context $CONTEXT --project ../migrations/sqlite -- --Provider sqlite
