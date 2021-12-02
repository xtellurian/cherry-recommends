using System.Threading.Tasks;
using SignalBox.Core.Internal;

namespace SignalBox.Core
{
    public interface IAuth0Service
    {
        Task AddTenantPermission(string creatorId, Tenant tenant);
        Task<UserInfo> AddUser(InviteRequest invite);
        Task<UserInfo> GetUserInfo(string userId);
    }
}