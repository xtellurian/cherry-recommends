using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core.Adapters.Hubspot;
using SignalBox.Core.Integrations;
using SignalBox.Core.Integrations.Hubspot;

namespace SignalBox.Core.Workflows
{
    public class HubspotWorkflows : IWorkflow
    {
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ITrackedUserStore trackedUserStore;
        private readonly ITrackedUserSystemMapStore systemMapStore;
        private readonly IStorageContext storageContext;
        private readonly IHubspotService hubspotService;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILogger<HubspotWorkflows> logger;
        private readonly IOptions<HubspotAppCredentials> hubspotCreds;

        public HubspotWorkflows(IIntegratedSystemStore integratedSystemStore,
                                ITrackedUserStore trackedUserStore,
                                ITrackedUserSystemMapStore systemMapStore,
                                IStorageContext storageContext,
                                IHubspotService hubspotService,
                                IDateTimeProvider dateTimeProvider,
                                ILogger<HubspotWorkflows> logger,
                                IOptions<HubspotAppCredentials> hubspotCreds)
        {
            this.integratedSystemStore = integratedSystemStore;
            this.trackedUserStore = trackedUserStore;
            this.systemMapStore = systemMapStore;
            this.storageContext = storageContext;
            this.hubspotService = hubspotService;
            this.dateTimeProvider = dateTimeProvider;
            this.logger = logger;
            this.hubspotCreds = hubspotCreds;
        }

        private async Task CheckAndRefreshCredentials(IntegratedSystem system)
        {
            if (!system.TokenResponseUpdated.HasValue || dateTimeProvider.Now > system.TokenResponseUpdated.Value.AddSeconds(system.TokenResponse.ExpiresIn))
            {
                await this.RefreshCredentials(system);
            }
        }

        private async Task<TrackedUserSystemMap> GetSystemMap(IntegratedSystem system, TrackedUser trackedUser)
        {
            await trackedUserStore.LoadMany(trackedUser, _ => _.IntegratedSystemMaps);
            var map = trackedUser.IntegratedSystemMaps.FirstOrDefault(_ => _.IntegratedSystemId == system.Id);
            if (map == null)
            {
                throw new ConfigurationException($"Tracked User {trackedUser.CommonId} is not lined to system {system.CommonId}");
            }

            return map;
        }

        public async Task<HubspotCache> GetCache(long integratedSystemId)
        {
            var system = await integratedSystemStore.Read(integratedSystemId);
            if (system.SystemType != IntegratedSystemTypes.Hubspot)
            {
                throw new BadRequestException($"{integratedSystemId} is not a Hubspot System.");
            }

            return system.GetCache<HubspotCache>();
        }

        public async Task<IEnumerable<HubspotContactProperty>> LoadContactProperties(long integratedSystemId)
        {
            var system = await integratedSystemStore.Read(integratedSystemId);
            await CheckAndRefreshCredentials(system);
            return await hubspotService.GetContactProperties(system);
        }

        public async Task<IEnumerable<HubspotContact>> LoadContacts(long integratedSystemId)
        {
            var system = await integratedSystemStore.Read(integratedSystemId);
            await CheckAndRefreshCredentials(system);
            return await hubspotService.GetContacts(system);
        }

        public async Task<IEnumerable<HubspotEvent>> LoadContactEvents(long integratedSystemId, string trackedUserId = null, int? limit = null)
        {
            var system = await integratedSystemStore.Read(integratedSystemId);
            await CheckAndRefreshCredentials(system);
            long? userId = null;
            if (trackedUserId != null)
            {
                if (await trackedUserStore.ExistsFromCommonId(trackedUserId))
                {
                    var trackedUser = await trackedUserStore.ReadFromCommonId(trackedUserId);
                    var map = await GetSystemMap(system, trackedUser);
                    userId = int.Parse(map.UserId);
                }
                else
                {
                    if (int.TryParse(trackedUserId, out var id))
                    {
                        var trackedUser = await trackedUserStore.Read(id);
                        var map = await GetSystemMap(system, trackedUser);
                        userId = int.Parse(map.UserId);
                    }
                    else
                    {
                        throw new BadRequestException($"Unknown Tracked User {trackedUserId}");
                    }
                }
            }

            return await hubspotService.GetContactEvents(system, dateTimeProvider.Now.AddMonths(-3).DateTime, null, userId, limit);
        }

        public async Task<IEnumerable<TrackedUser>> GetAssociatedTrackedUsersFromTicket(string integratedSystemCommonId, string ticketId)
        {
            var system = await integratedSystemStore.ReadFromCommonId(integratedSystemCommonId);
            await CheckAndRefreshCredentials(system);
            var associations = await hubspotService.GetAssociatedContactsFromTicket(system, ticketId);
            var contactIds = associations
                .Where(_ => _.Type == "ticket_to_contact")
                .Select(_ => _.Id);
            var systemMaps = await systemMapStore.Query(1,  // first page
                    _ => _.TrackedUser,
                    _ => contactIds.Contains(_.UserId) && _.IntegratedSystemId == system.Id);

            return systemMaps.Items.Select(_ => _.TrackedUser);
        }

        public Task<HubspotAppCredentials> GetHubspotCredentials()
        {
            if (hubspotCreds.Value.ClientId == null)
            {
                throw new WorkflowException("Hubspot integration not configured correctly. Contact admin.");
            }
            return Task.FromResult(hubspotCreds.Value);
        }

        public async Task SaveTokenFromCode(long integratedSystemId, string code, string redirectUri)
        {
            var integratedSystem = await integratedSystemStore.Read(integratedSystemId);

            try
            {
                var tokenResponse = await hubspotService.ExchangeCode(hubspotCreds.Value.ClientId, hubspotCreds.Value.ClientSecret, redirectUri, code);
                // get the details of the hubspot system
                var details = await hubspotService.GetAccountDetails(tokenResponse);
                integratedSystem.SetCache(new HubspotCache(details));

                integratedSystem.CommonId = details.PortalId.ToString();
                integratedSystem.TokenResponse = tokenResponse;
                integratedSystem.TokenResponseUpdated = dateTimeProvider.Now;
                integratedSystem.IntegrationStatus = IntegrationStatuses.OK;

                await integratedSystemStore.Update(integratedSystem);
                await storageContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException("An error occurred when accessing Hubspot", ex);
            }
        }

        private async Task RefreshCredentials(IntegratedSystem integratedSystem)
        {
            logger.LogInformation($"Refreshing credentials for integrated system: {integratedSystem.Id}");
            try
            {
                var tokenResponse = await hubspotService.UseRefreshToken(hubspotCreds.Value.ClientId, hubspotCreds.Value.ClientSecret, integratedSystem.TokenResponse.RefreshToken);

                integratedSystem.TokenResponse = tokenResponse;
                integratedSystem.TokenResponseUpdated = dateTimeProvider.Now;
                integratedSystem.IntegrationStatus = IntegrationStatuses.OK;

                await integratedSystemStore.Update(integratedSystem);
                await storageContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException("An error occurred when accessing Hubspot", ex);
            }
        }
    }
}
