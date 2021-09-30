using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITenantResolutionStrategy
    {
        Task<string> ResolveName(HttpRequestModel request);
    }
}