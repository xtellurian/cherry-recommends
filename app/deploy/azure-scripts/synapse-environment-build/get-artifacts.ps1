$home_dir=$(pwd)

$environmentName=$args[0]
$synapseDirectory="../../../synapse/"

cd ../../../azure
pulumi stack select $environmentName
$STACK=$(pulumi stack --show-name)

echo "Using Pulumi Stack $STACK"
$RG=$(pulumi stack output AppResourceGroup)
$AnalyticsKeyVaultName=$(pulumi stack output AnalyticsKeyVaultName)
$SqlServerName=$(pulumi stack output SqlServerName)
$SqlServerUsername=$(pulumi stack output SqlServerUsername)
$SynapseWorkspaceName=$(pulumi stack output SynapseWorkspaceName)
$SynapseStorageAccountName=$(pulumi stack output SynapseStorageAccountName)

cd ..\deploy\azure-scripts\synapse-environment-build

echo "Getting Linked Service"
$linkedServiceJSON = az synapse linked-service list --workspace-name $SynapseWorkspaceName | ConvertFrom-Json

foreach ($linkedServiceName in $linkedServiceJSON.name) {

    if ($linkedServiceName -like "*WorkspaceDefault*"){continue}

    echo "Extracting configuration for $linkedServiceName"

    $linkedServiceDefinition = az synapse linked-service show --workspace-name $SynapseWorkspaceName --name $linkedServiceName | ConvertFrom-Json
    $linkedServiceDefinition = $linkedServiceDefinition | Select-Object * -ExcludeProperty etag, id, resourceGroup
    $linkedServiceDefinition = $linkedServiceDefinition | ConvertTo-Json -Depth 99
    
    $linkedServiceDefinition = $linkedServiceDefinition -replace $AnalyticsKeyVaultName, "{AnalyticsKeyVaultName}"
    $linkedServiceDefinition = $linkedServiceDefinition -replace $SqlServerName, "{SqlServerName}"
    $linkedServiceDefinition = $linkedServiceDefinition -replace $SqlServerUsername, "{SqlServerUsername}"
    $linkedServiceDefinition = $linkedServiceDefinition -replace $SynapseWorkspaceName, "{SynapseWorkspaceName}"
    $linkedServiceDefinition = $linkedServiceDefinition -replace $SynapseStorageAccountName, "{SynapseStorageAccountName}"

    $linkedServiceName = $linkedServiceName -replace $AnalyticsKeyVaultName, "{AnalyticsKeyVaultName}"
    $linkedServiceName = $linkedServiceName -replace $SqlServerUsername, "{SqlServerUsername}"
    $linkedServiceName = "ls_" + $linkedServiceName + ".json"

    echo $linkedServiceDefinition > $synapseDirectory$linkedServiceName
}

echo "Getting Notebooks"
$notebookJSON = az synapse notebook list --workspace-name $SynapseWorkspaceName | ConvertFrom-Json

foreach ($notebookName in $notebookJSON.name) {
    echo "Extracting configuration for $notebookName"

    az synapse notebook export --workspace-name $SynapseWorkspaceName --name $notebookName --output-folder $synapseDirectory
}

echo "Getting Datasets"
$dsJSON = az synapse dataset list --workspace-name $SynapseWorkspaceName | ConvertFrom-Json

foreach ($dsJSONName in $dsJSON.name) {
    echo "Extracting configuration for $dsJSONName"

    $dsDefinition = az synapse dataset show --workspace-name $SynapseWorkspaceName --name $dsJSONName | ConvertFrom-Json
    $dsDefinition = $dsDefinition | Select-Object * -ExcludeProperty etag, id, resourceGroup
    if (-not $dsDefinition.properties.schema) {
        echo "Empty data set schema definition"
        $dsDefinition = [ordered]@{
            name = $dsDefinition.name;
            type = $dsDefinition.type;
            properties = $dsDefinition.properties | Select-Object * -ExcludeProperty schema
            }
    }
    $dsDefinition = $dsDefinition | ConvertTo-Json -Depth 99

    $dsDefinition = $dsDefinition -replace $AnalyticsKeyVaultName, "{AnalyticsKeyVaultName}"
    $dsDefinition = $dsDefinition -replace $SqlServerName, "{SqlServerName}"
    $dsDefinition = $dsDefinition -replace $SqlServerUsername, "{SqlServerUsername}"
    $dsDefinition = $dsDefinition -replace $SynapseWorkspaceName, "{SynapseWorkspaceName}"
    $dsDefinition = $dsDefinition -replace $SynapseStorageAccountName, "{SynapseStorageAccountName}"

    $dsJSONName = "ds_" + $dsJSONName + ".json"

    echo $dsDefinition > $synapseDirectory$dsJSONName
}

echo "Getting Pipelines"
$pipelineJSON = az synapse pipeline list --workspace-name $SynapseWorkspaceName | ConvertFrom-Json

foreach ($pipelineJSONName in $pipelineJSON.name) {
    echo "Extracting configuration for $pipelineJSONName"

    $pipelineDefinition = az synapse pipeline show --workspace-name $SynapseWorkspaceName --name $pipelineJSONName | ConvertFrom-Json
    $pipelineDefinition = $pipelineDefinition | Select-Object * -ExcludeProperty etag, id, resourceGroup

    $pipelineDefinition = [ordered]@{
        name = $pipelineDefinition.name;
        type = $pipelineDefinition.type;
        properties = [ordered]@{
            activities = $pipelineDefinition.activities;
            parameters = $pipelineDefinition.parameters
        }
        } | ConvertTo-Json -Depth 99

    $pipelineJSONName = "pipeline_" + $pipelineJSONName + ".json"

    echo $pipelineDefinition > $synapseDirectory$pipelineJSONName

    (Get-Content -path $synapseDirectory$pipelineJSONName -Raw) -replace "blockSizeInMb", "blockSizeInMB" | Set-Content $synapseDirectory$pipelineJSONName
}
