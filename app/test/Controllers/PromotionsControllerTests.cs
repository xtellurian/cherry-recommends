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
    public class PromotionsControllerTests
    {
        [Fact]
        public async Task CanQuery()
        {
            var mockRecommendableItemStore = new Mock<IRecommendableItemStore>();
            var mockItemsRecommenderStore = new Mock<IItemsRecommenderStore>();
            var mockStorageContext = new Mock<IStorageContext>();

            mockRecommendableItemStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);

            string promotionId = "promo1";
            var promoItem = new RecommendableItem(promotionId, "item1", 1, 1, BenefitType.Percent, 3, PromotionType.Discount, null);
            var promoItems = new List<RecommendableItem>() { promoItem };

            PaginateRequest paginate = new PaginateRequest();
            mockRecommendableItemStore
                .Setup(_ => _.Query(It.IsAny<EntityStoreQueryOptions<RecommendableItem>>()))
                .ReturnsAsync(new Paginated<RecommendableItem>(promoItems, 1, promoItems.Count, 1));

            var workflow = new RecommendableItemWorkflows(mockRecommendableItemStore.Object,
                mockItemsRecommenderStore.Object, mockStorageContext.Object);

            var sut = new PromotionsController(mockRecommendableItemStore.Object, workflow);
            PaginateRequest paginateReq = new PaginateRequest();

            // create HttpRequest object and query string
            var request = new Dictionary<string, StringValues>
            {
                { "promotionType", "" },
                { "benefitType", "" }
            };

            var queryCollection = new QueryCollection(request);
            var query = new QueryFeature(queryCollection);

            var features = new FeatureCollection();
            features.Set<IQueryFeature>(query);

            // assign HttpContext to Controller
            var context = new DefaultHttpContext(features);
            sut.ControllerContext.HttpContext = context;

            var result = await sut.Query(paginateReq, new SearchEntities());

            Assert.NotNull(result);
            Assert.IsType<Paginated<RecommendableItem>>(result);
            Assert.NotEmpty(result.Items);
        }


        [Fact]
        public async Task CanCreate()
        {
            var mockRecommendableItemStore = new Mock<IRecommendableItemStore>();
            var mockItemsRecommenderStore = new Mock<IItemsRecommenderStore>();
            var mockStorageContext = new Mock<IStorageContext>();

            var properties = new DynamicPropertyDictionary() { { "discount", 0.05 } };
            var recommendableItem = new RecommendableItem("item1", "item1", 1, 1, BenefitType.Percent, 3, PromotionType.Discount, properties);

            mockRecommendableItemStore.Setup(x => x.Create(It.Is<RecommendableItem>(_ => _.CommonId == recommendableItem.CommonId)))
                .ReturnsAsync(recommendableItem);
            mockRecommendableItemStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);

            var workflow = new RecommendableItemWorkflows(mockRecommendableItemStore.Object,
                mockItemsRecommenderStore.Object, mockStorageContext.Object);

            CreatePromotionDto dto = new CreatePromotionDto();
            dto.CommonId = recommendableItem.CommonId;
            dto.Name = recommendableItem.Name;
            dto.DirectCost = recommendableItem.DirectCost.Value;
            dto.BenefitType = recommendableItem.BenefitType;
            dto.BenefitValue = recommendableItem.BenefitValue;
            dto.PromotionType = recommendableItem.PromotionType;
            dto.NumberOfRedemptions = recommendableItem.NumberOfRedemptions;
            dto.Description = recommendableItem.Description;
            dto.Properties = recommendableItem.Properties;

            var sut = new PromotionsController(mockRecommendableItemStore.Object, workflow);
            var result = await sut.Create(dto);

            Assert.NotNull(result);
            Assert.IsType<RecommendableItem>(result);
        }

        [Fact]
        public async Task CanUpdate()
        {
            var mockRecommendableItemStore = new Mock<IRecommendableItemStore>();
            var mockItemsRecommenderStore = new Mock<IItemsRecommenderStore>();
            var mockStorageContext = new Mock<IStorageContext>();

            string promotionId = "promo1";
            var properties = new DynamicPropertyDictionary() { { "discount", 0.05 } };
            var promoItem = new RecommendableItem(promotionId, "item1", 1, 1, BenefitType.Percent, 3, PromotionType.Discount, properties);

            mockRecommendableItemStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            mockRecommendableItemStore.Setup(x => x.Create(It.Is<RecommendableItem>(_ => _.CommonId == promoItem.CommonId)))
               .ReturnsAsync(promoItem);
            mockRecommendableItemStore.SetupCommonStoreRead<Mock<IRecommendableItemStore>, IRecommendableItemStore, RecommendableItem>(promoItem);

            var workflow = new RecommendableItemWorkflows(mockRecommendableItemStore.Object,
                mockItemsRecommenderStore.Object, mockStorageContext.Object);

            var sut = new PromotionsController(mockRecommendableItemStore.Object, workflow);
            UpdatePromotionDto dto = new UpdatePromotionDto();

            dto.Name = "updatedPromo";
            dto.DirectCost = 1;
            dto.BenefitType = BenefitType.Fixed;
            dto.BenefitValue = 1;
            dto.PromotionType = PromotionType.Other;
            dto.NumberOfRedemptions = 2;
            dto.Description = "Updated Promo";
            dto.Properties = null;

            var result = await sut.Update(promotionId, dto);

            Assert.NotNull(result);
            Assert.IsType<RecommendableItem>(result);
        }
    }
}