using System.Security.Claims;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Services
{
    public class EFTenantProvider : ITenantProvider
    {
#nullable enable
        private readonly ITenantStore tenantStore;
        private Tenant? current;
        private string? name;

        public EFTenantProvider(ITenantStore tenantStore)
        {
            this.tenantStore = tenantStore;
        }

        public string? RequestedTenantName => name;

        public string? CurrentDatabaseName => this.current?.DatabaseName;

        public Tenant? Current()
        {
            return current;
        }

        public async Task SetTenantName(string name)
        {
            this.name = name;
            if (await tenantStore.TenantExists(name))
            {
                this.current = await tenantStore.ReadFromName(this.name);
            }
        }
    }
}