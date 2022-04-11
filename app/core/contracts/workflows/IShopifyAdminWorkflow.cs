using System.Threading.Tasks;
using SignalBox.Core.Adapters.Shopify;

namespace SignalBox.Core
{
#nullable enable
    public interface IShopifyAdminWorkflow : IShopifyWorkflowBase
    {
        Task<IntegratedSystem> Connect(string code, string shop, string webhookReceiverUrl, long? environmentId);
        Task Disconnect(IntegratedSystem system);
        Task<ShopifyShop?> GetShopInformation(IntegratedSystem system);
    }
}