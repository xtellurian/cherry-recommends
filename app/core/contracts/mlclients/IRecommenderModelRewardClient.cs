using System;
using System.Threading.Tasks;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
    public interface IRecommenderModelRewardClient
    {
        [Obsolete]
        Task Reward(ICampaign campaign, RewardingContext context);
    }
}