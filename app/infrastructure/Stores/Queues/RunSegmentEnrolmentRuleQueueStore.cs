using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public class RunSegmentEnrolmentRuleQueueStore : AzureQueueStoreBase<RunSegmentEnrolmentRuleQueueMessage>, IRunSegmentEnrolmentRuleQueueStore
    {
        public RunSegmentEnrolmentRuleQueueStore(IOptions<AzureQueueConfig> options)
        : base(options, SignalBox.Core.Constants.AzureQueueNames.RunSegmentEnrolmentRule)
        {
        }
    }
}