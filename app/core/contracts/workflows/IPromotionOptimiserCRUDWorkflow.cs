using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IPromotionOptimiserCRUDWorkflow
    {
        Task<PromotionOptimiser> Create(ItemsRecommender recommender);
        Task<PromotionOptimiser> Read(string recommenderId, bool? useInternalId = null);
        Task<PromotionOptimiser> UpdateAllWeights(string recommenderId, IEnumerable<IWeighted> weights, bool? useInternalId = null);
        Task<PromotionOptimiser> UpdateWeight(string recommenderId, long weightId, double weight, bool? useInternalId = null);
        Task<bool> Delete(long id);
    }
}
