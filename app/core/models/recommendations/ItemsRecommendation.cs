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
                                    RecommendingContext context,
                                   IEnumerable<ScoredRecommendableItem> items)
         : base(context.Correlator, RecommenderTypes.Items, context.Trigger)
        {
            Recommender = recommender;
            RecommenderId = recommender.Id;
            Customer = context.Customer;
            Items = items.Select(_ => _.Item).ToList();
            Scores = items.Select(_ => new ScoreContainer(_.Item, _.Score)).ToList();
            TargetMetric = recommender.TargetMetric;
            TargetMetricId = recommender.TargetMetricId;
        }

#nullable enable
        public long? RecommenderId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ItemsRecommender? Recommender { get; set; }
        [JsonIgnore]
        public ICollection<RecommendableItem> Items { get; set; }
        [JsonIgnore]
        public List<ScoreContainer> Scores { get; set; }
        public long? MaxScoreItemId => Scores.FirstOrDefault(_ => _.Score == Scores.Max(_ => _.Score)).ItemId;

        private double? GetScore(RecommendableItem item) => Scores.FirstOrDefault(_ => _.ItemId == item.Id || _.ItemCommonId == item.CommonId).Score;

        [JsonPropertyName("scoredItems")]
        public IOrderedEnumerable<ScoredRecommendableItem> ScoredItems
            => this.Items?
                .Select(_ => new ScoredRecommendableItem(_, GetScore(_)))?
                .OrderByDescending(_ => _.Score)!; // nullable due to backwards compat issue
    }
}