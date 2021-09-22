using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Core
{
    public class ModelInputDto : IModelInput
    {
        public ModelInputDto()
        { }
        public ModelInputDto(string commonUserId)
        {
            this.CommonUserId = commonUserId;
            this.Arguments = new Dictionary<string, object>();
        }
        public ModelInputDto(string commonUserId, IDictionary<string, object> arguments)
        {
            this.CommonUserId = commonUserId;
            this.Arguments = arguments ?? new Dictionary<string, object>();
        }
        [Required]
        public string CommonUserId { get; set; }
        public IDictionary<string, object> Arguments { get; set; }
        public IDictionary<string, object> Features { get; set; }
    }
}