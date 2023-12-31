
using Pulumi;
using AzureNative = Pulumi.AzureNative;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using System.Collections.Generic;

namespace SignalBox.Azure
{
    class AzureML : ComponentWithStorage
    {
        private static readonly InputMap<string> tags = new InputMap<string>
            {
                {"Component", "Analytics"}
            };

        public AzureML(ResourceGroup rg, AzureSynapse synapse, MultitenantDatabaseComponent multitenant)
        {
            var azureConfig = new Config("azure-native");
            var config = new Config("azure-ml");
            var multitenantDbSecretName = "multitenantDbPassword";

            var storageAccount = new StorageAccount("sa", new StorageAccountArgs
            {
                ResourceGroupName = rg.Name,
                Sku = new SkuArgs
                {
                    Name = SkuName.Standard_LRS
                },
                Kind = Kind.StorageV2,
                Tags = tags
            });

            var container = new BlobContainer("reportContainer", new BlobContainerArgs
            {
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name,
                ContainerName = "reports",
                Metadata = {
                    {"facing", "client"}
                }
            });

            // this is for temporary image base reporting
            var recommendersContainer = new BlobContainer("recomenderContainer", new BlobContainerArgs
            {
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name,
                ContainerName = "recommenders",
                Metadata = {
                    {"facing", "client"}
                }
            });

            this.PrimaryStorageKey = Output.Tuple(rg.Name, storageAccount.Name).Apply(names =>
                Output.CreateSecret(GetStorageAccountPrimaryKey(names.Item1, names.Item2)));

            this.PrimaryStorageConnectionString = Output.Tuple(rg.Name, storageAccount.Name).Apply(names =>
                Output.CreateSecret(GetStorageAccountPrimaryConnectionString(names.Item1, names.Item2)));


            var appInsights = new AzureNative.Insights.Component("MLAppInsights", new AzureNative.Insights.ComponentArgs
            {
                ApplicationType = "web",
                Kind = "web",
                ResourceGroupName = rg.Name,
                Tags = tags
            });

            var containerRegistry = new AzureNative.ContainerRegistry.Registry("MLResgistry", new AzureNative.ContainerRegistry.RegistryArgs
            {
                Sku = new AzureNative.ContainerRegistry.Inputs.SkuArgs
                {
                    Name = "standard",
                },
                AdminUserEnabled = true,
                ResourceGroupName = rg.Name,
                Tags = tags

            });

            var accessPolicies = new List<AzureNative.KeyVault.Inputs.AccessPolicyEntryArgs>
            {
                new AzureNative.KeyVault.Inputs.AccessPolicyEntryArgs
                {
                    ObjectId = Output.Create(Utility.GetClientServicePrincal()).Apply(_ => _.ObjectId),
                    Permissions = new AzureNative.KeyVault.Inputs.PermissionsArgs
                    {
                        Certificates =
                        {
                            "all"
                        },
                        Keys =
                        {
                            "all"
                        },
                        Secrets =
                        {
                            "all"

                        },
                    },
                    TenantId = azureConfig.Require("tenantId"),
                }
            };

            if ((new Config("azure-synapse")).GetBoolean("enable") != false)
            {
                accessPolicies.Add(new AzureNative.KeyVault.Inputs.AccessPolicyEntryArgs
                {
                    ObjectId = Output.Format(@$"{synapse.SynapsePrincipalId}"),
                    Permissions = new AzureNative.KeyVault.Inputs.PermissionsArgs
                    {
                        Certificates =
                            {
                                "all"
                            },
                        Keys =
                            {
                                "all"
                            },
                        Secrets =
                            {
                                "all"

                            },
                    },
                    TenantId = azureConfig.Require("tenantId"),
                });
            }

            var keyVault = new AzureNative.KeyVault.Vault("AnalyticsKV", new AzureNative.KeyVault.VaultArgs
            {
                ResourceGroupName = rg.Name,
                Tags = tags,
                Properties = new AzureNative.KeyVault.Inputs.VaultPropertiesArgs
                {
                    TenantId = azureConfig.Require("tenantId"),
                    EnabledForDeployment = true,
                    EnabledForDiskEncryption = true,
                    EnabledForTemplateDeployment = true,
                    AccessPolicies = accessPolicies,
                    Sku = new AzureNative.KeyVault.Inputs.SkuArgs
                    {
                        Name = AzureNative.KeyVault.SkuName.Standard,
                        Family = AzureNative.KeyVault.SkuFamily.A
                    }
                },
            },
            new CustomResourceOptions { Aliases = { new Alias { Name = "MLKV" } } }
            );

            var secret = new AzureNative.KeyVault.Secret("multitenantDbPassword", new AzureNative.KeyVault.SecretArgs
            {
                Properties = new AzureNative.KeyVault.Inputs.SecretPropertiesArgs
                {
                    Value = multitenant.Password,
                },
                ResourceGroupName = rg.Name,
                SecretName = multitenantDbSecretName,
                VaultName = keyVault.Name,
            });

            var workspace = new AzureNative.MachineLearningServices.Workspace("workspace", new AzureNative.MachineLearningServices.WorkspaceArgs
            {
                ApplicationInsights = appInsights.Id,
                ContainerRegistry = containerRegistry.Id,
                Description = config.Get("description") ?? "Azure ML Workspace for SignalBox Dev",
                FriendlyName = config.Get("friendlyname") ?? "SignalBox Dev ML",
                HbiWorkspace = false,
                Identity = new AzureNative.MachineLearningServices.Inputs.IdentityArgs
                {
                    Type = AzureNative.MachineLearningServices.ResourceIdentityType.SystemAssigned,
                },
                KeyVault = keyVault.Id,
                ResourceGroupName = rg.Name,
                Sku = new AzureNative.MachineLearningServices.Inputs.SkuArgs
                {
                    Name = "Basic",
                    Tier = "Basic",
                },
                StorageAccount = storageAccount.Id,
                WorkspaceName = config.Require("name"),
            });

            this.AnalyticsKeyVaultName = keyVault.Name;
        }

        public Output<string> AnalyticsKeyVaultName { get; }
        public Output<string> PrimaryStorageKey { get; }
        public Output<string> PrimaryStorageConnectionString { get; }
    }
}