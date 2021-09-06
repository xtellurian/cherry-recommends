using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public static class HistoricTrackedUserFeatureStoreExtensions
    {
        public static async Task<IEnumerable<HistoricTrackedUserFeature>> ReadAllLatestFeatures(this IHistoricTrackedUserFeatureStore store, TrackedUser trackedUser)
        {
            var features = await store.GetFeaturesFor(trackedUser);
            var results = new List<HistoricTrackedUserFeature>();
            foreach (var feature in features)
            {
                results.Add(await store.ReadFeature(trackedUser, feature));
            }
            return results;
        }
    }
}