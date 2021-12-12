using System.Threading.Tasks;
using SignalBox.Core.Internal;

namespace SignalBox.Core
{
    public interface IAuth0Service
    {
        Task AddTenantPermission(string creatorId, Tenant tenant);
        Task<UserInfo> AddUser(InviteRequest invite);
        Task<UserMetadata> GetMetadata(string userId);
        Task<UserInfo> GetUserInfo(string userId);
        Task<UserInfo> SetMetadata(string userId, UserMetadata metadata);
    }
}