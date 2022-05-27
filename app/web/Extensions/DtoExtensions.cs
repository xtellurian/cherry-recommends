using System.Collections.Generic;
using System.Linq;
using SignalBox.Core;
using SignalBox.Core.Metrics;
using SignalBox.Core.Campaigns;
using SignalBox.Web.Dto;

#nullable enable
namespace SignalBox.Web
{
    public static class DtoExtensions
    {
        public static List<CampaignArgument>? ToCoreRepresentation(
            this IEnumerable<CreateOrUpdateCampaignArgument>? dtoArguments)
        {
            return dtoArguments?.Select(a => new CampaignArgument(a.CommonId, a.ArgumentType, a.IsRequired))?.ToList();
        }

        public static TriggerCollection ToCoreRepresentation(this SetTriggersDto triggersDto, TriggerCollection? current)
        {
            return new TriggerCollection
            {
                MetricsChanged = triggersDto.MetricsChanged
            };
        }

        public static List<FilterSelectAggregateStep>? ToCoreRepresentation(
           this IEnumerable<FilterSelectAggregateStepDto>? dtoSteps)
        {
            return dtoSteps?.Select(s =>
            {
                switch (s.Type)
                {
                    case "Filter":
                        return new FilterSelectAggregateStep(s.Order, new FilterStep(s.EventTypeMatch));
                    case "Select":
                        return new FilterSelectAggregateStep(s.Order, new SelectStep(s.PropertyNameMatch));
                    case "Aggregate":
                        return new FilterSelectAggregateStep(s.Order, new AggregateStep
                        {
                            AggregationType = System.Enum.Parse<AggregationTypes>(s.AggregationType)
                        });
                    default:
                        throw new Core.BadRequestException($"{s.Type} is an unknown step type");
                }
            })?.OrderBy(_ => _.Order).ToList();
        }

        public static AggregateCustomerMetric ToCoreRepresentation(this AggregateCustomerMetricDto dto)
        {
            if (dto.AggregationType == null)
            {
                throw new BadRequestException("AggregationType is required");
            };
            return new AggregateCustomerMetric
            {
                AggregationType = dto.AggregationType.Value,
                CategoricalValue = dto.CategoricalValue,
                MetricId = dto.MetricId
            };
        }
        public static JoinTwoMetrics ToCoreRepresentation(this JoinTwoMetricsDto dto)
        {
            if (dto.JoinType == null)
            {
                throw new BadRequestException("JoinType is required");
            };
            return new JoinTwoMetrics
            {
                Metric1Id = dto.Metric1Id ?? throw new BadRequestException("metric1Id is required"),
                Metric2Id = dto.Metric2Id ?? throw new BadRequestException("metric2Id is required"),
                JoinType = dto.JoinType ?? throw new BadRequestException("joinType is required")
            };
        }
    }
}