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

CONTEXT="SignalBoxDbContext"

dotnet ef database drop --context $CONTEXT --project ../migrations/sqlite -- --Provider sqlite