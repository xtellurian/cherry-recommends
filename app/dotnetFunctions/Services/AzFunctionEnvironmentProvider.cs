using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Infrastructure;

namespace SignalBox.Functions.Services
{
#nullable enable
    public class AzFunctionEnvironmentProvider : IEnvironmentProvider
    {
        private long? _override;

        public long? CurrentEnvironmentId => _override;

        public async Task<Environment?> ReadCurrent(IEnvironmentStore store)
        {
            if (_override != null)
            {
                return await store.Read(_override.Value);
            }
            else
            {
                return null;
            }
        }

        public Task SetOverride(long environmentId)
        {
            this._override = environmentId;
            return Task.CompletedTask;
        }
    }
}