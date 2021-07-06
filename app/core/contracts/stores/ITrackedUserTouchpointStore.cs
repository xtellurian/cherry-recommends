using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITrackedUserTouchpointStore : IEntityStore<TrackedUserTouchpoint>
    {
        Task<TrackedUserTouchpoint> ReadTouchpoint(TrackedUser trackedUser, Touchpoint touchpoint, int? version = null);
        Task<bool> TouchpointExists(TrackedUser trackedUser, Touchpoint touchpoint, int? version = null);
        Task<int> CurrentMaximumTouchpointVersion(TrackedUser trackedUser, Touchpoint touchpoint);
        Task<IEnumerable<Touchpoint>> GetTouchpointsFor(TrackedUser trackedUser);
    }
}