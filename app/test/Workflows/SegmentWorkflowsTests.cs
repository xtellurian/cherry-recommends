
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
    public class SegmentWorkflowsTests
    {
        private IDateTimeProvider dt = new SystemDateTimeProvider();

        [Fact]

        public async Task CanCreateSegment()
        {
            // arrange
            var mock = new Mock<IStorageContext>();

            var logger = NullLogger<SegmentWorkflows>.Instance;
            var userStore = new InMemoryTrackedUserStore();
            var segmentStore = new InMemorySegmentStore();

            var sut = new SegmentWorkflows(logger, segmentStore, userStore, mock.Object);
           
            //act
            var results = await sut.CreateSegment("name");

            //assert
            Assert.NotNull(results);
            var storedSegment = await segmentStore.Read(results.Id);
            Assert.NotNull(storedSegment);
            Assert.Equal(storedSegment.Name, results.Name);

            //verify SaveChanges() runs exactly once
            mock.Verify((m => m.SaveChanges()), Times.Once());
        }
    }
}
