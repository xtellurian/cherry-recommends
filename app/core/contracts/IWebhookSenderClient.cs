using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommendations.Destinations;

namespace SignalBox.Core
{
    public interface IWebhookSenderClient
    {
        Task Send<TRecommendation>(WebhookDestination destination, TRecommendation recommendation)
        where TRecommendation : RecommendationEntity;
        Task Send<TRecommendation>(SegmentSourceFunctionDestination destination, TRecommendation recommendation)
        where TRecommendation : RecommendationEntity;
        Task Send<TPayload>(IWebhookDestination destination, TPayload payload);
    }
}