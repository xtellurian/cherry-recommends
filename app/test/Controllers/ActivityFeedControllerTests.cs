using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Controllers;
using SignalBox.Web.Dto;
using Xunit;

namespace SignalBox.Test.Controllers
{
    public class ActivityFeedControllerTests
    {
        [Fact]
        public async Task CanGetActivityFeed()
        {
            var mockLogger = Utility.MockLogger<ActivityFeedController>();
            var mockWorkflow = new Mock<IActivityFeedWorkflow>();

            PaginateRequest paginateReq = new PaginateRequest();
            var sut = new ActivityFeedController(mockLogger.Object, mockWorkflow.Object);

            var result = await sut.GetActivityFeedEntities(paginateReq);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<ActivityFeedEntity>>(result);
        }
    }
}