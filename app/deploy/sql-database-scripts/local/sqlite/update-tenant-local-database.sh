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
echo "Migrating $CONTEXT"
dotnet ef database update --context $CONTEXT --project ../migrations/sqlite -- --Provider sqlite
