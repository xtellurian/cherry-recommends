using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class PresentationsWorkflows : IWorkflow
    {
        private readonly ILogger<PresentationsWorkflows> logger;
        private readonly IStorageContext storageContext;
        private readonly ITrackedUserStore userStore;
        private readonly IOfferRecommendationStore recommendationStore;
        private readonly IPresentationOutcomeStore presentationOutcomeStore;
        private readonly IExperimentStore experimentStore;
        private readonly IRecommendationCorrelatorStore correlatorStore;
        private readonly IOfferStore offerStore;

        public PresentationsWorkflows(ILogger<PresentationsWorkflows> logger,
                                   IStorageContext storageContext,
                                   ITrackedUserStore userStore,
                                   IOfferRecommendationStore recommendationStore,
                                   IPresentationOutcomeStore presentationOutcomeStore,
                                   IExperimentStore experimentStore,
                                   IRecommendationCorrelatorStore correlatorStore,
                                   IOfferStore offerStore)
        {
            this.logger = logger;
            this.storageContext = storageContext;
            this.userStore = userStore;
            this.recommendationStore = recommendationStore;
            this.presentationOutcomeStore = presentationOutcomeStore;
            this.experimentStore = experimentStore;
            this.correlatorStore = correlatorStore;
            this.offerStore = offerStore;
        }

        public async Task<OfferRecommendation> RecommendOffer(long experimentId,
                                                                        string? commonUserId,
                                                                        Dictionary<string, object>? features)
        {
            var experiment = await experimentStore.Read(experimentId);
            TrackedUser? trackedUser = null;
            if (commonUserId != null)
            {
                trackedUser = (await userStore.CreateIfNotExists(new List<string> { commonUserId })).First();
            }
            var context = new PresentationContext(trackedUser, experiment, features);
            var presenter = GetRecommender();
            var presentation = await presenter.Recommend(context);
            await recommendationStore.Create(presentation);
            await storageContext.SaveChanges();
            return presentation;
        }

        public async Task<PresentationOutcome> TrackPresentationOutcome(long experimentId,
                                                                        string iterationId,
                                                                        long recommendationId,
                                                                        long offerId,
                                                                        string outcome)
        {
            var experiment = await experimentStore.Read(experimentId);
            var offer = await offerStore.Read(offerId);
            var iteration = experiment.Iterations.First(_ => _.Id == iterationId);
            var recommendation = await recommendationStore.Read(recommendationId);
            var result = await presentationOutcomeStore.Create(
                new PresentationOutcome(experiment, recommendation, iteration.Id, iteration.Order, offer, outcome));

            await storageContext.SaveChanges();
            return result;
        }

        private IRecommender<OfferRecommendation> GetRecommender()
        {
            return new RandomRecommender(correlatorStore);
        }
    }
}