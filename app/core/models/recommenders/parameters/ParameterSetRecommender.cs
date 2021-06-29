using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SignalBox.Core.Recommenders
{
    public class ParameterSetRecommender : CommonEntity
    {
        public ParameterSetRecommender()
        { }

        public ParameterSetRecommender(string commonId,
                                       string name,
                                       IEnumerable<Parameter> parameters,
                                       IEnumerable<ParameterBounds> bounds,
                                       IEnumerable<RecommenderArgument> arguments) : base(commonId, name)
        {
            // validate # paramters. No more than 3 to limit query load on the database
            if (parameters.Count() > 3)
            {
                throw new ConfigurationException("Parameter Set Recomenders should not have more than 3 Parameters.");
            }

            this.Parameters = parameters.ToList();
            this.ParameterBounds = bounds.ToList();
            this.Arguments = arguments.ToList();
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ModelRegistration ModelRegistration { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Parameter> Parameters { get; set; }
        public List<ParameterBounds> ParameterBounds { get; set; }
        public List<RecommenderArgument> Arguments { get; set; }

        [JsonIgnore]
        public string ScoringUrl { get; set; }
        [JsonIgnore]
        public string Key { get; set; }
    }
}