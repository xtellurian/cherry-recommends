using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public class NewTrackedUserEventQueueStore : AzureQueueStoreBase<NewTrackedUserEventQueueMessage>, INewTrackedUserEventQueueStore
    {
        public NewTrackedUserEventQueueStore(IOptions<AzureQueueConfig> options) : base(options, "new-tracked-users")
        {
        }
    }
}