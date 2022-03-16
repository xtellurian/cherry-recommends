using System.ComponentModel.DataAnnotations;
using SignalBox.Core;
using SignalBox.Core.Predicates;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class CreateMetricEnrolmentRuleDto : DtoBase
    {
        public override void Validate()
        {
            base.Validate();
            // this will need to be in validate, once there is a categorical predicate option
            if (NumericPredicate == null)
            {
                throw new BadRequestException("NumericPredicate is required");
            }
        }

        [Required]
        public long? MetricId { get; set; }

        public NumericPredicate? NumericPredicate { get; set; }
    }
}