environmentName="$1"

set -e

trap 'catch $? $LINENO' EXIT
catch() {
  if [ "$1" != "0" ]; then
    echo "Error $1 occurred on line $2."
  fi
}

home_dir=$(pwd)

cd ../../../azure
pulumi stack select $environmentName

STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"
RG=$(pulumi stack output AppResourceGroup)
AnalyticsKeyVaultName=$(pulumi stack output AnalyticsKeyVaultName)
SqlServerName=$(pulumi stack output SqlServerName)
SqlServerUsername=$(pulumi stack output SqlServerUsername)
SynapseWorkspaceName=$(pulumi stack output SynapseWorkspaceName)
SynapseStorageAccountName=$(pulumi stack output SynapseStorageAccountName)

cd ../synapse/

echo "Getting Linked Service"

for linkedServiceName in $(ls ls_*.json)
do
   echo "Deploying configuration for $linkedServiceName"
   tempLinkedServiceName=`echo ${linkedServiceName/.json/""}`
   tempLinkedServiceName=`echo ${tempLinkedServiceName/ls_/""}`
   tempLinkedServiceFileName="temp_linkedservice.json"
   
   cp $linkedServiceName $tempLinkedServiceFileName
   sed -i '' "s/{AnalyticsKeyVaultName}/$AnalyticsKeyVaultName/" $tempLinkedServiceFileName
   sed -i '' "s/{SqlServerName}/$SqlServerName/" $tempLinkedServiceFileName
   sed -i '' "s/{SqlServerUsername}/$SqlServerUsername/" $tempLinkedServiceFileName
   sed -i '' "s/{SynapseWorkspaceName}/$SynapseWorkspaceName/" $tempLinkedServiceFileName
   sed -i '' "s/{SynapseStorageAccountName}/$SynapseStorageAccountName/" $tempLinkedServiceFileName
   
   az synapse linked-service create --name $tempLinkedServiceName --workspace-name $SynapseWorkspaceName --file @temp_linkedservice.json

done

echo "Getting Notebooks"

OIFS="$IFS"
IFS=$'\n'
for notebookName in `find . -type f -name "*.ipynb"`
do
   echo "Extracting configuration for $notebookName"
   notebookName=`echo ${notebookName/.\//""}`
   tempNotebookName="@$notebookName"
   notebookName=`echo ${notebookName/.ipynb/""}`

   az synapse notebook create --workspace-name $SynapseWorkspaceName --name $notebookName --file $tempNotebookName --spark-pool-name "synapseSpark"

done
IFS="$OIFS"

echo "Getting Datasets"

for dsJSONName in $(ls ds_*.json)
do
   echo "Deploying configuration for $dsJSONName"

   tempdsServiceName=`echo ${dsJSONName/.json/""}`
   tempdsServiceName=`echo ${tempdsServiceName/ds_/""}`
   tempdsFileName="temp_ds.json"
   cp $dsJSONName $tempdsFileName
   sed -i '' "s/{AnalyticsKeyVaultName}/$AnalyticsKeyVaultName/" $tempdsFileName
   sed -i '' "s/{SqlServerName}/$SqlServerName/" $tempdsFileName
   sed -i '' "s/{SqlServerUsername}/$SqlServerUsername/" $tempdsFileName
   sed -i '' "s/{SynapseWorkspaceName}/$SynapseWorkspaceName/" $tempdsFileName
   sed -i '' "s/{SynapseStorageAccountName}/$SynapseStorageAccountName/" $tempdsFileName
   echo "Azure"
   az synapse dataset create --name $tempdsServiceName --workspace-name $SynapseWorkspaceName --file @temp_ds.json

done

echo "Getting Pipelines"

OIFS="$IFS"
IFS=$'\n'
for pipelineJSONName in $(ls pipeline_*.json)
do
   echo "Extracting configuration for $pipelineJSONName"
   tempPipelineJSONName="@$pipelineJSONName"
   pipelineJSONName=`echo ${pipelineJSONName/.json/""}`
   pipelineJSONName=`echo ${pipelineJSONName/pipeline_/""}`

    az synapse pipeline create --workspace-name $SynapseWorkspaceName --name $pipelineJSONName --file $tempPipelineJSONName

done
IFS="$OIFS"

cd ../deploy/azure-scripts/synapse-environment-build

