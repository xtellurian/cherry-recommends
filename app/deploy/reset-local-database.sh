set -e
cd ../web

DATABASE=$1
MIGRATION=$2

if [ -z "$DATABASE" ]
then
      echo "Usage: $0 <database-name> <migration>"
      exit 1
fi


if [ -z "$MIGRATION" ]
then
      echo "Usage: $0 <database-name> <migration>"
      exit 1
fi

CONTEXT="SignalBoxDbContext"

dotnet ef database update $MIGRATION --context $CONTEXT --project ../migrations/sqlite -- --Provider sqlite --Hosting:SingleTenantDatabaseName $DATABASE --Hosting:Multitenant "false"
