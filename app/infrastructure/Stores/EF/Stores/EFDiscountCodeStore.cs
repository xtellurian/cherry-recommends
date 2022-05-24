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

        public Task<EntityResult<DiscountCode>> ReadByCode(string code)
        {
            var entity = QuerySet.FirstOrDefault(_ => _.Code == code);

            return Task.FromResult(new EntityResult<DiscountCode>(entity));
        }

        public Task<EntityResult<DiscountCode>> ReadLatestByPromotion(RecommendableItem promotion)
        {
            var entity = QuerySet.OrderByDescending(_ => _.Created).FirstOrDefault(_ => _.PromotionId == promotion.Id);

            return Task.FromResult(new EntityResult<DiscountCode>(entity));
        }
    }
}