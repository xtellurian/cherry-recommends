using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Adapters.Segment;
using SignalBox.Core.Serialization;
using SignalBox.Core.Workflows;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class WebhookWorkflowsTests
    {
        public static IEnumerable<object[]> PageEvent()
        {
            yield return new object[] { DataLoader.LoadSegmentWebhookJson("page"), EventKinds.PageView };
        }
        public static IEnumerable<object[]> IdentifyEvent()
        {
            yield return new object[] { DataLoader.LoadSegmentWebhookJson("identify"), EventKinds.Identify };
        }
        public static IEnumerable<object[]> ScreenEvent()
        {
            yield return new object[] { DataLoader.LoadSegmentWebhookJson("screen"), EventKinds.PageView };
        }
        public static IEnumerable<object[]> TrackEvent()
        {
            yield return new object[] { DataLoader.LoadSegmentWebhookJson("track"), EventKinds.Behaviour };
        }
        public static IEnumerable<object[]> GroupEvent()
        {
            yield return new object[] { DataLoader.LoadSegmentWebhookJson("group"), EventKinds.AddToBusiness };
        }


        [Theory]
        [MemberData(nameof(GroupEvent))]
        public async Task CanIngest_Segment_Group(SegmentModel groupEvent, EventKinds expectedKind)
        {
            // arrange
            var serialized = Serializer.Serialize(groupEvent);
            var endpointId = Guid.NewGuid().ToString();
            var integratedSystem = new IntegratedSystem("segment-test", "Segment", IntegratedSystemTypes.Segment);
            var mockLogger = Utility.MockLogger<WebhookWorkflows>();
            var mockWebhookReceiverStore = new Mock<IWebhookReceiverStore>();
            var mockTenantProvider = new Mock<ITenantProvider>();
            mockWebhookReceiverStore.Setup(_ => _.ReadFromEndpointId(It.Is<string>(e => e == endpointId)))
                .ReturnsAsync(new WebhookReceiver(endpointId, integratedSystem));
            var mockCustomerEventsWorkflows = new Mock<ICustomerEventsWorkflow>();
            mockCustomerEventsWorkflows.Setup(
                _ => _.Ingest(It.Is<IEnumerable<CustomerEventInput>>(x => x.First().Kind == expectedKind)))
                .ReturnsAsync(new EventLoggingResponse { EventsEnqueued = 1 });
            var sut = new WebhookWorkflows(mockLogger.Object, mockTenantProvider.Object, mockWebhookReceiverStore.Object, mockCustomerEventsWorkflows.Object);
            // act
            await sut.ProcessWebhook(endpointId, serialized, null);
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
            var mockLogger = Utility.MockLogger<WebhookWorkflows>();
            var mockWebhookReceiverStore = new Mock<IWebhookReceiverStore>();
            var mockTenantProvider = new Mock<ITenantProvider>();
            mockWebhookReceiverStore.Setup(_ => _.ReadFromEndpointId(It.Is<string>(e => e == endpointId)))
                .ReturnsAsync(new WebhookReceiver(endpointId, integratedSystem));
            var mockCustomerEventsWorkflows = new Mock<ICustomerEventsWorkflow>();
            mockCustomerEventsWorkflows.Setup(
                _ => _.Ingest(It.Is<IEnumerable<CustomerEventInput>>(x => x.Count() == 1)))
                .ReturnsAsync(new EventLoggingResponse { EventsEnqueued = 1 });
            var sut = new WebhookWorkflows(mockLogger.Object, mockTenantProvider.Object, mockWebhookReceiverStore.Object, mockCustomerEventsWorkflows.Object);
            // act
            await sut.ProcessWebhook(endpointId, serialized, null);
            //assert
            mockCustomerEventsWorkflows.Verify(
                _ => _.Ingest(It.Is<IEnumerable<CustomerEventInput>>(
                    x => x.Count() == 1 &&
                    x.First().Kind == expectedKind)), Times.Once());

        }
    }
}