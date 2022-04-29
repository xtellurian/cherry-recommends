using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Adapters.Klaviyo;

namespace SignalBox.Core
{
    public interface IKlaviyoSystemWorkflow
    {
        Task<IntegratedSystem> SetApiKeys(IntegratedSystem system, string publicKey, string privateKey);
        Task<IEnumerable<KlaviyoList>> GetLists(IntegratedSystem system);
    }
}