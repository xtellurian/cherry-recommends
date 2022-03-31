using System.Collections.Generic;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Adapters.Segment;
using Xunit;

namespace SignalBox.Test.Converters
{
    public class SegmentWebhookConversionTests
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

        [Theory]
        [MemberData(nameof(PageEvent))]
        [MemberData(nameof(IdentifyEvent))]
        [MemberData(nameof(ScreenEvent))]
        [MemberData(nameof(TrackEvent))]
        [MemberData(nameof(GroupEvent))]
        public void CanConvertSegmentPayloadToCustomerEvents(SegmentModel p, EventKinds eventKind)
        {
            var integratedSystem = new IntegratedSystem("segmentTest", "Segment", IntegratedSystemTypes.Segment)
            {
                Id = 1,
                EnvironmentId = 2
            };
            var mockTenantProvider = new Mock<ITenantProvider>();
            mockTenantProvider.Setup(_ => _.RequestedTenantName).Returns("tenant-name");

            var e = SegmentConverter.ToCustomerEventInput(p, mockTenantProvider.Object, integratedSystem);
            Assert.Equal(integratedSystem.Id, e.SourceSystemId);
            Assert.Equal("tenant-name", e.TenantName);
            Assert.Equal(p.UserId, e.CustomerId);
            Assert.Equal(eventKind, e.Kind);
            Assert.Equal(p.MessageId, e.EventId);
            Assert.Equal(integratedSystem.EnvironmentId, e.EnvironmentId);
            if (p.Properties != null)
            {
                foreach (var pKey in p.Properties.Keys)
                {
                    Assert.True(e.Properties.ContainsKey(pKey));
                    Assert.Equal(p.Properties[pKey], e.Properties[pKey]);
                }
            }
            if (p.Traits != null)
            {
                foreach (var tKey in p.Traits.Keys)
                {
                    Assert.True(e.Properties.ContainsKey(tKey));
                    Assert.Equal(p.Traits[tKey], e.Properties[tKey]);
                }
            }

        }
    }
}