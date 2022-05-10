using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Stores
{
    public class DiscountCodeWorkflowTests
    {
        [Fact]
        public async Task CanGenerateDiscountCodes_Returns_New()
        {
            // arrange
            var mockStorageContext = Utility.MockStorageContext();
            var dateTimeProvider = Utility.DateTimeProvider();
            var mockLogger = Utility.MockLogger<DiscountCodeWorkflows>();
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();
            var mockDiscountCodeStore = new Mock<IDiscountCodeStore>();
            var mockDiscountCodeGenerator = new Mock<IDiscountCodeGenerator>();

            var integratedSystem = new IntegratedSystem("my-integrated-system", "shopify", IntegratedSystemTypes.Shopify);
            integratedSystem.IntegrationStatus = IntegrationStatuses.OK;
            integratedSystem.IsDiscountCodeGenerator = true;

            var promotion = EntityFactory.Promotion();
            promotion.PromotionType = PromotionType.Discount;
            promotion.BenefitType = BenefitType.Percent;
            promotion.BenefitValue = 10;

            string code = DiscountCode.GenerateCode(promotion.CommonId, codeLength: 8);
            var discountCode = new DiscountCode(promotion, code, dateTimeProvider.Now, dateTimeProvider.Now.AddDays(14));

            mockDiscountCodeStore
                .Setup(_ => _.GetLatestByPromotion(It.IsAny<RecommendableItem>()))
                .ReturnsAsync(new EntityResult<DiscountCode>(null));
            mockDiscountCodeStore
                .Setup(_ => _.Create(It.IsAny<DiscountCode>()))
                .ReturnsAsync(discountCode);
            mockDiscountCodeStore
                .Setup(_ => _.Context)
                .Returns(mockStorageContext.Object);
            mockIntegratedSystemStore
                .Setup(_ => _.Query(null))
                .ReturnsAsync(
                    new Paginated<IntegratedSystem>(
                        new List<IntegratedSystem>{
                            integratedSystem
                        }, 1, 1, 1));
            mockDiscountCodeGenerator
                .Setup(_ => _.SystemType)
                .Returns(IntegratedSystemTypes.Shopify);
            var discountCodeGenerators = new List<IDiscountCodeGenerator>
            {
                mockDiscountCodeGenerator.Object
            };
            // act
            var workflow = new DiscountCodeWorkflows(
                mockLogger.Object,
                dateTimeProvider,
                mockIntegratedSystemStore.Object,
                mockDiscountCodeStore.Object,
                discountCodeGenerators
            );
            var result = await workflow.GenerateDiscountCodes(promotion);
            // assert
            mockDiscountCodeStore.Verify(_ => _.GetLatestByPromotion(It.IsAny<RecommendableItem>()), Times.Once);
            mockDiscountCodeStore.Verify(_ => _.Create(It.IsAny<DiscountCode>()), Times.Once);
            mockDiscountCodeGenerator.Verify(_ => _.Generate(
                It.IsAny<IntegratedSystem>(),
                It.IsAny<RecommendableItem>(),
                It.IsAny<DiscountCode>()), Times.Once);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task CanGenerateDiscountCodes_Returns_Existing()
        {
            // arrange
            var mockStorageContext = Utility.MockStorageContext();
            var dateTimeProvider = Utility.DateTimeProvider();
            var mockLogger = Utility.MockLogger<DiscountCodeWorkflows>();
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();
            var mockDiscountCodeStore = new Mock<IDiscountCodeStore>();
            var mockDiscountCodeGenerator = new Mock<IDiscountCodeGenerator>();

            var integratedSystem = new IntegratedSystem("my-integrated-system", "shopify", IntegratedSystemTypes.Shopify);
            integratedSystem.IntegrationStatus = IntegrationStatuses.OK;
            integratedSystem.IsDiscountCodeGenerator = true;

            var promotion = EntityFactory.Promotion();
            promotion.PromotionType = PromotionType.Discount;
            promotion.BenefitType = BenefitType.Percent;
            promotion.BenefitValue = 10;

            string code = DiscountCode.GenerateCode(promotion.CommonId, codeLength: 8);
            var discountCode = new DiscountCode(promotion, code, dateTimeProvider.Now, dateTimeProvider.Now.AddDays(14))
            {
                Created = dateTimeProvider.Now.UtcDateTime
            };
            mockDiscountCodeStore
                .Setup(_ => _.GetLatestByPromotion(It.IsAny<RecommendableItem>()))
                .ReturnsAsync(new EntityResult<DiscountCode>(discountCode));
            mockDiscountCodeStore
                .Setup(_ => _.Create(It.IsAny<DiscountCode>()))
                .ReturnsAsync(discountCode);
            mockDiscountCodeStore
                .Setup(_ => _.Context)
                .Returns(mockStorageContext.Object);
            mockIntegratedSystemStore
                .Setup(_ => _.Query(null))
                .ReturnsAsync(
                    new Paginated<IntegratedSystem>(
                        new List<IntegratedSystem>{
                            integratedSystem
                        }, 1, 1, 1));
            mockDiscountCodeGenerator
                .Setup(_ => _.SystemType)
                .Returns(IntegratedSystemTypes.Shopify);
            var discountCodeGenerators = new List<IDiscountCodeGenerator>
            {
                mockDiscountCodeGenerator.Object
            };
            // act
            var workflow = new DiscountCodeWorkflows(
                mockLogger.Object,
                dateTimeProvider,
                mockIntegratedSystemStore.Object,
                mockDiscountCodeStore.Object,
                discountCodeGenerators
            );
            var result = await workflow.GenerateDiscountCodes(promotion);
            // assert
            mockDiscountCodeStore.Verify(_ => _.GetLatestByPromotion(It.IsAny<RecommendableItem>()), Times.Once);
            mockDiscountCodeStore.Verify(_ => _.Create(It.IsAny<DiscountCode>()), Times.Never);
            mockDiscountCodeGenerator.Verify(_ => _.Generate(
                It.IsAny<IntegratedSystem>(),
                It.IsAny<RecommendableItem>(),
                It.IsAny<DiscountCode>()), Times.Never);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task CanGenerateDiscountCodes_Returns_Empty()
        {
            // arrange
            var mockStorageContext = Utility.MockStorageContext();
            var dateTimeProvider = Utility.DateTimeProvider();
            var mockLogger = Utility.MockLogger<DiscountCodeWorkflows>();
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();
            var mockDiscountCodeStore = new Mock<IDiscountCodeStore>();
            var mockDiscountCodeGenerator = new Mock<IDiscountCodeGenerator>();

            var integratedSystem = new IntegratedSystem("my-integrated-system", "shopify", IntegratedSystemTypes.Shopify);
            integratedSystem.IntegrationStatus = IntegrationStatuses.NotConfigured;
            integratedSystem.IsDiscountCodeGenerator = true;

            var promotion = EntityFactory.Promotion();
            promotion.PromotionType = PromotionType.Discount;
            promotion.BenefitType = BenefitType.Percent;
            promotion.BenefitValue = 10;

            string code = DiscountCode.GenerateCode(promotion.CommonId, codeLength: 8);
            var discountCode = new DiscountCode(promotion, code, dateTimeProvider.Now, dateTimeProvider.Now.AddDays(14));

            mockDiscountCodeStore
                .Setup(_ => _.GetLatestByPromotion(It.IsAny<RecommendableItem>()))
                .ReturnsAsync(new EntityResult<DiscountCode>(discountCode));
            mockDiscountCodeStore
                .Setup(_ => _.Create(It.IsAny<DiscountCode>()))
                .ReturnsAsync(discountCode);
            mockDiscountCodeStore
                .Setup(_ => _.Context)
                .Returns(mockStorageContext.Object);
            mockIntegratedSystemStore
                .Setup(_ => _.Query(null))
                .ReturnsAsync(
                    new Paginated<IntegratedSystem>(
                        new List<IntegratedSystem>{
                            integratedSystem
                        }, 1, 1, 1));
            mockDiscountCodeGenerator
                .Setup(_ => _.SystemType)
                .Returns(IntegratedSystemTypes.Shopify);
            var discountCodeGenerators = new List<IDiscountCodeGenerator>
            {
                mockDiscountCodeGenerator.Object
            };
            // act
            var workflow = new DiscountCodeWorkflows(
                mockLogger.Object,
                dateTimeProvider,
                mockIntegratedSystemStore.Object,
                mockDiscountCodeStore.Object,
                discountCodeGenerators
            );
            var result = await workflow.GenerateDiscountCodes(promotion);
            // assert
            mockDiscountCodeStore.Verify(_ => _.GetLatestByPromotion(It.IsAny<RecommendableItem>()), Times.Never);
            mockDiscountCodeStore.Verify(_ => _.Create(It.IsAny<DiscountCode>()), Times.Never);
            mockDiscountCodeGenerator.Verify(_ => _.Generate(
                It.IsAny<IntegratedSystem>(),
                It.IsAny<RecommendableItem>(),
                It.IsAny<DiscountCode>()), Times.Never);
            Assert.Empty(result);
        }
    }
}