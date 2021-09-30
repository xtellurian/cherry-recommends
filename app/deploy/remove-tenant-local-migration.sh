set -e
cd ../web

CONTEXT="MultiTenantDbContext"
MIGRATIONS_DIR="SignalBox/Sub$CONTEXT"

PROVIDER='sqlite'

dotnet ef migrations remove --context $CONTEXT --project "../migrations/$PROVIDER" -- --Provider $PROVIDER

echo "Done"