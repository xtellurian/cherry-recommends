using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITenantResolutionStrategy
    {
#nullable enable
        bool IsMultitenant { get; }
        Task<string?> ResolveName(HttpRequestModel request);
    }
}