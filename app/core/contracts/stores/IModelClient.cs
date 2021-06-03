using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IModelClient
    {
        Task<EvaluationResult> Invoke(ModelRegistration model, IDictionary<string, object> features);
    }
}