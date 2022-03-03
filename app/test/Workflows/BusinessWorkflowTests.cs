using System.Collections.Generic;
using System.Linq;
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
            var mockCustomerStore = new Mock<ICustomerStore>();

            var workflow = new BusinessWorkflows(mockStorageContext.Object, mockBusinessStore.Object, mockCustomerStore.Object, Utility.MockLogger<BusinessWorkflows>().Object);
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
            var mockCustomerStore = new Mock<ICustomerStore>();

            var workflow = new BusinessWorkflows(mockStorageContext.Object, mockBusinessStore.Object, mockCustomerStore.Object, Utility.MockLogger<BusinessWorkflows>().Object);

            var act = () => workflow.CreateBusiness(commonId, null, null);
            CommonIdException exception = await Assert.ThrowsAsync<CommonIdException>(act);
        }

        [Fact]
        public async Task RemoveBusinessMembershipTest()
        {
            var mockBusinessStore = new Mock<IBusinessStore>();
            var mockStorageContext = new Mock<IStorageContext>();
            var mockCustomerStore = new Mock<ICustomerStore>();

            var workflow = new BusinessWorkflows(mockStorageContext.Object, mockBusinessStore.Object, mockCustomerStore.Object, Utility.MockLogger<BusinessWorkflows>().Object);

            var businessMembership = new BusinessMembership();
            businessMembership.BusinessId = 0;
            businessMembership.CustomerId = 1;

            var customer = new Customer("customer1", "customer1");
            customer.BusinessMembership = businessMembership;

            mockCustomerStore.Setup(_ => _.Read(It.Is<long>(_ => _ == 1))).ReturnsAsync(customer);

            var business = new Business("test");
            var result = await workflow.RemoveBusinessMembership(business, 1);

            Assert.Null(result.BusinessMembership);
            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task RemoveBusinessMembershipNoBusinessMembershipTest()
        {
            var mockBusinessStore = new Mock<IBusinessStore>();
            var mockStorageContext = new Mock<IStorageContext>();
            var mockCustomerStore = new Mock<ICustomerStore>();

            var workflow = new BusinessWorkflows(mockStorageContext.Object, mockBusinessStore.Object, mockCustomerStore.Object, Utility.MockLogger<BusinessWorkflows>().Object);

            mockCustomerStore.Setup(_ => _.Read(It.Is<long>(_ => _ == 1))).ReturnsAsync(new Customer("customer1", "customer1"));
            var business = new Business("test");

            var act = () => workflow.RemoveBusinessMembership(business, 1);
            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task RemoveBusinessMembershipIncorrectBusinessIdTest()
        {
            var mockBusinessStore = new Mock<IBusinessStore>();
            var mockStorageContext = new Mock<IStorageContext>();
            var mockCustomerStore = new Mock<ICustomerStore>();

            var workflow = new BusinessWorkflows(mockStorageContext.Object, mockBusinessStore.Object, mockCustomerStore.Object, Utility.MockLogger<BusinessWorkflows>().Object);

            var businessMembership = new BusinessMembership();
            businessMembership.BusinessId = 1;
            businessMembership.CustomerId = 1;

            var customer = new Customer("customer1", "customer1");
            customer.BusinessMembership = businessMembership;

            mockCustomerStore.Setup(_ => _.Read(It.Is<long>(_ => _ == 1))).ReturnsAsync(customer);

            var business = new Business("test"); // Id=0

            var act = () => workflow.RemoveBusinessMembership(business, 1);
            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task RemoveBusinessMembershipNoCustomerTest()
        {
            var mockBusinessStore = new Mock<IBusinessStore>();
            var mockStorageContext = new Mock<IStorageContext>();
            var mockCustomerStore = new Mock<ICustomerStore>();

            var workflow = new BusinessWorkflows(mockStorageContext.Object, mockBusinessStore.Object, mockCustomerStore.Object, Utility.MockLogger<BusinessWorkflows>().Object);
            var business = new Business("test");

            var act = () => workflow.RemoveBusinessMembership(business, 1);
            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(act);
        }

    }
}