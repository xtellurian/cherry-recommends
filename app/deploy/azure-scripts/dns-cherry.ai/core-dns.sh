## this file contains the CORE DNS records for Cherry.AI.

echo Current Subscription is $(az account show --query name --out tsv)

ZONE="cherry.ai"
RG="dns"
echo DNS zone is $ZONE

# set CName for Outlook autodiscover
az network dns record-set cname set-record -g $RG -z $ZONE -n autodiscover -c "autodiscover.outlook.com"

# Create an MX record for outlook email
az network dns record-set mx add-record -g $RG -z $ZONE -n @ -e "cherry-ai.mail.protection.outlook.com" -p 0

# add O365 
az network dns record-set txt add-record -g $RG -z $ZONE -n @ -v "MS=ms74302510"
az network dns record-set txt add-record -g $RG -z $ZONE -n @ -v "v=spf1 include:spf.protection.outlook.com -all"

echo "Done."
