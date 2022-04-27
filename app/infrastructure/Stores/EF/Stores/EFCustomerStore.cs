using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFCustomerStore : EFCommonEntityStoreBase<Customer>, ICustomerStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFCustomerStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.Customers)
        { }

        public async Task<long> GetInternalId(string commonId)
        {
            var entity = await QuerySet.SingleAsync(_ => _.CommonId == commonId);
            return entity.Id;
        }

        public async Task<Customer> ReadFromCommonUserId(string commonId)
        {
            return await QuerySet.SingleAsync(_ => _.CommonId == commonId);
        }
    }
}