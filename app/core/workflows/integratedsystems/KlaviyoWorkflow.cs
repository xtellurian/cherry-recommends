using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Integrations;

namespace SignalBox.Core.Workflows
{
    public class KlaviyoSystemWorkflow : IWorkflow, IKlaviyoSystemWorkflow
    {
        private readonly HttpClient httpClient;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ILogger<KlaviyoSystemWorkflow> logger;

        public KlaviyoSystemWorkflow(HttpClient httpClient,
                                    IIntegratedSystemStore integratedSystemStore,
                                    ILogger<KlaviyoSystemWorkflow> logger)
        {
            this.httpClient = httpClient;
            this.integratedSystemStore = integratedSystemStore;
            this.logger = logger;
        }

        public async Task<IntegratedSystem> SetApiKeys(IntegratedSystem system, string publicKey, string privateKey)
        {
            if (system.SystemType != IntegratedSystemTypes.Klaviyo)
            {
                throw new BadRequestException("Only Klaviyo system is allowed.");
            }

            KlaviyoApiKeys apiKeys = new KlaviyoApiKeys(publicKey, privateKey);
            var cache = new KlaviyoCache(apiKeys);
            system.SetCache(cache);

            system.IntegrationStatus = IntegrationStatuses.OK;
            await integratedSystemStore.Context.SaveChanges();
            return system;
        }
    }
}