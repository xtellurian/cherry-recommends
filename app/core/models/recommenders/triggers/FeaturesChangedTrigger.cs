using System.Collections.Generic;

#nullable enable
namespace SignalBox.Core.Recommenders
{
    public class MetricsChangedTrigger : TriggerBase
    {
        public MetricsChangedTrigger()
        { }

        public MetricsChangedTrigger(string name, IEnumerable<string> metricCommonIds) : base(name)
        {
            this.MetricCommonIds = new HashSet<string>(metricCommonIds);
        }

        // todo: 
        public HashSet<string>? FeatureCommonIds
        {
            get => MetricCommonIds; set
            {
                if (value != null)
                {

                    MetricCommonIds = value;
                }
            }
        }
        public HashSet<string>? MetricCommonIds { get; set; }
    }
}