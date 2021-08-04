set -e
cd ../web

MIGRATIONS_DIR="SignalBox"
CONTEXT="SignalBoxDbContext"

dotnet ef migrations list --context $CONTEXT --project "../migrations/sqlite" -- --provider $PROVIDER