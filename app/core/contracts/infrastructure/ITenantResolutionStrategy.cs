using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITenantResolutionStrategy
    {
        bool IsMultitenant { get; }
        Task<string> ResolveName(HttpRequestModel request);
    }
}