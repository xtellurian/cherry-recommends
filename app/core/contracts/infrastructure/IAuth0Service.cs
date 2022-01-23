using System.Threading.Tasks;
using SignalBox.Core.Internal;

namespace SignalBox.Core
{
    public interface IAuth0Service
    {
        Task AddPermissionToClientGrant(string clientId, Tenant tenant);
        Task AddTenantPermission(string creatorId, Tenant tenant);
        Task<UserInfo> AddUser(InviteRequest invite);
        Task CreateRoleForTenant(Tenant tenant);
        Task<UserMetadata> GetMetadata(string userId);
        Task<string> GetTenantRoleId(Tenant tenant);
        Task<UserInfo> GetUserInfo(string userId);
        Task<UserInfo> SetMetadata(string userId, UserMetadata metadata);
    }
}