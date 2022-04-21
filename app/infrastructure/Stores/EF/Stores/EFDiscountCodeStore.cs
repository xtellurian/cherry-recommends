using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFDiscountCodeStore : EFEnvironmentScopedEntityStoreBase<DiscountCode>, IDiscountCodeStore
    {
        protected override bool IsEnvironmentScoped => true;

        public EFDiscountCodeStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.DiscountCodes)
        { }

        public Task<EntityResult<DiscountCode>> GetFromCode(string code)
        {
            var entity = QuerySet.Include(_ => _.Promotion).FirstOrDefault(_ => _.Code == code);

            return Task.FromResult(new EntityResult<DiscountCode>(entity));
        }

        public Task<EntityResult<DiscountCode>> GetLatestByPromotion(RecommendableItem promotion)
        {
            var entity = QuerySet.Include(_ => _.Promotion).OrderByDescending(_ => _.Created).FirstOrDefault(_ => _.PromotionId == promotion.Id);

            return Task.FromResult(new EntityResult<DiscountCode>(entity));
        }
    }
}