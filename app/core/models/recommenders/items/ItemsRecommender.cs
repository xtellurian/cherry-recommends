using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Recommenders
{
    public class ItemsRecommender : RecommenderEntityBase, IRecommender
    {
        protected ItemsRecommender()
        { }

#nullable enable
        public ItemsRecommender(string commonId,
                                string? name,
                                RecommendableItem? defaultItem,
                                ICollection<RecommendableItem>? items,
                                IEnumerable<RecommenderArgument>? arguments,
                                RecommenderSettings? settings,
                                int numberOfItemsToRecommend = 1) : base(commonId, name, arguments, settings)
        {
            Items = items ?? new List<RecommendableItem>();
            DefaultItem = defaultItem;
            DefaultItemId = defaultItem?.Id;
            NumberOfItemsToRecommend = numberOfItemsToRecommend;

            if (numberOfItemsToRecommend > 9)
            {
                throw new BadRequestException("Maximum number of items to recommend cannot be greater than 9");
            }
            if (Items.Any() && numberOfItemsToRecommend > Items.Count)
            {
                throw new BadRequestException($"Maximum number of items cannot be greater than the number of items provided ({Items.Count})");
            }
        }

        public long? DefaultItemId { get; set; }
        public RecommendableItem? DefaultItem { get; set; }
        public int? NumberOfItemsToRecommend { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<RecommendableItem> Items { get; set; }
        [JsonIgnore]
        public ICollection<ItemsRecommendation> Recommendations { get; set; } = null!;

    }
}