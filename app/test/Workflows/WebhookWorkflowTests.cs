using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Adapters.Segment;
using SignalBox.Core.Adapters.Shopify;
using SignalBox.Core.Serialization;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class WebhookWorkflowsTests
    {
        public static IEnumerable<object[]> PageEvent()
        {
            yield return new object[] { DataLoader.LoadFromJsonData<SegmentModel>("segmentEvents.json", "page"), EventKinds.PageView };
        }
        public static IEnumerable<object[]> IdentifyEvent()
        {
            yield return new object[] { DataLoader.LoadFromJsonData<SegmentModel>("segmentEvents.json", "identify"), EventKinds.Identify };
        }
        public static IEnumerable<object[]> ScreenEvent()
        {
            yield return new object[] { DataLoader.LoadFromJsonData<SegmentModel>("segmentEvents.json", "screen"), EventKinds.PageView };
        }
        public static IEnumerable<object[]> TrackEvent()
        {
            yield return new object[] { DataLoader.LoadFromJsonData<SegmentModel>("segmentEvents.json", "track"), EventKinds.Behaviour };
        }
        public static IEnumerable<object[]> GroupEvent()
        {
            yield return new object[] { DataLoader.LoadFromJsonData<SegmentModel>("segmentEvents.json", "group"), EventKinds.AddToBusiness };
        }

        public static IEnumerable<object[]> ShopifyOrder()
        {
            yield return new object[] { DataLoader.LoadFromJsonData<ShopifyOrder>("shopifyEvents.json", "order"), EventKinds.Custom };
        }


        [Theory]
        [MemberData(nameof(GroupEvent))]
        public async Task CanIngest_Segment_Group(SegmentModel groupEvent, EventKinds expectedKind)
        {
            // arrange
            var serialized = Serializer.Serialize(groupEvent);
            var endpointId = Guid.NewGuid().ToString();
            var integratedSystem = new IntegratedSystem("segment-test", "Segment", IntegratedSystemTypes.Segment);
            var webhookReceiver = new WebhookReceiver(endpointId, integratedSystem);
            var mockLogger = Utility.MockLogger<WebhookWorkflows>();
            var mockWebhookReceiverStore = new Mock<IWebhookReceiverStore>();
            var mockTenantProvider = new Mock<ITenantProvider>();
            mockWebhookReceiverStore.Setup(_ => _.ReadFromEndpointId(It.Is<string>(e => e == endpointId)))
                .ReturnsAsync(new EntityResult<WebhookReceiver>(webhookReceiver));
            var mockCustomerEventsWorkflows = new Mock<ICustomerEventsWorkflow>();
            mockCustomerEventsWorkflows.Setup(
                _ => _.Ingest(It.Is<IEnumerable<CustomerEventInput>>(x => x.First().Kind == expectedKind)))
                .ReturnsAsync(new EventLoggingResponse { EventsEnqueued = 1 });
            var segmentWebhookWorkflow = new SegmentWebhookWorkflow(Utility.MockStorageContext().Object, Utility.MockLogger<SegmentWebhookWorkflow>().Object, mockCustomerEventsWorkflows.Object, mockTenantProvider.Object);
            var mockShopifyWebhookWorkflow = new Mock<IShopifyWebhookWorkflow>();
            var sut = new WebhookWorkflows(mockLogger.Object, mockTenantProvider.Object, mockWebhookReceiverStore.Object, mockCustomerEventsWorkflows.Object, segmentWebhookWorkflow, mockShopifyWebhookWorkflow.Object);
            var headers = Enumerable.Empty<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>>();
            // act
            await sut.ProcessWebhook(endpointId, serialized, headers, null);
            //assert
            mockCustomerEventsWorkflows.Verify(
                _ => _.Ingest(It.Is<IEnumerable<CustomerEventInput>>(
                    x => x.Count() == 1 &&
                    x.First().BusinessCommonId == groupEvent.GroupId &&
                    x.First().Properties.Keys.Count >= groupEvent.Traits.Keys.Count)), Times.Once());

        }

        [Theory]
        [MemberData(nameof(PageEvent))]
        [MemberData(nameof(IdentifyEvent))]
        [MemberData(nameof(ScreenEvent))]
        [MemberData(nameof(TrackEvent))]
        [MemberData(nameof(GroupEvent))]
        public async Task CanIngest_Segment_WebhookPayloads(SegmentModel model, EventKinds expectedKind)
        {
            // arrange
            var serialized = Serializer.Serialize(model);
            var endpointId = Guid.NewGuid().ToString();
            var integratedSystem = new IntegratedSystem("segment-test", "Segment", IntegratedSystemTypes.Segment);
            var webhookReceiver = new WebhookReceiver(endpointId, integratedSystem);
            var mockLogger = Utility.MockLogger<WebhookWorkflows>();
            var mockWebhookReceiverStore = new Mock<IWebhookReceiverStore>();
            var mockTenantProvider = new Mock<ITenantProvider>();
            mockWebhookReceiverStore.Setup(_ => _.ReadFromEndpointId(It.Is<string>(e => e == endpointId)))
                .ReturnsAsync(new EntityResult<WebhookReceiver>(webhookReceiver));
            var mockCustomerEventsWorkflows = new Mock<ICustomerEventsWorkflow>();
            mockCustomerEventsWorkflows.Setup(
                _ => _.Ingest(It.Is<IEnumerable<CustomerEventInput>>(x => x.Count() == 1)))
                .ReturnsAsync(new EventLoggingResponse { EventsEnqueued = 1 });
            var segmentWebhookWorkflow = new SegmentWebhookWorkflow(Utility.MockStorageContext().Object, Utility.MockLogger<SegmentWebhookWorkflow>().Object, mockCustomerEventsWorkflows.Object, mockTenantProvider.Object);
            var mockShopifyWebhookWorkflow = new Mock<IShopifyWebhookWorkflow>();
            var sut = new WebhookWorkflows(mockLogger.Object, mockTenantProvider.Object, mockWebhookReceiverStore.Object, mockCustomerEventsWorkflows.Object, segmentWebhookWorkflow, mockShopifyWebhookWorkflow.Object);
            var headers = Enumerable.Empty<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>>();
            // act
            await sut.ProcessWebhook(endpointId, serialized, headers, null);
            //assert
            mockCustomerEventsWorkflows.Verify(
                _ => _.Ingest(It.Is<IEnumerable<CustomerEventInput>>(
                    x => x.Count() == 1 &&
                    x.First().Kind == expectedKind)), Times.Once());

        }

        [Theory]
        [MemberData(nameof(ShopifyOrder))]
        public async Task CanIngest_Shopify_OrdersPaid_Payload(ShopifyOrder model, EventKinds expectedKind)
        {
            // arrange
            var serialized = Serializer.Serialize(model);
            var endpointId = Guid.NewGuid().ToString();
            var integratedSystem = new IntegratedSystem("shopify-test", "Shopify", IntegratedSystemTypes.Shopify)
            {
                IntegrationStatus = IntegrationStatuses.OK
            };
            var webhookReceiver = new WebhookReceiver(endpointId, integratedSystem);
            var mockLogger = Utility.MockLogger<WebhookWorkflows>();
            var mockWebhookReceiverStore = new Mock<IWebhookReceiverStore>();
            var mockTenantProvider = new Mock<ITenantProvider>();
            mockWebhookReceiverStore.Setup(_ => _.ReadFromEndpointId(It.Is<string>(e => e == endpointId)))
                .ReturnsAsync(new EntityResult<WebhookReceiver>(webhookReceiver));
            var mockCustomerEventsWorkflows = new Mock<ICustomerEventsWorkflow>();
            mockCustomerEventsWorkflows.Setup(
                _ => _.Ingest(It.Is<IEnumerable<CustomerEventInput>>(x => x.Count() == 1)))
                .ReturnsAsync(new EventLoggingResponse { EventsEnqueued = 1 });
            var segmentWebhookWorkflow = new Mock<ISegmentWebhookWorkflow>();
            var mockStorageContext = Utility.MockStorageContext();
            var mockShopifyService = new Mock<IShopifyService>();
            mockShopifyService.Setup(
                _ => _.IsAuthenticWebhook(It.Is<IEnumerable<KeyValuePair<string, StringValues>>>(x => x.Any()), It.Is<string>(x => !string.IsNullOrEmpty(x))))
                .ReturnsAsync(true);
            var mockIntegratedSystemStore = new Mock<IIntegratedSystemStore>();
            mockIntegratedSystemStore.Setup(_ => _.Context).Returns(mockStorageContext.Object);
            var mockShopifyAdminWorkflow = new Mock<IShopifyAdminWorkflow>();
            var mockBillingInfo = new Mock<IOptions<Core.Integrations.ShopifyBilling>>();
            var mockShopifyWebhookWorkflow = new ShopifyWebhookWorkflow(mockStorageContext.Object, mockShopifyService.Object, Utility.MockLogger<ShopifyWebhookWorkflow>().Object, mockIntegratedSystemStore.Object, mockCustomerEventsWorkflows.Object, mockTenantProvider.Object, mockShopifyAdminWorkflow.Object, mockBillingInfo.Object);
            var sut = new WebhookWorkflows(mockLogger.Object, mockTenantProvider.Object, mockWebhookReceiverStore.Object, mockCustomerEventsWorkflows.Object, segmentWebhookWorkflow.Object, mockShopifyWebhookWorkflow);
            var mockHeaders = new Dictionary<string, StringValues>()
            {
                { "X-Shopify-Topic", "orders/paid" },
                { "X-Shopify-Hmac-Sha256", "Hju9XK2yLcxwnyLg8qy9NcyAZiXDhqEuE808rpfvAwc=" },
                { "X-Shopify-Shop-Domain", "dev-cherry-ai.myshopify.com" },
                { "X-Shopify-API-Version", "2022-01" },
                { "X-Shopify-Webhook-Id", "b0068344-227f-4423-9612-95a84bda2e5b" },
            };
            var headers = mockHeaders.Select(_ => _);
            // act
            await sut.ProcessWebhook(endpointId, serialized, headers, null);
            //assert
            mockCustomerEventsWorkflows.Verify(
                _ => _.Ingest(It.Is<IEnumerable<CustomerEventInput>>(
                    x => x.Count() == 1 &&
                    x.First().Kind == expectedKind)), Times.Once());
        }
    }
}