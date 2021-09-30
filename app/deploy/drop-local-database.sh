set -e

cd ../web

CONTEXT="SignalBoxDbContext"

dotnet ef database drop --context $CONTEXT --project ../migrations/sqlite -- --Provider sqlite