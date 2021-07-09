using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Recommenders
{
    public class ProductRecommender : CommonEntity, IRecommender
    {
        protected ProductRecommender()
        { }

#nullable enable
        public ProductRecommender(string commonId, string? name, Touchpoint? touchpoint, ICollection<Product>? products) : base(commonId, name)
        {
            Products = products ?? new List<Product>();
            Touchpoint = touchpoint;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Product> Products { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ModelRegistration? ModelRegistration { get; set; }
        [JsonIgnore]
        public ICollection<ProductRecommendation> Recommendations { get; set; } = null!;

        //TODO: replace touchpoint w/ Features
        // this is a way to get summary data to the model.
        // we might replace this with a set of Features. (tbd)
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Touchpoint? Touchpoint { get; set; }
    }
}