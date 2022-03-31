using System.Threading.Tasks;
using SignalBox.Core.Adapters.Shopify;

namespace SignalBox.Core
{
    public interface IShopifyAdminWorkflow : IShopifyWorkflowBase
    {
        Task Connect(IntegratedSystem system, string code, string shopifyUrl, string webhookReceiverUrl);
        Task Disconnect(IntegratedSystem system);
        Task<ShopifyShop> GetShopInformation(IntegratedSystem system);
    }
}