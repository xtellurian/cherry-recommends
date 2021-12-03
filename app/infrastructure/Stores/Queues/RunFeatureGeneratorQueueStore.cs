using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public class RunFeatureGeneratorQueueStore : AzureQueueStoreBase<RunFeatureGeneratorQueueMessage>, IRunFeatureGeneratorQueueStore
    {
        public RunFeatureGeneratorQueueStore(IOptions<AzureQueueConfig> options)
        : base(options, SignalBox.Core.Constants.AzureQueueNames.RunFeatureGenerator)
        {
        }
    }
}