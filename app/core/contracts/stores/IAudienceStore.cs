using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IAudienceStore : IEntityStore<Audience>
    {
        Task<EntityResult<Audience>> GetAudience(RecommenderEntityBase recommender);
        Task<bool> IsCustomerInAudience(Customer customer, Audience audience);
    }
}