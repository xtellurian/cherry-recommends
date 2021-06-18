using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class HubspotCode
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string RedirectUri { get; set; }
    }
}