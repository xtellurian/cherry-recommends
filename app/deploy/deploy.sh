set -e
cd ../azure

RUNTIME='linux-x64'
WEBAPPNAME=$(pulumi stack output WebappName)
RG=$(pulumi stack output ResourceGroup)
cd ../web
echo "Deploying Web App to ${WEBAPPNAME}"
# az webapp up -n $WEBAPPNAME --logs #--dryrun
# dotnet publish -c Release -r $RUNTIME #-p:PublishSingleFile=true
dotnet publish -c Release
# cd "./bin/Release/net5.0/$RUNTIME/publish/"
cd "./bin/Release/net5.0/publish/" # cross platform
zip -r deploy.zip .
az webapp deployment source config-zip -g $RG -n $WEBAPPNAME --src deploy.zip

echo "Deployed app to $WEBAPPNAME.azurewebsites.net"