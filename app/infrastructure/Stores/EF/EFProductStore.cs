using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFProductStore : EFCommonEntityStoreBase<Product>, IProductStore
    {
        public EFProductStore(SignalBoxDbContext context, IEnvironmentService environmentService)
        : base(context, environmentService, (c) => c.Products)
        {
        }
    }
}