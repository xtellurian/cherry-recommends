using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class Feature : CommonEntity
    {
        public Feature()
        { }
        public Feature(string commonId, string name) : base(commonId, name)
        { }

        [JsonIgnore]
        public ICollection<HistoricTrackedUserFeature> HistoricTrackedUserFeatures { get; set; }
    }
}