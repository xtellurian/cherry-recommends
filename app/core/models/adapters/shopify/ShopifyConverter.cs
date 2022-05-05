using System;
using System.Collections.Generic;

namespace SignalBox.Core.Adapters.Shopify
{
    public static class ShopifyConverter
    {
        public static CustomerEventInput ToCustomerEventInput(this ShopifyOrder model, string webhookId, string topic, ITenantProvider tenantProvider, IntegratedSystem sys)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            long? correlatorId = null;
            string eventType = $"Shopify|{topic ?? "Unknown"}";
            var properties = ExtractProperties(model);
            var customerId = model.Email ?? model.Customer?.Email ?? $"{model.Customer?.Id}";

            switch (topic)
            {
                case "orders/paid":
                    // Since we captured an orders paid event, we expect the UpdatedAt to be the paidAt
                    properties.Add("paidAt", model.UpdatedAt);
                    break;
                default:
                    break;
            }

            return new CustomerEventInput
            (
                tenantName: tenantProvider.RequestedTenantName,
                customerId: customerId,
                businessCommonId: null,
                eventId: webhookId,
                timestamp: model.UpdatedAt, // DateTimeProvider.Now will be used when null
                environmentId: sys.EnvironmentId,
                recommendationCorrelatorId: correlatorId,
                sourceSystemId: sys.Id,
                kind: EventKinds.Custom,
                eventType: eventType,
                properties: properties
            );
        }

        private static Dictionary<string, object> ExtractProperties(ShopifyOrder model)
        {
            var properties = new Dictionary<string, object>();

            properties.Add("orderId", model.Id);
            properties.Add("orderNumber", model.OrderNumber);
            properties.Add("financialStatus", model.FinancialStatus);
            properties.Add("currency", model.Currency);
            properties.Add("taxesIncluded", model.TaxesIncluded);
            properties.Add("totalPrice", model.TotalPrice);
            properties.Add("subtotalPrice", model.SubtotalPrice);
            properties.Add("totalTax", model.TotalTax);
            properties.Add("totalDiscounts", model.TotalDiscounts);
            properties.Add("totalLineItemsPrice", model.TotalLineItemsPrice);
            properties.Add("discountCodes", model.DiscountCodes);
            properties.Add("discountApplications", model.DiscountApplications);
            properties.Add("lineItems", model.LineItems);
            properties.Add("shippingLines", model.ShippingLines);
            properties.Add("taxLines", model.TaxLines);
            properties.Add("customerId", model.Customer?.Id);
            properties.Add("customerEmail", model.Customer?.Email);
            properties.Add("customerFirstName", model.Customer?.FirstName);
            properties.Add("customerLastName", model.Customer?.LastName);
            properties.Add("test", model.Test);

            return properties;
        }
    }
}
