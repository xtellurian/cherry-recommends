using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Infrastructure;

namespace SignalBox.Functions.Services
{
#nullable enable
    public class AzFunctionEnvironmentProvider : IEnvironmentProvider
    {
        private long? _override;

        public bool isOveridden = false;
        public long? CurrentEnvironmentId => isOveridden ? _override : null;

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

        public void SetOverride(long? environmentId)
        {
            isOveridden = true;
            _override = environmentId;
        }
    }
}