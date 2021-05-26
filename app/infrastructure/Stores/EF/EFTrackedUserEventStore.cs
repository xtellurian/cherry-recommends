using System.Collections.Generic;
using System.Linq;
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
            var eventIds = events.Select(_ => _.EventId);
            var toRemove = Set.Where(_ => eventIds.Contains(_.EventId));
            if (toRemove.Any())
            {
                Set.RemoveRange(await toRemove.ToListAsync());
            }

            await Set.AddRangeAsync(events);
            return events;
        }

        public async Task<IEnumerable<TrackedUserEvent>> ReadEventsForUser(string commonUserId)
        {
            return await Set.Where(_ => _.CommonUserId == commonUserId).ToListAsync();
        }

        public async Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string eventType)
        {
            return await Set.Where(_ => _.EventType == eventType).ToListAsync();
        }
    }
}