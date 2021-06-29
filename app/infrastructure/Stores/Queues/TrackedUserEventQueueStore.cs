using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public class TrackedUserEventQueueStore : AzureQueueStoreBase<TrackedUserEventsQueueMessage>, ITrackedUserEventQueueStore
    {
        public TrackedUserEventQueueStore(IOptions<AzureQueueConfig> options) : base(options, "tracked-user-events")
        {
        }
    }
}