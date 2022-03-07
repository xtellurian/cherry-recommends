using Microsoft.Extensions.Logging;
using SignalBox.Core.Workflows;

namespace SignalBox.Core.Recommenders
{
    public class RewardingContext : RecommenderContextBase
    {
        public RewardingContext(ILogger<IWorkflow> logger) : base(logger)
        {
        }

        public long? NormaliseToMaximum { get; set; }
    }
}