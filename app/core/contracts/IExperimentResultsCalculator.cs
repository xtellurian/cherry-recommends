using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IExperimentResultsCalculator
    {
        Task<ExperimentResults> CalculateResults(
            Experiment experiment,
            ITrackedUserStore trackedUserStore,
            IPresentationOutcomeStore outcomeStore,
            IOfferStore offerStore,
            IScorer scorer);
    }
}