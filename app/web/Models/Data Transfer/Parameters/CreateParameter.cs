using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto.Parameters
{
    public class CreateParameter : CommonDtoBase
    {
        /// <summary>
        /// One of Categorical or Numeric
        /// </summary>
        [RegularExpression("Numerical|Categorical", ErrorMessage = "ParameterType must be one of Numerical, Categorical")]
        public string ParameterType { get; set; }
        public string Description { get; set; }
        public object DefaultValue { get; set; }
    }
}