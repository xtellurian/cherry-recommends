using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class AddAudienceSegmentDto
    {
        /// <summary>
        /// Id of the segment to add to the Audience
        /// </summary>
        [Required]
        public long SegmentId { get; set; }
    }
}