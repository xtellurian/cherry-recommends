namespace SignalBox.Core
{
#nullable enable
    public static class CustomerEventExtensions
    {
        private static bool TryGetStringValueFromProperties(this CustomerEvent e, string key, out string? value)
        {
            if (e.Properties.ContainsKey(key))
            {
                value = e.Properties[key]?.ToString();
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public static bool TryGetPromotionId(this CustomerEvent e, out string? promotionId)
        {
            return e.TryGetStringValueFromProperties("promotionId", out promotionId);
        }
    }
}