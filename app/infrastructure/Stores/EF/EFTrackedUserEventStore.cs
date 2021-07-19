using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTrackedUserEventStore : EFEntityStoreBase<TrackedUserEvent>, ITrackedUserEventStore
    {
        public EFTrackedUserEventStore(SignalBoxDbContext context)
        : base(context, (c) => c.TrackedUserEvents)
        {
        }

        public async Task<IEnumerable<TrackedUserEvent>> AddTrackedUserEvents(IEnumerable<TrackedUserEvent> events)
        {
            // need to chunk this query, because too many ids in a single Contains() breaks the db connection.
            foreach (var chunks in events.ToChunks(255))
            {
                var eventIds = chunks.Select(_ => _.EventId);
                var toRemove = Set.Where(_ => eventIds.Contains(_.EventId));
                if (toRemove.Any())
                {
                    Set.RemoveRange(await toRemove.ToListAsync());
                }
            }

            await Set.AddRangeAsync(events);
            return events;
        }

        public async Task<int> CountTrackedUsers(Expression<Func<TrackedUserEvent, bool>> predicate = null)
        {
            return await Set.Where(predicate ?? ((x) => true)).Select(_ => _.CommonUserId).Distinct().CountAsync();
        }

        public async Task<IEnumerable<TrackedUserEvent>> ReadEventsForUser(string commonUserId)
        {
            return await Set.Where(_ => _.CommonUserId == commonUserId).ToListAsync();
        }

        public async Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            return await Set.Where(_ => _.EventType == eventType && _.Timestamp > since && _.Timestamp < until).ToListAsync();
        }

        public async Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            return await Set.Where(_ => _.Kind == kind && _.EventType == eventType && _.Timestamp > since && _.Timestamp < until).ToListAsync();
        }

        public async Task<IEnumerable<TrackedUserEvent>> ReadEventsOfKind(string kind, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            return await Set.Where(_ => _.Kind == kind && _.Timestamp > since && _.Timestamp < until).ToListAsync();
        }

        public async Task<IEnumerable<string>> ReadUniqueEventTypes(string kind)
        {
            return await Set.Where(_ => _.Kind == kind).Select(_ => _.EventType).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<string>> ReadUniqueEventTypes()
        {
            return await Set.Select(_ => _.EventType).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<string>> ReadUniqueKinds()
        {
            return await Set.Select(_ => _.Kind).Distinct().ToListAsync();
        }

        public async Task<long> CountEventsOfKind(string kind, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            Expression<Func<TrackedUserEvent, bool>> predicate = _ => _.Kind == kind && _.Timestamp > since && _.Timestamp < until;
            return await base.Count(predicate);
        }
        public async Task<long> CountEventsOfType(string kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            Expression<Func<TrackedUserEvent, bool>> predicate = _ => _.Kind == kind && _.EventType == eventType && _.Timestamp > since && _.Timestamp < until;
            return await base.Count(predicate);
        }
        public async Task<long> CountEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            Expression<Func<TrackedUserEvent, bool>> predicate = _ => _.EventType == eventType && _.Timestamp > since && _.Timestamp < until;
            return await base.Count(predicate);
        }

        public async Task<IEnumerable<TrackedUserEvent>> Latest(DateTimeOffset after)
        {
            return await Set
                .Where(_ => _.Timestamp > after) // filtering by this column, has an index, and therefore is performant.
                .OrderByDescending(_ => _.Created)
                .Take(32).Skip(0)
                .ToListAsync();
        }
    }
}