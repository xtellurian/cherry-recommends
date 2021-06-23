using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.Services
{
    public class HttpParameterSetRecommenderService : IRecommender<ParameterSetRecommendation>
    {
        public Task<ParameterSetRecommendation> Recommend(RecommendationRequestArguments context)
        {
            throw new System.NotImplementedException();
        }
    }
}