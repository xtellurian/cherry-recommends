using System.Collections.Generic;
using SignalBox.Core;

#nullable enable
namespace SignalBox.Web.Dto
{
    public class CreateOrUpdateCustomerDto : DtoBase
    {
        public string? GetCustomerId() => CustomerId ?? CommonUserId;
        /// <summary>
        /// If the Customer ID already exists, then overwrite the name, email, and properties
        /// </summary>
        public bool? Overwrite { get; set; }
        public string? CommonUserId { get; set; } = null!; // to be deprecated
        public string? CustomerId { get; set; } = null!;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Dictionary<string, object>? Properties { get; set; }
        public IntegratedSystemReference? IntegratedSystemReference { get; set; }
    }
}