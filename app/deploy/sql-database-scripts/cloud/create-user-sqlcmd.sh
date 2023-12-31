set -e

if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../../
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi

cd $APP_PATH/azure

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"

SERVER=$(pulumi stack output SqlServerName)
SQLSERVERUSERNAME=$(pulumi stack output SqlServerUsername --show-secrets)
SQLSERVERPASSWORD=$(pulumi stack output SqlServerPassword --show-secrets)
ADMINUSERNAME=$(pulumi stack output SqlServerAdminUserName --show-secrets)
ADMINPASSWORD=$(pulumi stack output SqlServerAdminPassword --show-secrets)
READUSERNAME=$(pulumi stack output SqlServerReadUserName --show-secrets)
READPASSWORD=$(pulumi stack output SqlServerReadPassword --show-secrets)

cd ../deploy/sql-database-scripts/cloud

echo "Execute User Creation/ Update for $ADMINUSERNAME and $READUSERNAME"
sqlcmd -S $SERVER.database.windows.net -U $SQLSERVERUSERNAME -P $SQLSERVERPASSWORD -d "master" -v AdminUserName=$ADMINUSERNAME AdminPassword=$ADMINPASSWORD ReadUserName=$READUSERNAME ReadPassword=$READPASSWORD -i create-service-principal.sql
echo "Created Service Principal."
# SQL Server does not allow DB Principal to be rcreated on master.
echo "Creating DB Principal on tenants database"
sqlcmd -S $SERVER.database.windows.net -U $SQLSERVERUSERNAME -P $SQLSERVERPASSWORD -d "tenants" -v AdminUserName=$ADMINUSERNAME AdminPassword=$ADMINPASSWORD ReadUserName=$READUSERNAME ReadPassword=$READPASSWORD -i create-db-principal.sql
echo "Creating DB Principal on single database"
sqlcmd -S $SERVER.database.windows.net -U $SQLSERVERUSERNAME -P $SQLSERVERPASSWORD -d "single" -v AdminUserName=$ADMINUSERNAME AdminPassword=$ADMINPASSWORD ReadUserName=$READUSERNAME ReadPassword=$READPASSWORD -i create-db-principal.sql
