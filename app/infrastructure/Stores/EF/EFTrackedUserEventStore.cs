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
        protected override Expression<Func<TrackedUserEvent, DateTimeOffset>> defaultOrderBy => _ => _.Timestamp;
        public EFTrackedUserEventStore(SignalBoxDbContext context)
        : base(context, (c) => c.TrackedUserEvents)
        {
        }

        public async Task<TrackedUserEvent> Read(string eventId)
        {
            if (await QuerySet.AnyAsync(_ => _.EventId == eventId))
            {
                return await QuerySet.FirstAsync(_ => _.EventId == eventId);
            }
            else if (long.TryParse(eventId, out var id))
            {
                try
                {
                    return await QuerySet.SingleAsync(_ => _.Id == id);
                }
                catch (Exception ex)
                {
                    throw new StorageException($"Event Id {eventId} not found", ex);
                }
            }
            else
            {
                throw new StorageException($"Event Id {eventId} not found");
            }
        }
        public async Task<IEnumerable<TrackedUserEvent>> AddTrackedUserEvents(IEnumerable<TrackedUserEvent> events)
        {
            // need to chunk this query, because too many ids in a single Contains() breaks the db connection.
            foreach (var chunks in events.ToChunks(255))
            {
                var eventIds = chunks.Select(_ => _.EventId);
                var toRemove = QuerySet.Where(_ => eventIds.Contains(_.EventId));
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
            return await QuerySet.Where(predicate ?? ((x) => true)).Select(_ => _.CommonUserId).Distinct().CountAsync();
        }

        public async Task<Paginated<TrackedUserEvent>> ReadEventsForUser(int page, TrackedUser trackedUser)
        {
            Expression<Func<TrackedUserEvent, bool>> predicate = _ => _.CommonUserId == trackedUser.CommonId;
            var itemCount = await QuerySet.CountAsync(predicate);
            List<TrackedUserEvent> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await Set
                    .Where(predicate)
                    .OrderByDescending(_ => _.Timestamp)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<TrackedUserEvent>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<TrackedUserEvent>(results, pageCount, itemCount, page);
        }

        public async Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            return await QuerySet.Where(_ => _.EventType == eventType && _.Timestamp > since && _.Timestamp < until).ToListAsync();
        }

        public async Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            return await QuerySet.Where(_ => _.Kind == kind && _.EventType == eventType && _.Timestamp > since && _.Timestamp < until).ToListAsync();
        }

        public async Task<IEnumerable<TrackedUserEvent>> ReadEventsOfKind(string kind, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            return await QuerySet.Where(_ => _.Kind == kind && _.Timestamp > since && _.Timestamp < until).ToListAsync();
        }

        public async Task<IEnumerable<string>> ReadUniqueEventTypes(string kind)
        {
            return await QuerySet.Where(_ => _.Kind == kind).Select(_ => _.EventType).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<string>> ReadUniqueEventTypes()
        {
            return await QuerySet.Select(_ => _.EventType).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<string>> ReadUniqueKinds()
        {
            return await QuerySet.Select(_ => _.Kind).Distinct().ToListAsync();
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
            return await QuerySet
                .Where(_ => _.Timestamp > after) // filtering by this column, has an index, and therefore is performant.
                .OrderByDescending(_ => _.Created)
                .Take(32).Skip(0)
                .ToListAsync();
        }
    }
}