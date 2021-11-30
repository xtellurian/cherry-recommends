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

CONTEXT="MultiTenantDbContext"
MIGRATIONS_DIR="SignalBox/Sub$CONTEXT"

dotnet ef migrations list --context $CONTEXT --project "../migrations/sqlite" -- --Provider $PROVIDER