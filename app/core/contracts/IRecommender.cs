using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IRecommender<T>
    {
        Task<T> Recommend(RecommendationRequestArguments context);
    }
}