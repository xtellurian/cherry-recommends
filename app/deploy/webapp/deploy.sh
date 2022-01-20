set -e
home_dir=$(pwd)

if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    APP_PATH="../.."
else
    echo "Using APP_PATH $APP_PATH"
fi

cd $APP_PATH/azure

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
WEBAPPNAME=$(pulumi stack output WebappName)
CANONICAL_ROOT_DOMAIN=$(pulumi stack output CanonicalRootDomain)
RG=$(pulumi stack output AppResourceGroup)

cd ../web
echo "Deploying Web App to ${WEBAPPNAME}"

dotnet publish -c Release
cd "./bin/Release/net6.0/publish/" # cross platform
rm -f deploy.zip # remove if exists
zip -r deploy.zip .
az webapp deployment source config-zip -g $RG -n $WEBAPPNAME --src deploy.zip

echo "Deployed app to $WEBAPPNAME.azurewebsites.net in stack $STACK"

echo "Canonical Root Domain is $CANONICAL_ROOT_DOMAIN"