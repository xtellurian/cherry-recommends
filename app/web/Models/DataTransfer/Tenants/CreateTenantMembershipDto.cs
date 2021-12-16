using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateTenantMembershipDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}