
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SignalBox.Core;
using SignalBox.Core.Metrics;

namespace SignalBox.Web.Dto
{
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
        public MetricGeneratorTypes GeneratorType { get; set; }

        public List<FilterSelectAggregateStepDto> Steps { get; set; }
        public AggregateCustomerMetricDto AggregateCustomerMetric { get; set; }
        public MetricGeneratorTimeWindow? TimeWindow { get; set; }
    }
}