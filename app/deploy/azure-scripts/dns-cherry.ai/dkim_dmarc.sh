

ZONE="cherry.ai"
RG="dns"

# add O365 DKIM
# CNAMEs
# Host Name : selector1._domainkey  Points to address or value: selector1-cherry-ai._domainkey.amphoradatacom.onmicrosoft.com
az network dns record-set cname set-record -g $RG -z $ZONE -n "selector1._domainkey" -c "selector1-cherry-ai._domainkey.amphoradatacom.onmicrosoft.com"
# Host Name : selector2._domainkey  Points to address or value: selector2-cherry-ai._domainkey.amphoradatacom.onmicrosoft.com
az network dns record-set cname set-record -g $RG -z $ZONE -n "selector2._domainkey" -c "selector2-cherry-ai._domainkey.amphoradatacom.onmicrosoft.com"

# set up DMARC too
# https://docs.microsoft.com/en-us/microsoft-365/security/office-365-security/use-dmarc-to-validate-email?view=o365-worldwide
# _dmarc.contoso.com 3600 IN  TXT  "v=DMARC1; p=none" 
az network dns record-set txt add-record -g dns -z $ZONE -n "_dmarc" -v "v=DMARC1; p=none"
