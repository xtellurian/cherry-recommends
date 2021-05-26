
using System.Threading.Tasks;
using Pulumi;
using AzureNative = Pulumi.AzureNative;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;

namespace Signalbox.Azure
{
    class AzureML
    {
        private static InputMap<string> tags = new InputMap<string>
            {
                {"Component", "AzureML"}
            };

        public AzureML(ResourceGroup rg, DatabaseComponent db)
        {
            var azureConfig = new Config("azure-native");
            var config = new Config("azure-ml");

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

            this.PrimaryStorageKey = Output.Tuple(rg.Name, storageAccount.Name).Apply(names =>
                Output.CreateSecret(GetStorageAccountPrimaryKey(names.Item1, names.Item2)));


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

            var keyVault = new AzureNative.KeyVault.Vault("MLKV", new AzureNative.KeyVault.VaultArgs
            {
                ResourceGroupName = rg.Name,
                Tags = tags,
                Properties = new AzureNative.KeyVault.Inputs.VaultPropertiesArgs
                {
                    TenantId = azureConfig.Require("tenantId"),
                    EnabledForDeployment = true,
                    EnabledForDiskEncryption = true,
                    EnabledForTemplateDeployment = true,
                    AccessPolicies =
    {
        new AzureNative.KeyVault.Inputs.AccessPolicyEntryArgs
        {
            ObjectId = Output.Create(AzureNative.Authorization.GetClientConfig.InvokeAsync()).Apply(_ => _.ObjectId),
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
        },
    },
                    Sku = new AzureNative.KeyVault.Inputs.SkuArgs
                    {
                        Name = AzureNative.KeyVault.SkuName.Standard,
                        Family = AzureNative.KeyVault.SkuFamily.A
                    }
                },
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
        }



        private static async Task<string> GetStorageAccountPrimaryKey(string resourceGroupName, string accountName)
        {
            var accountKeys = await ListStorageAccountKeys.InvokeAsync(new ListStorageAccountKeysArgs
            {
                ResourceGroupName = resourceGroupName,
                AccountName = accountName
            });
            return accountKeys.Keys[0].Value;
        }

        public Output<string> PrimaryStorageKey { get; }
    }
}