using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITenantProvider
    {
#nullable enable
        string? RequestedTenantName { get; }
        string? CurrentDatabaseName { get; }

        Task SetTenantName(string name);
        Tenant? Current();
    }
}