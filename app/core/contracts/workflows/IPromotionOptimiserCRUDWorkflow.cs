using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
    public interface IPromotionOptimiserCRUDWorkflow
    {
        Task<PromotionOptimiser> Create(PromotionsCampaign campaign);
        Task<PromotionOptimiser> Read(string recommenderId, bool? useInternalId = null);
        Task<PromotionOptimiser> UpdateAllWeights(string recommenderId, IEnumerable<IWeighted> weights, long? segmentId = null, bool? useInternalId = null);
        Task<PromotionOptimiser> UpdateWeight(string recommenderId, long weightId, double weight, long? segmentId = null, bool? useInternalId = null);
        Task<bool> Delete(long id);
        Task<IEnumerable<PromotionOptimiserWeight>> ReadWeights(string campaignId, long? segmentId = null, bool? useInternalId = null);
        Task<Paginated<Segment>> ReadSegments(string campaignId, bool? useInternalId = null);
        Task<PromotionOptimiser> AddSegment(string campaignId, long segmentId, bool? useInternalId = null);
        Task<bool> RemoveSegment(string campaignId, long segmentId, bool? useInternalId = null);
    }
}
