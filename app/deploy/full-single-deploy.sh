set -e
cd ..
APP_PATH=$(pwd)

cd $APP_PATH/azure
pulumi up -y

cd $APP_PATH/deploy

cd azure-scripts/functions/
APP_PATH=$APP_PATH ./deploy-dotnet-functions.sh 
APP_PATH=$APP_PATH ./deploy-python-functions.sh

cd $APP_PATH/deploy/sql-database-scripts/cloud
APP_PATH=$APP_PATH ./update-sql-database.sh single
APP_PATH=$APP_PATH ./create-user-sqlcmd.sh

cd $APP_PATH/deploy/webapp
APP_PATH=$APP_PATH ./deploy.sh

echo "Remember to move the database, if required."
