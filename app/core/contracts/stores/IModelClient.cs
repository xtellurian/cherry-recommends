using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IModelClient
    {
        Task<EvaluationResult> Evaluate(ModelRegistration model, IDictionary<string, object> features);
    }
}