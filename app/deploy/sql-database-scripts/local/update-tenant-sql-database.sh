set -e

if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../../
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi

cd $APP_PATH/web

CONTEXT="MultiTenantDbContext"
DATABASE=tenants
CS="Server=127.0.0.1,1433;Database=$DATABASE;User Id=SA;Password=YourStrong@Passw0rd"

dotnet ef database update --context $CONTEXT --connection "$CS" --project ../migrations/sqlserver -- --Provider sqlserver --Hosting:Multitenant "true"
