set -e
cd ../web

DATABASE=$1

if [ -z "$DATABASE" ]
then
      echo "Usage: $0 <database-name>"
      exit 1
fi


CONTEXT="SignalBoxDbContext"
echo "Migrating $CONTEXT"
dotnet ef database update --context $CONTEXT --project ../migrations/sqlite -- --Provider sqlite --Hosting:SingleTenantDatabaseName $DATABASE

