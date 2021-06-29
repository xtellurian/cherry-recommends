set -e
home_dir=$(pwd)
cd ../azure

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
WEBAPPNAME=$(pulumi stack output WebappName)
RG=$(pulumi stack output AppResourceGroup)

cd ../web
echo "Deploying Web App to ${WEBAPPNAME}"

dotnet publish -c Release
cd "./bin/Release/net5.0/publish/" # cross platform
rm deploy.zip # remove existing
zip -r deploy.zip .
az webapp deployment source config-zip -g $RG -n $WEBAPPNAME --src deploy.zip

echo "Deployed app to $WEBAPPNAME.azurewebsites.net in stack $STACK"