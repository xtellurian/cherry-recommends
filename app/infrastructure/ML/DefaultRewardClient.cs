using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.ML
{
    public class DefaultRewardClient : IRecommenderModelRewardClient
    {
        public Task Reward(ICampaign campaign, RewardingContext context)
        {
            return Task.CompletedTask;
        }
    }
}