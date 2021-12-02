using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public class NewTenantMembershipQueueStore : AzureQueueStoreBase<NewTenantMembershipQueueMessage>, INewTenantMembershipQueueStore
    {
        public NewTenantMembershipQueueStore(IOptions<AzureQueueConfig> options)
        : base(options, SignalBox.Core.Constants.AzureQueueNames.NewTenantMemberships)
        {
        }
    }
}