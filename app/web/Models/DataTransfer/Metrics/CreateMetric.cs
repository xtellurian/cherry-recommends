
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
            if (Scope == MetricScopes.Global && ValueType != MetricValueType.Numeric)
            {
                throw new BadRequestException("Global scope metrics must be numeric");
            }
        }

        [Required]
        public MetricValueType? ValueType { get; set; }
        [Required]
        public MetricScopes Scope { get; set; }
    }
}