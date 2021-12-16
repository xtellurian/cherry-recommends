using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    /// <summary>
    /// Links a user to an existing Integrated System resource.
    /// </summary>
    public class IntegratedSystemReference
    {
        /// <summary>
        /// The SignalBox Identifier of the integrated system.
        /// </summary>
        [Required]
        public long IntegratedSystemId { get; set; }
        /// <summary>
        /// The unqiue User Id in the external system, e.g. Hubspot Contact Id.
        /// </summary>
        [Required]
        public string UserId { get; set; }
    }
}