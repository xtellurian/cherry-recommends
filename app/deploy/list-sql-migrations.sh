set -e
cd ../azure

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
CS=$(pulumi stack output DatabaseConnectionString --show-secrets)

cd ../web

MIGRATIONS_DIR="SignalBox"
CONTEXT="SignalBoxDbContext"

dotnet ef migrations list --context $CONTEXT --connection "$CS" --project "../migrations/sqlserver" -- --provider sqlserver