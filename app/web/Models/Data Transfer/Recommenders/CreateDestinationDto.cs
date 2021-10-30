using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateDestinationDto : DtoBase
    {
        [Required]
        [RegularExpression("Webhook|SegmentSourceFunction", ErrorMessage = "DestinationType must be one of Webhook, SegmentSourceFunction")]
        public string DestinationType { get; set; }
        [Required]
        public string Endpoint { get; set; }
        [Required]
        public long IntegratedSystemId { get; set; }
    }
}