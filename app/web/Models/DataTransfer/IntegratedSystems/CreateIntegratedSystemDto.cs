using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateIntegratedSystemDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("Segment|Hubspot|Custom", ErrorMessage = "SystemType must be one of Segment, Hubspot, Custom")]
        public string SystemType { get; set; }

    }
}