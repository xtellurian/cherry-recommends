
using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;

namespace Signalbox.Azure
{
    class Storage
    {
        private static InputMap<string> tags = new InputMap<string>
            {
                {"Component", "AzureML"}
            };

        public Storage(ResourceGroup rg)
        {
            var storageAccount = new StorageAccount("funcstor", new StorageAccountArgs
            {
                ResourceGroupName = rg.Name,
                Sku = new SkuArgs
                {
                    Name = SkuName.Standard_LRS
                },
                Kind = Kind.StorageV2,
                Tags = tags
            });

            this.StorageAccount = storageAccount;

            this.PrimaryStorageKey = Output.Tuple(rg.Name, storageAccount.Name).Apply(names =>
               Output.CreateSecret(GetStorageAccountPrimaryKey(names.Item1, names.Item2)));

            this.PrimaryConnectionString = Output.Tuple(rg.Name, storageAccount.Name).Apply(names =>
                Output.CreateSecret(GetStorageAccountPrimaryConnectionString(names.Item1, names.Item2)));
        }

        public StorageAccount StorageAccount { get; }
        public Output<string> PrimaryStorageKey { get; }
        public Output<string> PrimaryConnectionString { get; }

        private static async Task<string> GetStorageAccountPrimaryKey(string resourceGroupName, string accountName)
        {
            var accountKeys = await ListStorageAccountKeys.InvokeAsync(new ListStorageAccountKeysArgs
            {
                ResourceGroupName = resourceGroupName,
                AccountName = accountName
            });
            return accountKeys.Keys[0].Value;
        }

        private static async Task<string> GetStorageAccountPrimaryConnectionString(string resourceGroupName, string accountName)
        {
            var accountKeys = await ListStorageAccountKeys.InvokeAsync(new ListStorageAccountKeysArgs
            {
                ResourceGroupName = resourceGroupName,
                AccountName = accountName
            });
            var primaryStorageKey = accountKeys.Keys[0].Value;
            return $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={primaryStorageKey};EndpointSuffix=core.windows.net";
        }

    }
}



