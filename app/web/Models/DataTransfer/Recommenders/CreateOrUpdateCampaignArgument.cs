using System.ComponentModel.DataAnnotations;
using SignalBox.Core.Campaigns;

namespace SignalBox.Web.Dto
{
    public class CreateOrUpdateCampaignArgument
    {
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string CommonId { get; set; } // use commonId for external API consistency
        public ArgumentTypes ArgumentType { get; set; }
        public bool IsRequired { get; set; } = false; // default not required.
    }
}