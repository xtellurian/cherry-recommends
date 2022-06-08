
using Pulumi;
using AzureNative = Pulumi.AzureNative;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Synapse;
using Pulumi.AzureNative.Synapse.Inputs;
using Pulumi.Random;
using ResourceIdentityType = Pulumi.AzureNative.Synapse.ResourceIdentityType;
using SkuArgs = Pulumi.AzureNative.Storage.Inputs.SkuArgs;
using System.Collections.Generic;

namespace SignalBox.Azure
{
    class AzureSynapse : ComponentWithStorage
    {
        private static InputMap<string> tags = new InputMap<string>
            {
                {"Component", "AzureSynapse"}
            };

        public AzureSynapse(ResourceGroup rg)
        {
            var azureConfig = new Config("azure-native");
            var config = new Config("azure-synapse");
            var username = config.Get("sqlAdmin") ?? "pulumi";
            var password = config.RequireSecret("sqlPassword");

            if (config.GetBoolean("enable") != false)
            {
                var storageAccount = new StorageAccount("synapsesa", new StorageAccountArgs
                {
                    ResourceGroupName = rg.Name,
                    Sku = new SkuArgs
                    {
                        Name = SkuName.Standard_LRS
                    },
                    IsHnsEnabled = true,
                    EnableHttpsTrafficOnly = true,
                    Kind = "StorageV2",
                    Tags = tags
                });

                var dataLakeStorageAccountUrl = Output.Format($"https://{storageAccount.Name}.dfs.core.windows.net");

                var container = new BlobContainer("synapseContainer", new BlobContainerArgs
                {
                    AccountName = storageAccount.Name,
                    ResourceGroupName = rg.Name,
                    ContainerName = "reports",
                    Metadata = {
                    {"facing", "client"}
                }
                });

                this.PrimaryStorageKey = Output.Tuple(rg.Name, storageAccount.Name).Apply(names =>
                    Output.CreateSecret(GetStorageAccountPrimaryKey(names.Item1, names.Item2)));

                this.PrimaryStorageConnectionString = Output.Tuple(rg.Name, storageAccount.Name).Apply(names =>
                    Output.CreateSecret(GetStorageAccountPrimaryConnectionString(names.Item1, names.Item2)));


                var appInsights = new AzureNative.Insights.Component("SynapseAppInsights", new AzureNative.Insights.ComponentArgs
                {
                    ApplicationType = "web",
                    Kind = "web",
                    ResourceGroupName = rg.Name,
                    Tags = tags
                });

                var workspace = new Workspace("synapseworkspace", new WorkspaceArgs
                {
                    ResourceGroupName = rg.Name,
                    DefaultDataLakeStorage = new DataLakeStorageAccountDetailsArgs
                    {
                        AccountUrl = dataLakeStorageAccountUrl,
                        Filesystem = "default"
                    },
                    Identity = new ManagedIdentityArgs
                    {
                        Type = ResourceIdentityType.SystemAssigned
                    },
                    SqlAdministratorLogin = username,
                    SqlAdministratorLoginPassword = password
                });

                this.SynapsePrincipalId = workspace.Identity.Apply(identity => identity?.PrincipalId ?? "<preview>");
                this.SynapseWorkspaceName = workspace.Name;
                this.SynapseStorageAccountName = storageAccount.Name;

                var allowAll = new IpFirewallRule("synapseIPFirewallRule", new IpFirewallRuleArgs
                {
                    ResourceGroupName = rg.Name,
                    WorkspaceName = workspace.Name,
                    EndIpAddress = "255.255.255.255",
                    StartIpAddress = "0.0.0.0"
                });

                var subscriptionId = rg.Id.Apply(id => id.Split('/')[2]);
                var roleDefinitionId = Output.Format($"/subscriptions/{subscriptionId}/providers/Microsoft.Authorization/roleDefinitions/ba92f5b4-2d11-453d-a403-e96b0029c9fe");

                var storageAccess = new RoleAssignment("synapseStorageAccess", new RoleAssignmentArgs
                {
                    RoleAssignmentName = new RandomUuid("roleName").Result,
                    Scope = storageAccount.Id,
                    PrincipalId = workspace.Identity.Apply(identity => identity?.PrincipalId ?? "<preview>"),
                    PrincipalType = "ServicePrincipal",
                    RoleDefinitionId = roleDefinitionId
                });

                var roles = new List<RoleAssignment>();

                foreach (var user in AzureUsers.AzureUserList)
                {
                    roles.Add(new RoleAssignment("synapseUserAccess-" + user.Name, new RoleAssignmentArgs
                    {
                        RoleAssignmentName = new RandomUuid("userRoleName-" + user.Name).Result,
                        Scope = storageAccount.Id,
                        PrincipalId = user.PrincipalId,
                        PrincipalType = user.PrincipalType,
                        RoleDefinitionId = roleDefinitionId
                    }));
                }

                var sparkPool = new BigDataPool("synapseSpark", new BigDataPoolArgs
                {
                    ResourceGroupName = rg.Name,
                    WorkspaceName = workspace.Name,
                    AutoPause = new AutoPausePropertiesArgs
                    {
                        DelayInMinutes = 15,
                        Enabled = true,
                    },
                    AutoScale = new AutoScalePropertiesArgs
                    {
                        Enabled = true,
                        MaxNodeCount = 3,
                        MinNodeCount = 3,
                    },
                    NodeCount = 3,
                    NodeSize = "Small",
                    NodeSizeFamily = "MemoryOptimized",
                    SparkVersion = "2.4"
                });
            }
            else
            {
                SynapsePrincipalId = Output.Create("");
                SynapseWorkspaceName = Output.Create("");
                SynapseStorageAccountName = Output.Create("");
                PrimaryStorageKey = Output.Create("");
                PrimaryStorageConnectionString = Output.Create("");
            }

            this.UserName = username;
            this.Password = password;
        }
        public string UserName { get; }
        public Output<string> Password { get; }
        public Output<string> SynapsePrincipalId { get; }
        public Output<string> SynapseWorkspaceName { get; }
        public Output<string> SynapseStorageAccountName { get; }
        public Output<string> PrimaryStorageKey { get; }
        public Output<string> PrimaryStorageConnectionString { get; }
    }
}