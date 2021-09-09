using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Recommenders
{
    public class ProductRecommender : RecommenderEntityBase, IRecommender
    {
        protected ProductRecommender()
        { }

#nullable enable
        public ProductRecommender(string commonId,
                                  string? name,
                                  Product? defaultProduct,
                                  ICollection<Product>? products,
                                  IEnumerable<RecommenderArgument>? arguments,
                                  RecommenderErrorHandling? errorHandling) : base(commonId, name, arguments, errorHandling)
        {
            Products = products ?? new List<Product>();
            DefaultProduct = defaultProduct;
        }

        public long? DefaultProductId { get; set; }
        public Product? DefaultProduct { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Product> Products { get; set; }
        [JsonIgnore]
        public ICollection<ProductRecommendation> ProductRecommendations { get; set; } = null!;
    }
}