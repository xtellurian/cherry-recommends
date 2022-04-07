using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFSWebhookReceiverStore : EFEnvironmentScopedEntityStoreBase<WebhookReceiver>, IWebhookReceiverStore
    {
        public EFSWebhookReceiverStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.WebhookReceivers)
        { }

        public async Task<IEnumerable<WebhookReceiver>> GetReceiversForIntegratedSystem(long integratedSystemId)
        {
            var integratedSystem = await context.IntegratedSystems
                .Include(_ => _.WebhookReceivers)
                .FirstAsync(_ => _.Id == integratedSystemId);
            return integratedSystem.WebhookReceivers;
        }

        public async Task<EntityResult<WebhookReceiver>> ReadFromEndpointId(string endpointId)
        {
            // use Set not QuerySet because we don't know the environment yet.
            var endpoint = await Set.Include(_ => _.IntegratedSystem).FirstOrDefaultAsync(_ => _.EndpointId == endpointId);
            if (endpoint?.EnvironmentId.HasValue ?? false)
            {
                environmentProvider.SetOverride(endpoint.EnvironmentId.Value);
            }
            return new EntityResult<WebhookReceiver>(endpoint);
        }
    }
}