using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Shims
{
    public class ShimTenantMembershipsStore : ITenantMembershipStore
    {
        public Task<TenantMembership> Create(TenantMembership membership)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsMember(Tenant tenant, string userId)
        {
            return Task.FromResult(false);
        }

        public Task<IEnumerable<TenantMembership>> ReadMemberships(string userId)
        {
            return Task.FromResult(new List<TenantMembership>() as IEnumerable<TenantMembership>);
        }

        public Task<IEnumerable<TenantMembership>> ReadMemberships(Tenant tenant)
        {
            return Task.FromResult(new List<TenantMembership>() as IEnumerable<TenantMembership>);
        }
    }
}