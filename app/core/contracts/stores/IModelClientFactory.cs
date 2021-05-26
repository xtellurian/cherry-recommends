using System.Threading.Tasks;

namespace SignalBox.Core
{
    // these can't be registered in SignalBox.Core because they depend on the implentation details
    public interface IModelClientFactory
    {
        Task<IModelClient> GetClient(ModelRegistration model);
    }
}