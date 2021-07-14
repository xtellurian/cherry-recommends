using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IFeatureStore : ICommonEntityStore<Feature>
    {
        Task<Paginated<TrackedUser>> QueryTrackedUsers(int page, long featureId);
    }
}