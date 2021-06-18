using System.Collections.Generic;
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
        private readonly IStorageContext storageContext;
        private readonly IHubspotService hubspotService;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILogger<HubspotWorkflows> logger;
        private readonly IOptions<HubspotAppCredentials> hubspotCreds;

        public HubspotWorkflows(IIntegratedSystemStore integratedSystemStore,
                                IStorageContext storageContext,
                                IHubspotService hubspotService,
                                IDateTimeProvider dateTimeProvider,
                                ILogger<HubspotWorkflows> logger,
                                IOptions<HubspotAppCredentials> hubspotCreds)
        {
            this.integratedSystemStore = integratedSystemStore;
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
