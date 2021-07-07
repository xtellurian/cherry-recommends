set -e
cd ../web

PROVIDER=$1

if [ -z "$PROVIDER" ]
then
      echo "Usage: $0 <provider>"
      exit 1
fi

echo "Provider: $PROVIDER"

MIGRATIONS_DIR="SignalBox"
CONTEXT="SignalBoxDbContext"

dotnet ef migrations list --context $CONTEXT --project "../migrations/$PROVIDER" -- --provider $PROVIDER