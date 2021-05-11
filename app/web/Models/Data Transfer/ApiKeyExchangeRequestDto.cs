using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class ApiKeyExchangeRequestDto
    {
        [Required]
        public string ApiKey { get; set; }
    }
}