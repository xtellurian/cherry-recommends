using System.Collections.Generic;
using System.Linq;
using SignalBox.Core.Features;
using SignalBox.Core.Recommenders;
using SignalBox.Web.Dto;

#nullable enable
namespace SignalBox.Web
{
    public static class DtoExtensions
    {
        public static List<RecommenderArgument>? ToCoreRepresentation(
            this IEnumerable<CreateOrUpdateRecommenderArgument>? dtoArguments)
        {
            return dtoArguments?.Select(a => new RecommenderArgument
            {
                ArgumentType = a.ArgumentType,
                CommonId = a.CommonId,
                DefaultValue = new DefaultArgumentContainer(a.ArgumentType, a.DefaultValue)
            })?.ToList();
        }

        public static TriggerCollection ToCoreRepresentation(this SetTriggersDto triggersDto, TriggerCollection? current)
        {
            return new TriggerCollection
            {
                FeaturesChanged = triggersDto.FeaturesChanged
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
    }
}