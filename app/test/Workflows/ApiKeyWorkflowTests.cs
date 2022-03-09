using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class ApiKeyWorkflowTests
    {

        [Fact]
        public async Task ExistingApiKey_InDatabase_IsValid()
        {
            var mockStorageContext = new Mock<IStorageContext>();
            var mockTenantProvider = new Mock<ITenantProvider>();
            var mockTokenFactory = new Mock<IApiTokenFactory>();
            var mockApiKeyStore = new Mock<IHashedApiKeyStore>();
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockM2MTokenCache = new Mock<IM2MTokenCache>();
            var mockHasher = new Mock<IHasher>();

            var existingApiKey = System.Guid.NewGuid().ToString();
            var missingApiKey = System.Guid.NewGuid().ToString();
            mockHasher.Setup(_ => _.Hash(It.IsAny<string>())).Returns<string>((v) => v); // return self as hash
            mockApiKeyStore.Setup(_ => _.HashExists(It.Is<string>(s => s == existingApiKey))).ReturnsAsync(true);
            mockApiKeyStore.Setup(_ => _.HashExists(It.Is<string>(s => s != existingApiKey))).ReturnsAsync(false);

            var sut = new ApiKeyWorkflows(
                mockStorageContext.Object,
                mockTenantProvider.Object,
                mockTokenFactory.Object,
                mockApiKeyStore.Object,
                mockDateTimeProvider.Object,
                Utility.MockLogger<ApiKeyWorkflows>().Object,
                mockM2MTokenCache.Object,
                mockHasher.Object
            );

            var isValid = await sut.IsValidApiKey(existingApiKey);
            var isInvalid = await sut.IsValidApiKey(missingApiKey);

            Assert.True(isValid);
            Assert.False(isInvalid);
        }
    }
}
