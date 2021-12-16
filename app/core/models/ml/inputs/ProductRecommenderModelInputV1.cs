using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public class ProductRecommenderModelInputV1 : IModelInput
    {
        public string CustomerId { get; set; }
        public string CommonUserId { get; set; }
        public string Version { get; set; }
        public IDictionary<string, object> Arguments { get; set; }
        public IDictionary<string, object> Features { get; set; }
        public IEnumerable<ParameterBounds> ParameterBounds { get; set; }

        public string GetCustomerId()
        {
            return CustomerId ?? CommonUserId ?? throw new BadRequestException("Customer ID and CommonUserId were null");
        }
    }
}