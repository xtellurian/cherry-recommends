using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IIntegratedSystemWorkflow
    {
        Task<IntegratedSystem> CreateIntegratedSystem(string name, string systemTypeName);
        Task<TrackedUserSystemMap> LinkCustomerToSystem(string systemUserId, long integratedSystemId, string commonUserId);
        Task<WebhookReceiver> AddWebhookReceiver(long integratedSystemId, bool? includeSharedSecret);
        Task SetIsDiscountCodeGenerator(long integratedSystemId, bool value);
    }
}