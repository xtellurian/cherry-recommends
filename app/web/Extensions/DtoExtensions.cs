using System.Collections.Generic;
using System.Linq;
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
    }
}