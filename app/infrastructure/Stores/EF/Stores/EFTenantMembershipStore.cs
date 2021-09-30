using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTenantMembershipStore : ITenantMembershipStore
    {
        private readonly MultiTenantDbContext context;

        public EFTenantMembershipStore(MultiTenantDbContext context)
        {
            this.context = context;
        }

        public async Task<TenantMembership> Create(TenantMembership membership)
        {
            if (await IsMember(membership.Tenant, membership.UserId))
            {
                throw new BadRequestException($"{membership.UserId} is already a member of {membership.Tenant.Name}");
            }
            var result = await context.Memberships.AddAsync(membership);
            return result.Entity;
        }

        public async Task<bool> IsMember(Tenant tenant, string userId)
        {
            return await context.Memberships.AnyAsync(_ => _.UserId == userId && _.TenantId == tenant.Id);
        }

        public async Task<IEnumerable<TenantMembership>> ReadMemberships(string userId)
        {
            return await this.context.Memberships
                .Where(_ => _.UserId == userId)
                .Include(_ => _.Tenant)
                .ToListAsync();
        }

        public async Task<IEnumerable<TenantMembership>> ReadMemberships(Tenant tenant)
        {
            return await this.context.Memberships
                .Where(_ => _.TenantId == tenant.Id)
                .ToListAsync();
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}