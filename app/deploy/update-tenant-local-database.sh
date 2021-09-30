set -e
cd ../web

CONTEXT="MultiTenantDbContext"
echo "Migrating $CONTEXT"
dotnet ef database update --context $CONTEXT --project ../migrations/sqlite -- --Provider sqlite
