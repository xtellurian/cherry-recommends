using System.Threading.Tasks;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
    public interface IAudienceStore : IEntityStore<Audience>
    {
        Task<EntityResult<Audience>> GetAudience(CampaignEntityBase recommender);
        Task<bool> IsCustomerInAudience(Customer customer, Audience audience);
    }
}