using System.ComponentModel.DataAnnotations;
using SignalBox.Core;

namespace SignalBox.Functions
{
    public class CreateTenantModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string CreatorId { get; set; }
        public string TermsOfServiceVersion { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new BadRequestException("name must not be empty");
            }
            if (string.IsNullOrEmpty(CreatorId))
            {
                throw new BadRequestException("creatorId must not be empty");
            }
        }
    }
}