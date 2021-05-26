using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITrackedUserEventStore
    {
        Task<IEnumerable<TrackedUserEvent>> AddTrackedUserEvents(IEnumerable<TrackedUserEvent> events);
        Task<IEnumerable<TrackedUserEvent>> ReadEventsForUser(string commonUserId);
        Task<IEnumerable<TrackedUserEvent>> ReadEventsOfType(string eventType);
    }
}