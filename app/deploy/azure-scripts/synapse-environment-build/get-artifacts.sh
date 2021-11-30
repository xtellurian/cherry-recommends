environmentName="$1"

set -e

trap 'catch $? $LINENO' EXIT
catch() {
  if [ "$1" != "0" ]; then
    echo "Error $1 occurred on line $2."
  fi
}

home_dir=$(pwd)
synapseDirectory="../../../synapse/"

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

cd ..\deploy\azure-scripts\synapse-environment-build

echo "Getting Linked Service"
az synapse linked-service list --workspace-name $SynapseWorkspaceName | jq -r '.[].name' | while read linkedServiceName; do

    if [[ ! $linkedServiceName =~ "WorkspaceDefault" ]]; then
        echo "Extracting configuration for $linkedServiceName"

        linkedServiceDefinition=`az synapse linked-service show --workspace-name $SynapseWorkspaceName --name $linkedServiceName | jq 'del(.etag, .id, .resourceGroup)'`
        linkedServiceDefinition=`echo ${linkedServiceDefinition/$AnalyticsKeyVaultName/"{AnalyticsKeyVaultName}"}`
        linkedServiceDefinition=`echo ${linkedServiceDefinition/$SqlServerName/"{SqlServerName}"}`
        linkedServiceDefinition=`echo ${linkedServiceDefinition/$SqlServerUsername/"{SqlServerUsername}"}`
        linkedServiceDefinition=`echo ${linkedServiceDefinition/$SynapseWorkspaceName/"{SynapseWorkspaceName}"}`
        linkedServiceDefinition=`echo ${linkedServiceDefinition/$SynapseStorageAccountName/"{SynapseStorageAccountName}"}`

        linkedServiceName=`echo ${linkedServiceName/$AnalyticsKeyVaultName/"{AnalyticsKeyVaultName}"}`
        linkedServiceName=`echo ${linkedServiceName/$SqlServerUsername/"{SqlServerUsername}"}`
        linkedServiceName=`echo ls_$linkedServiceName.json`

        echo $linkedServiceDefinition > $synapseDirectory$linkedServiceName
    fi
done

echo "Getting Notebooks"
az synapse notebook list --workspace-name $SynapseWorkspaceName | jq -c '.[]' | while read notebookName; do
    notebookName=`echo $notebookName | jq '.name'`
    notebookName=`echo $notebookName | sed 's/"//g'`
    echo "Extracting configuration for $notebookName"

    az synapse notebook export --workspace-name $SynapseWorkspaceName --name $notebookName --output-folder $synapseDirectory

done

echo "Getting Datasets"
az synapse dataset list --workspace-name $SynapseWorkspaceName | jq -r '.[].name' | while read dsJSONName; do

    dsDefinition=`az synapse dataset show --workspace-name $SynapseWorkspaceName --name $dsJSONName | jq 'del(.etag, .id, .resourceGroup)'`
    
    dsDefinition=`echo ${dsDefinition/$AnalyticsKeyVaultName/"{AnalyticsKeyVaultName}"}`
    dsDefinition=`echo ${dsDefinition/$SqlServerName/"{SqlServerName}"}`
    dsDefinition=`echo ${dsDefinition/$SqlServerUsername/"{SqlServerUsername}"}`
    dsDefinition=`echo ${dsDefinition/$SynapseWorkspaceName/"{SynapseWorkspaceName}"}`
    dsDefinition=`echo ${dsDefinition/$SynapseStorageAccountName/"{SynapseStorageAccountName}"}`
    dsDefinition=`echo $dsDefinition | sed 's/"escapeChar": "\\\\"/"escapeChar": "\\\\\\\\\\\"/g'`
    if [ `echo $dsDefinition | jq '.properties.schema'` = "null" ]; then
        echo "Empty data set schema definition"
        name=`echo $dsDefinition | jq -r '.name'`
        type=`echo $dsDefinition | jq -r '.type'`
        properties=`echo $dsDefinition | jq '.properties' | jq -c -r 'del(.schema)'`
        dsDefinition=`jq -n --arg n $name --arg t $type '{"name": $n, "type":$t}'`
        dsDefinition=`echo $dsDefinition | jq  -r --argjson p "$properties" '.properties += $p'`
        dsDefinition=`echo $dsDefinition | sed 's/"escapeChar": "\\\\"/"escapeChar": "\\\\\\\\\\\"/g'`

    fi
    dsJSONName=`echo ds_$dsJSONName.json`

    echo $dsDefinition > $synapseDirectory$dsJSONName
done

echo "Getting Pipelines"
az synapse pipeline list --workspace-name $SynapseWorkspaceName | jq -r '.[].name' | while read pipelineJSONName; do
    echo "Extracting configuration for $pipelineJSONName"

    pipelineDefinition=`az synapse pipeline show --workspace-name $SynapseWorkspaceName --name $pipelineJSONName | jq 'del(.etag, .id, .resourceGroup)'`

    name=`echo $pipelineDefinition | jq -r '.name'`
    type=`echo $pipelineDefinition | jq -r '.type'`
    activities=`echo $pipelineDefinition | jq '.activities'`
    param=`echo $pipelineDefinition | jq '.parameters'`
    pipelineDefinition=`jq -n --arg n $name --arg t $type '{"name": $n, "type":$t}'`
    pipelineDefinition=`echo $pipelineDefinition | jq  -r --argjson a "$activities" '.properties.activities += $a'`
    pipelineDefinition=`echo $pipelineDefinition | jq  -r --argjson p "$param" '.properties.parameters += $p'`

    pipelineJSONName="pipeline_$pipelineJSONName.json"

    echo $pipelineDefinition > $synapseDirectory$pipelineJSONName
done
