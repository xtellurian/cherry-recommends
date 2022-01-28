using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations.Destinations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
    public abstract class RecommenderWorkflowBase<TRecommender> : IWorkflow where TRecommender : RecommenderEntityBase
    {
        protected readonly IRecommenderStore<TRecommender> store;
        protected readonly IIntegratedSystemStore systemStore;
        protected readonly IMetricStore featureStore;
        private readonly RecommenderReportImageWorkflows imageWorkflows;

        protected RecommenderWorkflowBase(IRecommenderStore<TRecommender> store,
                                          IIntegratedSystemStore systemStore,
                                          IMetricStore featureStore,
                                          RecommenderReportImageWorkflows imageWorkflows)
        {
            this.store = store;
            this.systemStore = systemStore;
            this.featureStore = featureStore;
            this.imageWorkflows = imageWorkflows;
        }

        // ------ ADD/REMOVE DESTINATIONS -----
        public async Task<RecommenderEntityBase> RemoveDestination(TRecommender recommender, long destinationId)
        {
            await store.LoadMany(recommender, _ => _.RecommendationDestinations);
            var destination = recommender.RecommendationDestinations.FirstOrDefault(_ => _.Id == destinationId);
            if (destination == null)
            {
                throw new BadRequestException($"Destination Id {destinationId} is not a destination of Recommender Id {recommender.Id}");
            }

            recommender.RecommendationDestinations.Remove(destination);
            await store.Context.SaveChanges();
            return recommender;
        }

        public async Task<RecommendationDestinationBase> AddDestination(TRecommender recommender,
                                                                        long systemId,
                                                                        string destinationType,
                                                                        string endpoint)
        {
            var maxDestinations = 5;
            var system = await systemStore.Read(systemId);
            await store.LoadMany(recommender, _ => _.RecommendationDestinations);

            if (recommender.RecommendationDestinations.Count > maxDestinations)
            {
                throw new BadRequestException($"The maximum number of destinations is {maxDestinations}");
            }

            RecommendationDestinationBase destination;
            switch (destinationType)
            {
                case null:
                    throw new BadRequestException("DestinationType cannot be null");
                case RecommendationDestinationBase.WebhookDestinationType:
                    destination = new WebhookDestination(recommender, system, endpoint);
                    break;
                case RecommendationDestinationBase.SegmentSourceFunctionDestinationType:
                    destination = new SegmentSourceFunctionDestination(recommender, system, endpoint);
                    break;
                default:
                    throw new BadRequestException($"DestinationType {destinationType} is an unknown type");

            }


            recommender.RecommendationDestinations.Add(destination);
            await store.Context.SaveChanges();

            if (destination.Discriminator == "RecommendationDestinationBase")
            {
                recommender.RecommendationDestinations.Remove(destination);
                await store.Context.SaveChanges();

                throw new ConfigurationException($"Could not create destination of type {destination.GetType().Name}. You may need a database migration");
            }

            return destination;
        }

        // ------ SET LEARNING FEATURES -----

        public async Task<TRecommender> SetLearningMetrics(TRecommender recommender, IEnumerable<string> metricIds, bool? useInternalId = null)
        {
            var metrics = new List<Metric>();
            foreach (var metricId in metricIds)
            {
                metrics.Add(await featureStore.GetEntity(metricId, useInternalId));
            }

            await store.LoadMany(recommender, _ => _.LearningFeatures);

            recommender.LearningFeatures = metrics;

            await store.Context.SaveChanges();
            return recommender;
        }

        public async Task<byte[]> DownloadReportImage(RecommenderEntityBase recommender)
        {
            return await imageWorkflows.DownloadImage(recommender);
        }
    }
}