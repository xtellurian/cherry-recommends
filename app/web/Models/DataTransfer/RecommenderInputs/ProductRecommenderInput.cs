using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable enable
namespace SignalBox.Web.Dto.RecommenderInputs
{
    public class ProductRecommenderInput
    {
        public string? GetCustomerId() => CustomerId ?? CommonUserId;
        public string? CommonUserId { get; set; }
        public string? CustomerId { get; set; }
        public Dictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
    }
}