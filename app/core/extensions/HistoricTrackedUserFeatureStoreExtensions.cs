using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public static class HistoricTrackedUserFeatureStoreExtensions
    {
        public static async Task<IEnumerable<HistoricTrackedUserFeature>> ReadAllLatestFeatures(this IHistoricTrackedUserFeatureStore store, Customer customer)
        {
            var features = await store.GetFeaturesFor(customer);
            var results = new List<HistoricTrackedUserFeature>();
            foreach (var feature in features)
            {
                results.Add(await store.ReadFeature(customer, feature));
            }
            return results;
        }
    }
}