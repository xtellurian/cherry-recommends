using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITenantStore
    {
        Task<IEnumerable<Tenant>> List();
        Task<Tenant> ReadFromName(string name);
        Task<Tenant> Create(Tenant tenant);
        Task SaveChanges();
        Task<bool> TenantExists(string name);
    }
}