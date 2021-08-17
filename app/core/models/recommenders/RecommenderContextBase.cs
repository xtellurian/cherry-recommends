using Microsoft.Extensions.Logging;
using SignalBox.Core.Workflows;

namespace SignalBox.Core.Recommenders
{
    public class RecommenderContextBase
    {
        protected RecommenderContextBase(ILogger<IWorkflow> logger)
        {
            Logger = logger;
        }

        public ILogger<IWorkflow> Logger { get; }
    }
}