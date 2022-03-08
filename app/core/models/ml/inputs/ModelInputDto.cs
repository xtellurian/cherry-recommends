using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SignalBox.Core.Recommenders;

#nullable enable
namespace SignalBox.Core
{
    public class ModelInputDto : IModelInput
    {
        public ModelInputDto()
        { }
        public ModelInputDto(string customerId)
        {
            this.CustomerId = customerId;
            this.CommonUserId = customerId;
            this.Arguments = new Dictionary<string, object>();
        }
        protected ModelInputDto(IDictionary<string, object> arguments)
        {
            this.Arguments = arguments ?? new Dictionary<string, object>();
        }

        public string? CustomerId { get; set; }
        public string? CommonUserId { get; set; }
        public string? BusinessId { get; set; }
        public IDictionary<string, object>? Arguments { get; set; }
        public IDictionary<string, object>? Metrics { get; set; }
        public IDictionary<string, object>? Features
        {
            get => Metrics; set
            {
                if (value != null)
                {
                    Metrics = value;
                }
            }
        }

        public IEnumerable<ParameterBounds>? ParameterBounds { get; set; }

        public string? GetCustomerId()
        {
            return CustomerId ?? CommonUserId;
        }
    }
}