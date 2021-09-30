using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class EFStorageContext : IStorageContext
    {
        private readonly SignalBoxDbContext context;

        public EFStorageContext(IDbContextProvider<SignalBoxDbContext> contextProvider)
        {
            this.context = contextProvider.Context;
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}