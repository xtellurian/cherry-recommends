using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public class InMemoryExperimentResultsCalculator : IExperimentResultsCalculator
    {
        public async Task<ExperimentResults> CalculateResults(
            Experiment experiment,
            ITrackedUserStore trackedUserStore,
            IPresentationOutcomeStore outcomeStore,
            IOfferStore offerStore,
            IScorer scorer)
        {
            var users = await trackedUserStore.List();// everyone is in the semgnet

            if (experiment.Offers == null)
            {
                throw new System.NullReferenceException("OfferIds can't be null in an experiment");
            }

            var offers = experiment.Offers.ToList();
            var offerAcceptEvents = new List<PresentationOutcome>();
            var allOutcomes = await outcomeStore.ListExperimentOutcomes(experiment);

            offerAcceptEvents.AddRange(allOutcomes.Where(_ => _.Outcome == "accept"));

            var offerRejectEvents = new List<PresentationOutcome>();

            offerRejectEvents.AddRange(allOutcomes.Where(_ => _.Outcome == "reject"));

            var usersAccepted = new ConcurrentBag<(TrackedUser user, Offer offer)>();
            var usersRejected = new ConcurrentBag<(TrackedUser user, Offer offer)>();


            var acceptedIds = offerAcceptEvents.Select(_ => _.Recommendation.CommonUserId);
            var rejectedIds = offerRejectEvents.Select(_ => _.Recommendation.CommonUserId);

            foreach (var user in users)
            {
                if (acceptedIds.Contains(user.CommonUserId))
                {
                    var offer = offerAcceptEvents.First(_ => _.Recommendation.CommonUserId == user.CommonUserId).Offer;
                    usersAccepted.Add((user, offer));
                }
                else
                {
                    var rejected = offerRejectEvents.FirstOrDefault(_ => _.Recommendation.CommonUserId == user.CommonUserId);
                    if (rejected != null)
                    {
                        var offer = offerRejectEvents.First(_ => _.Recommendation.CommonUserId == user.CommonUserId).Offer;
                        usersRejected.Add((user, offer));
                    }
                }
            }

            // TODO: this should make some sense
            var stats = new List<OfferStats>();
            foreach (var offer in offers)
            {
                var accepting = usersAccepted
                    .Where(_ => _.offer.Id == offer.Id)
                    .Select(_ => _.user);
                var rejecting = usersRejected
                    .Where(_ => _.offer.Id == offer.Id)
                    .Select(_ => _.user);

                var score = await scorer.ScoreOffer(offer, accepting, rejecting);
                // now calc the score for this segment for this offer
                var s = new OfferStats(offer, accepting.Count(), rejecting.Count(), scorer.Name, score.Value);
                stats.Add(s);
            }

            var significantEventCount = offerAcceptEvents.Count + offerRejectEvents.Count;

            // calc the benefit.
            var meanAccepted = stats.Select(_ => _.NumberAccepted).DefaultIfEmpty(0).Average();
            var meanPrice = stats.Select(_ => _.Offer.Price).DefaultIfEmpty(0).Average();
            // let's just guess that the improvement is 10% of the mean offer price x mean accepted.
            var benefit = meanAccepted * meanPrice / 10;

            return new ExperimentResults(experiment, offers, stats, significantEventCount, benefit);

        }
    }
}