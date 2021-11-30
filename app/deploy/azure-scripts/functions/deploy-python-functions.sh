set -e
# enable conda for this script
eval "$(conda shell.bash hook)"

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

echo "Activating conda env"
conda activate pythonFunctions
echo "Publishing python functions to $FUNCTIONAPPNAME"
func azure functionapp publish $FUNCTIONAPPNAME
conda deactivate
echo "Conda env deactivated"
echo "Deployed functions $FUNCTIONAPPNAME in stack $STACK"
