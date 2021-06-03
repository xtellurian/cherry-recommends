using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFProductStore : EFEntityStoreBase<Product>, IProductStore
    {
        public EFProductStore(SignalBoxDbContext context)
        : base(context, (c) => c.Products)
        {
        }

        public override async Task<Product> Read(long id)
        {
            return await Set
                .Include(_ => _.Skus)
                .SingleAsync(_ => _.Id == id);
        }
    }
}