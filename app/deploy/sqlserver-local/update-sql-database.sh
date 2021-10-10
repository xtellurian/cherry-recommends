set -e

# CS="Server=tcp:$SERVER.database.windows.net,1433;Initial Catalog=$DATABASE;User ID=$USER;Password=$PW;Min Pool Size=0;Max Pool Size=30;Persist Security Info=False;";

CONTEXT=SignalBoxDbContext
DATABASE=signalbox
CS="Server=127.0.0.1,1433;Database=$DATABASE;User Id=SA;Password=YourStrong@Passw0rd"

cd ../../web

CONTEXT="SignalBoxDbContext"

dotnet ef database update --context $CONTEXT --connection "$CS" --project ../migrations/sqlserver -- --Provider sqlserver --Hosting:SingleTenantDatabaseName $DATABASE  --Hosting:Multitenant "false"
