namespace SignalBox.Core.Recommenders
{
#nullable enable
    public class TriggerCollection
    {
        public TriggerCollection() { }
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