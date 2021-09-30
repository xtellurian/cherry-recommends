using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Shims
{
    public class ShimTenantStore : ITenantStore
    {
        public Task<Tenant> Create(Tenant tenant)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Tenant>> List()
        {
            return Task.FromResult<IEnumerable<Tenant>>(new List<Tenant>());
        }

        public Task<Tenant> ReadFromName(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> TenantExists(string name)
        {
            return Task.FromResult(false);
        }
    }
}