using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateIntegratedSystemDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string SystemType { get; set; }

    }
}