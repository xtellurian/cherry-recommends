using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IRecommenderModelClient<TInput, TOutput> where TInput : IModelInput where TOutput : IModelOutput
    {
        Task<TOutput> Invoke(IRecommender recommender, string version, TInput input);
    }
}