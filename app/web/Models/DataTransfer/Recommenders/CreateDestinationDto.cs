using System.ComponentModel.DataAnnotations;
using SignalBox.Core;
using SignalBox.Core.Metrics.Destinations;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class CreateDestinationDto : DtoBase
    {
        [Required]
        [RegularExpression("Webhook|SegmentSourceFunction|HubspotContactProperty",
            ErrorMessage = "DestinationType must be one of Webhook, SegmentSourceFunction, HubspotContactProperty")]
        public string? DestinationType { get; set; }
        [Required]
        public long IntegratedSystemId { get; set; }
        public string? Endpoint { get; set; }
        public string? PropertyName { get; set; }

        public override void Validate()
        {
            base.Validate();
            if (!string.IsNullOrEmpty(Endpoint) && !string.IsNullOrEmpty(PropertyName))
            {
                throw new BadRequestException("Only one of endpoint and propertyName must be set.");
            }
            if (DestinationType == MetricDestinationBase.WebhookDestinationType && string.IsNullOrEmpty(Endpoint))
            {
                throw new BadRequestException("Webhooks require an endpoint");
            }
            if (DestinationType == MetricDestinationBase.HubspotContactPropertyDestinationType && string.IsNullOrEmpty(PropertyName))
            {
                throw new BadRequestException("Hubspot Property Name require an property name");
            }
        }
    }
}