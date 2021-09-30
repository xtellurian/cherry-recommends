set -e

cd ../azure

DATABASE=$1

if [ -z "$DATABASE" ]
then
      echo "Usage: $0 <database-name>"
      exit 1
fi

CS=$(pulumi stack output DatabaseConnectionString --show-secrets)

cd ../web

CONTEXT="SignalBoxDbContext"

dotnet ef database drop --context $CONTEXT --project ../migrations/sqlserver -- --Provider sqlserver --Hosting:SingleTenantDatabaseName $DATABASE
