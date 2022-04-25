using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateIntegratedSystemDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("Segment|Hubspot|Shopify|Klaviyo|Custom|Website", ErrorMessage = "SystemType must be one of Segment, Hubspot, Shopify, Klaviyo, Custom, Website")]
        public string SystemType { get; set; }

    }
}