set -e
cd ../web

MIGRATION=$1

if [ -z "$MIGRATION" ]
then
      echo "Usage: $0 <migration>"
      exit 1
fi

CONTEXT="SignalBoxDbContext"

dotnet ef database update $MIGRATION --context $CONTEXT --project ../migrations/sqlite -- --provider sqlite
