using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Core
{
#nullable enable
    public interface IEnvironmentProvider
    {
        long? CurrentEnvironmentId { get; }
        Task<Environment?> ReadCurrent(IEnvironmentStore store);
        Task SetOverride(long environmentId);
    }
}