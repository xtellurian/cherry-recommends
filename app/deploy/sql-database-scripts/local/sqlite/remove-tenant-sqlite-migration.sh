set -e

if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../../../
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi

cd $APP_PATH/web

CONTEXT="MultiTenantDbContext"
MIGRATIONS_DIR="SignalBox/Sub$CONTEXT"

PROVIDER='sqlite'

dotnet ef migrations remove --context $CONTEXT --project "../migrations/$PROVIDER" -- --Provider $PROVIDER

echo "Done"