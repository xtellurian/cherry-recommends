set -e
cd ../web

CONTEXT="MultiTenantDbContext"
MIGRATIONS_DIR="SignalBox/Sub$CONTEXT"

dotnet ef migrations list --context $CONTEXT --project "../migrations/sqlite" -- --Provider $PROVIDER