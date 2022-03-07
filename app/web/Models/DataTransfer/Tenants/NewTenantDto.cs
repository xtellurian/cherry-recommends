using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class NewTenantDto : DtoBase
    {
        [Required]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Tenant names must be 4-24 characters long")]
        public string Name { get; set; }

        [Required]
        public string TermsOfServiceVersion { get; set; }
    }
}