using System.ComponentModel.DataAnnotations;
using SignalBox.Core.Metrics;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class JoinTwoMetricsDto : DtoBase
    {
        [Required]
        public long? Metric1Id { get; set; }
        [Required]
        public long? Metric2Id { get; set; }
        [Required]
        public JoinType? JoinType { get; set; }
    }
}