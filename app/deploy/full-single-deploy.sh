set -e
cd ../azure
pulumi up -y
cd ../deploy
./deploy.sh
./update-sql-database.sh db
./deploy-dotnet-functions.sh

echo "Remember to move the database, if required."
