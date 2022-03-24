using SignalBox.Core;

namespace SignalBox.Test
{
#nullable enable
    public static class EntityFactory
    {
        private static string RandStr => System.Guid.NewGuid().ToString();
        public static RecommendableItem Promotion(string? commonId = null, string? name = null)
        {
            return new RecommendableItem(commonId ?? RandStr, name ?? RandStr, null, 1, BenefitType.Percent, 0.2, PromotionType.Discount, null);
        }
    }
}