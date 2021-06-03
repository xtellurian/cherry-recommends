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

# use the name of the current branch for the migration
CURRENT_BRANCH=$(git rev-parse --abbrev-ref HEAD)
delim="_"
to_replace="\/"
MIGRATION="${CURRENT_BRANCH////_}"
echo "Migration: $MIGRATION"

dotnet ef migrations add $MIGRATION --context $CONTEXT --output-dir "$MIGRATIONS_DIR" --project "../migrations/$PROVIDER" -- --provider $PROVIDER

echo "Done"