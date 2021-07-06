using System.Threading.Tasks;

namespace SignalBox.Core
{
    // these can't be registered in SignalBox.Core because they depend on the implentation details
    public interface IModelClientFactory
    {
        Task<IModelClient<TInput, TOutput>> GetClient<TInput, TOutput>(ModelRegistration model)
            where TInput : IModelInput
            where TOutput : IModelOutput;

        Task<IModelClient<TInput, TOutput>> GetUnregisteredClient<TInput, TOutput>()
            where TInput : IModelInput
            where TOutput : IModelOutput;
    }
}