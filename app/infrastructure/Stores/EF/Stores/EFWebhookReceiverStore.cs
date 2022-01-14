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

        public async Task<WebhookReceiver> ReadFromEndpointId(string endpointId)
        {
            // todo: make this a better error when not exist, rather than 500
            // use Set not QuerySet because we don't know the environment yet.
            var endpoint = await Set.Include(_ => _.IntegratedSystem).FirstAsync(_ => _.EndpointId == endpointId);
            if (endpoint.EnvironmentId.HasValue)
            {
                environmentProvider.SetOverride(endpoint.EnvironmentId.Value);
            }
            return endpoint;
        }
    }
}