set -e

cd ../azure

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
CS=$(pulumi stack output TenantDatabaseConnectionString --show-secrets)

cd ../web

CONTEXT="MultiTenantDbContext"

dotnet ef database update --context $CONTEXT --connection "$CS" --project ../migrations/sqlserver -- --Provider sqlserver
