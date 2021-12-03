using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#nullable enable
namespace SignalBox.Core
{
    public interface ITrackedUserStore : ICommonEntityStore<TrackedUser>
    {
        Task<long> GetInternalId(string commonUserId);
        Task<IEnumerable<TrackedUser>> CreateIfNotExists(IEnumerable<string> commonUserIds);
        Task<TrackedUser> CreateIfNotExists(string commonId, string? name = null);
        IAsyncEnumerable<TrackedUser> Iterate(int? limit = null);
    }
}