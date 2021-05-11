set -e

cd ../azure

CS=$(pulumi stack output DatabaseConnectionString --show-secrets)

cd ../web

CONTEXT="SignalBoxDbContext"

dotnet ef database update --context $CONTEXT --connection "$CS" --project ../migrations/sqlserver -- --provider sqlserver
