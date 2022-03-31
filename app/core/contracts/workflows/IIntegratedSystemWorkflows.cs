using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IIntegratedSystemWorkflow
    {
        Task<IntegratedSystem> CreateIntegratedSystem(string name, string systemTypeName);
        Task<TrackedUserSystemMap> LinkTrackedUserToSystem(string systemUserId, long integratedSystemId, string commonUserId);
        Task<WebhookReceiver> AddWebhookReceiver(long integratedSystemId, bool? includeSharedSecret);
    }
}