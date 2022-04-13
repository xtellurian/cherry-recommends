#!/bin/bash

# ensure migrations are up to date.
echo "Ensuring local sql database is migrated"
cd ../deploy/sql-database-scripts/local/sqlserver/
./update-sql-database.sh
./update-tenant-sql-database.sh
cd ../../../../web

echo "Starting API server..."
ASPNETCORE_ENVIRONMENT=Development dotnet run > dotnet.web.log &

sleep 10 # wait for the thing to start
echo "Logs > "
cat dotnet.web.log
echo "END LOGS"
