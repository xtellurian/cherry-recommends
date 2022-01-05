set -e
cd ..
APP_PATH=$(pwd)

cd $APP_PATH/azure
pulumi up -y

cd $APP_PATH/deploy/sql-database-scripts/cloud
Hosting__Multitenant="true" APP_PATH=$APP_PATH ./update-tenant-sql-database.sh

cd $APP_PATH/deploy/azure-scripts/functions
APP_PATH=$APP_PATH ./deploy-dotnet-functions.sh
APP_PATH=$APP_PATH ./deploy-python-functions.sh
APP_PATH=$APP_PATH ./migrate-database-functions.sh

cd $APP_PATH/deploy/webapp
echo "OK. Deploying webapp..."
APP_PATH=$APP_PATH ./deploy.sh

echo "Finished multi deploy."
