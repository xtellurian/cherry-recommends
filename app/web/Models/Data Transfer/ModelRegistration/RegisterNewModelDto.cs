using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto.ModelRegistration
{
    public class RegisterNewModelDto : DtoBase
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ScoringUrl { get; set; }
        [Required]
        public string Key { get; set; }
        public string SwaggerUrl { get; set; }
        [Required]
        [RegularExpression("SingleClassClassifier|ParameterSetRecommenderV1|ProductRecommenderV1",
            ErrorMessage = "SystemType must be one of SingleClassClassifier, ParameterSetRecommenderV1, ProductRecommenderV1")]
        public string ModelType { get; set; }
        [Required]
        [RegularExpression("AzureMLContainerInstance|AzurePersonalizer", 
            ErrorMessage = "SystemType must be one of AzureMLContainerInstance, AzurePersonalizer")]
        public string HostingType { get; set; }
    }
}