using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Adapters.Klaviyo;
using SignalBox.Core.Integrations;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public interface IKlaviyoService
    {
        Task<IEnumerable<KlaviyoList>> GetLists(KlaviyoApiKeys apiKeys);
        Task<IEnumerable<KlaviyoProfileResponse>> SubscribeCustomerToList<TRecommendation>(KlaviyoApiKeys apiKeys, string listTriggerId, TRecommendation recommendation) where TRecommendation : RecommendationEntity;
    }
}