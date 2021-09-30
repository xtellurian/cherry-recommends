using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFProductStore : EFCommonEntityStoreBase<Product>, IProductStore
    {
        public EFProductStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentService environmentService)
        : base(contextProvider, environmentService, (c) => c.Products)
        { }
    }
}