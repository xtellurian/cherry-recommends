set -e

if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../../../
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi

DATABASE=$1

if [ -z "$DATABASE" ]
then
      echo "Usage: $0 <database-name>"
      exit 1
fi

cd $APP_PATH/web


MIGRATIONS_DIR="SignalBox"
CONTEXT="SignalBoxDbContext"
PROVIDER='sqlserver'

dotnet ef migrations remove --context $CONTEXT --project "../migrations/$PROVIDER" -- --Provider $PROVIDER 

echo "Done"
