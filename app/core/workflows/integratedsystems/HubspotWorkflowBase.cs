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
        protected readonly IOptions<HubspotAppCredentials> hubspotCreds;
        protected readonly IIntegratedSystemStore integratedSystemStore;
        protected readonly ITrackedUserStore trackedUserStore;
        protected readonly IDateTimeProvider dateTimeProvider;

        public HubspotWorkflowBase(ILogger<HubspotWorkflowBase> logger,
                                   IHubspotService hubspotService,
                                   IOptions<HubspotAppCredentials> hubspotCreds,
                                   IIntegratedSystemStore integratedSystemStore,
                                   ITrackedUserStore trackedUserStore,
                                   IDateTimeProvider dateTimeProvider)
        {
            this.logger = logger;
            this.hubspotService = hubspotService;
            this.hubspotCreds = hubspotCreds;
            this.integratedSystemStore = integratedSystemStore;
            this.trackedUserStore = trackedUserStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        protected async Task CheckAndRefreshCredentials(IntegratedSystem system)
        {
            if (!system.TokenResponseUpdated.HasValue || dateTimeProvider.Now > system.TokenResponseUpdated.Value.AddSeconds(system.TokenResponse.ExpiresIn))
            {
                await this.RefreshCredentials(system);
            }
        }

        protected async Task<TrackedUserSystemMap> GetSystemMap(IntegratedSystem system, TrackedUser trackedUser)
        {
            await trackedUserStore.LoadMany(trackedUser, _ => _.IntegratedSystemMaps);
            var map = trackedUser.IntegratedSystemMaps.FirstOrDefault(_ => _.IntegratedSystemId == system.Id);
            if (map == null)
            {
                throw new ConfigurationException($"Tracked User {trackedUser.CommonId} is not lined to system {system.CommonId}");
            }

            return map;
        }

        protected async Task RefreshCredentials(IntegratedSystem integratedSystem)
        {
            logger.LogInformation($"Refreshing credentials for integrated system: {integratedSystem.Id}");
            try
            {
                var tokenResponse = await hubspotService.UseRefreshToken(hubspotCreds.Value.ClientId, hubspotCreds.Value.ClientSecret, integratedSystem.TokenResponse.RefreshToken);

                integratedSystem.TokenResponse = tokenResponse;
                integratedSystem.TokenResponseUpdated = dateTimeProvider.Now;
                integratedSystem.IntegrationStatus = IntegrationStatuses.OK;

                await integratedSystemStore.Update(integratedSystem);
                await integratedSystemStore.Context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException("An error occurred when accessing Hubspot", ex);
            }
        }

    }
}