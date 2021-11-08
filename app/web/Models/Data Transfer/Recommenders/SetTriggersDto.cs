using SignalBox.Core.Recommenders;

#nullable enable
namespace SignalBox.Web.Dto
{
    public class SetTriggersDto : DtoBase
    {
        public FeaturesChangedTrigger? FeaturesChanged { get; set; }
    }
}