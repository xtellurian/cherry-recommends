using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public class RunAllMetricGeneratorsQueueStore : AzureQueueStoreBase<RunAllMetricGeneratorsQueueMessage>, IRunAllMetricGeneratorsQueueStore
    {
        public RunAllMetricGeneratorsQueueStore(IOptions<AzureQueueConfig> options)
        : base(options, SignalBox.Core.Constants.AzureQueueNames.RunAllMetricGenerators)
        {
        }
    }
}