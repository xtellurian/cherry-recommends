$home_dir=$(pwd)
cd ../azure

$STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
$WEBAPPNAME=$(pulumi stack output WebappName)
$CANONICAL_ROOT_DOMAIN=$(pulumi stack output CanonicalRootDomain)
$RG=$(pulumi stack output AppResourceGroup)

cd ../web
echo "Deploying Web App to ${WEBAPPNAME}"

dotnet publish -c Release
cd "./bin/Release/net5.0/publish/" # cross platform

echo "Compressing to deploy.zip"
wsl rm deploy.zip # remove existing
wsl zip -r deploy.zip .

echo "Deploying $WEBAPPNAME.azurewebsites.net"
wsl az webapp deployment source config-zip -g $RG -n $WEBAPPNAME --src deploy.zip

echo "Deployed app to $WEBAPPNAME.azurewebsites.net in stack $STACK"

echo "Canonical Root Domain is $CANONICAL_ROOT_DOMAIN"