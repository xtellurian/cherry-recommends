using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class ScoreContainer
    {
#nullable enable
        [JsonConstructor]
        public ScoreContainer()
        { }

        public ScoreContainer(RecommendableItem item, double? score)
        {
            ItemId = item.Id;
            ItemCommonId = item.CommonId;
            Score = score;
        }
        public long? ItemId { get; set; }
        public string? ItemCommonId { get; set; }
        public double? Score { get; set; }
    }
}