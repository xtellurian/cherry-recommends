using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class InMemoryEventStore : ITrackedUserEventStore
    {
        private List<TrackedUserEvent> eventStore = new List<TrackedUserEvent>();

        public Task<IEnumerable<TrackedUserEvent>> AddTrackedUserEvents(IEnumerable<TrackedUserEvent> events)
        {
            events = events.Where(_ => _ != null); // remove nulls
            eventStore.AddRange(events);
            return Task.FromResult(events);
        }

        public Task<int> Count(Expression<Func<TrackedUserEvent, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountEventsOfKind(string kind, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountEventsOfType(string kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountTrackedUsers(Expression<Func<TrackedUserEvent, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrackedUserEvent>> Latest(DateTimeOffset after)
        {
            throw new NotImplementedException();
        }

        public Task LoadMany<TProperty>(TrackedUserEvent entity, Expression<Func<TrackedUserEvent, IEnumerable<TProperty>>> propertyExpression) where TProperty : class
        {
            throw new NotImplementedException();
        }

        public Task<TProperty> Max<TProperty>(Expression<Func<TrackedUserEvent, TProperty>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TProperty> Min<TProperty>(Expression<Func<TrackedUserEvent, TProperty>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TrackedUserEvent> Read(string eventId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrackedUserEvent>> ReadEventsForUser(string commonUserId)
        {
            return Task.FromResult(eventStore.Where(_ => _.CommonUserId == commonUserId));
        }

        public Task<IEnumerable<TrackedUserEvent>> ReadEventsForUser(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrackedUserEvent>> ReadEventsOfKind(string kind, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            return Task.FromResult(eventStore.Where(_ => _.EventType == eventType));
        }

        public Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> ReadUniqueEventTypes(string kind)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<string>> ReadUniqueEventTypes()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> ReadUniqueKinds()
        {
            throw new System.NotImplementedException();
        }
    }
}