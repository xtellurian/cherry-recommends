using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IAuth0Service
    {
        Task AddTenantPermission(string creatorId, Tenant tenant);
    }
}