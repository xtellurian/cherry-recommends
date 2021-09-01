using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Recommendations
{
    public class ItemsRecommendation : RecommendationEntity
    {
        protected ItemsRecommendation()
        { }

        public ItemsRecommendation(ItemsRecommender recommender,
                                   TrackedUser trackedUser,
                                   RecommendationCorrelator correlator,
                                   IEnumerable<ScoredRecommendableItem> items)
         : base(correlator, RecommenderTypes.Product, "Items")
        {
            Recommender = recommender;
            RecommenderId = recommender.Id;
            TrackedUser = trackedUser;
            Items = items.Select(_ => _.Item).ToList();
            Scores = items.Select(_ => new ScoreContainer(_.Item, _.Score)).ToList();
        }

#nullable enable
        public long? RecommenderId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ItemsRecommender? Recommender { get; set; }
        [JsonIgnore]
        public ICollection<RecommendableItem> Items { get; set; }
        [JsonIgnore]
        public List<ScoreContainer> Scores { get; set; }

        private double? GetScore(RecommendableItem item) => Scores.FirstOrDefault(_ => _.ItemId == item.Id || _.ItemCommonId == item.CommonId).Score;

        [JsonPropertyName("ScoredItems")]
        public IEnumerable<ScoredRecommendableItem> ScoredItems => this.Items.Select(_ => new ScoredRecommendableItem(_, GetScore(_)));
    }
}