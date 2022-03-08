set -e
cd ../web

PROVIDER=$1
CONTEXT=$2

if [ -z "$PROVIDER" ]
then
      echo "Usage: $0 <provider> <context>"
      echo "Provider manually set to sqlserver. Continuing."
      PROVIDER="sqlserver"
fi


if [ -z "$CONTEXT" ]
then
      echo "Using default context: SignalBoxDbContext"
      CONTEXT="SignalBoxDbContext"
      MIGRATIONS_DIR="SignalBox/Sub$CONTEXT"
else
      MIGRATIONS_DIR="SignalBox/Sub$CONTEXT"
fi

echo "DATABASE PROVIDER: $PROVIDER"
echo "CONTEXT: $CONTEXT"
echo "MIGRATIONS DIRECTORY: $MIGRATIONS_DIR"


# use the name of the current branch for the migration
CURRENT_BRANCH=$(git rev-parse --abbrev-ref HEAD)
delim="_"
to_replace="\/"
MIGRATION="${CURRENT_BRANCH////_}"
echo "MIGRATION: $MIGRATION"

echo "---"

dotnet ef migrations add $MIGRATION --context $CONTEXT --output-dir "$MIGRATIONS_DIR" --project "../migrations/$PROVIDER" -- --Provider $PROVIDER

echo "Done"