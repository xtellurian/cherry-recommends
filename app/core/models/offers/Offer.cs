using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class Offer : NamedEntity
    {
        public Offer()
        { }

#nullable enable
        public Offer(string name, string currency, double price, double? cost, string? discountCode) : base(name)
        {
            Currency = currency;
            Price = price;
            Cost = cost ?? 0;
            DiscountCode = discountCode;
        }

        public string Currency { get; set; }
        public double Price { get; set; } // paid by the user
        public double? Cost { get; set; } // paid by the business
        public string? DiscountCode { get; set; }
        [JsonIgnore]
        public ICollection<PresentationOutcome> Outcomes { get; set; } = null!;

        [JsonIgnore]
        public ICollection<Experiment> Experiments { get; set; } = null!;
    }
}