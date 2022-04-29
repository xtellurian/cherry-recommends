using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class KlaviyoWorkflowTests
    {
        [Fact]
        public async Task SetApiKeysTest()
        {
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();
            var mockKlaviyoService = new Mock<IKlaviyoService>();
            var mockChannelStore = new Mock<IEmailChannelStore>();

            mockIntegratedSystemStore.SetupContext<IIntegratedSystemStore, IntegratedSystem>();

            var workflow = new KlaviyoSystemWorkflow(new System.Net.Http.HttpClient(), mockIntegratedSystemStore.Object, mockKlaviyoService.Object, mockChannelStore.Object, Utility.MockLogger<KlaviyoSystemWorkflow>().Object);
            var integratedSystem = new IntegratedSystem("test", "test", IntegratedSystemTypes.Klaviyo);
            var result = await workflow.SetApiKeys(integratedSystem, "abcde", "fghij12345");

            Assert.NotNull(integratedSystem.Cache);
            mockIntegratedSystemStore.Verify(_ => _.Context.SaveChanges(), Times.Once);
        }

        [Theory]
        [InlineData(IntegratedSystemTypes.Custom)]
        [InlineData(IntegratedSystemTypes.Hubspot)]
        [InlineData(IntegratedSystemTypes.Segment)]
        [InlineData(IntegratedSystemTypes.Shopify)]
        [InlineData(IntegratedSystemTypes.Website)]
        public async Task SetApiKeys_Exception_Test(IntegratedSystemTypes systemType)
        {
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();
            var mockKlaviyoService = new Mock<IKlaviyoService>();
            var mockChannelStore = new Mock<IEmailChannelStore>();

            mockIntegratedSystemStore.SetupContext<IIntegratedSystemStore, IntegratedSystem>();

            var workflow = new KlaviyoSystemWorkflow(new System.Net.Http.HttpClient(), mockIntegratedSystemStore.Object, mockKlaviyoService.Object, mockChannelStore.Object, Utility.MockLogger<KlaviyoSystemWorkflow>().Object);
            var integratedSystem = new IntegratedSystem("abc", "Test", systemType);

            await Assert.ThrowsAsync<BadRequestException>(() => workflow.SetApiKeys(integratedSystem, "abc", "def"));
        }
    }
}