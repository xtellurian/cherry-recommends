using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITrackedUserActionStore : IEntityStore<TrackedUserAction>
    {
        Task<TrackedUserAction> ReadLatestAction(string commonUserId, string actionName);
        Task<IEnumerable<string>> ReadUniqueActionNames(string commonUserId);
    }
}