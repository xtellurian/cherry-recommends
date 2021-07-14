using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITrackedUserFeatureStore : IEntityStore<TrackedUserFeature>
    {
        Task<TrackedUserFeature> ReadFeature(TrackedUser trackedUser, Feature feature, int? version = null);
        Task<bool> FeatureExists(TrackedUser trackedUser, Feature feature, int? version = null);
        Task<int> CurrentMaximumFeatureVersion(TrackedUser trackedUser, Feature feature);
        Task<IEnumerable<Feature>> GetFeaturesFor(TrackedUser trackedUser);
    }
}