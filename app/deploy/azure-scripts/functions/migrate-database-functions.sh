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
FUNCTIONKEY=$(pulumi stack output --show-secrets DotnetFunctionAppDefaultKey)

# do the migration
echo "Running migration..."
migration=`curl -LI --request POST -H "Content-Length: 0" --url "https://$FUNCTIONAPPNAME.azurewebsites.net/api/tenants/*/migrations?code=$FUNCTIONKEY" | head -n 1 | cut -d ' ' -f2`
if [ -z "$migration" ] || [ -z $migration=200 ]
then
    echo "Database migration failed!"
    exit 1
fi
echo "Successfully migrated tenants!"

# run an auth0 update
# we do this because auth0 has a bad habit of deleting roles or scopes on update
# since we use these roles and scopes as IAM, this ensures they still exist
# on a normal deploy, this should have no effect
echo "Running migration..."
auth0update=`curl -LI --request POST -H "Content-Length: 0" --url "https://$FUNCTIONAPPNAME.azurewebsites.net/api/tenants/*/members/*?code=$FUNCTIONKEY" | head -n 1 | cut -d ' ' -f2`
if [ -z "$auth0update" ] || [ -z $auth0update=200 ]
then
    echo "Auth0 Update /tenants/*/members/* failed!"
    exit 1
fi
echo "Successfully updated auth0!"
