using SignalBox.Core.Recommenders;

#nullable enable
namespace SignalBox.Web.Dto
{
    public class SetTriggersDto : DtoBase
    {
        public MetricsChangedTrigger? FeaturesChanged
        {
            get => MetricsChanged; set
            {
                if (value != null)
                {
                    MetricsChanged = value;
                }
            }
        }
        public MetricsChangedTrigger? MetricsChanged { get; set; }
    }
}