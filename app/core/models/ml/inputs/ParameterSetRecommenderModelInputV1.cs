using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
#nullable enable
    public class ParameterSetRecommenderModelInputV1 : IModelInput
    {
        public ParameterSetRecommenderModelInputV1()
        { }
        public ParameterSetRecommenderModelInputV1(string customerId)
        {
            this.CustomerId = customerId;
        }

        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string? CustomerId { get; set; } = System.Guid.NewGuid().ToString(); // create a new GUID here if empty
        public string? CommonUserId { get; set; }
        [Required]
        public IDictionary<string, object>? Arguments { get; set; } = null!;
        public IDictionary<string, object>? Features { get; set; }
        public IEnumerable<ParameterBounds>? ParameterBounds { get; set; }

        public string GetCustomerId()
        {
            return CustomerId ?? CommonUserId ?? throw new BadRequestException("Customer ID and CommonUserId were null");
        }
    }
}