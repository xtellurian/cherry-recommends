## this file contains the CORE DNS records for Cherry.AI.

SUB_ID=$(az account show --query id --out tsv)
echo Current Subscription is $(az account show --query name --out tsv)

CORE_SUB_ID="1faa2acc-3425-461a-8eef-0d0a4648c66c"

if [ "$CORE_SUB_ID" == "$SUB_ID" ]; then
    echo "Using azure subscription id = $SUB_ID"
else
    echo "Error. Use Cherry.Core subscription."
    exit 1
fi

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

ZONE=$ZONE RG=$RG ./azure-static-website-dns.sh

## ------- HUBSPOT ------------
echo "Setting Hubspot DNS"
az network dns record-set cname set-record -g $RG -z $ZONE -n "hs1-7572639._domainkey" -c "cherry-ai.hs20a.dkim.hubspotemail.net."
az network dns record-set cname set-record -g $RG -z $ZONE -n "hs2-7572639._domainkey" -c "cherry-ai.hs20b.dkim.hubspotemail.net."

## ------- MAILCHIMP ------------
echo "Setting Mailchimp DNS"
az network dns record-set cname set-record -g $RG -z $ZONE -n "k2._domainkey" -c "dkim2.mcsv.net"
az network dns record-set cname set-record -g $RG -z $ZONE -n "k3._domainkey" -c "dkim3.mcsv.net"

## ------- AMAZON AWS ------------
echo "Setting Amazon AWS DNS"

az network dns record-set cname set-record -g $RG -z $ZONE -n "ayh5h7ueo5tgd6uvalxmvl2l6kwrlmgc._domainkey" -c "ayh5h7ueo5tgd6uvalxmvl2l6kwrlmgc.dkim.amazonses.com"
az network dns record-set cname set-record -g $RG -z $ZONE -n "3rz4rxlb3gvf6bnlqs2b3kf3nts2irou._domainkey" -c "3rz4rxlb3gvf6bnlqs2b3kf3nts2irou.dkim.amazonses.com"
az network dns record-set cname set-record -g $RG -z $ZONE -n "axhxhqlvoj45nzxr6erxovohnzcxqpig._domainkey" -c "axhxhqlvoj45nzxr6erxovohnzcxqpig.dkim.amazonses.com"

## ------- Google Postmaster ------------
echo "Google DNS Verification"
az network dns record-set txt add-record -g $RG -z $ZONE -n @ -v "google-site-verification=khcyAISyeAvaQhoyfG5tiIKkcWPR_1spxCfFjcZoZpg"

## ------- Auth0 ------------
echo "Auth0 DNS Verification"
az network dns record-set cname set-record -g $RG -z $ZONE -n "login" -c "cherry-cd-olztepl4ihgfrcwi.edge.tenants.au.auth0.com"
az network dns record-set cname set-record -g $RG -z $ZONE -n "dev.login" -c "signalbox-dev-cd-vh3bkdx3gkylvptk.edge.tenants.us.auth0.com"
az network dns record-set cname set-record -g $RG -z $ZONE -n "canary.login" -c "signalbox-cd-1b7eqgjmvibxcdru.edge.tenants.us.auth0.com"

echo "Done."
