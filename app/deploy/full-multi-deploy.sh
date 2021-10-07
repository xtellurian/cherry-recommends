set -e
cd ../azure

pulumi up -y

cd ../deploy

./deploy-dotnet-functions.sh

echo "Databases should be migrated using the Azure Function"
echo "Press any key to continue..."
read;

echo "OK. Deploying app..."

./deploy.sh

echo "Finished multi deploy."
