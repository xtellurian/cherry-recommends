using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Stores
{
    public class BusinessWorkflowTests
    {
        [Theory]
        [InlineData("jollibee", "Jollibee Chickenjoy", "Bida ang sarap")]
        [InlineData("kfc", "KFC", null)]
        [InlineData("mcdo", null, null)]
        public async Task CreateBusinessTest(string commonId, string name, string description)
        {
            var mockBusinessStore = new Mock<IBusinessStore>();
            var mockStorageContext = new Mock<IStorageContext>();

            var workflow = new BusinessWorkflows(mockStorageContext.Object, mockBusinessStore.Object, Utility.MockLogger<BusinessWorkflows>().Object);
            await workflow.CreateBusiness(commonId, name, description);
                        
            mockBusinessStore.Verify(_ => _.Create(It.Is<Business>(b => b.CommonId == commonId)));
            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("mcdonald's")]
        [InlineData("Krusty Crab")]
        public async Task CreateBusinessCommonIdExceptionTest(string commonId)
        {
            var mockBusinessStore = new Mock<IBusinessStore>();
            var mockStorageContext = new Mock<IStorageContext>();

            var workflow = new BusinessWorkflows(mockStorageContext.Object, mockBusinessStore.Object, Utility.MockLogger<BusinessWorkflows>().Object);

            var act = () => workflow.CreateBusiness(commonId, null, null);
            CommonIdException exception = await Assert.ThrowsAsync<CommonIdException>(act);
        }
    }
}