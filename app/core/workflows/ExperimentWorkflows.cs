using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class ExperimentWorkflows : IWorkflow
    {
        private readonly ILogger<ExperimentWorkflows> logger;
        private readonly IStorageContext storageContext;
        private readonly IExperimentResultsCalculator calculator;
        private readonly ITrackedUserStore userStore;
        private readonly IPresentationOutcomeStore outcomeStore;
        private readonly IExperimentStore experimentStore;
        private readonly IOfferStore offerStore;

        public ExperimentWorkflows(ILogger<ExperimentWorkflows> logger,
                                   IStorageContext storageContext,
                                   IExperimentResultsCalculator calculator,
                                   ITrackedUserStore userStore,
                                   IPresentationOutcomeStore outcomeStore,
                                   IExperimentStore experimentStore,
                                   IOfferStore offerStore)
        {
            this.logger = logger;
            this.storageContext = storageContext;
            this.calculator = calculator;
            this.userStore = userStore;
            this.outcomeStore = outcomeStore;
            this.experimentStore = experimentStore;
            this.offerStore = offerStore;
        }

        public async Task<Experiment> CreateExperiment(string? name,
                                                       IEnumerable<long> offerIds,
                                                       int concurrentOffers)
        {
           
            if (name == null)
            {
                throw new NullReferenceException("Experiment Name cannot be null");
            }
            foreach (var offerId in offerIds)
            {
                if (!await offerStore.Exists(offerId))
                {
                    throw new EntityNotFoundException<Offer>(offerId);
                }
            }
            var offers = new List<Offer>();
            foreach (var oid in offerIds)
            {
                offers.Add(await offerStore.Read(oid));
            }

            var experiment = await experimentStore.Create(new Experiment(name!, offers, concurrentOffers));
            await storageContext.SaveChanges();
            return experiment;
        }
        public async Task<ExperimentResults> CalculateResults(long experimentId, string? scorer)
        {
            var experiment = await experimentStore.Read(experimentId);
            var results = await calculator.CalculateResults(experiment,
                                                            userStore,
                                                            outcomeStore,
                                                            offerStore,
                                                            GetScorer(scorer));

            await storageContext.SaveChanges();
            return results;
        }

        private IRecommender<OfferRecommendation> GetRecommender()
        {
            return new RandomRecommender();
        }

        private IScorer GetScorer(string? name)
        {
            if (name == null)
            {
                // default
                return new ExpectedValueScorer();
            }
            else if (string.Equals("random", name, System.StringComparison.InvariantCultureIgnoreCase))
            {
                return new RandomScorer();
            }
            else if (string.Equals("expected_value", name, System.StringComparison.InvariantCultureIgnoreCase))
            {
                return new ExpectedValueScorer();
            }
            else
            {
                // default
                return new ExpectedValueScorer();
            }

        }

    }
}