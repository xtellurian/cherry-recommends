
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class SetKlaviyoApiKeysDto
    {
        [Required]
        public string PublicKey { get; set; }
        [Required]
        public string PrivateKey { get; set; }
    }
}