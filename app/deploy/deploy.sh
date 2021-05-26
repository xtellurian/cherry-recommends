set -e
home_dir=$(pwd)
cd ../azure

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
WEBAPPNAME=$(pulumi stack output WebappName)
FUNCTIONAPPNAME=$(pulumi stack output FunctionAppName)
RG=$(pulumi stack output AppResourceGroup)

cd ../web
echo "Deploying Web App to ${WEBAPPNAME}"

dotnet publish -c Release
cd "./bin/Release/net5.0/publish/" # cross platform
zip -r deploy.zip .
az webapp deployment source config-zip -g $RG -n $WEBAPPNAME --src deploy.zip

echo "Deployed app to $WEBAPPNAME.azurewebsites.net"

cd $home_dir
cd ../pythonFunctions
echo "Publishing python functions to $FUNCTIONAPPNAME"
func azure functionapp publish $FUNCTIONAPPNAME
echo "Deployed to $FUNCTIONAPPNAME"