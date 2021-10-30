using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateApiKeyDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression("Server|Web", ErrorMessage = "ApiKeyType must be one of Server, Web")]
        public string ApiKeyType { get; set; }
        public string Scope { get; set; }
    }
}