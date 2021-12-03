using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public class RunAllFeatureGeneratorsQueueStore : AzureQueueStoreBase<RunAllFeatureGeneratorsQueueMessage>, IRunAllFeatureGeneratorsQueueStore
    {
        public RunAllFeatureGeneratorsQueueStore(IOptions<AzureQueueConfig> options)
        : base(options, SignalBox.Core.Constants.AzureQueueNames.RunAllFeatureGenerators)
        {
        }
    }
}