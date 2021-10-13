cd ../azure
pulumi up -y
cd ../deploy
echo "Running deploy.ps1"
./deploy.ps1
echo "Running update-sql-database.ps1"
cd ../../../../../deploy
./update-sql-database.ps1 db
echo "Running deploy-dotnet-functions.ps1"
cd ../deploy
./deploy-dotnet-functions.ps1
cd ../deploy

echo "Remember to move the database, if required."
