using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Adapters.Klaviyo;
using SignalBox.Core.Integrations;

namespace SignalBox.Core.Workflows
{
    public class KlaviyoSystemWorkflow : IWorkflow, IKlaviyoSystemWorkflow
    {
        private readonly HttpClient httpClient;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly IKlaviyoService klaviyoService;
        private readonly IEmailChannelStore emailChannelStore;
        private readonly ILogger<KlaviyoSystemWorkflow> logger;

        public KlaviyoSystemWorkflow(HttpClient httpClient,
                                    IIntegratedSystemStore integratedSystemStore,
                                    IKlaviyoService klaviyoService,
                                    IEmailChannelStore emailChannelStore,
                                    ILogger<KlaviyoSystemWorkflow> logger)
        {
            this.httpClient = httpClient;
            this.integratedSystemStore = integratedSystemStore;
            this.klaviyoService = klaviyoService;
            this.emailChannelStore = emailChannelStore;
            this.logger = logger;
        }

        public async Task<IEnumerable<KlaviyoList>> GetLists(IntegratedSystem system)
        {
            KlaviyoApiKeys apiKeys = GetApiKeys(system);

            return await klaviyoService.GetLists(apiKeys);
        }

        private KlaviyoApiKeys GetApiKeys(IntegratedSystem system)
        {
            if (system.SystemType != IntegratedSystemTypes.Klaviyo)
            {
                throw new BadRequestException("Only Klaviyo system is allowed.");
            }

            var cache = system.GetCache<KlaviyoCache>();
            KlaviyoApiKeys apiKeys = cache.ApiKeys;
            return apiKeys;
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