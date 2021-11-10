using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Features.Destinations;

namespace SignalBox.Core.Workflows
{
    public class FeatureWorkflows : FeatureWorkflowBase, IWorkflow
    {
        private readonly IIntegratedSystemStore systemStore;
        private readonly ITrackedUserStore trackedUserStore;


        public FeatureWorkflows(IFeatureStore featureStore,
                                   IHistoricTrackedUserFeatureStore trackedUserFeatureStore,
                                   IIntegratedSystemStore systemStore,
                                   RecommenderTriggersWorkflows triggersWorkflows,
                                   HubspotPushWorkflows hubspotPushWorkflows,
                                   IWebhookSenderClient webhookSenderClient,
                                   ITrackedUserStore trackedUserStore,
                                   ITelemetry telemetry,
                                   ILogger<FeatureWorkflows> logger)
                                   : base(featureStore, trackedUserFeatureStore, triggersWorkflows, hubspotPushWorkflows, webhookSenderClient, telemetry, logger)
        {
            this.systemStore = systemStore;
            this.trackedUserStore = trackedUserStore;
        }

        public async Task<Feature> CreateFeature(string commonId, string name)
        {
            var feature = await featureStore.Create(new Feature(commonId, name));
            await featureStore.Context.SaveChanges();
            return feature;
        }

        public async Task<Paginated<TrackedUser>> GetTrackedUsers(Feature feature, int page)
        {
            return await featureStore.QueryTrackedUsers(page, feature.Id);
        }

        public async Task<HistoricTrackedUserFeature> ReadFeatureValues(TrackedUser trackedUser, string featureCommonId, int? version = null)
        {
            var feature = await featureStore.ReadFromCommonId(featureCommonId);
            return await trackedUserFeatureStore.ReadFeature(trackedUser, feature, version);
        }

        // ------ ADD/REMOVE DESTINATIONS -----
        public async Task<FeatureDestinationBase> RemoveDestination(Feature feature, long destinationId)
        {
            await featureStore.LoadMany(feature, _ => _.Destinations);
            var destination = feature.Destinations.FirstOrDefault(_ => _.Id == destinationId);
            if (destination == null)
            {
                throw new BadRequestException($"Destination Id {destinationId} is not a destination of Recommender Id {feature.Id}");
            }

            feature.Destinations.Remove(destination);
            await featureStore.Context.SaveChanges();
            return destination;
        }

        public async Task<FeatureDestinationBase> AddDestination(Feature feature,
                                                    long systemId,
                                                    string destinationType,
                                                    string endpoint = null,
                                                    string propertyName = null)
        {
            var maxDestinations = 5;
            var system = await systemStore.Read(systemId);
            await featureStore.LoadMany(feature, _ => _.Destinations);

            if (feature.Destinations.Count > maxDestinations)
            {
                throw new BadRequestException($"The maximum number of destinations is {maxDestinations}");
            }

            FeatureDestinationBase destination;
            switch (destinationType)
            {
                case null:
                    throw new BadRequestException("DestinationType cannot be null");
                case FeatureDestinationBase.WebhookDestinationType:
                    destination = new WebhookFeatureDestination(feature, system, endpoint);
                    break;
                case FeatureDestinationBase.HubspotContactPropertyDestinationType:
                    destination = new HubspotContactPropertyFeatureDestination(feature, system, propertyName);
                    break;
                default:
                    throw new BadRequestException($"DestinationType {destinationType} is an unknown type");

            }


            feature.Destinations.Add(destination);
            await featureStore.Context.SaveChanges();

            if (destination.Discriminator == nameof(FeatureDestinationBase))
            {
                feature.Destinations.Remove(destination);
                await featureStore.Context.SaveChanges();

                throw new ConfigurationException($"Could not create destination of type {destination.GetType().Name}. You may need a database migration");
            }

            return destination;
        }
    }
}