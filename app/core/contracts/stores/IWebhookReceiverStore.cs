using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IWebhookReceiverStore : IEntityStore<WebhookReceiver>
    {
        Task<WebhookReceiver> ReadFromEndpointId(string endpointId);
        Task<IEnumerable<WebhookReceiver>> GetReceiversForIntegratedSystem(long integratedSystemId);
    }
}