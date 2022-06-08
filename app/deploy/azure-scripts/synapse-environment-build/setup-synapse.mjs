#!/usr/bin/env zx

// IMPORTANT. Permission to run this script.
// For this to run in the pipeline, the executing ServicePrincipal must be given permission to act in Azure Synapse
// That means going to Synapse Studio, then Manage > Access Control
// Giving the user or Service Principal Synapse Contributor works
// Dev deployment SP Object ID: 6f687f17-7b02-484a-8f87-88442b24fed2
// Canary/ Prod deployment SP Object ID: 886c22ab-16af-47be-9fe6-b7dc44e7f200

const fs = require("fs");
const ls_keyVault = require("./templates/linked-services/ls_keyVault.json");
const ls_multitenant = require("./templates/linked-services/ls_multitenantdb.json");

const ds_tenant_data = require("./templates/data-sources/ds_tenant_data.json");
const ds_tenants_lookup = require("./templates/data-sources/ds_tenants_lookup.json");
const ds_tenant_adls_data_avro = require("./templates/data-sources/ds_tenant_adls_data_avro.json");
const ds_tenant_adls_data_parquet = require("./templates/data-sources/ds_tenant_adls_data_parquet.json");

const p_copy_tenant_data = require("./templates/pipelines/p_copy_tenant_data.json");

const t_12_hour_eventscopy = require("./templates/triggers/trigger_12h_eventscopy.json");

const createTempJsonFile = (name, obj) => {
  const p = `.working/${name}.json`;
  fs.writeFileSync(p, JSON.stringify(obj));
  return p;
};

const createLinkedService = async (workspace, name, js) => {
  const file = createTempJsonFile(name, js);
  return await $`az synapse linked-service create --workspace-name ${workspace} --name ${name} --file @"${file}"`;
};

const createDataset = async (workspace, name, js) => {
  const file = createTempJsonFile(name, js);
  return await $`az synapse dataset create --workspace-name ${workspace} --name ${name} --file @"${file}"`;
};

const createPipeline = async (workspace, name, js) => {
  const file = createTempJsonFile(name, js);
  return await $`az synapse pipeline create --workspace-name ${workspace} --name ${name} --file @"${file}"`;
};

const createTrigger = async (workspace, name, js) => {
  const file = createTempJsonFile(name, js);
  return await $`az synapse trigger create --workspace-name ${workspace} --name ${name} --file @"${file}"`;
};

const importNotebook = async (workspace, name, file, folderPath) => {
  return await $`az synapse notebook import --workspace-name ${workspace} --name ${name} --file @"${file}" --folder-path=${folderPath}`;
};
// --

cd("../../../azure");

const stackConfigOutput = await $`pulumi config -j`;
const stackConfig = JSON.parse(stackConfigOutput.stdout);
const synapseEnabled = stackConfig["azure-synapse:enable"];

if (synapseEnabled?.value !== "true") {
  console.log("Synapse is not enabled.");
  process.exit(0);
} else {
  console.log("Synapse is enabled. Continuing.");
}

const stackVariableOutput = await $`pulumi stack output --show-secrets -j`;
const stackOutputs = JSON.parse(stackVariableOutput.stdout);
const sqlServerName = stackOutputs.SqlServerName;
const sqlServerUserName = stackOutputs.SqlServerUsername;
const workspace = stackOutputs.SynapseWorkspaceName;

cd("../deploy/azure-scripts/synapse-environment-build");

// Create Linked Services

ls_keyVault.properties.baseUrl = ls_keyVault.properties.baseUrl.replace(
  "{name}",
  stackOutputs.AnalyticsKeyVaultName
);

ls_keyVault.properties.typeProperties.baseUrl =
  ls_keyVault.properties.typeProperties.baseUrl.replace(
    "{name}",
    stackOutputs.AnalyticsKeyVaultName
  );

await createLinkedService(workspace, "AnalyticsKeyVault", ls_keyVault);

ls_multitenant.name = "tenants";
ls_multitenant.properties.connectionString =
  ls_multitenant.properties.connectionString
    .replace("{{SqlServerName}}", sqlServerName)
    .replace("{{database}}", "tenants")
    .replace("{{SqlServerUsername}}", sqlServerUserName);

ls_multitenant.properties.password.store.referenceName = "AnalyticsKeyVault";

await createLinkedService(workspace, "multitenantdb", ls_multitenant);

// create datasets
createDataset(workspace, "tenant_data", ds_tenant_data);

createDataset(workspace, "tenants_lookup", ds_tenants_lookup);

ds_tenant_adls_data_avro.properties.linkedServiceName.referenceName = `${workspace}-WorkspaceDefaultStorage`;
createDataset(workspace, "tenant_adls_data_avro", ds_tenant_adls_data_avro);
ds_tenant_adls_data_parquet.properties.linkedServiceName.referenceName = `${workspace}-WorkspaceDefaultStorage`;
createDataset(
  workspace,
  "tenant_adls_data_parquet",
  ds_tenant_adls_data_parquet
);

// pipeline
createPipeline(workspace, "copy_tenant_data", p_copy_tenant_data);

// pipeline trigger
createTrigger(workspace, "12HourEventsCopy", t_12_hour_eventscopy);

importNotebook(
  workspace,
  "sample",
  "./templates/notebooks/sample.ipynb",
  "samples"
);
