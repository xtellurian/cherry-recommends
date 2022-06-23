#!/bin/bash

set -e 

hash=$(git rev-parse --short HEAD)
filename="channel.browser.js"
echo "Uploading primary script file"
az storage blob upload --account-name "jschannelscript" -f dist/channel.browser.js -c "js-channel-script" -n ${filename} --overwrite
echo "Uploading secondary script file"
az storage blob upload --account-name "jschannelscript" -f dist/channel.browser.js -c "js-channel-script" -n "$hash.$filename" --overwrite
echo "Finish uploading files"