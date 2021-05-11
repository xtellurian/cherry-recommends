
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
    public class TrackedUserWorkflowsTests
    {
        private IDateTimeProvider dt = new SystemDateTimeProvider();

        [Fact]

        public async Task CanCreateTrackedUser()
        {
            // arrange
            var mock = new Mock<IStorageContext>();

            var userStore = new InMemoryTrackedUserStore();
            var eventStore = new InMemoryEventStore();

            var sut = new TrackedUserWorkflows(mock.Object, userStore, eventStore, dt);
           
            //act
            var results = await sut.CreateTrackedUser("ext_id");

            //assert
            Assert.NotNull(results);
            var storedTrackedUser = await userStore.Read(results.Id);
            Assert.NotNull(storedTrackedUser);
            Assert.Equal(storedTrackedUser.Name, results.Name);
            Assert.Equal(storedTrackedUser.ExternalId, results.ExternalId);

            //verify SaveChanges() runs exactly once
            mock.Verify((m => m.SaveChanges()), Times.Once());
        }
    }
}
