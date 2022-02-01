

## ------- WEBFLOW ------------
echo "Setting Webflow DNS"
# add A records for Webflow
az network dns record-set a add-record -g $RG -z $ZONE -n @ -a "75.2.70.75"
az network dns record-set a add-record -g $RG -z $ZONE -n @ -a "99.83.190.102"

# add www CName record for www 
az network dns record-set cname set-record -g $RG -z $ZONE -n www -c "proxy-ssl.webflow.com"

# add www CName record for customer-analytics 
az network dns record-set cname set-record -g $RG -z $ZONE -n "customer-analytics" -c "proxy-ssl.webflow.com"


echo "Finished setting webflow DNS"