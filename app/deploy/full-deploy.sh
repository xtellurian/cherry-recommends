set -e
cd ../azure
pulumi up -y
cd ../deploy
./deploy.sh
./update-sql-database.sh
./deploy-dotnet-functions.sh