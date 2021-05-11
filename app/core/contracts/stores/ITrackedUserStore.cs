using System.Collections.Generic;
using System.Threading.Tasks;
#nullable enable
namespace SignalBox.Core
{
    public interface ITrackedUserStore : IEntityStore<TrackedUser>
    {
        Task<long> GetInternalId(string externalId);
        Task<string> GetExternalId(long internalId);
        Task<TrackedUser> ReadFromExternalId(string externalId);
        Task<IEnumerable<TrackedUser>> CreateIfNotExists(IEnumerable<string> externalIds);
        Task<bool> ExistsExternalId(string externalId);
    }
}