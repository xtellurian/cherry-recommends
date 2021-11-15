$home_dir=$(pwd)

$environmentName=$args[0]

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

cd ../synapse/

echo "Getting Linked Service"
$linkedServiceJSON = Get-ChildItem ls_*.json | Select-Object name

foreach ($linkedServiceName in $linkedServiceJSON.name) {
    echo "Deploying configuration for $linkedServiceName"

    $tempLinkedServiceName = $linkedServiceName -replace ".json", ""
    $tempLinkedServiceName = $tempLinkedServiceName -replace "ls_", ""
    $tempLinkedServiceFileName = "temp_linkedservice.json"

    cp $linkedServiceName $tempLinkedServiceFileName

    (Get-Content -path $tempLinkedServiceFileName -Raw) -replace "{AnalyticsKeyVaultName}", $AnalyticsKeyVaultName | Set-Content $tempLinkedServiceFileName
    (Get-Content -path $tempLinkedServiceFileName -Raw) -replace "{SqlServerName}", $SqlServerName | Set-Content $tempLinkedServiceFileName
    (Get-Content -path $tempLinkedServiceFileName -Raw) -replace "{SqlServerUsername}", $SqlServerUsername | Set-Content $tempLinkedServiceFileName
    (Get-Content -path $tempLinkedServiceFileName -Raw) -replace "{SynapseWorkspaceName}", $SynapseWorkspaceName | Set-Content $tempLinkedServiceFileName
    (Get-Content -path $tempLinkedServiceFileName -Raw) -replace "{SynapseStorageAccountName}", $SynapseStorageAccountName | Set-Content $tempLinkedServiceFileName
    
    az synapse linked-service create --name $tempLinkedServiceName --workspace-name $SynapseWorkspaceName --file `@temp_linkedservice.json
}

echo "Getting Notebooks"
$notebookJSON = Get-ChildItem *.ipynb | Select-Object name

foreach ($notebookName in $notebookJSON.name) {
    echo "Extracting configuration for $notebookName"

    $tempNotebookName = "`@" + $notebookName
    $notebookName = $notebookName -replace ".ipynb", ""

    az synapse notebook create --workspace-name $SynapseWorkspaceName --name $notebookName --file $tempNotebookName --spark-pool-name "synapseSpark"
}

echo "Getting Datasets"
$dsJSON = Get-ChildItem ds_*.json | Select-Object name

foreach ($dsJSONName in $dsJSON.name) {
    echo "Extracting configuration for $dsJSONName"

    $tempdsServiceName = $dsJSONName -replace ".json", ""
    $tempdsServiceName = $tempdsServiceName -replace "ds_", ""
    $tempdsFileName = "temp_ds.json"

    cp $dsJSONName $tempdsFileName

    (Get-Content -path $tempdsFileName -Raw) -replace "{AnalyticsKeyVaultName}", $AnalyticsKeyVaultName | Set-Content $tempdsFileName
    (Get-Content -path $tempdsFileName -Raw) -replace "{SqlServerName}", $SqlServerName | Set-Content $tempdsFileName
    (Get-Content -path $tempdsFileName -Raw) -replace "{SqlServerUsername}", $SqlServerUsername | Set-Content $tempdsFileName
    (Get-Content -path $tempdsFileName -Raw) -replace "{SynapseWorkspaceName}", $SynapseWorkspaceName | Set-Content $tempdsFileName
    (Get-Content -path $tempdsFileName -Raw) -replace "{SynapseStorageAccountName}", $SynapseStorageAccountName | Set-Content $tempdsFileName
    
    az synapse dataset create --name $tempdsServiceName --workspace-name $SynapseWorkspaceName --file `@temp_ds.json
}

echo "Getting Pipelines"
$pipelineJSON = Get-ChildItem pipeline_*.json | Select-Object name

foreach ($pipelineJSONName in $pipelineJSON.name) {
    echo "Extracting configuration for $pipelineJSONName"

    $tempPipelineJSONName = "`@" + $pipelineJSONName
    $pipelineJSONName = $pipelineJSONName -replace ".json", ""
    $pipelineJSONName = $pipelineJSONName -replace "pipeline_", ""

    az synapse pipeline create --workspace-name $SynapseWorkspaceName --name $pipelineJSONName --file $tempPipelineJSONName
}

cd ..\deploy\azure-scripts\synapse-environment-build

