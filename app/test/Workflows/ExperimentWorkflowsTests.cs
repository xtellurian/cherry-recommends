
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.Services;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class ExperimentWorkflowsTests
    {
        private IDateTimeProvider dt = new SystemDateTimeProvider();

        [Fact]

        public async Task CanCreateExperiment()
        {
            // arrange
            var mock = new Mock<IStorageContext>();
            
            var logger = NullLogger<ExperimentWorkflows>.Instance;
            var calculator = new InMemoryExperimentResultsCalculator();
            var userStore = new InMemoryTrackedUserStore();
            var outcomeStore = new InMemoryOutcomeStore();
            var experimentStore = new InMemoryExperimentStore();
            var offerStore = new InMemoryOfferStore();
            var offer1 = await offerStore.Create(new Offer());
            var offer2 = await offerStore.Create(new Offer());

            var sut = new ExperimentWorkflows(logger, mock.Object, calculator, userStore, 
                                                outcomeStore, experimentStore, offerStore);
           

            //act
            var results = await sut.CreateExperiment("name", new List<long>{offer1.Id,offer2.Id}, 1);

            //assert
            Assert.NotNull(results);
            var storedExperiment = await experimentStore.Read(results.Id);
            Assert.NotNull(storedExperiment);
            Assert.Equal(storedExperiment.Name, results.Name);
            Assert.Equal(storedExperiment.Offers, results.Offers);
            Assert.Equal(storedExperiment.ConcurrentOffers, results.ConcurrentOffers);
            Assert.Equal(storedExperiment.Iterations, results.Iterations);

            //verify SaveChanges() runs exactly once
            mock.Verify((m => m.SaveChanges()), Times.Once());
        }
    }
}
