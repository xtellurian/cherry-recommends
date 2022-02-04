#!/bin/bash

## ------- AZ Static WebApp cherry-website-prod ------------
echo "Setting Azure Static WebApp cherry-website-prod DNS"

az network dns record-set cname set-record -g $RG -z $ZONE -n www -c "ambitious-forest-00d3dee10.1.azurestaticapps.net"

# txt record for naked domain
az network dns record-set txt add-record -g $RG -z $ZONE -n @ -v "4hbxtq92m0gwz8gntzy3wztqm9r3883k"

STATIC_ID=$(az staticwebapp show -n cherry-website-prod --query id -o tsv)
az network dns record-set a create -g $RG -z $ZONE -n @ --target-resource "$STATIC_ID"

echo "Finished setting Azure Static WebApp cherry-website-prod DNS"
