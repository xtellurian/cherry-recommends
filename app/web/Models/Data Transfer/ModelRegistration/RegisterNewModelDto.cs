using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto.ModelRegistration
{
    public class RegisterNewModelDto : DtoBase
    {
        public string Name { get; set; }
        [Required]
        public string ScoringUrl { get; set; }
        [Required]
        public string SwaggerUrl { get; set; }
        [Required]
        public string Key { get; set; }
        public string ModelType { get; set; }
        public string HostingType { get; set; }
    }
}