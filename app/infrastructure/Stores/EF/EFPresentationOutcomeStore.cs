using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class EFPresentationOutcomeStore : EFEntityStoreBase<PresentationOutcome>, IPresentationOutcomeStore
    {
        public EFPresentationOutcomeStore(SignalBoxDbContext context) : base(context, _ => _.PresentationOutcomes)
        { }

        public async Task<IEnumerable<PresentationOutcome>> ListExperimentOutcomes(Experiment experiment)
        {
            var outcomes = await context.Experiments
                .Include(e => e.Offers)
                .ThenInclude(o => o.Outcomes)
                .SelectMany(_ => _.Offers.SelectMany(_ => _.Outcomes))
                .Include(_ => _.Offer)
                .Include(_ => _.Recommendation)
                .ToListAsync();
            return outcomes;

            // return await context.PresentationOutcomes
            //     .Include(_ => _.Experiment)
            //     .Include(_ => _.Offer)
            //     .Include(_ => _.Recommendation)
            //     .Where(_ => offerIds.Contains(_.Offer.Id))
            //     .ToListAsync();
        }
    }
}