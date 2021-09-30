using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITenantMembershipStore
    {
        Task<TenantMembership> Create(TenantMembership membership);
        Task<IEnumerable<TenantMembership>> ReadMemberships(string userId);
        Task<IEnumerable<TenantMembership>> ReadMemberships(Tenant tenant);
        Task<bool> IsMember(Tenant tenant, string userId);
    }
}