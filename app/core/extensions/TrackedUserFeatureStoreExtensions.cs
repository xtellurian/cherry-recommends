using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public static class TrackedUserFeatureStoreExtensions
    {
        public static async Task<IEnumerable<TrackedUserFeature>> ReadAllLatestFeatures(this ITrackedUserFeatureStore store, TrackedUser trackedUser)
        {
            var features = await store.GetFeaturesFor(trackedUser);
            var results = new List<TrackedUserFeature>();
            foreach (var feature in features)
            {
                results.Add(await store.ReadFeature(trackedUser, feature));
            }
            return results;
        }
    }
}