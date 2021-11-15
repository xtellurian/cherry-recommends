using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public class NewTenantQueueStore : AzureQueueStoreBase<NewTenantQueueMessage>, INewTenantQueueStore
    {
        public NewTenantQueueStore(IOptions<AzureQueueConfig> options)
        : base(options, SignalBox.Core.Constants.AzureQueueNames.NewTenants)
        {
        }
    }
}