using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IRecommender
    {
        Task<OfferRecommendation> Recommend(PresentationContext context);
    }
}