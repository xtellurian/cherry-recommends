
using System.ComponentModel.DataAnnotations;
using SignalBox.Core;
using SignalBox.Core.Metrics;

namespace SignalBox.Web.Dto
{
    public class CreateMetric : CommonDtoBase
    {
        public override void Validate()
        {
            base.Validate();
            if (Scope == MetricScopes.Customer && ValueType == null)
            {
                throw new BadRequestException("valueType is required when metricScope: Customer");
            }
        }

        public MetricValueType? ValueType { get; set; }
        [Required]
        public MetricScopes Scope { get; set; }
    }
}