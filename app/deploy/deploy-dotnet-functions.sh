set -e
home_dir=$(pwd)
cd ../azure

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
FUNCTIONAPPNAME=$(pulumi stack output DotnetFunctionAppName)
RG=$(pulumi stack output AppResourceGroup)

cd $home_dir
cd ../dotnetFunctions
echo "Publishing python functions to $FUNCTIONAPPNAME"
func azure functionapp publish $FUNCTIONAPPNAME
echo "Deployed functions $FUNCTIONAPPNAME in stack $STACK"
