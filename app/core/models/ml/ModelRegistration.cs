using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public enum ModelTypes
    {
        SingleClassClassifier,
        ParameterSetRecommenderV1
    }

    public enum HostingTypes
    {
        AzureMLContainerInstance
    }

    public class ModelRegistration : NamedEntity
    {
        public ModelRegistration()
        { }

        public ModelRegistration(string name,
                                 ModelTypes modelType,
                                 HostingTypes hostingType,
                                 string scoringUrl,
                                 string key,
                                 SwaggerDefinition swagger) : base(name)
        {
            ModelType = modelType;
            HostingType = hostingType;
            ScoringUrl = scoringUrl;
            Key = key;
            Swagger = swagger;
        }

        public ModelTypes ModelType { get; set; }
        public HostingTypes HostingType { get; set; }
        public string ScoringUrl { get; set; }
        [JsonIgnore]
        public string Key { get; set; }
        public SwaggerDefinition Swagger { get; set; }
    }
}