using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class FeatureWorkflows : FeatureWorkflowBase, IWorkflow
    {


        private readonly ITrackedUserStore trackedUserStore;


        public FeatureWorkflows(IFeatureStore featureStore,
                                   IHistoricTrackedUserFeatureStore trackedUserFeatureStore,
                                   RecommenderTriggersWorkflows triggersWorkflows,
                                   ITrackedUserStore trackedUserStore,
                                   ILogger<FeatureWorkflows> logger) : base(featureStore, trackedUserFeatureStore, triggersWorkflows, logger)
        {
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
    }
}