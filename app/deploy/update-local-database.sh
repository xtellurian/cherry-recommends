set -e
cd ../web

CONTEXT="SignalBoxDbContext"

dotnet ef database update --context $CONTEXT --project ../migrations/sqlite -- --provider sqlite
