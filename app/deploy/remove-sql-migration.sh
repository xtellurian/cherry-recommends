set -e
cd ../web

cd ../azure

DATABASE=$1

if [ -z "$DATABASE" ]
then
      echo "Usage: $0 <database-name>"
      exit 1
fi

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"

SERVER=$(pulumi stack output SqlServerName)
USER=$(pulumi stack output SqlServerUsername)
PW=$(pulumi stack output SqlServerPassword --show-secrets)

CS="Server=tcp:$SERVER.database.windows.net,1433;Initial Catalog=$DATABASE;User ID=$USER;Password=$PW;Min Pool Size=0;Max Pool Size=30;Persist Security Info=False;";

cd ../web


MIGRATIONS_DIR="SignalBox"
CONTEXT="SignalBoxDbContext"
PROVIDER='sqlserver'

dotnet ef migrations remove --context $CONTEXT --project "../migrations/$PROVIDER" -- --Provider $PROVIDER  --ConnectionStrings:Application "$CS"

echo "Done"