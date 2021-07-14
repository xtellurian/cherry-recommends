using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateApiKeyDto
    {
        [Required]
        public string Name { get; set; }
        public string Scope { get; set; }
    }
}