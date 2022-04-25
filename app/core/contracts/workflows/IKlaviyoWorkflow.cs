using System.Threading.Tasks;
using SignalBox.Core.Integrations;

namespace SignalBox.Core
{
    public interface IKlaviyoSystemWorkflow
    {
        Task<IntegratedSystem> SetApiKeys(IntegratedSystem system, string publicKey, string privateKey);
    }
}