set -e

if [ -z "$APP_PATH" ]
then
    echo "Using default app path"
    cd ../../..
    APP_PATH=$(pwd)
else
    echo "Using APP_PATH $APP_PATH"
fi

home_dir=$(pwd)
cd $APP_PATH/azure

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
FUNCTIONAPPNAME=$(pulumi stack output DotnetFunctionAppName)
RG=$(pulumi stack output AppResourceGroup)

cd $home_dir
cd $APP_PATH/dotnetFunctions
echo "Publishing dotnet functions to $FUNCTIONAPPNAME"
func azure functionapp publish $FUNCTIONAPPNAME

echo "Waiting for warmup"
sleep 10
echo "Requesting warmup."
# start warming up the dotnet functions app
check_dependency=`curl --request GET --url "https://$FUNCTIONAPPNAME.azurewebsites.net"`

echo "Deployed functions $FUNCTIONAPPNAME in stack $STACK"
