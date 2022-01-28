
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class CreateMetricGenerator : DtoBase
    {
        public override void Validate()
        {
            base.Validate();
            if (this.GeneratorType == "FilterSelectAggregate")
            {
                // validate filter select aggregate
                if (this.Steps == null || !this.Steps.Any())
                {
                    throw new BadRequestException("At least one step is required.");
                }
            }
        }

        public string FeatureCommonId
        {
            get => MetricCommonId; set
            {
                if (value != null)
                {
                    MetricCommonId = value;
                }
            }
        }

        public string MetricCommonId { get; set; }
        [RegularExpression("MonthsSinceEarliestEvent|FilterSelectAggregate",
            ErrorMessage = "GeneratorType must be one of MonthsSinceEarliestEvent, FilterSelectAggregate")]
        public string GeneratorType { get; set; }

        public List<FilterSelectAggregateStepDto> Steps { get; set; }

    }
}