using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITrackedUserEventStore
    {
        Task<int> Count(Expression<Func<TrackedUserEvent, bool>> predicate = null);
        Task<int> CountTrackedUsers(Expression<Func<TrackedUserEvent, bool>> predicate = null);
        Task<IEnumerable<TrackedUserEvent>> AddTrackedUserEvents(IEnumerable<TrackedUserEvent> events);
        Task<IEnumerable<TrackedUserEvent>> ReadEventsForUser(string commonUserId);
        Task<IEnumerable<TrackedUserEvent>> ReadEventsOfKind(string kind, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<string>> ReadUniqueKinds();
        Task<IEnumerable<string>> ReadUniqueEventTypes();
        Task<IEnumerable<string>> ReadUniqueEventTypes(string kind);
    }
}