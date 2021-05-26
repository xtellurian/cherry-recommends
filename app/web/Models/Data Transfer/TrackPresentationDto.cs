using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class TrackPresentationDto : DtoBase
    {
        public string CommonUserId { get; set; }
        [Required]
        public long ExperimentId { get; set; }
        public string IterationId { get; set; }
        [Required]
        public long RecommendationId { get; set; }
        [Required]
        public long OfferId { get; set; }
        public string Outcome { get; set; } // accept reject ignore
    }
}