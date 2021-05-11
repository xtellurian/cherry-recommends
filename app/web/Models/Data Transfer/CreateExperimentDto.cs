using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class CreateExperimentDto : DtoBase
    {
        public IEnumerable<long> OfferIds { get; set; } = new List<long>();

        [StringLength(50, MinimumLength = 4)]
        [Required]
        public string? Name { get; set; }
        [Range(1, 5)]
        public int ConcurrentOffers { get; set; }
    }
}