using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public enum ModelTypes
    {
        SingleClassClassifier,
        ParameterSetRecommenderV1,
        ProductRecommenderV1,
        ItemsRecommenderV1,
    }

    public enum HostingTypes
    {
        AzureMLContainerInstance,
        AzurePersonalizer,
        AzureFunctions,
    }

    public class ModelRegistration : Entity
    {
        public ModelRegistration()
        { }

        public ModelRegistration(string name,
                                 ModelTypes modelType,
                                 HostingTypes hostingType,
                                 string scoringUrl,
                                 string key,
                                 SwaggerDefinition swagger)
        {
            ModelType = modelType;
            HostingType = hostingType;
            ScoringUrl = scoringUrl;
            Key = key;
            Swagger = swagger;
            Name = name;
        }

        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<RecommenderEntityBase> Recommenders { get; set; }
        [JsonIgnore]
        public ICollection<RecommendationCorrelator> Correlators { get; set; }
        public ModelTypes ModelType { get; set; }
        public HostingTypes HostingType { get; set; }
        public string ScoringUrl { get; set; }
        [JsonIgnore]
        public string Key { get; set; }
        public SwaggerDefinition Swagger { get; set; }
    }
}