using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core.Integrations;

namespace SignalBox.Core.Workflows
{
    public abstract class HubspotWorkflowBase
    {
        protected readonly ILogger<HubspotWorkflowBase> logger;
        protected readonly IHubspotService hubspotService;
        protected readonly HubspotAppCredentials hubspotCreds;
        protected readonly IIntegratedSystemStore integratedSystemStore;
        protected readonly ICustomerStore customerStore;
        protected readonly IDateTimeProvider dateTimeProvider;

        public HubspotWorkflowBase(ILogger<HubspotWorkflowBase> logger,
                                   IHubspotService hubspotService,
                                   IOptions<HubspotAppCredentials> hubspotCredOptions,
                                   IIntegratedSystemStore integratedSystemStore,
                                   ICustomerStore trackedUserStore,
                                   IDateTimeProvider dateTimeProvider)
        {
            this.logger = logger;
            this.hubspotService = hubspotService;
            this.hubspotCreds = hubspotCredOptions.Value;
            this.integratedSystemStore = integratedSystemStore;
            this.customerStore = trackedUserStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        protected async Task CheckAndRefreshCredentials(IntegratedSystem system)
        {
            if (!system.TokenResponseUpdated.HasValue || dateTimeProvider.Now > system.TokenResponseUpdated.Value.AddSeconds(system.TokenResponse.ExpiresIn))
            {
                await this.RefreshCredentials(system);
            }
        }

        protected async Task<TrackedUserSystemMap> GetSystemMap(IntegratedSystem system, Customer customer)
        {
            await customerStore.LoadMany(customer, _ => _.IntegratedSystemMaps);
            var map = customer.IntegratedSystemMaps.FirstOrDefault(_ => _.IntegratedSystemId == system.Id);
            if (map == null)
            {
                throw new ConfigurationException($"Tracked User {customer.CommonId} is not linked to system {system.CommonId}");
            }

            return map;
        }

        private async Task RefreshCredentials(IntegratedSystem integratedSystem)
        {
            logger.LogInformation($"Refreshing credentials for integrated system: {integratedSystem.Id}");
            try
            {
                var tokenResponse = await hubspotService.UseRefreshToken(hubspotCreds.ClientId, hubspotCreds.ClientSecret, integratedSystem.TokenResponse.RefreshToken);

                integratedSystem.TokenResponse = tokenResponse;
                integratedSystem.TokenResponseUpdated = dateTimeProvider.Now;
                integratedSystem.IntegrationStatus = IntegrationStatuses.OK;

                await integratedSystemStore.Update(integratedSystem);
                await integratedSystemStore.Context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message);
                throw new WorkflowException("An error occurred when accessing Hubspot", ex);
            }
        }

    }
}