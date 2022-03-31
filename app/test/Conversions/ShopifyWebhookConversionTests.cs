using System.Collections.Generic;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Adapters.Shopify;
using Xunit;

namespace SignalBox.Test.Converters
{
    public class ShopifyWebhookConversionTests
    {
        public static IEnumerable<object[]> ShopifyOrder()
        {
            yield return new object[] { DataLoader.LoadFromJsonData<ShopifyOrder>("shopifyEvents.json", "order"), EventKinds.Custom };
        }

        [Theory]
        [MemberData(nameof(ShopifyOrder))]
        public void CanConvertShopifyPayloadToCustomerEvents(ShopifyOrder p, EventKinds eventKind)
        {
            var webhookId = "ABCDEFGHIJKL0123456789";
            var topic = "orders/paid";
            var integratedSystem = new IntegratedSystem("shopifyTest", "Shopify", IntegratedSystemTypes.Shopify)
            {
                Id = 1,
                EnvironmentId = 2
            };
            var mockTenantProvider = new Mock<ITenantProvider>();
            mockTenantProvider.Setup(_ => _.RequestedTenantName).Returns("tenant-name");

            var e = ShopifyConverter.ToCustomerEventInput(p, webhookId, topic, mockTenantProvider.Object, integratedSystem);
            Assert.Equal(integratedSystem.Id, e.SourceSystemId);
            Assert.Equal("tenant-name", e.TenantName);
            Assert.Equal(p.Email ?? p.Customer?.Email ?? $"{p.Customer?.Id}", e.CustomerId);
            Assert.Equal(eventKind, e.Kind);
            Assert.Equal(webhookId, e.EventId);
            Assert.Equal(integratedSystem.EnvironmentId, e.EnvironmentId);
            Assert.NotEmpty(e.Properties);
        }
    }
}