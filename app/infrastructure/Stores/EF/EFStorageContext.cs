using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class EFStorageContext : IStorageContext
    {
        private readonly SignalBoxDbContext context;

        public EFStorageContext(SignalBoxDbContext context)
        {
            this.context = context;
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}