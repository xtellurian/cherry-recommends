set -e
cd ..
APP_PATH=$(pwd)

cd $APP_PATH/azure

# addresses https://github.com/pulumi/pulumi-azure-native/discussions/1565
az config set core.only_show_errors=true
pulumi up -y
az config set core.only_show_errors=false


cd $APP_PATH/deploy

cd azure-scripts/functions/
APP_PATH=$APP_PATH ./deploy-python-functions.sh
APP_PATH=$APP_PATH ./deploy-dotnet-functions.sh
APP_PATH=$APP_PATH ./wait-for-githash.sh

cd $APP_PATH/deploy/sql-database-scripts/cloud
APP_PATH=$APP_PATH ./update-sql-database.sh single
APP_PATH=$APP_PATH ./create-user-sqlcmd.sh

# configure synapse
cd $APP_PATH/deploy/azure-scripts/synapse-environment-build/
zx setup-synapse.mjs

cd $APP_PATH/deploy/webapp
APP_PATH=$APP_PATH ./deploy.sh

echo "Remember to move the database, if required."
