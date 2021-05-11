using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class InMemoryOutcomeStore : InMemoryStore<PresentationOutcome>, IPresentationOutcomeStore
    {
        public Task<IEnumerable<PresentationOutcome>> ListExperimentOutcomes(Experiment experiment)
        {
            var x = store.Values
                .Where(_ => experiment.Offers.Select(o => o.Id).Contains(_.Offer.Id))
                .ToList();
            
            return Task.FromResult<IEnumerable<PresentationOutcome>>(x);
        }
    }
}
