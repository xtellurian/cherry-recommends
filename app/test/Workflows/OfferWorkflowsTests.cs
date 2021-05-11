
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.Services;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class OfferWorkflowsTests
    {
        private IDateTimeProvider dt = new SystemDateTimeProvider();

        [Fact]

        public async Task CanCreateOffer()
        {
            // arrange
            var mock = new Mock<IStorageContext>();

            var offerStore = new InMemoryOfferStore();

            var sut = new OfferWorkflows(mock.Object, offerStore);

            //act
            var results = await sut.CreateOffer("name", 5, 2, "AUD");

            //assert
            Assert.NotNull(results);
            var storedOffer = await offerStore.Read(results.Id);
            Assert.NotNull(storedOffer);
            Assert.Equal(storedOffer.Name, results.Name);
            Assert.Equal(storedOffer.Price, results.Price);
            Assert.Equal(storedOffer.Cost, results.Cost);
            Assert.Equal(storedOffer.Currency, results.Currency);

            //verify SaveChanges() runs exactly once
            mock.Verify((m => m.SaveChanges()), Times.Once());
        }
    }
}
