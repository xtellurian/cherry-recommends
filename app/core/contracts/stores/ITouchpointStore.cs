using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITouchpointStore : ICommonEntityStore<Touchpoint>
    {
        Task<Paginated<TrackedUser>> QueryTrackedUsers(int page, long touchpointId);
    }
}