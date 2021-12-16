using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITrackedUserTouchpointStore : IEntityStore<TrackedUserTouchpoint>
    {
        Task<TrackedUserTouchpoint> ReadTouchpoint(Customer customer, Touchpoint touchpoint, int? version = null);
        Task<bool> TouchpointExists(Customer customer, Touchpoint touchpoint, int? version = null);
        Task<int> CurrentMaximumTouchpointVersion(Customer customer, Touchpoint touchpoint);
        Task<IEnumerable<Touchpoint>> GetTouchpointsFor(Customer customer);
    }
}