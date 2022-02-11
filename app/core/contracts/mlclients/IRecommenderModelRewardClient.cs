using System;
using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IRecommenderModelRewardClient
    {
        [Obsolete]
        Task Reward(IRecommender recommender, RewardingContext context);
    }
}