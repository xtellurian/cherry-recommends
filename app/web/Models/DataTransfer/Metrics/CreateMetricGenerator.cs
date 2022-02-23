
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SignalBox.Core;
using SignalBox.Core.Metrics;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class CreateMetricGenerator : DtoBase
    {
        public override void Validate()
        {
            base.Validate();
            if (GeneratorType == MetricGeneratorTypes.FilterSelectAggregate)
            {
                // validate filter select aggregate
                if (this.Steps == null || !this.Steps.Any())
                {
                    throw new BadRequestException("At least one step is required.");
                }
            }
            else if (GeneratorType == MetricGeneratorTypes.JoinTwoMetrics)
            {
                if (JoinTwoMetrics == null)
                {
                    throw new BadRequestException("joinTwoMetrics is a required property");
                }
            }
        }

        public string? FeatureCommonId
        {
            get => MetricCommonId; set
            {
                if (value != null)
                {
                    MetricCommonId = value;
                }
            }
        }

        [Required]
        public string? MetricCommonId { get; set; }
        [Required]
        public MetricGeneratorTypes? GeneratorType { get; set; }

        // below are set based on the generator type
        public List<FilterSelectAggregateStepDto>? Steps { get; set; }
        public AggregateCustomerMetricDto? AggregateCustomerMetric { get; set; }
        public JoinTwoMetricsDto? JoinTwoMetrics { get; set; }
        public MetricGeneratorTimeWindow? TimeWindow { get; set; }
    }
}