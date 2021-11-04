cd ../azure-docs

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
ACCOUNT_NAME=$(pulumi stack output StorageAccountName)
KEY=$(pulumi stack output --show-secrets PrimaryStorageKey)
Static_Endpoint=$(pulumi stack output StaticEndpoint)

cd ..

cd docusaurus

npm run build
echo Deploying to Storage Account - $ACCOUNT_NAME
az storage blob upload-batch -s './build' -d '$web' --account-name $ACCOUNT_NAME --account-key $KEY

echo "Deployed $STACK docs to $Static_Endpoint"