using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IHistoricTrackedUserFeatureStore : IEntityStore<HistoricTrackedUserFeature>
    {
        // Should return the latest feature if version = null
        Task<HistoricTrackedUserFeature> ReadFeature(Customer customer, Feature feature, int? version = null);
        Task<bool> FeatureExists(Customer customer, Feature feature, int? version = null);
        Task<int> CurrentMaximumFeatureVersion(Customer customer, Feature feature);
        Task<IEnumerable<Feature>> GetFeaturesFor(Customer customer);
    }
}