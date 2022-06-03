using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
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

            mockCustomerStore.Setup(_ => _.Read(It.Is<long>(_ => _ == 1), It.IsAny<EntityStoreReadOptions>())).ReturnsAsync(customer);

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

            mockCustomerStore.Setup(_ => _.Read(It.Is<long>(_ => _ == 1), It.IsAny<EntityStoreReadOptions>())).ReturnsAsync(new Customer("customer1", "customer1"));
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

            mockCustomerStore.Setup(_ => _.Read(It.Is<long>(_ => _ == 1), It.IsAny<EntityStoreReadOptions>())).ReturnsAsync(customer);

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

        [Fact]
        public async Task AddCustomerToBusiness_BusinessNotExists()
        {
            // arrange
            var mockBusinessStore = new Mock<IBusinessStore>();
            var mockStorageContext = new Mock<IStorageContext>();
            var mockCustomerStore = new Mock<ICustomerStore>();
            var businessCommonId = "business-id";
            var workflow = new BusinessWorkflows(mockStorageContext.Object, mockBusinessStore.Object, mockCustomerStore.Object, Utility.MockLogger<BusinessWorkflows>().Object);
            var customer = new Customer("customerId");
            var businessProperties = new Dictionary<string, object>
            {
                {"headCount",  100},
                {"accountStatic",  "active"},
                {"nested",  new Dictionary<string, object>
                    {
                        {"testNested", 5}
                    }
                }
            };
            mockBusinessStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockBusinessStore.Setup(_ => _.Create(It.Is<Business>(b => b.CommonId == businessCommonId)))
                .ReturnsAsync((Business bs) => bs);

            // act
            var membership = await workflow.AddToBusiness(businessCommonId, customer, businessProperties);

            mockBusinessStore.Verify(_ => _.Create(It.Is<Business>(b => b.CommonId == businessCommonId)));
            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Once);
            Assert.Equal(businessCommonId, customer.BusinessMembership.Business.CommonId);
            Assert.Equal(customer.CustomerId, customer.BusinessMembership.Customer.CustomerId);
        }

        [Fact]
        public async Task AddCustomerToBusiness_BusinessExists()
        {
            // arrange
            var mockBusinessStore = new Mock<IBusinessStore>();
            var mockStorageContext = new Mock<IStorageContext>();
            var mockCustomerStore = new Mock<ICustomerStore>();
            var businessCommonId = "business-id";
            var workflow = new BusinessWorkflows(mockStorageContext.Object, mockBusinessStore.Object, mockCustomerStore.Object, Utility.MockLogger<BusinessWorkflows>().Object);
            var customer = new Customer("customerId");
            var businessProperties = new Dictionary<string, object>
            {
                {"headCount",  100},
                {"accountStatic",  "active"},
                {"nested",  new Dictionary<string, object>
                    {
                        {"testNested", 5}
                    }
                }
            };
            mockBusinessStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockBusinessStore.Setup(_ => _.ExistsFromCommonId(It.Is<string>(x => x == businessCommonId)))
                .ReturnsAsync(true);
            mockBusinessStore.Setup(_ => _.ReadFromCommonId(It.Is<string>(x => x == businessCommonId)))
                .ReturnsAsync(new Business(businessCommonId));

            // act
            var membership = await workflow.AddToBusiness(businessCommonId, customer, businessProperties);

            // assert
            mockBusinessStore.Verify(_ => _.ReadFromCommonId(It.Is<string>(s => s == businessCommonId)));
            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Once);
            Assert.Equal(businessCommonId, customer.BusinessMembership.Business.CommonId);
            Assert.Equal(customer.CustomerId, customer.BusinessMembership.Customer.CustomerId);
        }

        [Fact]
        public async Task AddCustomerToBusiness_MembershipExists()
        {
            // arrange
            var mockBusinessStore = new Mock<IBusinessStore>();
            var mockStorageContext = new Mock<IStorageContext>();
            var mockCustomerStore = new Mock<ICustomerStore>();
            var businessCommonId = "business-id";
            var newBusinessCommonId = "newbusiness-id";
            var workflow = new BusinessWorkflows(mockStorageContext.Object, mockBusinessStore.Object, mockCustomerStore.Object, Utility.MockLogger<BusinessWorkflows>().Object);
            var business = new Business(businessCommonId)
            {
                Id = 1
            };
            var newBusiness = new Business(newBusinessCommonId)
            {
                Id = 2
            };
            var customer = new Customer("customerId")
            {
                Id = 1,
            };
            customer.BusinessMembership = new BusinessMembership
            {
                Business = business,
                BusinessId = business.Id,
                Customer = customer,
                CustomerId = customer.Id
            };
            var businessProperties = new Dictionary<string, object>
            {
                {"headCount",  100},
                {"accountStatic",  "active"},
                {"nested",  new Dictionary<string, object>
                    {
                        {"testNested", 5}
                    }
                }
            };
            mockBusinessStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockBusinessStore.Setup(_ => _.Read(It.Is<long>(x => x == business.Id), It.IsAny<EntityStoreReadOptions>()))
                .ReturnsAsync(business);
            mockBusinessStore.Setup(_ => _.ExistsFromCommonId(It.Is<string>(x => x == business.CommonId)))
                .ReturnsAsync(true);
            mockBusinessStore.Setup(_ => _.ReadFromCommonId(It.Is<string>(x => x == business.CommonId)))
                .ReturnsAsync(business);
            mockBusinessStore.Setup(_ => _.ExistsFromCommonId(It.Is<string>(x => x == newBusiness.CommonId)))
                .ReturnsAsync(true);
            mockBusinessStore.Setup(_ => _.ReadFromCommonId(It.Is<string>(x => x == newBusiness.CommonId)))
                .ReturnsAsync(newBusiness);
            // act
            var membership = await workflow.AddToBusiness(newBusinessCommonId, customer, businessProperties);

            // assert
            mockBusinessStore.Verify(_ => _.Read(It.Is<long>(s => s == business.Id), It.IsAny<EntityStoreReadOptions>()));
            mockStorageContext.Verify(_ => _.SaveChanges(), Times.Once);
            Assert.Equal(membership.BusinessId, newBusiness.Id);
            Assert.Equal(customer.BusinessMembership, membership);
            Assert.Equal(customer.CustomerId, customer.BusinessMembership.Customer.CustomerId);
        }
    }
}