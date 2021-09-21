using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Infrastructure;

namespace SignalBox.Functions.Services
{
#nullable enable
    public class AzFunctionEnvironmentService : IEnvironmentService
    {
        public long? CurrentEnvironmentId => null;

        public Task<Environment?> ReadCurrent(IEnvironmentStore store)
        {
            return Task.FromResult<Environment?>(null);
        }
    }
}