using System.Threading.Tasks;
using SignalBox.Core.Adapters.Shopify;

namespace SignalBox.Core
{
#nullable enable
    public interface IShopifyAdminWorkflow : IShopifyWorkflowBase
    {
        Task<IntegratedSystem> Connect(string code, string shop, string webhookReceiverUrl, long? environmentId);
        Task Disconnect(IntegratedSystem system);
        /// <summary> 
        /// Creates a pending app subscription charged to the Shopify store.
        /// </summary>
        /// <param name="system">the integrated system.</param>
        /// <param name="returnUrl">the return url on charge acceptance.</param>
        Task<ShopifyRecurringCharge?> ChargeBilling(IntegratedSystem system, string returnUrl);
        Task<ShopifyShop?> GetShopInformation(IntegratedSystem system);
    }
}