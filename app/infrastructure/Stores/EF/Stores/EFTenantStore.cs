using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTenantStore : ITenantStore
    {
        private readonly MultiTenantDbContext context;

        public EFTenantStore(MultiTenantDbContext context)
        {
            this.context = context;
        }

        public async Task<Tenant> Create(Tenant tenant)
        {
            var result = await context.Tenants.AddAsync(tenant);
            return result.Entity;
        }

        public async Task<bool> TenantExists(string name)
        {
            return await context.Tenants.AnyAsync(_ => _.Name == name.ToLowerInvariant());
        }

        public async Task<Tenant> ReadFromName(string name)
        {
            return await this.context.Tenants.FirstAsync(_ => _.Name == name.ToLowerInvariant());
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tenant>> List()
        {
            return await context.Tenants.ToListAsync();
        }
    }
}