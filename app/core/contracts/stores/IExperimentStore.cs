using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IExperimentStore : IEntityStore<Experiment>
    {
        // Task<Experiment> CreateExperiment(Experiment experiment);
        // Task<Experiment> UpdateExperiment(Experiment experiment);
        // Task<Experiment> GetExperiment(string experimentId);
        // Task<IEnumerable<Experiment>> GetExperiments();
    }
}