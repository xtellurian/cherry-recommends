using System.Collections.Generic;

#nullable enable
namespace SignalBox.Core.Recommenders
{
    public class FeaturesChangedTrigger : TriggerBase
    {
        public FeaturesChangedTrigger()
        { }

        public FeaturesChangedTrigger(string name, IEnumerable<string> featureCommonIds) : base(name)
        {
            this.FeatureCommonIds = new HashSet<string>(featureCommonIds);
        }

        // todo: 
        public HashSet<string>? FeatureCommonIds { get; set; }
    }
}