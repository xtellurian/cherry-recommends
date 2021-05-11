set -e

cd ../azure

CS=$(pulumi stack output DatabaseConnectionString --show-secrets)

cd ../web

CONTEXT="SignalBoxDbContext"

dotnet ef database drop --context $CONTEXT --project ../migrations/sqlserver -- --provider sqlserver --ConnectionStrings:Application "$CS"
