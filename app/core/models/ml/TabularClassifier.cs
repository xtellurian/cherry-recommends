using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public abstract class TabularClassifier : MLModelClient, IModelClient
    {
        public abstract Task<EvaluationResult> Invoke(ModelRegistration model, IDictionary<string, object> features);
    }
}