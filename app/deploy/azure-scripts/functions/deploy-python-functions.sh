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
FUNCTIONAPPNAME=$(pulumi stack output FunctionAppName)
RG=$(pulumi stack output AppResourceGroup)

cd $home_dir
cd $APP_PATH/pythonFunctions
echo "Publishing python functions to $FUNCTIONAPPNAME"
func azure functionapp publish $FUNCTIONAPPNAME
echo "Deployed functions $FUNCTIONAPPNAME in stack $STACK"
