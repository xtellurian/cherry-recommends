using System.Threading.Tasks;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public interface IProductRecommendationStore : IRecommendationStore<ProductRecommendation>
    { }
}