using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Adapters.Klaviyo;
using SignalBox.Core.Integrations;

namespace SignalBox.Core
{
    public interface IKlaviyoService
    {
        Task<IEnumerable<KlaviyoList>> GetLists(KlaviyoApiKeys apiKeys);
    }
}