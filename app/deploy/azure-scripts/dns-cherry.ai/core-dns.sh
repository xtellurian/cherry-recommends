## this file contains the CORE DNS records for Cherry.AI.

echo Current Subscription is $(az account show --query name --out tsv)

ZONE="cherry.ai"
RG="dns"
echo DNS zone is $ZONE

## -------- O365 --------------
echo "Setting Outlook DNS"
# set CName for Outlook autodiscover
az network dns record-set cname set-record -g $RG -z $ZONE -n autodiscover -c "autodiscover.outlook.com"

# Create an MX record for outlook email
az network dns record-set mx add-record -g $RG -z $ZONE -n @ -e "cherry-ai.mail.protection.outlook.com" -p 0

# add O365 security
az network dns record-set txt add-record -g $RG -z $ZONE -n @ -v "MS=ms74302510"
az network dns record-set txt add-record -g $RG -z $ZONE -n @ -v "v=spf1 include:spf.protection.outlook.com -all"

## ------- WEBFLOW ------------
echo "Setting Webflow DNS"
# add www CName record for Webflow
az network dns record-set cname set-record -g $RG -z $ZONE -n www -c "proxy-ssl.webflow.com"
# add A records for Webflow
az network dns record-set a add-record -g $RG -z $ZONE -n @ -a "75.2.70.75"
az network dns record-set a add-record -g $RG -z $ZONE -n @ -a "99.83.190.102"


echo "Done."
