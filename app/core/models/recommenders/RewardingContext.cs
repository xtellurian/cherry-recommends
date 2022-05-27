using Microsoft.Extensions.Logging;
using SignalBox.Core.Workflows;

namespace SignalBox.Core.Campaigns
{
    public class RewardingContext : CampaignContextBase
    {
        public RewardingContext(ILogger<IWorkflow> logger) : base(logger)
        {
        }

        public long? NormaliseToMaximum { get; set; }
    }
}