using System.ComponentModel.DataAnnotations;
using SignalBox.Core.Metrics;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class AggregateCustomerMetricDto : DtoBase
    {
        [Required]
        public long MetricId { get; set; }
        [Required]
        public AggregationTypes? AggregationType { get; set; }
        public string? CategoricalValue { get; set; } // value to sum (i.e. count) if the other metric is a categorical metric

    }
}