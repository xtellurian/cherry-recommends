using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public class NewTrackedUserEventQueueStore : AzureQueueStoreBase<NewCustomerEventQueueMessage>, INewTrackedUserEventQueueStore
    {
        public NewTrackedUserEventQueueStore(IOptions<AzureQueueConfig> options)
        : base(options, SignalBox.Core.Constants.AzureQueueNames.NewTrackedUsers)
        {
        }
    }
}