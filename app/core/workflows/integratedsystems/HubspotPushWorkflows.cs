
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core.Adapters.Hubspot;
using SignalBox.Core.Metrics.Destinations;
using SignalBox.Core.Integrations;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core.Workflows
{
    public class HubspotPushWorkflows : HubspotWorkflowBase, IWorkflow
    {
        private readonly ITrackedUserSystemMapStore systemMapStore;
        private readonly PromotionsCampaignInvokationWorkflows itemsRecommenderWorkflows;

        public HubspotPushWorkflows(ILogger<HubspotEtlWorkflows> logger,
                                   IHubspotService hubspotService,
                                   IOptions<HubspotAppCredentials> hubspotCreds,
                                   IIntegratedSystemStore integratedSystemStore,
                                   ITrackedUserSystemMapStore systemMapStore,
                                   ICustomerStore trackedUserStore,
                                   IDateTimeProvider dateTimeProvider,
                                   PromotionsCampaignInvokationWorkflows itemsRecommenderWorkflows)
        : base(logger, hubspotService, hubspotCreds, integratedSystemStore, trackedUserStore, dateTimeProvider)
        {
            this.systemMapStore = systemMapStore;
            this.itemsRecommenderWorkflows = itemsRecommenderWorkflows;
        }

        public async Task<HubspotDataPushReport> RecommendForAllHubspotContacts(IntegratedSystem system, PromotionsCampaign recommender)
        {
            if (system.SystemType != IntegratedSystemTypes.Hubspot)
            {
                throw new BadRequestException($"Cannot process non-Hubspot systems here");
            }
            await base.CheckAndRefreshCredentials(system);

            var report = new HubspotDataPushReport(system.CommonId);
            var groupName = "four2"; // public API
            await hubspotService.EnsureContactPropertyGroupCreated(system, new HubspotContactPropertyGroup(groupName, "Four2"));
            var property = await hubspotService.EnsureContactPropertyCreated(system, new HubspotContactProperty(recommender.CommonId,
                                                                                                         recommender.Name,
                                                                                                         "string",
                                                                                                         description: null,
                                                                                                         false,
                                                                                                         groupName));

            // do this for all tracked users
            await foreach (var map in systemMapStore.Iterate(
                new EntityStoreIterateOptions<TrackedUserSystemMap>(_ => _.IntegratedSystemId == system.Id)))
            {
                await systemMapStore.Load(map, _ => _.Customer);
                if (await SetRecommendationProperty(system, recommender, map.Customer))
                {
                    report.NumberOfContactsUpdated += 1;
                }
                report.NumberOfHubspotRequests += 1;
            }

            return report;
        }

        private async Task<bool> SetRecommendationProperty(IntegratedSystem system, PromotionsCampaign recommender, Customer tu)
        {
            if (await systemMapStore.MapExists(tu, system))
            {
                var recommendationResponse = await itemsRecommenderWorkflows.InvokePromotionsCampaign(recommender, new ItemsModelInputDto(tu.CommonId));
                var topItemId = recommendationResponse.ScoredItems.OrderByDescending(_ => _.Score).FirstOrDefault().ItemCommonId;
                var map = await systemMapStore.FindMap(tu, system);
                await hubspotService.SetPropertyValue(system, new HubspotContactPropertyValue(map.UserId, recommender.CommonId, topItemId));
                return true;
            }
            else
            {
                logger.LogWarning($"Tracked User {tu.Id} has not system map for system {system.Id}");
                return false;
            }
        }

        public async Task SetMetricValueOnContact(HubspotContactPropertyMetricDestination destination, HistoricCustomerMetric metricValue)
        {
            var system = await integratedSystemStore.Read(destination.ConnectedSystemId);
            await CheckAndRefreshCredentials(system);
            if (await systemMapStore.MapExists(metricValue.Customer, system))
            {
                var map = await systemMapStore.FindMap(metricValue.Customer, system);
                await hubspotService.SetPropertyValue(system, new HubspotContactPropertyValue(map.UserId, destination.HubspotPropertyName, metricValue.Value.ToString()));
            }
            else
            {
                logger.LogWarning($"Missing System Map for Customer: {metricValue.Customer.CustomerId}");
            }
        }
    }

    public class HubspotDataPushReport
    {
        public HubspotDataPushReport(string commonId)
        {
            this.HubspotSystemCommonId = commonId;
        }
        public string HubspotSystemCommonId { get; set; }
        public int NumberOfHubspotRequests { get; set; }
        public int NumberOfContactsUpdated { get; set; }
    }
}