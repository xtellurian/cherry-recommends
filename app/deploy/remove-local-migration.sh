set -e
cd ../web

MIGRATIONS_DIR="SignalBox"
CONTEXT="SignalBoxDbContext"

PROVIDER='sqlite'

dotnet ef migrations remove --context $CONTEXT --project "../migrations/$PROVIDER" -- --provider $PROVIDER

echo "Done"