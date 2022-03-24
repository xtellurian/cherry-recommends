using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class ChannelWorkflowTests
    {
        [Fact]
        public async Task CreateChannel_Webhook_ValidTest()
        {
            var mockChannelStore = new Mock<IWebhookChannelStore>();

            mockChannelStore.SetupContext<IWebhookChannelStore, WebhookChannel>();

            var workflow = new ChannelWorkflow(mockChannelStore.Object, Utility.MockLogger<ChannelWorkflow>().Object);
            var name = "MyChannel";
            var integratedSystem = new IntegratedSystem("abc", "Test", IntegratedSystemTypes.Custom);
            await workflow.CreateChannel(name, ChannelTypes.Webhook, integratedSystem);

            mockChannelStore.Verify(_ => _.Create(It.Is<WebhookChannel>(b => b.Name == name)));
            mockChannelStore.Verify(_ => _.Context.SaveChanges(), Times.Once);
        }

        [Theory]
        [InlineData(ChannelTypes.Email)]
        [InlineData(ChannelTypes.Web)]
        public async Task CreateChannel_ExceptionTest(ChannelTypes channelType)
        {
            var mockChannelStore = new Mock<IWebhookChannelStore>();

            var workflow = new ChannelWorkflow(mockChannelStore.Object, Utility.MockLogger<ChannelWorkflow>().Object);
            var integratedSystem = new IntegratedSystem("abc", "Test", IntegratedSystemTypes.Custom);

            await Assert.ThrowsAsync<BadRequestException>(() => workflow.CreateChannel("MyChannel", channelType, integratedSystem));
        }
    }
}