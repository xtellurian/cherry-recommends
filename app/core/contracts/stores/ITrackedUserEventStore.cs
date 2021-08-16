using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITrackedUserEventStore
    {
        Task<TrackedUserEvent> Read(string eventId);
        Task LoadMany<TProperty>(TrackedUserEvent entity, Expression<Func<TrackedUserEvent, IEnumerable<TProperty>>> propertyExpression) where TProperty : class;
        Task<IEnumerable<TrackedUserEvent>> Latest(DateTimeOffset after);
        Task<int> Count(Expression<Func<TrackedUserEvent, bool>> predicate = null);
        Task<TProperty> Max<TProperty>(Expression<Func<TrackedUserEvent, TProperty>> selector);
        Task<TProperty> Min<TProperty>(Expression<Func<TrackedUserEvent, TProperty>> selector);
        Task<int> CountTrackedUsers(Expression<Func<TrackedUserEvent, bool>> predicate = null);
        Task<IEnumerable<TrackedUserEvent>> AddTrackedUserEvents(IEnumerable<TrackedUserEvent> events);
        Task<Paginated<TrackedUserEvent>> ReadEventsForUser(int page, TrackedUser user);
        Task<IEnumerable<TrackedUserEvent>> ReadEventsOfKind(string kind, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<long> CountEventsOfKind(string kind, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<string>> ReadUniqueKinds();
        Task<IEnumerable<string>> ReadUniqueEventTypes();
        Task<IEnumerable<string>> ReadUniqueEventTypes(string kind);
        Task<long> CountEventsOfType(string kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<long> CountEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
    }
}