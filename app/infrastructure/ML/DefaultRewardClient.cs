using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.ML
{
    public class DefaultRewardClient : IRecommenderModelRewardClient
    {
        public Task Reward(IRecommender recommender, RewardingContext context, TrackedUserAction action)
        {
            return Task.CompletedTask;
        }
    }
}