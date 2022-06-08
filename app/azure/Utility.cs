using System.Threading.Tasks;
using Pulumi.AzureNative.Authorization;

namespace SignalBox.Azure
{
    public static class Utility
    {
        private static GetClientConfigResult? clientConfigResult; // internal cache
        public static async Task<GetClientConfigResult> GetClientServicePrincal(Pulumi.InvokeOptions? options = null)
        {
            clientConfigResult ??= await GetClientConfig.InvokeAsync(options);
            return clientConfigResult;
        }
    }
}