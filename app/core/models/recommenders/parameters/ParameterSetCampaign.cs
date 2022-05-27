using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Campaigns
{
    public class ParameterSetCampaign : CampaignEntityBase, ICampaign
    {
        public ParameterSetCampaign()
        { }

        public ParameterSetCampaign(string commonId,
                                       string name,
                                       IEnumerable<Parameter> parameters,
                                       IEnumerable<ParameterBounds> bounds,
                                       IEnumerable<CampaignArgument> arguments,
                                       CampaignSettings settings) : base(commonId, name, arguments, settings)
        {
            // validate # paramters. No more than 3 to limit query load on the database
            if (parameters.Count() > 3)
            {
                throw new ConfigurationException("Parameter Set Recomenders should not have more than 3 Parameters.");
            }

            this.Parameters = parameters.ToList();
            this.ParameterBounds = bounds.ToList();
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Parameter> Parameters { get; set; }
        public List<ParameterBounds> ParameterBounds { get; set; }
        [JsonIgnore]
        public ICollection<ParameterSetRecommendation> Recommendations { get; set; }

        [JsonIgnore]
        public string ScoringUrl { get; set; }
        [JsonIgnore]
        public string Key { get; set; }
    }
}