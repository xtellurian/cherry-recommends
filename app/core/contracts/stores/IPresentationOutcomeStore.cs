using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IPresentationOutcomeStore : IStore<PresentationOutcome>
    {
        Task<IEnumerable<PresentationOutcome>> ListExperimentOutcomes(Experiment experiment);
    }
}