using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class InMemoryStorageContext : IStorageContext
    {
        public Task SaveChanges()
        {
            return Task.CompletedTask;
        }
    }
}