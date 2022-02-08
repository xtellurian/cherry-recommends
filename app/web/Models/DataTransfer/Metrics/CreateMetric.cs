
using System.ComponentModel.DataAnnotations;
using SignalBox.Core.Metrics;

namespace SignalBox.Web.Dto
{
    public class CreateMetric : CommonDtoBase
    {
        [Required]
        public MetricValueType ValueType { get; set; }
    }
}