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
            var mockWebhookChannelStore = new Mock<IWebhookChannelStore>();
            var mockWebChannelStore = new Mock<IWebChannelStore>();

            mockWebhookChannelStore.SetupContext<IWebhookChannelStore, WebhookChannel>();

            var workflow = new ChannelWorkflow(mockWebhookChannelStore.Object, mockWebChannelStore.Object, Utility.MockLogger<ChannelWorkflow>().Object);
            var name = "MyChannel";
            var integratedSystem = new IntegratedSystem("abc", "Test", IntegratedSystemTypes.Custom);
            await workflow.CreateChannel(name, ChannelTypes.Webhook, integratedSystem);

            mockWebhookChannelStore.Verify(_ => _.Create(It.Is<WebhookChannel>(b => b.Name == name)));
            mockWebhookChannelStore.Verify(_ => _.Context.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task CreateChannel_Webhook_Invalid_IntegratedSystem_Test()
        {
            var mockWebhookChannelStore = new Mock<IWebhookChannelStore>();
            var mockWebChannelStore = new Mock<IWebChannelStore>();

            mockWebhookChannelStore.SetupContext<IWebhookChannelStore, WebhookChannel>();

            var workflow = new ChannelWorkflow(mockWebhookChannelStore.Object, mockWebChannelStore.Object, Utility.MockLogger<ChannelWorkflow>().Object);
            var integratedSystem = new IntegratedSystem("abc", "Test", IntegratedSystemTypes.Segment);

            await Assert.ThrowsAsync<BadRequestException>(() => workflow.CreateChannel("MyChannel", ChannelTypes.Webhook, integratedSystem));
        }


        [Fact]
        public async Task CreateChannel_Web_ValidTest()
        {
            var mockWebhookChannelStore = new Mock<IWebhookChannelStore>();
            var mockWebChannelStore = new Mock<IWebChannelStore>();

            mockWebChannelStore.SetupContext<IWebChannelStore, WebChannel>();

            var workflow = new ChannelWorkflow(mockWebhookChannelStore.Object, mockWebChannelStore.Object, Utility.MockLogger<ChannelWorkflow>().Object);
            var name = "MyChannel";
            var integratedSystem = new IntegratedSystem("abc", "Test", IntegratedSystemTypes.Website);
            await workflow.CreateChannel(name, ChannelTypes.Web, integratedSystem);

            mockWebChannelStore.Verify(_ => _.Create(It.Is<WebChannel>(b => b.Name == name)));
            mockWebChannelStore.Verify(_ => _.Context.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task CreateChannel_Web_Invalid_IntegratedSystem_Test()
        {
            var mockWebhookChannelStore = new Mock<IWebhookChannelStore>();
            var mockWebChannelStore = new Mock<IWebChannelStore>();

            mockWebChannelStore.SetupContext<IWebChannelStore, WebChannel>();

            var workflow = new ChannelWorkflow(mockWebhookChannelStore.Object, mockWebChannelStore.Object, Utility.MockLogger<ChannelWorkflow>().Object);
            var integratedSystem = new IntegratedSystem("abc", "Test", IntegratedSystemTypes.Custom);

            await Assert.ThrowsAsync<BadRequestException>(() => workflow.CreateChannel("MyChannel", ChannelTypes.Web, integratedSystem));
        }

        [Fact]
        public async Task CreateChannel_ExceptionTest()
        {
            var mockWebhookChannelStore = new Mock<IWebhookChannelStore>();
            var mockWebChannelStore = new Mock<IWebChannelStore>();

            var workflow = new ChannelWorkflow(mockWebhookChannelStore.Object, mockWebChannelStore.Object, Utility.MockLogger<ChannelWorkflow>().Object);
            var integratedSystem = new IntegratedSystem("abc", "Test", IntegratedSystemTypes.Custom);

            await Assert.ThrowsAsync<BadRequestException>(() => workflow.CreateChannel("MyChannel", ChannelTypes.Email, integratedSystem));
        }
    }
}