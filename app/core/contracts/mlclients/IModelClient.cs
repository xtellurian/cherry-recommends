using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IModelClient<TInput, TOutput> where TInput : IModelInput where TOutput : IModelOutput
    {
        Task<TOutput> Invoke(ModelRegistration model, string version, TInput input);
    }
}