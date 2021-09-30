set -e
cd ../web

DATABASE=$1

if [ -z "$DATABASE" ]
then
      echo "Usage: $0 <database-name>"
      exit 1
fi

MIGRATIONS_DIR="SignalBox"
CONTEXT="SignalBoxDbContext"
PROVIDER="sqlite"

dotnet ef migrations list --context $CONTEXT --project "../migrations/sqlite" -- --Provider $PROVIDER --Hosting:SingleTenantDatabaseName $DATABASE --Hosting:Multitenant "false"