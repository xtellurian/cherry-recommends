using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFSWebhookReceiverStore : EFEntityStoreBase<WebhookReceiver>, IWebhookReceiverStore
    {
        public EFSWebhookReceiverStore(SignalBoxDbContext context)
        : base(context, (c) => c.WebhookReceivers)
        {
        }

        public async Task<IEnumerable<WebhookReceiver>> GetReceiversForIntegratedSystem(long integratedSystemId)
        {
            var integratedSystem = await context.IntegratedSystems
                .Include(_ => _.WebhookReceivers)
                .FirstAsync(_ => _.Id == integratedSystemId);
            return integratedSystem.WebhookReceivers;
        }

        public async Task<WebhookReceiver> ReadFromEndpointId(string endpointId)
        {
            return await Set.Include(_ => _.IntegratedSystem).FirstAsync(_ => _.EndpointId == endpointId);
        }
    }
}