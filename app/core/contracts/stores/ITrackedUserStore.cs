using System.Collections.Generic;
using System.Threading.Tasks;
#nullable enable
namespace SignalBox.Core
{
    public interface ITrackedUserStore : IEntityStore<TrackedUser>
    {
        Task<long> GetInternalId(string commonUserId);
        Task<string> GetCommonUserId(long internalId);
        Task<TrackedUser> ReadFromCommonUserId(string commonUserId);
        Task<IEnumerable<TrackedUser>> CreateIfNotExists(IEnumerable<string> commonUserIds);
        Task<bool> ExistsCommonUserId(string commonId);
    }
}