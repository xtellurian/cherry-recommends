set -e

cd ../azure


MIGRATION=$1

if [ -z "$MIGRATION" ]
then
      echo "Usage: $0 <migration>"
      exit 1
fi

CONTEXT="SignalBoxDbContext"


STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
CS=$(pulumi stack output DatabaseConnectionString --show-secrets)

cd ../web

CONTEXT="SignalBoxDbContext"


dotnet ef database update $MIGRATION --context $CONTEXT --connection "$CS" --project ../migrations/sqlserver -- --provider sqlserver
