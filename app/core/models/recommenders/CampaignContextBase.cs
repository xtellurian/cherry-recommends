using Microsoft.Extensions.Logging;
using SignalBox.Core.Workflows;

namespace SignalBox.Core.Campaigns
{
    public class CampaignContextBase
    {
        protected CampaignContextBase(ILogger<IWorkflow> logger)
        {
            Logger = logger;
        }

        public ILogger<IWorkflow> Logger { get; }
    }
}