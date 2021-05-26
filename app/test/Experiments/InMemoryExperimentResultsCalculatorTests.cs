using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.Services;
using Xunit;

namespace SignalBox.Test.Stores
{
    public class InMemoryExperimentResultsCalculatorTests
    {
        private IDateTimeProvider dt = new SystemDateTimeProvider();

        [Theory]
        [InlineData(typeof(RandomScorer))]
        [InlineData(typeof(ExpectedValueScorer))]
        public async Task CanCalculateResultsFromSmallSet(Type scorerType)
        {
            var scorer = (IScorer)Activator.CreateInstance(scorerType);

            var trackedUserStore = new InMemoryTrackedUserStore();
            var outcomeStore = new InMemoryOutcomeStore();
            var segmentStore = new InMemorySegmentStore();
            var offerStore = new InMemoryOfferStore();

            // arrange
            var sut = new InMemoryExperimentResultsCalculator();

            // create some offers
            var offers = new List<Offer>
            {
                new Offer
                {
                    Price = 3
                },
                new Offer
                {
                    Price = 5
                },
                new Offer
                {
                    Price = 7
                }
            };

            foreach (var o in offers)
            {
                await offerStore.Create(o);
            }


            // then create some data accepting/rejecting offers within the expeurment
            var users = await CreateSomeUsers(trackedUserStore);
            var experiment = new Experiment("A Name", offers, 1);
            await SimulateSomeAcceptsOrRejects(users, experiment, outcomeStore);

            // then create an experiment for hte offers
            experiment.Id = 1;
            // act
            var results = await sut.CalculateResults(experiment, trackedUserStore, outcomeStore, offerStore, scorer);

            //assert
            Assert.NotNull(results.OfferStats);
            Assert.NotEmpty(results.OfferStats);
        }

        private Random random = new Random();
        private async Task<ICollection<TrackedUser>> CreateSomeUsers(ITrackedUserStore userStore)
        {
            var users = new List<TrackedUser>();
            for (var i = 0; i < 10; i++)
            {
                var commonId = i.ToString();
                users.Add(await userStore.Create(new TrackedUser(commonId)));
            }

            return users;
        }

        private async Task SimulateSomeAcceptsOrRejects(IEnumerable<TrackedUser> users,
                                                        Experiment experiment,
                                                        IPresentationOutcomeStore outcomeStore)
        {
            foreach (var u in users)
            {
                var recommendation = new OfferRecommendation();
                var randomOffer = experiment.Offers.ToList()[random.Next(experiment.Offers.Count())];
                if (random.NextDouble() > 0.5)
                {
                    // then accept
                    await outcomeStore.Create(new PresentationOutcome(experiment,
                                                                      recommendation,
                                                                      experiment.CurrentIteration.Id,
                                                                      experiment.CurrentIteration.Order,
                                                                      randomOffer,
                                                                      "accept"));
                }
                else
                {
                    // reject
                    await outcomeStore.Create(new PresentationOutcome(experiment,
                                                                      recommendation,
                                                                      experiment.CurrentIteration.Id,
                                                                      experiment.CurrentIteration.Order,
                                                                      randomOffer,
                                                                      "reject"));
                }
            }
        }
    }
}
