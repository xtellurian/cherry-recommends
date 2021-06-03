using System.Threading.Tasks;
using Pulumi.AzureNative.Storage;

namespace SignalBox.Azure
{
    abstract class ComponentWithStorage
    {
        protected static async Task<string> GetStorageAccountPrimaryKey(string resourceGroupName, string accountName)
        {
            var accountKeys = await ListStorageAccountKeys.InvokeAsync(new ListStorageAccountKeysArgs
            {
                ResourceGroupName = resourceGroupName,
                AccountName = accountName
            });
            return accountKeys.Keys[0].Value;
        }

        protected static async Task<string> GetStorageAccountPrimaryConnectionString(string resourceGroupName, string accountName)
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