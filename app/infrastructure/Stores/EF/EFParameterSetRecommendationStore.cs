using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFParameterSetRecommendationStore : EFEntityStoreBase<ParameterSetRecommendation>, IParameterSetRecommendationStore
    {
        public EFParameterSetRecommendationStore(SignalBoxDbContext context) 
        : base(context, (c) => c.ParameterSetRecommendations)
        {
        }
    }
}