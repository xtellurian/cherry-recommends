using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITrackedUserEventStore
    {
        Task AddTrackedUserEvents(IEnumerable<TrackedUserEvent> events);
        Task<IEnumerable<TrackedUserEvent>> ReadEventsForUser(string trackedUserExternalId);
        Task<IEnumerable<TrackedUserEvent>> ReadEventsForKey(string key, string value = null);
    }
}