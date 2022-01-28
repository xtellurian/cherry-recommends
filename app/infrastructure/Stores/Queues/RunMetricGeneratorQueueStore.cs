using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public class RunMetricGeneratorQueueStore : AzureQueueStoreBase<RunMetricGeneratorQueueMessage>, IRunMetricGeneratorQueueStore
    {
        public RunMetricGeneratorQueueStore(IOptions<AzureQueueConfig> options)
        : base(options, SignalBox.Core.Constants.AzureQueueNames.RunMetricGenerator)
        {
        }
    }
}