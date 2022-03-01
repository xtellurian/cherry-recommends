using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json.Linq;
using SignalBox.Core;
using SignalBox.Core.Adapters.Segment;
using Xunit;

namespace SignalBox.Test
{
    public class AggregateCustomerMetricWorkflowTests
    {
        // https://segment.com/docs/connections/destinations/catalog/webhooks/
        public static SegmentModel LoadJson(string key)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Conversions", "segmentEvents.json");
            var json = File.ReadAllText(filePath);
            return JObject.Parse(json)[key].ToObject<SegmentModel>();
        }
        public static IEnumerable<object[]> PageEvent()
        {
            yield return new object[] { LoadJson("page"), EventKinds.PageView };
        }
        public static IEnumerable<object[]> IdentifyEvent()
        {
            yield return new object[] { LoadJson("identify"), EventKinds.Identify };
        }
        public static IEnumerable<object[]> ScreenEvent()
        {
            yield return new object[] { LoadJson("screen"), EventKinds.PageView };
        }
        public static IEnumerable<object[]> TrackEvent()
        {
            yield return new object[] { LoadJson("track"), EventKinds.Behaviour };
        }

        [Theory]
        [MemberData(nameof(PageEvent))]
        [MemberData(nameof(IdentifyEvent))]
        [MemberData(nameof(ScreenEvent))]
        [MemberData(nameof(TrackEvent))]
        public void CanConvertSegmentPayloadToCustomerEvents(SegmentModel p, EventKinds eventKind)
        {
            var integratedSystem = new IntegratedSystem("segmentTest", "Segment", IntegratedSystemTypes.Segment)
            {
                Id = 1,
                EnvironmentId = 2
            };
            var mockTenantProvider = new Mock<ITenantProvider>();
            mockTenantProvider.Setup(_ => _.RequestedTenantName).Returns("tenant-name");

            var e = Converter.ToTrackedUserEventInput(p, mockTenantProvider.Object, integratedSystem);
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