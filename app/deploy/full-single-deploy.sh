set -e
cd ../azure
pulumi up -y
cd ../deploy
./deploy-dotnet-functions.sh
./deploy-python-functions.sh
./update-sql-database.sh db
./deploy.sh

echo "Remember to move the database, if required."
