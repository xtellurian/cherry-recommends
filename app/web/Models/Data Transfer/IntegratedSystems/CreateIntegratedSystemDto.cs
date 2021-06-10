using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateIntegratedSystemDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("Segment|Hubspot", ErrorMessage = "SystemType must be one of Segment, Hubspot")]
        public string SystemType { get; set; }

    }
}