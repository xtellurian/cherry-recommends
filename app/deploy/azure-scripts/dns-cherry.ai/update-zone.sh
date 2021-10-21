
STACK=$1
if [ -z "$STACK" ]
then
      echo "Usage: $0 <stack>"
      exit 1
fi

ZONE="cherry.ai"
echo DNS zone is $ZONE
echo "Switching Subscriptions..."

STARTING_AZ_SUB_ID=$(az account show --query id --out tsv)
az account set --subscription "Cherry.Core"

az account show

echo "Getting pulumi stack info for $STACK..."

cd ../../../azure


SUBDOMAIN=$(pulumi stack output -s $STACK NetworkSubdomain)
WEBAPP_NAME=$(pulumi stack output -s $STACK WebappName)
WEBAPP_URL=$WEBAPP_NAME.azurewebsites.net
WEBAPP_DOMAIN_VERIFY=$(pulumi stack output -s $STACK WebAppCustomDomainVerificationId)
SSL_DOMAIN_TOKEN=$(pulumi stack output -s $STACK SslDomainVerificationToken)

echo "Pulumi stack data acquired."

# add text record for web app custom domain verification
az network dns record-set txt add-record -g dns -z $ZONE -n asuid -v $WEBAPP_DOMAIN_VERIFY
# add text record for SSL Cert order domain ownership
az network dns record-set txt add-record -g dns -z $ZONE -n @ -v $SSL_DOMAIN_TOKEN

# add cname for the subdomain to the webapp
az network dns record-set cname set-record -g dns -z $ZONE -n $SUBDOMAIN -c $WEBAPP_URL
# add wildcard subdomains for every tenant
az network dns record-set cname set-record -g dns -z $ZONE -n *.$SUBDOMAIN -c $WEBAPP_URL

echo "Resetting Azure Subscription"
az account set --subscription $STARTING_AZ_SUB_ID

az account show
