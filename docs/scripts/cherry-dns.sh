
echo Current Subscription is $(az account show --query name --out tsv)

ZONE="cherry.ai"
RG="dns"
echo DNS zone is $ZONE

cd ../azure-docs
CNAME_VALUE=$(pulumi stack output WebEndpointCNameValue)

echo Setting docs.$ZONE CName = $CNAME_VALUE

az network dns record-set cname set-record -g $RG -z $ZONE -n docs -c "$CNAME_VALUE"

echo Done.