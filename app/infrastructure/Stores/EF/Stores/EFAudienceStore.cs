using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFAudienceStore : EFEntityStoreBase<Audience>, IAudienceStore
    {
        public EFAudienceStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, _ => _.Audiences)
        { }

        public async Task<EntityResult<Audience>> GetAudience(CampaignEntityBase recommender)
        {
            var entity = await QuerySet.Include(_ => _.Segments).FirstOrDefaultAsync(_ => _.RecommenderId == recommender.Id);
            var result = new EntityResult<Audience>(entity);

            return result;
        }

        public Task<bool> IsCustomerInAudience(Customer customer, Audience audience)
        {
            return Task.FromResult(
                QuerySet.Any(_ => _.Id == audience.Id
                && _.Segments.All(s => s.InSegment.Any(i => i.CustomerId == customer.Id))));
        }
    }
}