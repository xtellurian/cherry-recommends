set -e
cd ../web

cd ../azure
CS=$(pulumi stack output DatabaseConnectionString --show-secrets)
cd ../web

MIGRATIONS_DIR="SignalBox"
CONTEXT="SignalBoxDbContext"
PROVIDER='sqlserver'

dotnet ef migrations remove --context $CONTEXT --project "../migrations/$PROVIDER" -- --provider $PROVIDER  --ConnectionStrings:Application "$CS"

echo "Done"