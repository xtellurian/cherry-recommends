using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateTouchpointMetadata : DtoBase
    {
        /// <summary>Set a unique Id for the touchpoint.</summary>
        [Required]
        [StringLength(maximumLength: 50)]
        public string CommonId { get; set; }

        /// <summary>Optional friendly name of the touchpoint.</summary>
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
    }
}